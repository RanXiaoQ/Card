using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhaseDisplayer : MonoBehaviour
{
    public TextMeshProUGUI m_PhaseText;

    private void Start()
    {
        BattleManager.m_Instance.phaseChangeEvent.AddListener(UpdateText);
    }

    private void Update()
    {

    }

    public void UpdateText()
    {
        m_PhaseText.text = BattleManager.m_Instance.GamePhase.ToString();
    }
}
