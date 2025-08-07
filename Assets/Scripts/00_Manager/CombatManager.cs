using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    private readonly Dictionary<int, CharacterInfo> dicCharacterInfo = new();

    protected override void Awake()
    {
        base.Awake();
    }

    public void InitCharacterInfoFromDeckPack(DeckPack myDeckPack)
    {
        dicCharacterInfo.Clear();

        foreach (var slot in myDeckPack.tokenSlots)
        {
            if (DataManager.Instance.dicCharacterCardData.TryGetValue(slot.tokenKey, out var token))
            {
                dicCharacterInfo[slot.tokenKey] = new CharacterInfo {
                    tokenKey = slot.tokenKey,
                    currentHp = token.hp,
                    currentMp = token.mp,
                };
            }
        }

        Debug.Log($"[CombatManager] ĳ���� ���� ���� �Ϸ�: �� {dicCharacterInfo.Count}��");
    }

    public int GetAliveCharacterCount()
    {
        return dicCharacterInfo.Values.Count(info => !info.IsDead);
    }
}
