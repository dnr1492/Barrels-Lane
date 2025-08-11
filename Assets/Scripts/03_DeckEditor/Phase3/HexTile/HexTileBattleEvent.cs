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

    public void OnPointerClick(PointerEventData e)
    {
        var ctrl = ControllerRegister.Get<MovementOrderController>();
        if (ctrl == null) return;

        //(1) Ŭ���� Hex ��ǥ ���
        var (col, row) = hexTile.GridPosition;
        var hex = new Vector2Int(col, row);

        //(2) �ش� Ÿ�Ͽ� '�� ��ū'�� ������ ĳ���� Ŭ�� �̺�Ʈ ���� (Ÿ���� ���� �����ϰ� �׻� ����)
        if (hexTile.AssignedTokenKey.HasValue && hexTile.IsMyToken) {
            int tokenKey = hexTile.AssignedTokenKey.Value;
            ctrl.OnCharacterClicked(tokenKey, hex);
            return;
        }

        //(3) �� �ܿ��� ������ Ŭ�� �õ�
        ctrl.OnHexClicked(hex);
    }
}
