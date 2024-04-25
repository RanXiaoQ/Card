using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardCounter : MonoBehaviour
{
    public TextMeshProUGUI m_CardNumber;
    private int m_Counter = 0;

    public bool SetCounter(int _value)
    {
        m_Counter += _value;
        OnCounterChange();
        if (m_Counter == 0)
        {
            Destroy(gameObject);
            return false;
        }
        return true;
    }

    private void OnCounterChange()
    {
        m_CardNumber.text = m_Counter.ToString();
    }
}
