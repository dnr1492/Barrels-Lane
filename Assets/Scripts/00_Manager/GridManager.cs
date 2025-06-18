using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : Singleton<GridManager>
{
    private float hexWidth;  //�������� ���� ũ��, �ش� �������� �⺻ ũ�� 70
    private float hexHeight;  //�������� ���� ũ�� (�������� �������� ��3/2 * �ʺ�), �ش� �������� �⺻ ũ�� 60

    private readonly int rows = 8;  //�� (����)
    private readonly int columns = 9;  //�� (����)

    private readonly Dictionary<(int col, int row), string> txtMap = new()
    {
        //��, ��
        { (8, 7), "H" }, { (8, 6), "L" },
        { (7, 6), "M" },
        { (6, 7), "H" }, { (6, 6), "L" },
        { (5, 6), "M" },
        { (4, 7), "C" }, { (4, 6), "L" },
        { (3, 6), "M" },
        { (2, 7), "H" }, { (2, 6), "L" },
        { (1, 6), "M" },
        { (0, 7), "H" }, { (0, 6), "L" },
    };

    private readonly Dictionary<(int col, int row), Image> imgCharacterMap = new();
    private readonly Dictionary<char, List<(int col, int row)>> tierSlots = new();
    private readonly Dictionary<int, (int col, int row)> tokenPosMap = new();  //��ū�� ���� Key(id)�� ��ǥ�� ����

    public void CreateHexGrid(RectTransform battleFieldRt, GameObject hexPrefab, RectTransform parant)
    {
        ResizeHexGrid(battleFieldRt);

        float gridWidth = (columns - 1) * hexWidth * 0.75f + hexWidth;
        float gridHeight = rows * hexHeight + (columns > 1 ? hexHeight / 2f : 0);

        Vector2 startOffset = new Vector2(
            -gridWidth / 2f + hexWidth / 2f,
            gridHeight / 2f - hexHeight / 2f
        );

        List<RectTransform> hexTransforms = new List<RectTransform>();

        //�׸��带 �����ϸ鼭 RectTransform�� ����Ʈ�� ����
        for (int col = 0; col < columns; col++)
        {
            int maxRow = rows - ((col % 2 == 1) ? 1 : 0);

            for (int row = 0; row < maxRow; row++)
            {
                GameObject hex = Object.Instantiate(hexPrefab, parant);
                RectTransform rt = hex.GetComponent<RectTransform>();
                hexTransforms.Add(rt);

                var widthScaleOffset = 2 * battleFieldRt.localScale.x;
                var heightScaleOffset = 0.25f * battleFieldRt.localScale.y;
                rt.sizeDelta = new Vector2(hexWidth - widthScaleOffset, hexHeight - heightScaleOffset);

                float x = col * hexWidth * 0.75f;
                float y = row * hexHeight + ((col % 2 == 1) ? hexHeight / 2f : 0f);

                rt.anchoredPosition = new Vector2(x + startOffset.x, -(y - startOffset.y));

                if (txtMap.TryGetValue((col, row), out string textValue))
                {
                    var textComponent = hex.GetComponentInChildren<TextMeshProUGUI>();
                    if (textComponent != null) textComponent.text = textValue;
                    Debug.Log($"�� {col}, �� {row} : {textValue}");
                }

                //1. Hex�� Mask �߰�
                //2. Hex ������ ĳ���Ϳ� �̹����� ����
                //3. ��ǥ�� �̹��� ���� ���
                hex.AddComponent<Mask>();
                GameObject imgCharacter = new GameObject("imgCharacter", typeof(RectTransform), typeof(Image));
                imgCharacter.transform.SetParent(hex.transform, false);
                imgCharacter.GetComponent<RectTransform>().sizeDelta = rt.sizeDelta;
                var image = imgCharacter.GetComponent<Image>();
                image.enabled = false;
                imgCharacterMap[(col, row)] = image;
            }
        }

        FitHexGrid(hexTransforms, parant);
        BuildTierSlotTable();  //�������� ȣ��
    }

    private void ResizeHexGrid(RectTransform battleFieldRt)
    {
        float availableWidth = battleFieldRt.rect.width;
        float availableHeight = battleFieldRt.rect.height;

        //���� hex ũ�� (���� �ʱⰪ)
        float baseHexWidth = 72f;
        float baseHexHeight = baseHexWidth * Mathf.Sqrt(3) / 2f;  //�������� ���� ����

        //Grid ��ü �ʺ�/���̸� �ʱ� ũ��� ���
        float rawTotalWidth = (columns - 1) * baseHexWidth * 0.75f + baseHexWidth;
        float rawTotalHeight = rows * baseHexHeight + (columns > 1 ? baseHexHeight / 2f : 0);

        //���� ũ�⿡ �°� ������ ����
        float scaleX = availableWidth / rawTotalWidth;
        float scaleY = availableHeight / rawTotalHeight;
        float scaleFactor = Mathf.Min(scaleX, scaleY);  //�ʹ� Ŀ���� �ʵ��� ���� �� ����

        //���� ���� hex ũ��
        hexWidth = baseHexWidth * scaleFactor;
        hexHeight = baseHexHeight * scaleFactor;
    }

    private void FitHexGrid(List<RectTransform> hexTransforms, RectTransform parant)
    {
        //��� Hex ������Ʈ�� �߾Ӱ��� ���ϱ�
        Vector2 totalCenter = Vector2.zero;
        foreach (var rt in hexTransforms) totalCenter += rt.anchoredPosition;
        totalCenter /= hexTransforms.Count;

        //�θ� RectTransform�� �߾ӿ� ���߱�
        Vector2 parentCenter = parant.rect.center;
        Vector2 offset = parentCenter - totalCenter;

        //�׸����� �� ��ġ�� �߾ӿ� �°� ����
        foreach (var rt in hexTransforms) rt.anchoredPosition += offset;
    }

    // ================================================================================================================ //

    public void DisplayCharacterTokenOnBattlefield(CharacterToken token)
    {
        //��ū Tier ���� ���� (H, L, C ...)
        char tier = token.Tier.ToString()[0];

        //�̹� ��ġ�Ǿ� �ִ� Token�̸� ��ū ����
        if (tokenPosMap.TryGetValue(token.Key, out var curPos))
        {
            ClearToken(curPos);
            tokenPosMap.Remove(token.Key);
            Debug.Log($"��ū ���� ��ǥ: {curPos} Tier: {tier}");
            return;
        }

        //��ġ�� ���� ����Ʈ
        var slots = tierSlots[tier];

        //������ ������ ù ĭ
        if (tier == 'C')
        {
            PlaceToken(slots[0], token);
            return;
        }

        //�� ĭ ã��
        foreach (var pos in slots)
        {
            if (imgCharacterMap[pos].sprite == null)
            {
                PlaceToken(pos, token);
                return;
            }
        }

        //�� �� ��� �� ù ĭ �����
        PlaceToken(slots[0], token);
    }

    private void BuildTierSlotTable()
    {
        tierSlots.Clear();

        //Tier ���ڿ� �ش��ϴ� ��ǥ ã��
        foreach (var kvp in txtMap)
        {
            //��ū Tier ���� ���� (H, L, C ...)
            char tier = kvp.Value[0];
            if (!tierSlots.TryGetValue(tier, out var list))
            {
                list = new List<(int, int)>();
                tierSlots[tier] = list;
            }

            list.Add(kvp.Key);
        }

        //���� �� ������, ���� ���̸� �Ʒ���(�� ū ��) �켱
        foreach (var list in tierSlots.Values)
            list.Sort((a, b) =>
                a.col != b.col ? a.col.CompareTo(b.col)
                               : b.row.CompareTo(a.row));
    }

    private void PlaceToken((int col, int row) pos, CharacterToken token)
    {
        var img = imgCharacterMap[pos];

        //�ٸ� ��ū�� �̹� �� ĭ�� ���� �־��ٸ� ��� ����
        foreach (var kvp in tokenPosMap)
            if (kvp.Value == pos)
            {
                tokenPosMap.Remove(kvp.Key);
                break;
            }

        //�ش� ��ǥ�� �̹����� ĳ���� �̹��� �Ҵ�
        img.sprite = token.GetCharacterSprite();
        img.enabled = true;
        tokenPosMap[token.Key] = pos;
        Debug.Log($"��ū ��ġ ��ǥ: {tokenPosMap[token.Key]} Tier: {token.Tier}");
    }

    private void ClearToken((int col, int row) pos)
    {
        var img = imgCharacterMap[pos];
        img.sprite = null;
        img.enabled = false;
    }
}