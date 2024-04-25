using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector2 m_StarPoint;
    private Vector2 m_EndingPoint;
    private RectTransform m_Arrow;

    private float m_ArrowLength;
    private float m_ArrowTheta;
    private Vector2 m_ArrowPosition;

    private void Start()
    {
        m_Arrow = transform.GetComponent<RectTransform>();
    }

    private void Update()
    {
        // 计算变量
        m_EndingPoint = Input.mousePosition - new Vector3(960, 540, 0);

        // 计算偏移
        Vector2 direction = (m_EndingPoint - m_StarPoint).normalized; // 方向向量
        float offsetDistance = 20.0f; // 偏移距离
        Vector2 offset = direction * offsetDistance; // 计算偏移向量

        // 加上偏移以确保箭头与鼠标位置有一定距离
        m_ArrowPosition = new Vector2((m_EndingPoint.x + m_StarPoint.x) / 2, (m_EndingPoint.y + m_StarPoint.y) / 2) - offset;
        m_ArrowLength = Vector2.Distance(m_StarPoint, m_EndingPoint) - offsetDistance; // 更新箭头长度以考虑偏移
        m_ArrowTheta = Mathf.Atan2(m_EndingPoint.y - m_StarPoint.y, m_EndingPoint.x - m_StarPoint.x);

        // 赋值
        m_Arrow.localPosition = m_ArrowPosition;
        m_Arrow.sizeDelta = new Vector2(m_ArrowLength, m_Arrow.sizeDelta.y);
        m_Arrow.localEulerAngles = new Vector3(0, 0, m_ArrowTheta * 180 / Mathf.PI);
    }

    /// <summary>
    /// 修正屏幕中心
    /// </summary>
    /// <param name="_startPoint"></param>
    public void SetStartPoint(Vector2 _startPoint)
    {
        m_StarPoint = _startPoint - new Vector2(960, 540);
    }
}
