using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LodingManager : Singleton<LodingManager>
{
    [SerializeField] GameObject root;  //��ü �ε� �г�
    [SerializeField] TextMeshProUGUI loadingText;  //�޽��� ��¿�

    protected override void Awake()
    {
        base.Awake();

        if (root != null)
            root.SetActive(false);
    }

    /// <summary>
    /// �ε� UI ǥ��
    /// </summary>
    public void Show(string message)
    {
        if (root != null)
            root.SetActive(true);

        if (loadingText != null)
            loadingText.text = message;
    }

    /// <summary>
    /// �ε� UI �����
    /// </summary>
    public void Hide()
    {
        if (root != null)
            root.SetActive(false);
    }
}
