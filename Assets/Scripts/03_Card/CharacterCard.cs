using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static EnumClass;

public class CharacterCard : Card
{
    [SerializeField] SkillSlotCollection skillSlotCollection;
    [SerializeField] TextMeshProUGUI txtName;
    [SerializeField] TextMeshProUGUI txtHp, txtMp;
    [SerializeField] TextMeshProUGUI[] txtCounts, txtRanks, txtNames;
    [SerializeField] Button[] btnCounts, btnSkills;
    [SerializeField] Image imgCharacter;
    [SerializeField] Button btnConfirm;

    private int[] skillCardCounts;
    private const int maxSkillCardCount = 4;  //�ִ� ����

    protected override void SetCharacterCard(CharacterToken clickedToken, Sprite sprite, CharacterCardData characterCardData)
    {
        btnConfirm.onClick.RemoveAllListeners();
        btnConfirm.onClick.AddListener(() => {
            ControllerRegister.Get<CharacterTokenController>().OnClickConfirm(clickedToken);
        });

        ResetUI();

        txtName.text = characterCardData.name;
        txtHp.text = characterCardData.hp.ToString();
        txtMp.text = characterCardData.mp.ToString();
        imgCharacter.sprite = sprite;

        var skillCardData = DataManager.Instance.dicSkillCardData;
        skillCardCounts = new int[btnCounts.Length];

        for (int i = 0; i < characterCardData.skills.Count; i++)
        {
            var skillId = characterCardData.skills[i];
            var skillData = skillCardData[skillId];
            if (skillData == null) continue;

            int index = i;

            //��ų �ż� �ʱ�ȭ �Ǵ� �ҷ����� (Confirm�� ��쿡�� �ҷ�����)
            int savedCount = clickedToken.State == CharacterTokenState.Confirm
                ? clickedToken.GetSkillCount(skillId)
                : 0;

            skillCardCounts[index] = savedCount;

            //�ؽ�Ʈ ����
            txtRanks[index].gameObject.SetActive(true);
            txtRanks[index].text = skillData.rank.ToString();

            txtNames[index].gameObject.SetActive(true);
            txtNames[index].text = skillData.name;

            txtCounts[index].gameObject.SetActive(true);
            txtCounts[index].text = savedCount.ToString();

            //��ư �ʱ�ȭ: Ŭ�� �� ��ų ���� ���� & ��ū�� �ݿ�
            btnCounts[index].onClick.AddListener(() =>
            {
                skillCardCounts[index] = (skillCardCounts[index] + 1) % (maxSkillCardCount + 1);
                txtCounts[index].text = skillCardCounts[index].ToString();
                clickedToken.SetSkillCount(skillId, skillCardCounts[index]);
                skillSlotCollection.Refresh();
            });

            btnSkills[index].onClick.AddListener(() => {
                UIManager.Instance.ShowPopup<UISkillInfoPopup>("UISkillInfoPopup", false).Init(skillData);
            });
        }
    }

    private void ResetUI()
    {
        foreach (var txt in txtCounts)
        {
            txt.gameObject.SetActive(false);
            txt.text = "0";
        }

        foreach (var txt in txtRanks)
        {
            txt.gameObject.SetActive(false);
            txt.text = string.Empty;
        }

        foreach (var txt in txtNames)
        {
            txt.gameObject.SetActive(false);
            txt.text = string.Empty;
        }

        foreach (var btn in btnSkills)
            btn.onClick.RemoveAllListeners();

        foreach (var btn in btnCounts)
            btn.onClick.RemoveAllListeners();
    }
}