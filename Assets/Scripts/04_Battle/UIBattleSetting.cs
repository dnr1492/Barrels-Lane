using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleSetting : UIPopupBase
{
    [SerializeField] GameObject uiCoinFlipPrefab;
    [SerializeField] Button btn_back;

    private Canvas rootCanvas;
    private UICoinFlip uiCoinFlip;

    [Header("Hex Grid")]
    [SerializeField] RectTransform hexParantRt /*map*/, battleFieldRt;
    [SerializeField] GameObject hexPrefab;  //������ ����� �̹����� �ִ� UI ������

    [Header("Setting SkillCardZone")]
    [SerializeField] Transform skillCardZone;  //SkillCard�� Parant
    [SerializeField] GameObject skillCardPrefab;
    private readonly List<SkillCard> settingSkillCards = new();
    private readonly float skillCardWidth = 130f;
    private readonly float horizontalPadding = 10f;
    private readonly float minVisiblePixelsWhenOverlapping = 10f;

    [Header("Setting SkillCardRoundZone")]
    [SerializeField] SkillCardRoundSlot[] roundSlots = new SkillCardRoundSlot[4];  //SkillCardRoundSlot 4ĭ ����

    public void Init()
    {
        rootCanvas = GetComponentInParent<Canvas>();

        GridManager.Instance.CreateHexGrid(battleFieldRt, hexPrefab, hexParantRt, false, true);

        UIEditorDeckPhase1 popup = UIManager.Instance.GetPopup<UIEditorDeckPhase1>("UIEditorDeckPhase1");
        var pack = popup.GetSelectedDeckPack();
        if (pack != null) GridManager.Instance.ShowDecksOnField(pack, ControllerRegister.Get<PhotonController>().OpponentDeckPack);

        var go = Instantiate(uiCoinFlipPrefab, transform);
        uiCoinFlip = go.GetComponent<UICoinFlip>();
        uiCoinFlip.Init(OnCoinDirectionSelected);
    }

    private void OnCoinDirectionSelected(int myCoinDriection)
    {
        ControllerRegister.Get<PhotonController>().RequestCoinFlip(myCoinDriection);
    }

    public void ShowCoinFlipResult(int result, bool hasFirstTurnChoice)
    {
        uiCoinFlip.PlayFlipAnimation(result, () => {
            if (hasFirstTurnChoice) {
                uiCoinFlip.ActiveTurnChoiceButton(true);  //���� or �İ� ���� ��ư ǥ��
                uiCoinFlip.Invoke(nameof(UICoinFlip.AutoSelectTurnOrder), 3f);
            }
            else {
                LoadingManager.Instance.Show("��밡 ���� �Ǵ� �İ��� �����ϴ� ���Դϴ�...");
                AIBattleHelper.AutoSelectTurnOrderAI(uiCoinFlip);
            }
        });
    }

    public void DestroyUICoinFlip()
    {
        if (uiCoinFlip != null) {
            Destroy(uiCoinFlip.gameObject);
            uiCoinFlip = null;
        }
    }

    #region ��ο��� ��ųī�带 CardZone�� ����
    public void SetDrawnSkillCard(List<SkillCardData> drawnSkillCards)
    {
        //���� ī�� ����
        foreach (var card in settingSkillCards) Destroy(card.gameObject);
        settingSkillCards.Clear();

        //���� ����
        foreach (var skillCardData in drawnSkillCards)
        {
            //SkillCardZone�� ����
            var go = Instantiate(skillCardPrefab, skillCardZone);
            var sprite = SpriteManager.Instance.dicSkillSprite.TryGetValue(skillCardData.name, out var sp) ? sp : null;
            var skillCard = go.GetComponent<SkillCard>();
            skillCard.Set(sprite, skillCardData);
            settingSkillCards.Add(skillCard);

            //�巡�� ���� + ��� �ݹ�
            var drag = go.GetComponent<SkillCardEvent>();
            drag.Set(skillCardData, rootCanvas, roundSlots, skillCardZone, RefreshSkillCardZoneLayout);
            drag.onDropToRoundSlot = OnDropToRoundSlot;  //���Կ� ����� �������� �� ó��
        }

        RefreshSkillCardZoneLayout();
        AdjustSkillCardZoonHeight(10, 10);

        Debug.Log($"[UIBattleSetting] ��ο� ī�� {settingSkillCards.Count}�� UI�� ���� �Ϸ�");
    }

    /// <summary>
    /// CardZone ���ΰ�ħ
    /// </summary>
    public void RefreshSkillCardZoneLayout()
    {
        SetSkillCardZoneLayout(settingSkillCards);
    }

    /// <summary>
    /// ��ųī�带 skillCardZone �ȿ��� ��� ���ĵǵ��� ��ġ
    /// ī�� ������ �������� ��������, Zone �ʺ� ����� �ʵ��� spacing�� �ڵ� ���
    /// ī�� �ε����� �������� ȭ�鿡�� ���� ���̵��� SiblingIndex()�� ���� ����
    /// </summary>
    /// <param name="cards"></param>
    private void SetSkillCardZoneLayout(List<SkillCard> cards)
    {
        var cardsInZone = cards
            .Where(c => c != null && c.transform.parent == skillCardZone)
            .ToList();
        int count = cardsInZone.Count;
        if (count == 0) return;

        RectTransform zoneRt = skillCardZone.GetComponent<RectTransform>();
        float availableWidth = zoneRt.rect.width - horizontalPadding * 2;

        //spacing�� �����ؼ� ī����� ���� �ȿ� �� �µ��� �ϱ�
        float spacing;

        if (count == 1) spacing = 0f;
        else
        {
            //��ü ī�� + ���� ���� availableWidth �ȿ� ������ spacing ���
            spacing = (availableWidth - (skillCardWidth * count)) / (count - 1);
            float minSpacing = -(skillCardWidth - minVisiblePixelsWhenOverlapping);
            spacing = Mathf.Clamp(spacing, minSpacing, skillCardWidth);  //������ ��ħ ����
        }

        float layoutWidth = (skillCardWidth * count) + (spacing * (count - 1));
        float startX = -layoutWidth / 2f + skillCardWidth / 2f;  //ù ī�� �߽��� ����

        for (int i = 0; i < count; i++)
        {
            var card = cardsInZone[i];
            var rt = card.GetComponent<RectTransform>();

            float x = startX + i * (skillCardWidth + spacing);
            rt.anchoredPosition = new Vector2(x, 0);

            //ī�� ���� ���� (���� �ε����� ��(ȭ�� ��)���� ������ ���� ��ġ)
            rt.SetSiblingIndex(count - 1 - i);
        }
    }

    /// <summary>
    /// ��� ��ųī�� �� ���� ���� ī�� �������� skillCardZone�� height�� �ڵ� ����
    /// ��, �Ʒ� ����(paddingTop, paddingBottom)�� �߰������� ����
    /// </summary>
    /// <param name="paddingTop">��� ����</param>
    /// <param name="paddingBottom">�ϴ� ����</param>
    private void AdjustSkillCardZoonHeight(float paddingTop, float paddingBottom)
    {
        float maxHeight = settingSkillCards.Max(card =>
            card.GetComponent<RectTransform>().rect.height);

        float finalHeight = maxHeight + paddingTop + paddingBottom;

        var rt = skillCardZone.GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, finalHeight);
    }
    #endregion

    #region ��ο��� ��ųī�带 Drag & Drop���� RoundZone�� ����
    private void OnDropToRoundSlot(SkillCardEvent drag, SkillCardRoundSlot slot)
    {
        //������ �ڽ����� �̵� + ����
        slot.Assign(drag.SkillCardData, drag);

        //CardZone�� ������
        RefreshSkillCardZoneLayout();  
    }
    #endregion

    protected override void ResetUI() { }
}