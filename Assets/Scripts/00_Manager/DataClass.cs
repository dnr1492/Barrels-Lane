using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TokenPack
{
    public List<TokenSlotData> tokenSlots = new();
}

[System.Serializable]
public class TokenSlotData
{
    public int tokenKey;  //ĳ���� ���� ID
    public int col, row;  //��ġ�� ��ġ
}