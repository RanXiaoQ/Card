using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class PlayerData : MonoBehaviour
{
    public CardStore m_CardStore;
    public int m_PlayerCoins;   //玩家的钱
    public int[] m_PlayerCards;   //玩家的卡库
    public int[] m_PlayerDeck;    //玩家的卡组

    public TextAsset m_PlayerData;

    private void Start()
    {
        m_CardStore = GameObject.Find("CardStore").GetComponent<CardStore>();
        m_PlayerData = Resources.Load<TextAsset>("Datas/PlayerData");
        m_CardStore.LoadCardData();
        LoadPlayerData();        
    }


    /// <summary>
    /// 加载卡库数据
    /// </summary>
    public void LoadPlayerData()
    {
        m_PlayerCards = new int[m_CardStore.m_CardList.Count];
        m_PlayerDeck = new int[m_CardStore.m_CardList.Count];
        string[] dataRow = m_PlayerData.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if(rowArray[0] == "#")
            {
                continue;
            }
            else if(rowArray[0] == "coins")
            {
                m_PlayerCoins = int.Parse(rowArray[1]);
            }
            else if (rowArray[0] == "card")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);
                //载入卡库
                m_PlayerCards[id] = num;
            }
            else if (rowArray[0] == "deck")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);
                //载入卡组
                m_PlayerDeck[id] = num;
            }
        }
    }

    /// <summary>
    /// 保存卡组
    /// </summary>
    public void SavePlayerData()
    {
        string path = Application.dataPath + "/Datas/PlayerData.csv";

        List<string> datas = new List<string>();
        datas.Add("coins," + m_PlayerCoins.ToString());
        for (int i = 0; i < m_PlayerCards.Length; i++)
        {
            if(m_PlayerCards[i] != 0)
            {
                datas.Add("card," + i.ToString() + "," + m_PlayerCards[i].ToString());
            }           
        }
        //保存卡组
        for (int i = 0; i < m_PlayerDeck.Length; i++)
        {
            if (m_PlayerDeck[i] != 0)
            {
                datas.Add("deck," + i.ToString() + "," + m_PlayerDeck[i].ToString());
            }
        }
        //保存数据
        File.WriteAllLines(path, datas);
        AssetDatabase.Refresh();        
    }
}
