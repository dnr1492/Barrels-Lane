using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    private List<SkillCardData> deck = new List<SkillCardData>();  //��ų ��

    //[SerializeField] GameObject cardPrefab;
    //private List<GameObject> handCards = new List<GameObject>();  //�п� �ִ� ��ų ī���

    private void Start()
    {
        SettingDeck();
    }

    private void SettingDeck()
    {
        var selectedCharacterCards = TempSelectCard();

        var allSkillCardData = DataManager.GetInstance().dicSkillCardData;
        List<SkillCardData> skillCardDeck = new List<SkillCardData>();

        foreach (var characterCard in selectedCharacterCards) {
            foreach (var skill in characterCard.skills) {
                skillCardDeck.Add(allSkillCardData[skill]);
            }
        }

        foreach (var data in skillCardDeck) {
            Debug.Log("���� �߰��� ��ų ī�� : " + data.name + " / " + data.rank);
        }

        //ī����� �����Ͽ� ���� ����ȭ
        ShuffleDeck(skillCardDeck);
    }

    private void ShuffleDeck(List<SkillCardData> cards)
    {
         deck.Clear();

        //ī�� ���� (Fisher-Yates �˰���)
        for (int i = 0; i < cards.Count; i++)
        {
            int random = Random.Range(i, cards.Count);
            SkillCardData temp = cards[i];
            cards[i] = cards[random];
            cards[random] = temp;
        }

        //���õ� ī�带 ���� �߰�
        deck.AddRange(cards);
    }

    //private void DrawCardsFromDeck(int numberOfCards)
    //{
    //    // ������ Ư�� ������ŭ ī�带 ��ο�
    //    for (int i = 0; i < numberOfCards && deck.Count > 0; i++)
    //    {
    //        SkillCardData drawnCard = deck[0];  // ������ ù ��° ī�� ��ο�
    //        deck.RemoveAt(0);  // ��ο��� ī��� ������ ����

    //        // ��ο��� ī�带 �п� ǥ��
    //        CreateCard(drawnCard);
    //    }
    //}

    //private void CreateCard(SkillCardData cardData)
    //{
    //    // ī�� ������ ����
    //    GameObject cardGo = Instantiate(cardPrefab);

    //    // ī�� ��ũ��Ʈ ��������
    //    Card cardScript = cardGo.GetComponent<Card>();
    //    cardScript.SetCardData(cardData);

    //    // �п� ī�� ��ġ (UI ó��)
    //    handCards.Add(cardGo);
    //    cardGo.SetActive(true);  // �п� �ִ� ī��� Ȱ��ȭ
    //}

    // ===== ���� ĳ���� ī�� ���� ���� ���� �ʿ� - ����� �ӽ� ===== //
    private List<CharacterCardData> TempSelectCard()
    {
        List<CharacterCardData> selectedCharacterCards = new List<CharacterCardData>();
        var allCards = DataManager.GetInstance().dicCharacterCardData;
        foreach (var card in allCards)
        {
            if (card.Key == 2 || card.Key == 3) continue;
            selectedCharacterCards.Add(card.Value);
        }
        return selectedCharacterCards;
    }
}