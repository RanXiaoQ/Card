using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour,IPointerDownHandler
{
    public GameObject m_Card;
    public GameObject m_SummonBlock;   //高亮格子
    public GameObject m_AttackBlock;   //攻击格子

    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_SummonBlock.activeInHierarchy == true)
        {
            BattleManager.m_Instance.SummonConfirm(transform);
        }      
    }
}
