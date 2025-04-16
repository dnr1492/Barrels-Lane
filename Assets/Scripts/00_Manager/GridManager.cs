using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    private static GridManager instance = null;

    private GridManager() { }

    public static GridManager GetInstance()
    {
        if (instance == null) instance = new GridManager();
        return instance;
    }

    private int rows = 8;  //�� (����)
    private int columns = 9;  //�� (����)
    private float hexWidth = 102f;  //�������� ���� ũ��, �ش� �������� �⺻ ũ�� 100
    private float hexHeight = 88.5f;  //�������� ���� ũ�� (�������� �������� ��3/2 * �ʺ�), �ش� �������� �⺻ ũ�� 88

    private readonly Dictionary<(int col, int row), string> txtMap = new()
    {
        //��, ��
        { (8, 7), "H" }, { (8, 6), "L" },
        { (7, 6), "M" },
        { (6, 7), "H" }, { (6, 6), "L" },
        { (5, 6), "M" },
        { (4, 7), "R" }, { (4, 6), "L" },
        { (3, 6), "M" },
        { (2, 7), "H" }, { (2, 6), "L" },
        { (1, 6), "M" },
        { (0, 7), "H" }, { (0, 6), "L" },
    };

    public void CreateHexGrid(RectTransform mainRt, GameObject hexPrefab, RectTransform parant)
    {
        Resize(mainRt);

        for (int col = 0; col < columns; col++)
        {
            //Ȧ�� ���� �ϳ� ���δ�
            int maxRow = rows - ((col % 2 == 1) ? 1 : 0);

            for (int row = 0; row < maxRow; row++)
            {
                GameObject hex = Object.Instantiate(hexPrefab, parant);
                RectTransform rt = hex.GetComponent<RectTransform>();
                var widthOffset = 2 * mainRt.localScale.x;
                var heightOffset = 0.25f * mainRt.localScale.y;
                rt.sizeDelta = new Vector2(hexWidth - widthOffset, hexHeight - heightOffset);

                float x = col * hexWidth * 0.75f + 4;
                float y = row * hexHeight + ((col % 2 == 1) ? hexHeight / 2f : 0f) + 1;

                //UI y���� �Ʒ��� ������ �ϹǷ� ��ȣ �ݴ�
                rt.anchoredPosition = new Vector2(x, -y);

                //�ؽ�Ʈ �Է�
                if (txtMap.TryGetValue((col, row), out string textValue))
                {
                    var textComponent = hex.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                    if (textComponent != null) textComponent.text = textValue;
                    Debug.Log($"�� {col}, �� {row} : {textValue}");
                }
            }
        }
    }

    private void Resize(RectTransform mainRt)
    {
        Vector3 scale = mainRt.localScale;
        hexWidth = hexWidth * scale.x;
        hexHeight = hexHeight * scale.y;
        Debug.Log($"Scale: {scale}, Hex Size: {hexWidth} x {hexHeight}");
    }
}