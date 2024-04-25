using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStore : MonoBehaviour
{
    public TextAsset m_CardData;
    public List<Card> m_CardList = new List<Card>();


    private void Start()
    {
        //LoadCardData();
        //TestLoad();
    }
    /// <summary>
    /// 加载卡牌数据
    /// </summary>
    public void LoadCardData()
    {
        string[] dataRow = m_CardData.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if(rowArray[0] == "#")
            {
                continue;
            }
            else if(rowArray[0] == "monster")
            {
                //新建怪兽卡
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                int atk = int.Parse(rowArray[3]);
                int health = int.Parse(rowArray[4]);
                MonsterCard monsterCard = new MonsterCard(id, name, atk, health);
                m_CardList.Add(monsterCard);

                //Debug.Log("读取到怪兽卡:" + monsterCard.m_CardName);
            }
            else if (rowArray[0] == "spell")
            {
                //新建魔法卡
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                string effect = rowArray[3];
                int atk = int.Parse(rowArray[4]);
                SpellCard spellCard = new SpellCard(id, name, effect,atk);
                m_CardList.Add(spellCard);
            }
        }
    }

    public void TestLoad()
    {
        foreach (var item in m_CardList)
        {
            Debug.Log("卡牌:" + item.m_CardName);
        }
    }


    /// <summary>
    /// 随机一张卡牌
    /// </summary>
    /// <returns></returns>
    public Card RandomCard()
    {
        Card card = m_CardList[Random.Range(0, m_CardList.Count)];
        return card;
    }

    /// <summary>
    /// 复制卡牌
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public Card CopyCard(int _id)
    {
        Card copyCard = new Card(_id, m_CardList[_id].m_CardName);
        if(m_CardList[_id] is MonsterCard)
        {
            var monsterCard = m_CardList[_id] as MonsterCard;
            copyCard = new MonsterCard(_id, monsterCard.m_CardName, monsterCard.m_Attack, monsterCard.m_HealthPoint);
        }
        else if(m_CardList[_id] is SpellCard)
        {
            var spellCard = m_CardList[_id] as SpellCard;
            copyCard = new SpellCard(_id, spellCard.m_CardName, spellCard.m_Effect,spellCard.m_Attack);
        }
        return copyCard;
    }
}
