using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumClass;

public class SkillSlotCollection : MonoBehaviour
{
    [SerializeField] GameObject skillSlotPrefab;

    private readonly List<SkillSlot> skillSlotPool = new();

    public void Refresh()
    {
        //��ü ��Ȱ��ȭ
        foreach (var slot in skillSlotPool) slot.gameObject.SetActive(false);

        var allTokens = ControllerRegister.Get<CharacterTokenController>().GetAllCharacterToken();
        var skillSpriteDict = SpriteManager.Instance.dicSkillSprite;
        var skillCountMap = new Dictionary<int, int>();

        foreach (var token in allTokens)
        {
            if (token.State != CharacterTokenState.Confirm) continue;

            foreach (var (skillId, count) in token.GetAllSkillCounts())
            {
                if (!skillCountMap.ContainsKey(skillId)) skillCountMap[skillId] = 0;
                skillCountMap[skillId] += count;
            }
        }

        int index = 0;
        foreach (var kv in skillCountMap)
        {
            int skillId = kv.Key;
            int count = kv.Value;

            var data = DataManager.Instance.dicSkillCardData[skillId];
            var sprite = skillSpriteDict.TryGetValue(data.name, out var sp) ? sp : null;

            SkillSlot slot;

            //Ǯ�� �̹� ������ ����
            if (index < skillSlotPool.Count) slot = skillSlotPool[index];
            else {
                //������ ���� ���� �� Ǯ�� �߰�
                var go = Instantiate(skillSlotPrefab, transform);
                slot = go.GetComponent<SkillSlot>();
                skillSlotPool.Add(slot);
            }

            slot.gameObject.SetActive(true);
            slot.Set(sprite, count);

            index++;
        }
    }
}
