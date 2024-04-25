using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Linq;

public enum GamePhase
{
    gameStart,
    playerDraw,
    playerAction,
    enemyDraw,
    enemyAction
}
public class BattleManager : MonoSingleton<BattleManager>
{
    public static BattleManager m_Instance;

    public PlayerData m_PlayerData;
    public PlayerData m_EnemyData;   //数据

    public List<Card> m_PlayerDeckList = new List<Card>();
    public List<Card> m_EnemyDeckList = new List<Card>();   //卡组

    public GameObject m_CardPrefab;   //卡牌

    public Transform m_PlayerHand;
    public Transform m_EnemyHand;   //手牌

    public GameObject[] m_PlayerBlocks;
    public GameObject[] m_EnemyBlocks;  //格子

    public GameObject m_PlayerIcon;
    public GameObject m_EnemyIcon;   //头像

    public GamePhase GamePhase = GamePhase.playerDraw;

    public UnityEvent phaseChangeEvent = new UnityEvent();

    public int[] m_SummonCountMax = new int[2];   //0 player,1 enemy
    private int[] m_SummonCounter = new int[2];

    //召唤辅助变量
    private GameObject m_WaitingMonster;
    private int m_WaitingPlayer;
    public GameObject m_ArrowPrefab;
    private GameObject m_Arrow;

    //
    private GameObject m_AttackingMonster;
    public GameObject m_AttackingPlayerOne;
    public GameObject m_AttackingPlayerTwo;
    public GameObject m_AttackArrow;
    private int m_AttackingPlayer;

    public TextMeshProUGUI m_PlayerHealthOne;
    public TextMeshProUGUI m_PlayerHealthTwo;

    public bool m_SpellBool = false;

    public Transform m_Canvas;

    private void Awake()
    {
        
        m_Instance = this;
    }
    private void Start()
    {
        if (m_PlayerData == null || m_EnemyData == null)
        {
            m_PlayerData = GameObject.Find("PlayerDataManager").GetComponent<PlayerData>();
            m_EnemyData = GameObject.Find("EnemyDataManager").GetComponent<PlayerData>();
        }
        GameStart();
        
    }

    private void Update()
    {
        
        if(Input.GetMouseButton(1))
        {
            m_WaitingMonster = null;
            DestroyArrow();
            //DestroyAttackArrow();
            CloseBlocks();
        }
    }


    #region  游戏流程
    /// <summary>
    /// 游戏开始
    /// </summary>
    public void GameStart()
    {
        //读取数据
        ReadDeck();
        //洗牌
        ShuffletDeck(0);
        ShuffletDeck(1);
        //游戏开始抽牌
        DrawCard(0, 3);
        DrawCard(1, 3);
        NextPhase();
        m_SummonCounter = m_SummonCountMax;
    }

    /// <summary>
    /// 读取卡组
    /// </summary>
    public void ReadDeck()
    {
        //加载玩家卡组
        for (int i = 0; i < m_PlayerData.m_PlayerDeck.Length; i++)
        {
            if(m_PlayerData.m_PlayerDeck[i] != 0)
            {
                int count = m_PlayerData.m_PlayerDeck[i];
                for (int j = 0; j < count; j++)
                {
                    m_PlayerDeckList.Add(m_PlayerData.m_CardStore.CopyCard(i));
                }
            }
        }
        //加载敌人卡组
        for (int i = 0; i < m_EnemyData.m_PlayerDeck.Length; i++)
        {
            if (m_EnemyData.m_PlayerDeck[i] != 0)
            {
                int count = m_EnemyData.m_PlayerDeck[i];
                for (int j = 0; j < count; j++)
                {
                    m_EnemyDeckList.Add(m_EnemyData.m_CardStore.CopyCard(i));
                }
            }
        }
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    /// <param name="_player"></param>
    public void ShuffletDeck(int _player)  //0为玩家，1为敌人
    {
        List<Card> shuffletDeck = new List<Card>();
        if(_player == 0)
        {
            shuffletDeck = m_PlayerDeckList;
        }
        if(_player == 1)
        {
            shuffletDeck = m_EnemyDeckList;
        }

        for (int i = 0; i < shuffletDeck.Count; i++)
        {
            int rad = Random.Range(0, shuffletDeck.Count);
            Card temp = shuffletDeck[i];
            shuffletDeck[i] = shuffletDeck[rad];
            shuffletDeck[rad] = temp;
        }
    }

    /// <summary>
    /// 抽卡
    /// </summary>
    /// <param name="_player">谁抽</param>
    /// <param name="_count">抽几张</param>
    public void DrawCard(int _player,int _count)   //0为玩家，1为敌人
    {
        List<Card> drawDeck = new List<Card>();
        Transform hand = transform;

        if(_player == 0)
        {
            drawDeck = m_PlayerDeckList;
            hand = m_PlayerHand;
        }
        else if(_player == 1)
        {
            drawDeck = m_EnemyDeckList;
            hand = m_EnemyHand;
        }

        for (int i = 0; i < _count; i++)
        {
            GameObject card = Instantiate(m_CardPrefab, hand);
            card.GetComponent<CardDisplay>().m_Card = drawDeck[0];
            card.GetComponent<BattleCard>().PlayerID = _player;
            drawDeck.RemoveAt(0);
        }
    }

    #region 抽牌点击
    /// <summary>
    /// 玩家抽牌
    /// </summary>
    /// <param name="i">抽牌数量</param>
    public void OnPlayerDraw(int i)
    {
        if(GamePhase == GamePhase.playerDraw)
        {
            DrawCard(0, i);
            NextPhase();
        }       
    }

    /// <summary>
    /// 敌人抽牌
    /// </summary>
    /// <param name="i">抽牌数量</param>
    public void OnEnemyDraw(int i)
    {
        if (GamePhase == GamePhase.enemyDraw)
        {
            DrawCard(1, i);
            NextPhase();
        }    
    }
    #endregion

    /// <summary>
    /// 回合结束
    /// </summary>
    public void TurnEnd()
    {
        if(GamePhase == GamePhase.playerAction || GamePhase == GamePhase.enemyAction)
        {
            NextPhase();
        }
    }


    /// <summary>
    /// 下一阶段
    /// </summary>
    public void NextPhase()
    {
        //等于最后一个阶段
        if((int)GamePhase == System.Enum.GetNames(GamePhase.GetType()).Length - 1)
        {
            GamePhase = GamePhase.playerDraw;
        }
        else
        {
            GamePhase += 1;
        }        
        phaseChangeEvent.Invoke();
    }

    #region 回合结束点击
    public void OnClickTurnEnd()
    {
        TurnEnd();
        m_SummonCountMax[0] = 3;
        m_SummonCountMax[1] = 3;
    }
    #endregion
    #endregion

    #region  战斗流程
    /// <summary>
    /// 发出召唤请求
    /// </summary>
    /// <param name="_player">玩家编号</param>
    /// <param name="_monster">怪兽卡</param>
    public void SummonRequest(int _player,GameObject _monster)
    {
        GameObject[] blocks;
        bool hasEmptyBlock = false;
        if(_player == 0 && GamePhase == GamePhase.playerAction)
        {
            blocks = m_PlayerBlocks;
        }
        else if(_player == 1 && GamePhase == GamePhase.enemyAction)
        {
            blocks = m_EnemyBlocks;
        }
        else
        {
            return;
        }
        if(m_SummonCounter[_player] > 0)
        {
            foreach (var block in blocks)
            {
                if(block.GetComponent<Block>().m_Card == null)
                {
                    block.GetComponent<Block>().m_SummonBlock.SetActive(true);                 
                    //等待召唤显示
                    hasEmptyBlock = true;
                }
            }
        }
        if(hasEmptyBlock)
        {
            m_WaitingMonster = _monster;
            m_WaitingPlayer = _player;
            m_AttackingPlayer = _player;
            CreatArrow(_monster.transform, m_ArrowPrefab);
        }
    }

    /// <summary>
    /// 召唤确认
    /// </summary>
    /// <param name="_block"></param>
    public void SummonConfirm(Transform _block)
    {
        Summon(m_WaitingPlayer, m_WaitingMonster, _block);
        CloseBlocks();
    }

    /// <summary>
    /// 召唤和攻击显示
    /// </summary>
    public void CloseBlocks()
    {
        m_AttackingPlayerOne.GetComponent<PlayerDisplayer>().m_AttackBlock.SetActive(false);
        m_AttackingPlayerTwo.GetComponent<PlayerDisplayer>().m_AttackBlock.SetActive(false);
        foreach (var block in m_PlayerBlocks)
        {
            block.GetComponent<Block>().m_SummonBlock.SetActive(false);
            block.GetComponent<Block>().m_AttackBlock.SetActive(false);
        }
        foreach (var block in m_EnemyBlocks)
        {
            block.GetComponent<Block>().m_SummonBlock.SetActive(false);
            block.GetComponent<Block>().m_AttackBlock.SetActive(false);
        }
        DestroyArrow();
        //DestroyAttackArrow();
    }

    /// <summary>
    /// 放置卡牌
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_monster"></param>
    /// <param name="_block"></param>
    public void Summon(int _player, GameObject _monster,Transform _block)
    {
        _monster.transform.SetParent(_block);
        _monster.transform.localPosition = Vector3.zero;
        _monster.GetComponent<BattleCard>().state = BattleCardState.inBlock;
        _block.GetComponent<Block>().m_Card = _monster;
        m_SummonCounter[_player]--;

        MonsterCard mc = _monster.GetComponent<CardDisplay>().m_Card as MonsterCard;
        _monster.GetComponent<BattleCard>().m_AttackCountMax = mc.m_AttackNumber;
        _monster.GetComponent<BattleCard>().ResetAttack();
    }
    #endregion

    #region  卡牌攻击
    /// <summary>
    /// 攻击请求
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_monster"></param>
    public void AttackRequst(int _player,GameObject _monster)
    {
        GameObject[] blocks;
        bool hasMonsterBlock = false;
        bool hasPlayerBlcok = false;
        if (_player == 0 && GamePhase == GamePhase.playerAction)
        {
            blocks = m_EnemyBlocks;
        }
        else if (_player == 1 && GamePhase == GamePhase.enemyAction)
        {
            blocks = m_PlayerBlocks;
        }
        else
        {
            return;
        }
        foreach (var block in blocks)
        {
            if (block.GetComponent<Block>().m_Card != null)
            {
                block.GetComponent<Block>().m_AttackBlock.SetActive(true);           
                block.GetComponent<Block>().m_Card.GetComponent<AttackTarget>().m_Attackable = true;
                //等待召唤显示
                hasMonsterBlock = true;
            }
        }
        if(int.Parse(m_PlayerHealthTwo.text) > 0)
        {
            hasPlayerBlcok = true;
            if (_player == 0)
            {
                m_AttackingPlayerTwo.GetComponent<PlayerDisplayer>().m_AttackBlock.SetActive(true);
                m_AttackingPlayerTwo.GetComponent<AttackTargetPlayer>().m_Attackable = true;
                m_AttackingPlayerOne.GetComponent<AttackTargetPlayer>().m_Attackable = false;
            }
            else
            {
                m_AttackingPlayerOne.GetComponent<PlayerDisplayer>().m_AttackBlock.SetActive(true);
                m_AttackingPlayerOne.GetComponent<AttackTargetPlayer>().m_Attackable = true;
                m_AttackingPlayerTwo.GetComponent<AttackTargetPlayer>().m_Attackable = false;
            }            
            //CreatArrow(_monster.transform, m_AttackArrow);
        }
        if(hasMonsterBlock || int.Parse(m_PlayerHealthOne.text) > 0 || int.Parse(m_PlayerHealthTwo.text) > 0)
        {
            m_AttackingMonster = _monster;
            CreatArrow(_monster.transform, m_AttackArrow);
        }
        if(int.Parse(m_PlayerHealthOne.text) < 0 || int.Parse(m_PlayerHealthTwo.text) < 0)
        {
            m_AttackingPlayerOne.GetComponent<AttackTargetPlayer>().m_Attackable = false;
            m_AttackingPlayerTwo.GetComponent<AttackTargetPlayer>().m_Attackable = false;
        }
    }

    /// <summary>
    /// 攻击怪兽
    /// </summary>
    /// <param name="_target">被攻击</param>
    public void AttackConfirm(GameObject _target)
    {
        Attack(m_AttackingMonster, _target);
        DestroyArrow();
        //DestroyAttackArrow();
        CloseBlocks();
        GameObject[] blocks;
        if (m_AttackingPlayer == 0)
        {
            blocks = m_EnemyBlocks;
        }
        else
        {
            blocks = m_PlayerBlocks;
        }

        foreach (var block in blocks)
        {
            if(block.GetComponent<Block>().m_Card != null)
            {
                block.GetComponent<Block>().m_Card.GetComponent<AttackTarget>().m_Attackable = false;
            }
        }
    }

    /// <summary>
    /// 攻击玩家
    /// </summary>
    /// <param name="_target"></param>
    public void AttackConfirmPlayer(GameObject _target)
    {
        AttackPlayer(m_AttackingMonster, _target);
        DestroyArrow();
        CloseBlocks();
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public void Attack(GameObject _attacker,GameObject _target)
    {
        MonsterCard monster = _attacker.GetComponent<CardDisplay>().m_Card as MonsterCard;
        _target.GetComponent<AttackTarget>().ApplyDamage(monster.m_Attack);
        _attacker.GetComponent<BattleCard>().CostAttackCount();
        _target.GetComponent<CardDisplay>().ShowCard();
    }

    /// <summary>
    /// 攻击玩家方法
    /// </summary>
    public void AttackPlayer(GameObject _attacker, GameObject _target)
    {
        Debug.Log("扣血");
        MonsterCard monster = _attacker.GetComponent<CardDisplay>().m_Card as MonsterCard;
        _target.GetComponent<AttackTargetPlayer>().ApplyDamage(monster.m_Attack);
        _attacker.GetComponent<BattleCard>().CostAttackCount();
        //_target.GetComponent<CardDisplay>().ShowCard();
        
    }
    #endregion

    #region 卡牌技能施放
    /// <summary>
    /// 卡牌技能施放请求
    /// </summary>
    /// <param name="_player">施放玩家</param>
    /// <param name="_target">施放目标</param>
    public void SpellReleaseRequst(int _player, GameObject _target)
    {
        GameObject[] blocks;
        bool hasMonsterBlock = false;
        m_SpellBool = true;
        if (_player == 0 && GamePhase == GamePhase.playerAction)
        {
            blocks = m_PlayerBlocks.Concat(m_EnemyBlocks).ToArray();
        }
        else if (_player == 1 && GamePhase == GamePhase.enemyAction)
        {
            blocks = m_PlayerBlocks.Concat(m_EnemyBlocks).ToArray();
        }
        else
        {
            return;
        }
        foreach (var block in blocks)
        {
            if (block.GetComponent<Block>().m_Card != null)
            {
                block.GetComponent<Block>().m_AttackBlock.SetActive(true);
                block.GetComponent<Block>().m_Card.GetComponent<AttackTarget>().m_Speelable = true;
                //等待召唤显示
                hasMonsterBlock = true;
            }
        }
        if (int.Parse(m_PlayerHealthTwo.text) > 0)
        {
            if (_player == 0 || _player == 1)
            {
                m_AttackingPlayerOne.GetComponent<PlayerDisplayer>().m_AttackBlock.SetActive(true);
                m_AttackingPlayerTwo.GetComponent<PlayerDisplayer>().m_AttackBlock.SetActive(true);
                m_AttackingPlayerTwo.GetComponent<AttackTargetPlayer>().m_Speelable = true;
                m_AttackingPlayerOne.GetComponent<AttackTargetPlayer>().m_Speelable = true;
            }
            //CreatArrow(_monster.transform, m_AttackArrow);
        }
        if (hasMonsterBlock || int.Parse(m_PlayerHealthOne.text) > 0 || int.Parse(m_PlayerHealthTwo.text) > 0)
        {
            m_AttackingMonster = _target;
            CreatArrow(_target.transform, m_AttackArrow);
        }
        if (int.Parse(m_PlayerHealthOne.text) < 0 || int.Parse(m_PlayerHealthTwo.text) < 0)
        {
            m_AttackingPlayerOne.GetComponent<AttackTargetPlayer>().m_Attackable = false;
            m_AttackingPlayerTwo.GetComponent<AttackTargetPlayer>().m_Attackable = false;
        }
    }

    /// <summary>
    /// 技能卡牌对怪兽施放技能
    /// </summary>
    /// <param name="_target">施放目标</param>
    public void SpellConfirmMonster(GameObject _target)
    {
        SpellMonster(m_AttackingMonster, _target);
        DestroyArrow();
        CloseBlocks();
    }

    public void SpellConfirmPlayer(GameObject _target)
    {
        SpellPlayer(m_AttackingMonster, _target);
        DestroyArrow();
        CloseBlocks();
    }

    

    /// <summary>
    /// 技能卡牌对怪兽施放技能方法
    /// </summary>
    /// <param name="_attacker">攻击卡牌</param>
    /// <param name="_target">施放目标</param>
    public void SpellMonster(GameObject _attacker, GameObject _target)
    {
        SpellCard spell = _attacker.GetComponent<CardDisplay>().m_Card as SpellCard;
        _target.GetComponent<AttackTarget>().ApplyDamage(spell.m_Attack);
        _target.GetComponent<CardDisplay>().ShowCard();
        m_SpellBool = false;
        Destroy(_attacker);
        //_attacker.GetComponent<BattleCard>().CostAttackCount();
    }

    public void SpellPlayer(GameObject _attacker, GameObject _target)
    {
        SpellCard spell = _attacker.GetComponent<CardDisplay>().m_Card as SpellCard;
        _target.GetComponent<AttackTargetPlayer>().ApplyDamage(spell.m_Attack);
        Destroy(_attacker);
    }
    #endregion

    /// <summary>
    /// 创建箭头
    /// </summary>
    /// <param name="_startPoint">起始点</param>
    /// <param name="_prefab"></param>
    public void CreatArrow(Transform _startPoint,GameObject _prefab)
    {
        m_Arrow = GameObject.Instantiate(_prefab, m_Canvas);
        m_Arrow.GetComponent<Arrow>().SetStartPoint(new Vector2(_startPoint.position.x, _startPoint.position.y));
    }

    /// <summary>
    /// 销毁箭头
    /// </summary>
    public void DestroyArrow()
    {
        Destroy(m_Arrow);      
    }

    public void DestroyAttackArrow()
    {
        Destroy(m_AttackArrow);
    }
}

