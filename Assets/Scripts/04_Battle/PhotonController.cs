using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Cysharp.Threading.Tasks;
using static EnumClass;

public class PhotonController : MonoBehaviourPunCallbacks
{
    public static bool IsConnected => PhotonNetwork.IsConnectedAndReady;
    private const string ROOM_NAME_PREFIX = "Room_";

    private bool pendingJoinRequest = false;
    private bool isMyDeckSent = false;
    private bool isOpponentDeckReceived = false;

    private void Awake()
    {
        ControllerRegister.Register(this);
    }

    public override void OnEnable() => PhotonNetwork.AddCallbackTarget(this);

    public override void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    /// <summary>
    /// Photon ���� ���� ���θ� �Ǵ��ؼ� ��� ��Ī�ϰų�, ���� �� �ڵ� ��Ī ����
    /// </summary>
    public void RequestJoinRoomAfterConnection()
    {
        if (PhotonNetwork.IsConnectedAndReady) {
            Debug.Log("Photon ���� �Ϸ� �� ��� �� ����");
            JoinOrCreateRoom();
        }
        else {
            Debug.Log("Photon ���� ���� �� �� �� ��Ī ���� �� ���� �õ�");
            pendingJoinRequest = true;
            ConnectToPhoton();
        }
    }

    #region Photon ������ ����
    private void ConnectToPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Photon ���� ���� �õ�...");
            PhotonNetwork.ConnectUsingSettings();
        }
        else Debug.Log("�̹� Photon�� �����");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon ���� ���� ����");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ���� �Ϸ�");

        if (pendingJoinRequest) {
            Debug.Log("����� ��Ī ��û �� JoinOrCreateRoom ȣ��");
            pendingJoinRequest = false;
            JoinOrCreateRoom();
        }
    }
    #endregion

    #region �� ���� �Ǵ� ����
    public void JoinOrCreateRoom()
    {
        string roomName = ROOM_NAME_PREFIX + Random.Range(1000, 9999);
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 2,
            IsVisible = true,
            IsOpen = true
        };

        PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"�� ���� ����! ���� �ο�: {PhotonNetwork.CurrentRoom.PlayerCount}");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2) {
            Debug.Log("���� ��� ���� �� ���� �� ����");
            StartDeckExchange(UIManager.Instance.GetPopup<UIEditorDeckPhase1>("UIEditorDeckPhase1").GetCurrentDeckPack());
        }
        else {
            Debug.Log("��� ��� ��... 3�� �� AI ���� ��ȯ");
            WaitForOpponentThenStartAI().Forget();
        }
    }
    #endregion

    #region AI �������� ��ȯ
    private async UniTaskVoid WaitForOpponentThenStartAI()
    {
        await UniTask.Delay(3000);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("��� ���� �� AI ���� ����");
            isMyDeckSent = true;

            // ===== TODO: ���� AI �� ���� �������� ��ü ===== //
            var aiDeck = new DeckPack();  
            await GridManager.Instance.ApplyDeckPack(aiDeck);

            OnOpponentDeckReceived();
        }
    }
    #endregion

    #region �� ���� ó�� (���� ����)
    public void StartDeckExchange(DeckPack deckPack)
    {
        string json = JsonUtility.ToJson(deckPack);
        object[] content = { json };

        RaiseEventOptions options = new RaiseEventOptions {
            Receivers = ReceiverGroup.Others
        };

        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.SendDeck, content, options, SendOptions.SendReliable);

        isMyDeckSent = true;
        TryStartBattleIfReady();
    }

    public void OnEvent(EventData photonEvent)
    {
        HandlePhotonEvent(photonEvent).Forget();
    }

    private async UniTaskVoid HandlePhotonEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)PhotonEventCode.SendDeck)
        {
            object[] data = (object[])photonEvent.CustomData;
            string json = (string)data[0];

            var opponentDeck = JsonUtility.FromJson<DeckPack>(json);

            await GridManager.Instance.ApplyDeckPack(opponentDeck);
            OnOpponentDeckReceived();
        }
    }
    #endregion

    #region ���� ���� ���� Ȯ��
    private void OnOpponentDeckReceived()
    {
        isOpponentDeckReceived = true;
        TryStartBattleIfReady();
    }

    private void TryStartBattleIfReady()
    {
        if (isMyDeckSent && isOpponentDeckReceived)
        {
            LoadingManager.Instance.Hide();

            Debug.Log("���� �� ���� �Ϸ� �� ���� ������ (����/�İ� ����) �ܰ�� ����");
            // ===== TODO: UIManager.Instance.Get<UIBattleReady>().StartCoinFlip(); ===== //
        }
    }
    #endregion

    #region �� ���
    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("�뿡�� �����ϴ�...");
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            Debug.Log("���� �뿡 ���� ���� �� ��Ī ���ุ ���");
            pendingJoinRequest = false;
        }
    }
    #endregion

    //public override void OnJoinRoomFailed(short returnCode, string message)
    //{
    //    Debug.Log($"�� ���� ����: {message}");
    //}

    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    Debug.Log($"Photon ���� ����: {cause}");
    //}

    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
    //    Debug.Log($"��� �÷��̾� ����: {otherPlayer.NickName} �� ������ ó�� �� �ʿ�");
    //}
}
