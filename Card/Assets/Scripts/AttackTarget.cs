using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackTarget : MonoBehaviour, IPointerDownHandler
{
    public bool m_Attackable;
    public bool m_Speelable;
    public void OnPointerDown(PointerEventData eventData)
    {
        if(m_Attackable)
        {
            BattleManager.Instance.AttackConfirm(gameObject);          
        }
        if (m_Speelable)
        {
            BattleManager.Instance.SpellConfirmMonster(gameObject);
            
            //BattleManager.Instance.Spell(gameObject);
        }
    }

    public void ApplyDamage(int _damage)
    {
        MonsterCard monster = GetComponent<CardDisplay>().m_Card as MonsterCard;
        monster.m_HealthPoint -= _damage;
        if(monster.m_HealthPoint <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SpellDamage(int _damage)
    {
        //SpellCard spell = GetComponent<CardDisplay>().m_Card as SpellCard;
        
    }
}
