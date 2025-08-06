using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleSetting : UIPopupBase
{
    [SerializeField] GameObject uiCoinFlipPrefab;
    [SerializeField] Button btn_back;

    [Header("Hex Grid")]
    [SerializeField] RectTransform hexParantRt /*map*/, battleFieldRt;
    [SerializeField] GameObject hexPrefab;  //������ ����� �̹����� �ִ� UI ������

    private UICoinFlip uiCoinFlip;
    private bool isMyTurnFirst;

    public void Init()
    {
        GridManager.Instance.CreateHexGrid(battleFieldRt, hexPrefab, hexParantRt, false, true);

        UIEditorDeckPhase1 popup = UIManager.Instance.GetPopup<UIEditorDeckPhase1>("UIEditorDeckPhase1");
        var pack = popup.GetSelectedDeckPack();
        if (pack != null) GridManager.Instance.ShowDecksOnField(pack, ControllerRegister.Get<PhotonController>().OpponentDeckPack);

        var go = Instantiate(uiCoinFlipPrefab, transform);
        uiCoinFlip = go.GetComponent<UICoinFlip>();
        uiCoinFlip.Init(OnCoinSelected);
    }

    private void OnCoinSelected(int myChoice)
    {
        ControllerRegister.Get<PhotonController>().RequestCoinFlip(myChoice);
    }

    public void ShowCoinFlipResult(int result, bool isMineFirst)
    {
        uiCoinFlip.PlayFlipAnimation(result, () => {
            SetTurnOrder(isMineFirst);
        });
    }

    public void SetTurnOrder(bool isMineFirst)
    {
        isMyTurnFirst = isMineFirst;
        Debug.Log("�� ���ΰ�? " + isMyTurnFirst);

        //if (isMyTurnFirst)
        //{
        //    DrawInitialCards();         //ī�� ��ο�
        //    ShowCardSettingUI();        //4�� ���� UI Ȱ��ȭ
        //}
        //else
        //{
        //    ShowOpponentWaitingUI();    //��� ���� ��ٸ��� ǥ��
        //}
    }

    protected override void ResetUI() { }
}