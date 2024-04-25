using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public Transform m_DeckPanel;  //卡组显示区域
    public Transform m_LibrayPanel;  //卡库显示区域

    public GameObject m_LibrayPrefab;
    public GameObject m_DeckPrefab;

    public GameObject DataManager;

    private PlayerData m_PlayerData;
    private CardStore m_CardStore;

    private Dictionary<int, GameObject> libraryDic = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> deckDic = new Dictionary<int, GameObject>();

    private void Start()
    {
        m_PlayerData = DataManager.GetComponent<PlayerData>();
        m_CardStore = DataManager.GetComponent<CardStore>();
        UpdateLibrary();
        UpdateDeck();
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// 更新卡库
    /// </summary>
    public void UpdateLibrary()
    {
        for (int i = 0; i < m_PlayerData.m_PlayerCards.Length; i++)
        {
            if(m_PlayerData.m_PlayerCards[i] > 0)
            {
                CreatCard(i, CardState.Library);
            }
        }
    }

    /// <summary>
    /// 更新卡组
    /// </summary>
    public void UpdateDeck()
    {
        for (int i = 0; i < m_PlayerData.m_PlayerDeck.Length; i++)
        {
            if (m_PlayerData.m_PlayerDeck[i] > 0)
            {
                CreatCard(i, CardState.Deck);
            }
        }
    }


    public void UpdateCard(CardState _state,int _id)
    {
        if (_state == CardState.Deck)
        {
            m_PlayerData.m_PlayerDeck[_id]--;
            m_PlayerData.m_PlayerCards[_id]++;

            if(!deckDic[_id].GetComponent<CardCounter>().SetCounter(-1))
            {
                deckDic.Remove(_id);
            }
            if (libraryDic.ContainsKey(_id))
            {
                libraryDic[_id].GetComponent<CardCounter>().SetCounter(1);
            }
            else
            {
                CreatCard(_id, CardState.Library);
            }
            
        }
        else if (_state == CardState.Library)
        {
            m_PlayerData.m_PlayerDeck[_id]++;
            m_PlayerData.m_PlayerCards[_id]--;
            if (deckDic.ContainsKey(_id))
            {
                deckDic[_id].GetComponent<CardCounter>().SetCounter(1);
            }
            else
            {
                CreatCard(_id, CardState.Deck);
            }
            if(!libraryDic[_id].GetComponent<CardCounter>().SetCounter(-1))
            {
                libraryDic.Remove(_id);
            }
        }
    }

    /// <summary>
    /// 创建卡牌
    /// </summary>
    /// <param name="_id"></param>
    public void CreatCard(int _id,CardState _cardState)
    {
        Transform targetPanel;
        GameObject targetPrefab;
        var refData = m_PlayerData.m_PlayerCards;
        Dictionary<int, GameObject> targetDic = libraryDic;
        if(_cardState == CardState.Library)
        {
            targetPanel = m_LibrayPanel;
            targetPrefab = m_LibrayPrefab;
        }
        else
        {
            targetPanel = m_DeckPanel;
            targetPrefab = m_DeckPrefab;
            refData = m_PlayerData.m_PlayerDeck;
            targetDic = deckDic;
        }
        GameObject newCard = Instantiate(targetPrefab, targetPanel);
        newCard.GetComponent<CardCounter>().SetCounter(refData[_id]);
        newCard.GetComponent<CardDisplay>().m_Card = m_CardStore.m_CardList[_id];
        targetDic.Add(_id, newCard);
    }
}
