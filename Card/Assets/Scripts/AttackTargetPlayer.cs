using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class AttackTargetPlayer : MonoBehaviour,IPointerDownHandler
{
    public bool m_Attackable;
    public bool m_Speelable;
    public TextMeshProUGUI m_PlayerHealth;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_Attackable)
        {
            BattleManager.Instance.AttackConfirmPlayer(gameObject);
        }
        if(m_Speelable)
        {
            BattleManager.Instance.SpellConfirmPlayer(gameObject);
            //BattleManager.Instance.Spell(gameObject);
        }
    }

    public void ApplyDamage(int _damage)
    {
        int i;
        //MonsterCard monster = GetComponent<CardDisplay>().m_Card as MonsterCard;
        //monster.m_HealthPoint -= _damage;
        //if (monster.m_HealthPoint <= 0)
        //{
        //    Destroy(gameObject);
        //}
        i = int.Parse(m_PlayerHealth.text);
        i -= _damage;
        m_PlayerHealth.text = i.ToString();
        if (int.Parse(m_PlayerHealth.text) <= 0)
        {
            Destroy(gameObject);
        }
    }
}
