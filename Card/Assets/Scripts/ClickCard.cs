using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CardState
{
    Library,
    Deck
}
public class ClickCard : MonoBehaviour,IPointerDownHandler
{
    private DeckManager m_Deckmanager;
    private PlayerData m_PlayerData;

    public CardState m_CardState;

    private void Start()
    {
        m_Deckmanager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
        m_PlayerData = GameObject.Find("DataManager").GetComponent<PlayerData>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        int id = this.GetComponent<CardDisplay>().m_Card.m_Id;
        m_Deckmanager.UpdateCard(m_CardState, id);        
    }
}
