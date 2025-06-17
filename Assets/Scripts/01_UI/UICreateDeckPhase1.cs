using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICreateDeckPhase1 : UIPopupBase
{
    // ===== �ӽ� ��ư 1�� ===== //
    [SerializeField] Button btn;

    private void Awake()
    {
        btn.onClick.AddListener(OnClick_Primordial);
    }

    private void OnClick_Primordial()
    {
        UIManager.Instance.ShowPopup<UICreateDeckPhase2>("UICreateDeckPhase2");
    }

    protected override void ResetUI()
    {
        // ===== �ʱ�ȭ ���� ���� ��� ===== //
    }
}
