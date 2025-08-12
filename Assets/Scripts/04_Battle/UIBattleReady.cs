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
    [SerializeField] RectTransform hexParentRt /*map*/, battleFieldRt;
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
        var photon = ControllerRegister.Get<PhotonController>();

        if (photon.MyDeckPack == null) {
            UIManager.Instance.ShowPopup<UIModalPopup>("UIModalPopup", false).Set("�� �̼���", $"������ ����� ���� �������ּ���.");
            return;
        }

        LoadingManager.Instance.Show("���� ���� ��� ��...", OnClickCancelMatching);

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
        GridManager.Instance.CreateHexGrid(battleFieldRt, hexPrefab, hexParentRt, false, false);

        UIEditorDeckPhase1 popup = UIManager.Instance.ShowPopup<UIEditorDeckPhase1>("UIEditorDeckPhase1");
        popup.SetEditMode(false);
        popup.onApplyBattleDeck = () => {
            var photon = ControllerRegister.Get<PhotonController>();
            photon.MyDeckPack = popup.GetSelectedDeckPack();
            if (photon.MyDeckPack != null) GridManager.Instance.ShowDecksOnField(photon.MyDeckPack);
            Debug.Log($"������ �� �̸�: {photon.MyDeckPack.deckName}");
        };
    }

    protected override void ResetUI() { }
}
