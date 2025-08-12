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
    public Action<SkillCardEvent> onSkillCardClick;

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
        foreach (var slot in allRoundSlots)
        {
            if (slot == null) continue;

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

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(e, results);

        SkillCardRoundSlot roundSlot = null;
        foreach (var r in results)
        {
            roundSlot = r.gameObject.GetComponentInParent<SkillCardRoundSlot>();
            if (roundSlot != null) break;
        }

        if (roundSlot != null)
        {
            SwapSkillCard(roundSlot);
            onDropToRoundSlot?.Invoke(this, roundSlot);
        }
        else
        {
            bool droppedOnSkillZone = results.Any(r => r.gameObject.GetComponentInParent<SkillCardZone>() != null);
            if (droppedOnSkillZone && skillCardZoneParent != null)
            {
                transform.SetParent(skillCardZoneParent, false);
                skillCardRt.anchoredPosition = Vector2.zero;

                //RoundZone �� CardZone �̵� ��, ��� ���� ������/����/�̹��� ����
                if (dragSlot != null)
                {
                    var moveCtrl = ControllerRegister.Get<MovementOrderController>();
                    moveCtrl.ReleaseReservation(dragSlot);
                    var selfCard = GetComponent<SkillCard>();
                    if (selfCard != null) selfCard.ResetImageIfMoveCard();
                }

                if (dragSlot != null) dragSlot.Clear();

                refreshSkillCardZoneLayout?.Invoke();
            }
            else
            {
                transform.SetParent(originalParent, false);
                skillCardRt.anchoredPosition = originalAnchoredPos;

                if (originalParent == skillCardZoneParent)
                    refreshSkillCardZoneLayout?.Invoke();
            }
        }

        foreach (var slot in allRoundSlots)
        {
            slot.HideHighlight();
            if (slot.AssignedSkillCardData != null && slot.GetComponentInChildren<SkillCardEvent>() == null)
                slot.Clear();
        }
    }

    private void SwapSkillCard(SkillCardRoundSlot dropSlot)
    {
        var moveCtrl = ControllerRegister.Get<MovementOrderController>();

        if (!dropSlot.IsEmpty)
        {
            var existingCard = dropSlot.GetComponentInChildren<SkillCardEvent>();
            if (existingCard != null)
            {
                if (dragSlot != null)
                {
                    dragSlot.Assign(existingCard.SkillCardData, existingCard);
                    moveCtrl.SwapOrders(dragSlot, dropSlot);
                }
                else
                {
                    //CardZone �� RoundSlot (����) ���
                    existingCard.transform.SetParent(skillCardZoneParent, false);
                    var exRt = (RectTransform)existingCard.transform;
                    exRt.anchoredPosition = Vector2.zero;

                    if (moveCtrl != null) moveCtrl.ReleaseReservation(dropSlot);
                    var existingSkillCard = existingCard.GetComponent<SkillCard>();
                    if (existingSkillCard != null) existingSkillCard.ResetImageIfMoveCard();

                    refreshSkillCardZoneLayout?.Invoke();
                }
            }
        }
        else
        {
            //�� �������� �̵�
            if (dragSlot != null)
            {
                moveCtrl.TransferOrder(dragSlot, dropSlot);
            }
        }
    }

    public void OnPointerClick(PointerEventData e)
    {
        onSkillCardClick?.Invoke(this);
    }
}
