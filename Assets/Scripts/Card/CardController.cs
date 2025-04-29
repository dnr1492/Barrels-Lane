using System.Collections.Generic;
using UnityEngine;
using static EnumClass;

public class CardController : MonoBehaviour
{
    // 1) �� �ڽ�Ʈ ����
    // 2) Ƽ� ���� �� ����
    // 3) Low Ƽ��� ���� ī�� �ִ� 3�����, Middle Ƽ��� ���� ī�� �ִ� 2����� �ߺ� ��� �� �� �ܿ��� �ߺ� ����
    [Header("Character Card")]
    private int maxCost;  //�ִ� Cost
    private int curTotalCost;  //���� Cost
    private List<int> selectedCharacterCardIDs;  //������ ĳ���� ī�� ID
    private Dictionary<int, int> selectedCharacterCardCounts;  //������ ĳ���� ī�� ���� - (ID, ����)
    private Dictionary<string, int> characterCardTierMaxCounts;  //ĳ���� ī�� Tier�� �ִ� ���� - (Tier, ����)
    private Dictionary<string, int> characterCardTierCurrentCounts;  //ĳ���� ī�� Tier�� ���� ���� - (Tier, ����)
    
    [Header("Skill Card")]
    [SerializeField] GameObject skillCardPrefab;
    [SerializeField] RectTransform skillCardParant;
    private List<SkillCardData> deck = new List<SkillCardData>();  //�ʵ忡 �ִ� ��ų ī�� ��
    private List<GameObject> handCards = new List<GameObject>();  //�տ� �ִ� ��ų ī���

    private void Start()
    {
        var gamePlayData = DataManager.Instance.gamePlayData;
        maxCost = gamePlayData.maxCost;
        curTotalCost = 0;

        selectedCharacterCardIDs = new List<int>();
        selectedCharacterCardCounts = new Dictionary<int, int>();
        characterCardTierMaxCounts = new Dictionary<string, int>() {
            { CharacterTierAndCost.Leader.ToString(), gamePlayData.maxLeaderCount },
            { CharacterTierAndCost.High.ToString(), gamePlayData.maxHighTierCount },
            { CharacterTierAndCost.Middle.ToString(), gamePlayData.maxMiddleTierCount },
            { CharacterTierAndCost.Low.ToString(), gamePlayData.maxLowTierCount },
        };
        characterCardTierCurrentCounts = new Dictionary<string, int>() {
            { CharacterTierAndCost.Leader.ToString(), 0 },
            { CharacterTierAndCost.High.ToString(), 0 },
            { CharacterTierAndCost.Middle.ToString(), 0 },
            { CharacterTierAndCost.Low.ToString(), 0 },
        };
    }

    //// ===== ���� ĳ���� ������ ���ؼ� ��� ĳ���� ī�� �����͸� ������ ǥ���� �� ��� �� ���� UI �������� �����ϱ� ===== //
    //// ===== *** Leader�� ������ 1�� *** ===== //
    //// ===== *** ĳ���� ī��� ��ū ������ 5�������μ� �ʵ忡 ���� *** ===== //
    //private void CreateCharacterCard()
    //{
    //}

    #region (Toggle) ĳ���� ī�� ����
    public bool SelectCharacterCard(int cardID)
    {
        var dataManager = DataManager.Instance;
        var cardData = dataManager.dicCharacterCardData[cardID];
        var tier = cardData.tier;
        int cost = (int)tier;

        //���� ���� �� Ȯ��
        int currentCardCount = selectedCharacterCardCounts.TryGetValue(cardID, out var cardCount) ? cardCount : 0;
        int currentTierCount = characterCardTierCurrentCounts[tier.ToString()];
        int maxTierCount = characterCardTierMaxCounts[tier.ToString()];

        //�ߺ� ���� ���� üũ
        switch (System.Enum.Parse<CharacterTierAndCost>(tier.ToString()))
        {
            case CharacterTierAndCost.Low:
                if (currentCardCount >= dataManager.gamePlayData.limitLow) return false;
                break;
            case CharacterTierAndCost.Middle:
                if (currentCardCount >= dataManager.gamePlayData.limitMiddle) return false;
                break;
            default:
                if (currentCardCount > 0) return false;
                break;
        }

        //Ƽ� �� ���� üũ
        if (currentTierCount >= maxTierCount) return false;

        //�ڽ�Ʈ �ʰ� üũ
        if (curTotalCost + cost > maxCost) return false;

        //ĳ���� ���� ó��
        if (currentCardCount == 0) selectedCharacterCardIDs.Add(cardID);
        selectedCharacterCardCounts[cardID] = currentCardCount + 1;
        characterCardTierCurrentCounts[tier.ToString()]++;
        curTotalCost += cost;

        return true;
    }
    #endregion

    #region (Toggle) ĳ���� ī�� ���� ����
    public bool UnselectCharacterCard(int cardID)
    {
        if (!selectedCharacterCardCounts.TryGetValue(cardID, out var count) || count <= 0)
            return false;

        var cardData = DataManager.Instance.dicCharacterCardData[cardID];
        var tier = cardData.tier;
        int cost = (int)tier;

        //���� ����
        if (--count == 0) {
            selectedCharacterCardCounts.Remove(cardID);
            selectedCharacterCardIDs.Remove(cardID);
        }
        else {
            selectedCharacterCardCounts[cardID] = count;
        }

        characterCardTierCurrentCounts[tier.ToString()]--;
        curTotalCost -= cost;

        return true;
    }
    #endregion

    #region ������ ĳ���� ī�忡 ���� ��ų ī�带 �����ͼ� ��� ����
    public void SettingDeck()
    {
        var selectedCharacterCards = GetSelectedCharacterCard();

        var allSkillCardData = DataManager.Instance.dicSkillCardData;
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

    private List<CharacterCardData> GetSelectedCharacterCard()
    {
        var allCards = DataManager.Instance.dicCharacterCardData;
        List<CharacterCardData> selectedCharacterCards = new List<CharacterCardData>();

        foreach (var cardID in selectedCharacterCardCounts.Keys) {
            if (allCards.TryGetValue(cardID, out var cardData)) {
                selectedCharacterCards.Add(cardData);
            }
        }

        return selectedCharacterCards;
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
    #endregion

    #region ������ ��ų ī�带 �̱�
    private void DrawCardsFromDeck(int drawCount)
    {
        //������ Ư�� ������ŭ ī�带 ��ο�
        for (int i = 0; i < drawCount && deck.Count > 0; i++)
        {
            //������ �� ù ��° ī�带 ��ο�
            SkillCardData drawnCard = deck[0];
            //��ο��� ī��� ������ ����
            deck.RemoveAt(0);  
            //��ο��� ī�带 �п� ǥ��
            CreateHandCard(drawnCard);
        }
    }

    private void CreateHandCard(SkillCardData card)
    {
        GameObject cardGo = Instantiate(skillCardPrefab, skillCardParant);
        Card cardScript = cardGo.GetComponent<Card>();
        cardScript.InitCardData(card, State.Front, CardType.SkillCard);

        //�п� ī�� ��ġ (UI ó��)
        handCards.Add(cardGo);
    }
    #endregion
}