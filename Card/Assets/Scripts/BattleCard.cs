using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BattleCardState
{
    inHand,
    inBlock
}

public class BattleCard : MonoBehaviour, IPointerDownHandler
{
    public int PlayerID;
    public BattleCardState state = BattleCardState.inHand;
    public int m_AttackCountMax;
    private int m_AttackCount;


    public void OnPointerDown(PointerEventData eventData)
    {
        
        //当在手牌里被点击
        if(GetComponent<CardDisplay>().m_Card is MonsterCard)
        {
            if(state == BattleCardState.inHand)
            {
                BattleManager.m_Instance.SummonRequest(PlayerID, gameObject);
            }
            else if(state == BattleCardState.inBlock && m_AttackCount > 0  && BattleManager.m_Instance.m_SpellBool == false)
            {
                BattleManager.m_Instance.AttackRequst(PlayerID, gameObject);
            }
        }
        if (GetComponent<CardDisplay>().m_Card is SpellCard)
        {
            if (state == BattleCardState.inHand)
            {
                BattleManager.m_Instance.SpellReleaseRequst(PlayerID, gameObject);
            }
        }
    }

    /// <summary>
    /// 重置攻击次数
    /// </summary>
    public void ResetAttack()
    {
        m_AttackCount = m_AttackCountMax;
    }

    public void CostAttackCount()
    {
        m_AttackCount--;
    }
}
