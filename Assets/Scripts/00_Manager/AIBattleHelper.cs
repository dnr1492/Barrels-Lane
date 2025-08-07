using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AIBattleHelper
{
    /// <summary>
    /// ����(��)�� ����� ���� �߿��� �������� ����
    /// </summary>
    /// <returns></returns>
    public static DeckPack GetRandomDeckAI()
    {
        var decks = BackendManager.Instance.GetSortedDecks();
        if (decks == null || decks.Count == 0) {
            Debug.Log("AI�� ���� �����ϴ�");
            return null;
        }

        var values = decks;
        int index = Random.Range(0, values.Count);
        Debug.Log($"���õ� AI�� �� �̸�: {values[index].Item2.deckName}");
        return values[index].Item2;
    }

    /// <summary>
    /// ���� �Ǵ� �İ� �ڵ� ����
    /// </summary>
    /// <param name="uiCoinFlip"></param>
    public static void AutoSelectTurnOrderAI(UICoinFlip uiCoinFlip)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
            //AI�� �ڵ����� �����ϵ��� ���� (1�� ������)
            uiCoinFlip.Invoke(nameof(UICoinFlip.AutoSelectTurnOrder), 1f);
        }
    }
}