using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UILoginPopup : UIPopupBase
{
    [SerializeField] Button btn_loginGuest, btn_retry;

    private void Start()
    {
        btn_loginGuest.gameObject.SetActive(false);
        btn_retry.gameObject.SetActive(false);

        LodingManager.Instance.Show("������ ���� ���Դϴ�...");

        ConnectNetwork();
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
                    string guestNickname = $"�Խ�Ʈ{Random.Range(1000, 9999)}";
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
        // ===== �ӽ÷� �� �������� ���� ��. ���� �κ� ȭ������ �������� �����ϱ� ===== //
        UIManager.Instance.ShowPopup<UIEditorDeckPhase1>("UIEditorDeckPhase1");
    }

    private async void ConnectNetwork()
    {
        const int maxRetry = 3;
        bool success = false;

        for (int i = 0; i < maxRetry; i++)
        {
            success = await BackendManager.Instance.InitBackendAsync();
            if (success) break;
            await UniTask.Delay(2000);
        }

        LodingManager.Instance.Hide();

        if (success)
        {
            btn_loginGuest.gameObject.SetActive(true);
            btn_loginGuest.onClick.RemoveAllListeners();
            btn_loginGuest.onClick.AddListener(OnClickLoginGuest);
        }
        else
        {
            btn_retry.gameObject.SetActive(true);
            btn_retry.onClick.RemoveAllListeners();
            btn_retry.onClick.AddListener(() =>
            {
                LodingManager.Instance.Show("������ ���� ���Դϴ�...");
                btn_retry.gameObject.SetActive(false);
                ConnectNetwork();
            });
        }
    }

    protected override void ResetUI() { }
}