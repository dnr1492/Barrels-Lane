using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class SkillCardEvent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Canvas rootCanvas;
    private SkillCardRoundSlot[] allRoundSlots;
    private Transform skillCardZoneParent;
    private Action refreshSkillCardZoneLayout;

    private RectTransform skillCardRt;
    private CanvasGroup canvasGroup;

    private Transform originalParent;
    private Vector2 originalAnchoredPos;
    private SkillCardRoundSlot dragSlot;  //�巡�� ���� �� ī�尡 �ִ� ���� (ī���� �����)

    public SkillCardData SkillCardData { get; private set; }

    public Action<SkillCardEvent, SkillCardRoundSlot> onDropToRoundSlot;

    public void Set(SkillCardData skillCardData, 
        Canvas rootCanvas, 
        SkillCardRoundSlot[] allRoundSlots,
        Transform skillCardZoneParent,
        Action refreshSkillCardZoneLayout)
    {
        this.rootCanvas = rootCanvas;
        this.allRoundSlots = allRoundSlots;
        this.skillCardZoneParent = skillCardZoneParent;
        this.refreshSkillCardZoneLayout = refreshSkillCardZoneLayout;

        skillCardRt = (RectTransform)transform;
        canvasGroup = GetComponent<CanvasGroup>();
        if (!canvasGroup) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        SkillCardData = skillCardData;
        
        //���̶���Ʈ ����
        foreach (var slot in this.allRoundSlots) slot.HideHighlight();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        originalParent = transform.parent;
        originalAnchoredPos = skillCardRt.anchoredPosition;
        canvasGroup.blocksRaycasts = false;  //������ ����� �޵��� ������ �̺�Ʈ�� ����
        transform.SetParent(rootCanvas.transform, true);  //�ֻ����� �÷��� ���콺 ����ٴϰ�
        dragSlot = originalParent ? originalParent.GetComponent<SkillCardRoundSlot>() : null;

        //���̶���Ʈ �ѱ�
        foreach (var slot in allRoundSlots) {
            if (slot == null) continue;

            if (!slot.CanAccept()) { 
                slot.HideHighlight(); 
                continue; 
            }

            if (slot.IsEmpty) slot.ShowEmptyHighlight();
            else slot.ShowSwapHighlight();
        }
    }

    public void OnDrag(PointerEventData e)
    {
        //���� ��ǥ�� ��ȯ
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)rootCanvas.transform, e.position, e.pressEventCamera, out var localPos);
        //��ųī�尡 ���콺 Ŀ���� ����ٴϰ�
        skillCardRt.localPosition = localPos;
    }

    public void OnEndDrag(PointerEventData e)
    {
        canvasGroup.blocksRaycasts = true;

        //����ĳ��Ʈ�� ���� ã�� (UI�� ����ĳ��Ʈ ���)
        //Raycast Target�� true�� ��� UI�� ã��
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(e, results);

        SkillCardRoundSlot roundSlot = null;
        foreach (var r in results)
        {
            roundSlot = r.gameObject.GetComponentInParent<SkillCardRoundSlot>();
            if (roundSlot != null) break;
        }

        //��ġ �Ǵ� ����
        if (roundSlot != null && roundSlot.CanAccept()) {
            SwapSkillCard(roundSlot);
            onDropToRoundSlot?.Invoke(this, roundSlot);
        }
        else {
            //RoundZone���� CardZone���� �̵�
            bool droppedOnSkillZone = results.Any(r => r.gameObject.GetComponentInParent<SkillCardZone>() != null);
            if (droppedOnSkillZone && skillCardZoneParent != null) {
                //���Կ��� ���� CardZone���� ��ȯ
                transform.SetParent(skillCardZoneParent, false);
                skillCardRt.anchoredPosition = Vector2.zero;

                //��� ���� ������ �ʱ�ȭ
                if (dragSlot != null) dragSlot.Clear();

                //���̾ƿ� �ٽ� ���
                refreshSkillCardZoneLayout?.Invoke();
            }
            else {
                //���� �� ����ġ
                transform.SetParent(originalParent, false);
                skillCardRt.anchoredPosition = originalAnchoredPos;
            }
        }

        foreach (var slot in allRoundSlots)
        {
            //���̶���Ʈ ����
            slot.HideHighlight();
            //�� ���� ������ �ʱ�ȭ
            if (slot.GetComponentInChildren<SkillCardEvent>() == null) slot.Clear();
        }
    }

    private void SwapSkillCard(SkillCardRoundSlot dropSlot)
    {
        //����Ϸ��� ����(roundSlot)�� �̹� ī�尡 ������ ���� ó��
        if (!dropSlot.IsEmpty)
        {
            var existingCard = dropSlot.GetComponentInChildren<SkillCardEvent>();
            //���Կ� ī�尡 �����Ѵٸ�
            if (existingCard != null)
            {
                if (dragSlot == null) return;

                //�巡�׸� ������ ���Կ� ������ �ٸ� ī�带 ��ġ�ϰ�,
                //�� ������ �����͵� ������ �ٸ� ī�� �����ͷ� ����ȭ
                dragSlot.Assign(existingCard.SkillCardData, existingCard);
            }
        }
    }

    // ================================ ���� �� ================================ //
    // ================================ ���� �� ================================ //
    // ================================ ���� �� ================================ //

    public Action<SkillCardEvent> onSkillCardClick;

    public void OnPointerClick(PointerEventData e)
    {
        Debug.Log("���� ��ųī�带 Ŭ���� ��� �̺�Ʈ");
        onSkillCardClick?.Invoke(this);
    }
}