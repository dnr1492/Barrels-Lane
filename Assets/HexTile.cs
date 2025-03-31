using UnityEngine;
using UnityEngine.EventSystems;

public class HexTile : MonoBehaviour, IPointerClickHandler
{
    public Vector2Int gridPosition;  //Ÿ���� ��ǥ
    private HexGridManager gridManager;
    private RectTransform rt;

    public void Setup(Vector2Int pos, HexGridManager manager)
    {
        rt = GetComponent<RectTransform>();
        gridPosition = pos;
        gridManager = manager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gridManager.SelectTile(rt);  //Ÿ�� Ŭ�� �� �׸��� �Ŵ����� ����
    }
}