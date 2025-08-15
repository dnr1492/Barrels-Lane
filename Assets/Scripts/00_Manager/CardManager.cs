using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    private readonly List<SkillCardData> totalSkillCards = new();  //��ü ��ųī��
    private readonly List<SkillCardData> drawnSkillCards = new();  //��ο�� ��ųī�� (+�⺻ �̵�ī��)
    private readonly Dictionary<int, List<SkillCardData>> poolByAlive = new();  //�� ��ȯ �� ���� ĳ������ ��ųī�常���� �� ���� ���

    public List<SkillCardData> GetDrawnSkillCards() => new(drawnSkillCards);

    protected override void Awake()
    {
        base.Awake();
    }

    public void InitDeckFromDeckPack(DeckPack pack)
    {
        totalSkillCards.Clear();
        poolByAlive.Clear();

        foreach (var slot in pack.tokenSlots)
        {
            int tokenKey = slot.tokenKey;

            foreach (var skill in slot.skillCounts)
            {
                int skillId = skill.skillId;
                int count = skill.count;

                for (int i = 0; i < count; i++)
                {
                    if (DataManager.Instance.dicSkillCardData.TryGetValue(skillId, out var skillCard))
                    {
                        totalSkillCards.Add(skillCard);
                        if (!poolByAlive.TryGetValue(tokenKey, out var list)) {
                            list = new List<SkillCardData>();
                            poolByAlive[tokenKey] = list;
                        }
                        list.Add(skillCard);
                    }
                    else Debug.Log($"[CardManager] �������� �ʴ� ��ųī�� ID: {skillId}");
                }
            }
        }

        Debug.Log($"[CardManager] ��ųī��� �� �� �Ϸ�: �� {totalSkillCards.Count}��");
    }

    public void DrawSkillCardsForAliveOwners(IEnumerable<int> aliveKeys, int count)
    {
        RebuildDeckForAliveOwners(aliveKeys);
        DrawSkillCards(count);
    }

    //���� ĳ������ ��ųī�常 ������ ��
    private void RebuildDeckForAliveOwners(IEnumerable<int> aliveKeys)
    {
        totalSkillCards.Clear();
        foreach (var key in aliveKeys)
        {
            if (poolByAlive.TryGetValue(key, out var list))
                totalSkillCards.AddRange(list);
        }
    }

    private void DrawSkillCards(int count)
    {
        totalSkillCards.Shuffle();
        drawnSkillCards.Clear();

        //0�� �ε����� �̵�ī��(1000) ����
        if (DataManager.Instance.dicSkillCardData.TryGetValue(1000, out var moveCard))
            drawnSkillCards.Add(moveCard);

        int drawn = 0;
        foreach (var card in totalSkillCards.ToList())
        {
            if (card.id == 1000) continue;  //�̵�ī��� ���� (�ߺ� ����)

            drawnSkillCards.Add(card);
            totalSkillCards.Remove(card);
            drawn++;

            if (drawn >= count) break;
        }

        Debug.Log($"[CardManager] ��ųī�� {drawn}�� + �̵�ī�� 1�� ��ο�");
    }
}