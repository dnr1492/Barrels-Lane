using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleReady : UIPopupBase
{
    [SerializeField] Button btn_back;
    [SerializeField] Button btn_startMatching;
    [SerializeField] Button btn_battleDeck;

    [Header("Hex Grid")]
    [SerializeField] RectTransform hexParantRt /*map*/, battleFieldRt;
    [SerializeField] GameObject hexPrefab;  //������ ����� �̹����� �ִ� UI ������

    private void Awake()
    {
        btn_back.onClick.AddListener(OnClickBack);
        btn_startMatching.onClick.AddListener(OnClickStartMatching);
        btn_battleDeck.onClick.AddListener(OnClickSelectBattleDeck);
    }

    private void OnClickBack()
    {
        UIManager.Instance.ShowPopup<UILobbyPopup>("UILobbyPopup");
    }

    private void OnClickStartMatching()
    {
        LoadingManager.Instance.Show("���� ���� ��� ��...", OnClickCancelMatching);

        var photon = ControllerRegister.Get<PhotonController>();
        if (!PhotonController.IsConnected) photon.RequestJoinRoomAfterConnection();
        else photon.JoinOrCreateRoom();
    }

    private void OnClickCancelMatching()
    {
        LoadingManager.Instance.Hide();

        var photon = ControllerRegister.Get<PhotonController>();
        photon.LeaveRoom();
    }

    private void OnClickSelectBattleDeck()
    {
        GridManager.Instance.CreateHexGrid(battleFieldRt, hexPrefab, hexParantRt, 33, 9.5f, false);

        UIEditorDeckPhase1 popup = UIManager.Instance.ShowPopup<UIEditorDeckPhase1>("UIEditorDeckPhase1");
        popup.SetEditMode(false);
        popup.onApplyBattleDeck = () => {
            var pack = popup.GetCurrentDeckPack();
            if (pack != null) GridManager.Instance.ShowDecksOnField(pack, ControllerRegister.Get<PhotonController>().GetOpponentDeckPack());
            Debug.Log($"������ �� �̸�: {pack.deckName}");
        };
    }

    protected override void ResetUI() { }
}
