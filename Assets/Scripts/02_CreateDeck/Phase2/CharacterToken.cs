using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EnumClass;

public class CharacterToken : MonoBehaviour
{
    [SerializeField] Image imgCharacter;
    [SerializeField] Button btn;
    [SerializeField] CustomBackground cb;

    private UIMain uiMain;
    private CharacterCard characterCard;
    
    public bool IsSelect { get; private set; }
    public int Key { get; private set; }
    public CharacterTierAndCost Tier { get; private set; }

    private void Start()
    {
        uiMain = FindObjectOfType<UIMain>();
        characterCard = FindObjectOfType<CharacterCard>();
        gameObject.SetActive(false);
    }

    public void Init(Sprite sprite, CharacterCardData characterCardData)
    {
        gameObject.SetActive(true);
        Key = characterCardData.id;
        Tier = characterCardData.tier;
        imgCharacter.sprite = sprite;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => {
            Debug.Log($"OnClick Event: {characterCardData.name}�� ���� ǥ��");

            //ĳ���� ī�� ���� ǥ��
            characterCard.InitCardData(sprite, characterCardData, State.Front, CardType.CharacterCard);

            //�ڽ�Ʈ ����
            var cost = characterCardData.tier == CharacterTierAndCost.Captain ? 0 : (int)characterCardData.tier;
            uiMain.SetMaxCost(cost);

            //ĳ���� ��ū ���� �� ���� ����
            ControllerRegister.Get<CharacterTokenController>().OnClickToken(this, characterCardData.tier);
        });
    }

    public void Select() => cb.SetSelect(IsSelect = !IsSelect);

    public void Deselect() => cb.SetSelect(IsSelect = false);
}