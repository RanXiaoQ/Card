using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPackage : MonoBehaviour
{
    public GameObject m_CardPrefabs;
    CardStore m_CardStore;
    public GameObject m_CardPool;
    public PlayerData m_PlayerData;
    List<GameObject> cards = new List<GameObject>();  //临时列表

    private void Start()
    {
        m_CardStore = GetComponent<CardStore>();
        //m_PlayerData = FindObjectOfType<PlayerData>();
    }

    /// <summary>
    /// 开卡包
    /// </summary>
    public void OnClickOpen()
    {
        if(m_PlayerData.m_PlayerCoins < 2)
        {
            return;
        }
        else
        {
            m_PlayerData.m_PlayerCoins -= 2;
        }

        ClearPool();
        for (int i = 0; i < 5; i++)
        {
            GameObject newCard = GameObject.Instantiate(m_CardPrefabs, m_CardPool.transform);

            newCard.GetComponent<CardDisplay>().m_Card = m_CardStore.RandomCard();
            cards.Add(newCard);
        }
        SaveCardData();
        m_PlayerData.SavePlayerData();
    }

    /// <summary>
    /// 清空列表
    /// </summary>
    public void ClearPool()
    {
        foreach (var card in cards)
        {
            Destroy(card);
        }
        cards.Clear();
    }


    public void SaveCardData()
    {
        foreach (var card in cards)
        {
            int id = card.GetComponent<CardDisplay>().m_Card.m_Id;
            m_PlayerData.m_PlayerCards[id] += 1;
        }
    }
}
