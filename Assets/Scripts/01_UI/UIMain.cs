using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour
{
    private void Awake()
    {
        // ===== �ӽ÷� ���� ���� ���� ===== //
        UIManager.Instance.ShowPopup<UICreateDeckPhase1>("UICreateDeckPhase1");
    }
}
