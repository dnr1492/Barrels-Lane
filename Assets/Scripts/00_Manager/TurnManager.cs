using Cysharp.Threading.Tasks;
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

    #region ��ġ ����
    public void StartMatch(bool iAmFirst)
    {
        trnIndex = 1;
        roundIndex = 0;
        this.isMyRound = iAmFirst;

        var photon = ControllerRegister.Get<PhotonController>();
        CardManager.Instance.InitDeckFromDeckPack(photon.MyDeckPack);             //�� �ʱ�ȭ
        CombatManager.Instance.InitCharacterInfoFromDeckPack(photon.MyDeckPack);  //ĳ���� ���� �ʱ�ȭ

        var movementOrderCtrl = ControllerRegister.Get<MovementOrderController>();
        if (movementOrderCtrl == null)
        {
            var go = new GameObject("MovementOrderController");
            go.AddComponent<MovementOrderController>();
            DontDestroyOnLoad(go);
        }

        ProceedToNextTurn();
    }
    #endregion

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
            .SetDrawnSkillCard(CardManager.Instance.GetDrawnSkillCards());
    }

    #region ���� ���� (���� ���� �� ���� �˻� �� ���� ������ ���� �� �̵�/��ųī�� ����)
    public async void StartRound()
    {
        roundIndex++;

        //�ִ� ���� �ʰ� �� ���� ������ ��ȯ
        if (roundIndex > 4) {
            EndRound();
            return;
        }

        //���� ���尡 �� �������� ����
        isMyRound = IsMyRound(roundIndex);

        // ==================== ���⼭ ��ųī�� ���� ���� �� ==================== //

        //�̵� ���� ����� + ���� ����
        var movementOrderCtrl = ControllerRegister.Get<MovementOrderController>();
        if (movementOrderCtrl != null)
        {
            //���� ���� ���� ��ü ����� (fromHexPos ����)
            bool ok = movementOrderCtrl.ValidateAllBeforeRound(); 

            //������� ����(1 �� 4). �Ϸ� �� �ļ� ó��(���� ��ų ��)�� �ݹ鿡�� �̾��.
            await UniTask.Create(async () => {
                bool done = false;
                movementOrderCtrl.ExecuteInOrder(() => {
                    Debug.Log("[Turn] �̵� ���� �Ϸ�");
                    // ===== TODO: �̵� �� �ٸ� ��ų ����/���� ������ ���⿡ �̾� ���̼���. ===== //
                    done = true;
                });

                //�Ϸ���� ������ ���
                while (!done) await UniTask.Yield(PlayerLoopTiming.Update);  
            });
        }
    }
    #endregion

    private bool IsMyRound(int round) => (round % 2 == 1) ? isMyRound : !isMyRound;

    private void EndRound()
    {
        Debug.Log($"[�� {trnIndex} ����]");

        trnIndex++;
        roundIndex = 0;

        ProceedToNextTurn();
    }
}
