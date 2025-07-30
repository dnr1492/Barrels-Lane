using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class UIMain : MonoBehaviour
{
    private void Start()
    {
        InitApp().Forget();
    }

    private async UniTaskVoid InitApp()
    {
        //1. UILoginPopup �������� �ݵ�� Instantiate�ǵ��� ShowPopup ȣ��
        UIManager.Instance.ShowPopup<UILoginPopup>("UILoginPopup", false);
        UILoginPopup loginPopup = UIManager.Instance.GetPopup<UILoginPopup>("UILoginPopup");

        //2. ���� ���� �õ�
        await loginPopup.ConnectNetwork();

        //3. �ڵ� �α��� �õ�
        if (await loginPopup.AttemptAutoLogin()) UIManager.Instance.ShowPopup<UILobbyPopup>("UILobbyPopup");
        else UIManager.Instance.ShowPopup<UILoginPopup>("UILoginPopup");
    }
}