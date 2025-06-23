using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EnumClass;

public class UICreateDeckPhase2 : UIPopupBase
{
    [SerializeField] Button btn_back, btn_save, btn_load, btn_reset;
    [SerializeField] CharacterCard characterCard;
    [SerializeField] SkillSlotCollection skillSlotCollection; 

    [Header("Hex Grid")]
    [SerializeField] RectTransform hexParantRt /*map*/, battleFieldRt;
    [SerializeField] GameObject hexPrefab;  //�簢�� �ȿ� ������ �̹��� �ִ� UI ������

    [Header("Cost")]
    [SerializeField] Slider sliCost;
    private int maxCost, sumCost;

    private void Awake()
    {
        btn_back.onClick.AddListener(OnClickBack);
        btn_save.onClick.AddListener(OnClickSave);
        btn_load.onClick.AddListener(OnClickLoad);
        btn_reset.onClick.AddListener(OnClickReset);
    }

    private void Start()
    {
        GridManager.Instance.CreateHexGrid(battleFieldRt, hexPrefab, hexParantRt);

        maxCost = DataManager.Instance.gamePlayData.maxCost;
        sliCost.value = 0;
        sliCost.maxValue = maxCost;
    }

    public void SetMaxCost(int cost)
    {
        int newCost = sumCost + cost;
        if (newCost < 0 || newCost > maxCost) return;

        sumCost = newCost;
        sliCost.value = sumCost;
    }

    public void OnClickBack()
    {
        UIManager.Instance.ShowPopup<UICreateDeckPhase1>("UICreateDeckPhase1");
    }

    public void OnClickSave()
    {
        if (!CheckSkillCountLimit()) return;

        GridManager.Instance.SaveTokenPack(GridManager.Instance.GetTokenPack());

        skillSlotCollection.Refresh();
    }

    public void OnClickLoad()
    {
        GridManager.Instance.LoadTokenPack(characterCard);

        skillSlotCollection.Refresh();
    }

    public void OnClickReset()
    {
        GridManager.Instance.ResetUIDeckPhase2(characterCard);

        skillSlotCollection.Refresh();
    }

    protected override void ResetUI()
    {
        GridManager.Instance.ResetUIDeckPhase2(characterCard);

        skillSlotCollection.Refresh();
    }

    /// <summary>
    /// ��ų ������ �˸°� �����ߴ��� Ȯ��
    /// (Ȯ���� ĳ���� x 4 = �ִ� ��ų ����)
    /// </summary>
    /// <returns></returns>
    private bool CheckSkillCountLimit()
    {
        var tokens = ControllerRegister.Get<CharacterTokenController>().GetAllCharacterToken();

        int confirmTokenCount = 0;
        int totalSkillCount = 0;

        foreach (var token in tokens)
        {
            if (token.State != CharacterTokenState.Confirm) continue;
            confirmTokenCount++;
            foreach (var count in token.GetAllSkillCounts().Values) totalSkillCount += count;
        }

        int maxSkillCount = confirmTokenCount * 4;

        if (totalSkillCount > maxSkillCount) {
            UIManager.Instance.ShowPopup<UIModalPopup>("UIModalPopup", false)
                .Set("��ų ���� �ʰ�", $"Ȯ���� ĳ���� {confirmTokenCount}�� �� 4 = {maxSkillCount}�� ���Ϸθ� ���� �����մϴ�.");
            return false;
        }

        return true;
    }
}
