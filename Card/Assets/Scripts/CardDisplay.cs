using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public TextMeshProUGUI m_CardName;
    public TextMeshProUGUI m_Attack;
    public TextMeshProUGUI m_Health;
    public TextMeshProUGUI m_Effect;
    public Image m_CardBg;
    public Card m_Card;

    private void Start()
    {
        ShowCard();
    }

    /// <summary>
    /// 显示卡牌数据
    /// </summary>
    public void ShowCard()
    {
        if(m_Card == null)
        {
            Debug.Log("1");            
        }
        m_CardName.text = m_Card.m_CardName;
        if (m_Card is MonsterCard)
        {
            var monster = m_Card as MonsterCard;
            m_Attack.text = monster.m_Attack.ToString();
            m_Health.text = monster.m_HealthPoint.ToString();
            m_Effect.gameObject.SetActive(false);
        }
        else if(m_Card is SpellCard)
        {
            var spell = m_Card as SpellCard;
            m_Effect.text = spell.m_Effect.ToString();

            m_Attack.gameObject.SetActive(false);
            m_Health.gameObject.SetActive(false);
        }
    }
}
