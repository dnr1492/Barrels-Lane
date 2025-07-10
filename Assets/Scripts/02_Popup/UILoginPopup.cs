using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UILoginPopup : UIPopupBase
{
    [SerializeField] Button btn_loginGuest;

    private async void Awake()
    {
        btn_loginGuest.interactable = false;

        bool success = await BackendManager.Instance.InitBackendAsync();
        btn_loginGuest.interactable = success;

        btn_loginGuest.onClick.AddListener(OnClickLoginGuest);
    }

    private void OnClickLoginGuest()
    {
        BackendManager.Instance.LoginGuest(
            onSuccess: () =>
            {
                string nickname = BackendManager.Instance.GetNickname();
                if (string.IsNullOrEmpty(nickname))
                {
                    //�ڵ� �г��� ����
                    string guestNickname = $"�Խ�Ʈ{UnityEngine.Random.Range(1000, 9999)}";
                    BackendManager.Instance.SetNickname(guestNickname,
                        onSuccess: () => {
                            Debug.Log($"�г��� �ڵ� ����: {guestNickname}");
                            OnLoginComplete();
                        },
                        onFail: err => {
                            Debug.LogError($"�г��� ���� ����: {err}");
                        });
                }
                else OnLoginComplete();
            },
            onFail: err => Debug.LogError($"�Խ�Ʈ �α��� ����: {err}")
        );
    }

    private void OnLoginComplete()
    {
        UIManager.Instance.ShowPopup<UIEditorDeckPhase1>("UIEditorDeckPhase1");
    }

    protected override void ResetUI() { }
}
