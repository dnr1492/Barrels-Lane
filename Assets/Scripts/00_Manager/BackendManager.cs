using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;

public class BackendManager
{
    private static BackendManager instance = null;

    private BackendManager() { }

    public static BackendManager GetInstance()
    {
        if (instance == null) instance = new BackendManager();
        return instance;
    }

    public void InitBackend()
    {
        //�ڳ� ������ ����
        var bro = Backend.Initialize();
        if (bro.IsSuccess()) Debug.Log($"�ڳ� ���� �ʱ�ȭ ���� : {bro}");  //204
        else Debug.Log($"�ڳ� ���� �ʱ�ȭ ���� : {bro}");  //400
    }
}