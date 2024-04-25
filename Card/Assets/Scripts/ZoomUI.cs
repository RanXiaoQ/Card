using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public float m_ZoomSize;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(m_ZoomSize, m_ZoomSize, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
}
