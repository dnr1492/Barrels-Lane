using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumClass;

public static class UISkillHexGridHelper
{
    private readonly static float spacingX = 1f;
    private readonly static float spacingY = 0.5f;
    private readonly static int visualOffset = 2;  //Hex ������ ��, �Ʒ� 2ĭ�� �� �߰�

    #region Skill HexGrid ����
    public static void CreateSkillHexGrid(RectTransform container, GameObject prefab, List<GameObject> hexList,
                                Dictionary<(int, int), GameObject> hexMap, float hexScale)
    {
        float baseHexWidth = 24f;
        float baseHexHeight = baseHexWidth * Mathf.Sqrt(3f) / 2f;

        float parentWidth = container.rect.width;
        float parentHeight = container.rect.height;

        float unitWidth = baseHexWidth * 0.75f + spacingX;
        float unitHeight = baseHexHeight + spacingY;

        int cols = Mathf.FloorToInt((parentWidth + spacingX) / unitWidth);
        int rows = Mathf.FloorToInt((parentHeight + spacingY) / unitHeight);
        int halfCols = cols / 2;
        int halfRows = rows / 2;

        float hexWidth = (parentWidth - spacingX * (cols - 1)) / (cols * 0.75f + 0.25f);
        float hexHeight = hexWidth * Mathf.Sqrt(3f) / 2f;

        hexWidth *= hexScale;
        hexHeight *= hexScale;

        for (int dq = -halfCols; dq <= halfCols; dq++)
        {
            for (int dr = -(halfRows + visualOffset); dr <= (halfRows + visualOffset); dr++)
            {
                var hex = Object.Instantiate(prefab, container);
                hex.GetComponent<HexTile>().ShowDecorations(false);
                hexList.Add(hex);
                hexMap[(dq, dr)] = hex;

                var rt = hex.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(hexWidth, hexHeight);

                float x = dq * (hexWidth * 0.75f + spacingX);
                float y = dr * (hexHeight + spacingY) + (dq % 2 != 0 ? (hexHeight + spacingY) / 2f : 0f);
                rt.anchoredPosition = new Vector2(x, -y);
            }
        }
    }
    #endregion

    #region Skill HexGrid �ʱ�ȭ
    public static void ClearSkillHexGrid(List<GameObject> hexList, Dictionary<(int, int), GameObject> hexMap)
    {
        foreach (var hex in hexList)
            Object.Destroy(hex);
        hexList.Clear();
        hexMap.Clear();
    }
    #endregion

    #region Skill HexGrid�� ��ų ������ ǥ��
    public static void ShowSkillHexRange(SkillCardData data, Dictionary<(int, int), GameObject> hexMap)
    {
        List<(int dq, int dr, Color color)> range = new();

        switch (data.rangeType)
        {
            case SkillRangeType.LineForward1:
                range.AddRange(SkillRangeHelper.GetLine((0, -1), 1, Color.red));
                break;
            case SkillRangeType.LineForward2:
                range.AddRange(SkillRangeHelper.GetLine((0, -1), 2, Color.red));
                break;
            case SkillRangeType.LineForward3:
                range.AddRange(SkillRangeHelper.GetLine((0, -1), 3, Color.red));
                break;
            case SkillRangeType.Ring1:
                range.AddRange(SkillRangeHelper.GetRing(1, Color.green));  //1ĭ, ring�� �ʷ�
                break;
            case SkillRangeType.Custom:
                range = data.customOffsetRange;
                break;
        }

        //���� ��ü ���� ǥ�� (�߽� ����)
        foreach (var (dq, dr, color) in range)
        {
            if (dq == 0 && dr == 0) continue;
            if (hexMap.TryGetValue((dq, dr), out var hex))
                hex.GetComponent<HexTile>().SetColor(color);
        }

        //�߽��� �׻� ȸ��
        if (hexMap.TryGetValue((0, 0), out var centerHex))
            centerHex.GetComponent<HexTile>().SetColor(Color.gray);
    }
    #endregion

    // ============== ����(Ring1)�� ��� ����� ������ �ȵǴ� ����... ================= //
}
