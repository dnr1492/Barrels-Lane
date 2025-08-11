using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexTileBattleEvent : MonoBehaviour, IPointerClickHandler
{
    private HexTile hexTile;

    private void Awake()
    {
        hexTile = GetComponent<HexTile>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var movementOrderCtrl = ControllerRegister.Get<MovementOrderController>();
        if (movementOrderCtrl == null || !movementOrderCtrl.IsTargeting || hexTile == null) return;

        //Ŭ���� Hex�� ��ǥ
        var (col, row) = hexTile.GridPosition;
        var hex = new Vector2Int(col, row);

        //ĳ���Ͱ� ������ ĳ���� ����
        if (hexTile.AssignedTokenKey.HasValue) movementOrderCtrl.OnCharacterClicked(hexTile.AssignedTokenKey.Value, hex);
        //ĳ���Ͱ� ������ Hex ���� (������)
        else movementOrderCtrl.OnHexClicked(hex);
    }
}
