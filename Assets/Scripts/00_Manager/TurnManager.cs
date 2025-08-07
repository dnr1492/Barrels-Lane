using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    private int trnIndex = 0;
    private int roundIndex = 0;
    private bool isMyRound;

    public int TurnIndex => trnIndex;

    protected override void Awake()
    {
        base.Awake();
    }

    public void StartTurn(bool isMyRound)
    {
        trnIndex = 1;
        roundIndex = 0;
        this.isMyRound = isMyRound;

        var photon = ControllerRegister.Get<PhotonController>();
        CardManager.Instance.InitDeckFromDeckPack(photon.MyDeckPack);
        CombatManager.Instance.InitCharacterInfoFromDeckPack(photon.MyDeckPack);

        ProceedToNextTurn();
    }

    // ================================ ���� �� ================================ //
    // ================================ ���� �� ================================ //
    // ================================ ���� �� ================================ //
    
    private void ProceedToNextTurn()
    {
        Debug.Log($"[�� {trnIndex} ����]");

        //1. ���� ĳ���� �� ���
        int aliveCharacterCount = CombatManager.Instance.GetAliveCharacterCount();

        //2. ī�� ��ο�
        CardManager.Instance.DrawSkillCards(aliveCharacterCount * 2);   //���� ĳ���� �� �� 2 �� ��ο� + �⺻ �̵�ī�� 1 ��

        //3. ��ο��� ��ųī�带 ǥ��
        UIManager.Instance.GetPopup<UIBattleSetting>("UIBattleSetting")
            .DisplayDrawnSkillCard(CardManager.Instance.GetDrawnSkillCards());

        ////4. OnSkillCardSettingComplete

        ////5. StartNextRound()�� ����
    }

    public void OnSkillCardSettingComplete(List<UISkillCard> selected)
    {
        Debug.Log($"[���� �Ϸ�] �� ���� ���� ����");
        StartNextRound();
    }

    private void StartNextRound()
    {
        roundIndex++;

        if (roundIndex > 4) {
            EndTurn();  //���� ������ ��ȯ
            return;
        }

        isMyRound = IsMyRound(roundIndex);

        // =====  ���⼭ ��ųī�� ���� ���� �� ===== //
    }

    private bool IsMyRound(int round) => (round % 2 == 1) ? isMyRound : !isMyRound;

    private void EndTurn()
    {
        Debug.Log($"[�� {trnIndex} ����]");

        trnIndex++;
        roundIndex = 0;

        ProceedToNextTurn();
    }
}
