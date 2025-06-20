using Coffee.UISoftMask;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private readonly Dictionary<(int col, int row), Image> imgCharacterMap = new();  //��ǥ�� �ش� ��ǥ�� Image ����
    private readonly Dictionary<char, List<(int col, int row)>> tierSlots = new();  //Ƽ��� �ش� ��ǥ ����
    private readonly Dictionary<int, (int col, int row)> tokenPosMap = new();  //��ū�� ���� Key(id)�� ��ǥ�� ����
    private readonly Dictionary<(int col, int row), HexTile> hexTileMap = new();  //��ǥ�� �ش� ��ǥ�� HexTile ����

    #region �ʵ� ����
    public void CreateHexGrid(RectTransform battleFieldRt, GameObject hexPrefab, RectTransform parant)
    {
        ResizeHexGrid(battleFieldRt);

        float gridWidth = (columns - 1) * hexWidth * 0.75f + hexWidth;
        float gridHeight = rows * hexHeight + (columns > 1 ? hexHeight / 2f : 0);

        Vector2 startOffset = new(
            -gridWidth / 2f + hexWidth / 2f,
            gridHeight / 2f - hexHeight / 2f
        );

        List<RectTransform> hexTransforms = new();

        //�׸��带 �����ϸ鼭 RectTransform�� ����Ʈ�� ����
        for (int col = 0; col < columns; col++)
        {
            int maxRow = rows - ((col % 2 == 1) ? 1 : 0);

            for (int row = 0; row < maxRow; row++)
            {
                GameObject hex = Instantiate(hexPrefab, parant);
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
                }

                var hexTile = hex.GetComponent<HexTile>();
                hexTile.Init((col, row));
                hexTileMap[(col, row)] = hexTile;

                //��ǥ�� �̹��� ���� ���
                var image = hexTile.transform.Find("mask").transform.Find("imgCharacter").GetComponent<Image>();
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
    #endregion

    #region �ʵ� ���� ������ ĳ���� ��ū ǥ�� �� ǥ�� ����
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
            PlaceToken(slots[0], token.Key, token.GetCharacterSprite());
            return;
        }

        //�� ĭ ã��
        foreach (var pos in slots)
        {
            if (imgCharacterMap[pos].sprite == null)
            {
                PlaceToken(pos, token.Key, token.GetCharacterSprite());
                return;
            }
        }

        //�� �� ��� �� ù ĭ �����
        PlaceToken(slots[0], token.Key, token.GetCharacterSprite());
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

    private void PlaceToken((int col, int row) pos, int tokenKey, Sprite sprite)
    {
        if (!hexTileMap.TryGetValue(pos, out var hexTile)) return;

        //���� ��ū ����
        foreach (var kvp in tokenPosMap.ToList())
        {
            if (kvp.Value == pos)
            {
                tokenPosMap.Remove(kvp.Key);
                hexTile.ClearToken();
                break;
            }
        }

        //�̹��� ����
        imgCharacterMap[pos].sprite = sprite;
        imgCharacterMap[pos].enabled = true;

        //HexTile�� �ش� ��ū Key�� ����
        hexTile.AssignToken(tokenKey);

        //��ġ ���
        tokenPosMap[tokenKey] = pos;
    }

    private void ClearToken((int col, int row) pos)
    {
        if (!hexTileMap.TryGetValue(pos, out var hexTile)) return;

        var image = imgCharacterMap[pos];
        image.sprite = null;
        image.enabled = false;

        hexTile.ClearToken();
    }
    #endregion

    /// <summary>
    /// (����(Hex)�� ���콺 ����) �Ÿ� ��� Ž��
    /// </summary>
    /// <param name="screenPos"></param>
    /// <param name="nearestSlot"></param>
    /// <returns></returns>
    public bool TryGetNearestSlot(Vector2 screenPos, out (int col, int row) nearestSlot)
    {
        nearestSlot = default;
        float minDistance = float.MaxValue;
        bool found = false;

        foreach (var kvp in imgCharacterMap)
        {
            var rt = kvp.Value.rectTransform;
            Vector2 screenWorldPos = rt.position;
            float distance = Vector2.Distance(screenWorldPos, screenPos);

            if (distance < minDistance) {
                minDistance = distance;
                nearestSlot = kvp.Key;
                found = true;
            }
        }

        return found && minDistance <= 100f;
    }

    /// <summary>
    /// ��ū�� �̵�
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="tile"></param>
    public void MoveToken((int col, int row) from, (int col, int row) to, HexTile tile)
    {
        if (from == to || !tile.AssignedTokenKey.HasValue) return;

        int fromKey = tile.AssignedTokenKey.Value;
        if (!txtMap.TryGetValue(to, out string dropSlotType)) return;

        char dropSlotChar = dropSlotType[0];
        var fromToken = ControllerRegister.Get<CharacterTokenController>().GetAllCharacterToken()
            .FirstOrDefault(t => t.Key == fromKey);

        if (fromToken == null || fromToken.Tier.ToString()[0] != dropSlotChar)
        {
            //��� ��ġ�� ������ Ƽ���� ������ �ƴϸ� ���� (���纻�� ����)
            return;
        }

        var fromImg = imgCharacterMap[from];
        var toImg = imgCharacterMap[to];
        var fromTile = hexTileMap[from];
        var toTile = hexTileMap[to];

        //�� ĭ���� �̵�
        if (!toTile.AssignedTokenKey.HasValue)
        {
            tokenPosMap[fromKey] = to;
            fromTile.ClearToken();
            toTile.AssignToken(fromKey);

            toImg.sprite = fromImg.sprite;
            toImg.enabled = true;
            fromImg.sprite = null;
            fromImg.enabled = false;
            return;
        }

        int toKey = toTile.AssignedTokenKey.Value;
        var toToken = ControllerRegister.Get<CharacterTokenController>().GetAllCharacterToken()
            .FirstOrDefault(t => t.Key == toKey);

        if (fromToken.Tier == toToken?.Tier)
        {
            tokenPosMap[fromKey] = to;
            tokenPosMap[toKey] = from;

            fromTile.AssignToken(toKey);
            toTile.AssignToken(fromKey);

            (fromImg.sprite, toImg.sprite) = (toImg.sprite, fromImg.sprite);
            fromImg.enabled = fromImg.sprite != null;
            toImg.enabled = toImg.sprite != null;
        }
        else
        {
            tile.transform.SetParent(fromTile.transform, false);
            tile.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    /// <summary>
    /// TokenPack���� ����
    /// </summary>
    /// <returns></returns>
    public TokenPack GetTokenPack()
    {
        var tokenPack = new TokenPack();

        foreach (var kvp in tokenPosMap)
        {
            var key = kvp.Key;
            var (col, row) = kvp.Value;
            tokenPack.tokenSlots.Add(new TokenSlotData { tokenKey = key, col = col, row = row });
        }

        return tokenPack;
    }

    #region (�ӽ÷� ���ÿ���) TokenPack �ε�
    public void LoadTokenPack()
    {
        TokenPack tokenPack = Load();

        var allTokens = ControllerRegister.Get<CharacterTokenController>().GetAllCharacterToken();

        foreach (var slot in tokenPack.tokenSlots)
        {
            var token = allTokens.FirstOrDefault(t => t.Key == slot.tokenKey);
            if (token == null) continue;

            PlaceToken((slot.col, slot.row), token.Key, token.GetCharacterSprite());  //��ġ �����Ͽ� ���� ��ġ
            token.Select();  //���� ���µ� ����
        }
    }

    private TokenPack Load()
    {
        string savePath = Application.persistentDataPath + "/token_pack.json";

        if (!System.IO.File.Exists(savePath))
        {
            Debug.LogWarning("����� TokenPack ����");
            return null;
        }

        string json = System.IO.File.ReadAllText(savePath);
        TokenPack tokenPack = JsonUtility.FromJson<TokenPack>(json);
        return tokenPack;
    }
    #endregion

    /// <summary>
    /// (�ӽ÷� ���ÿ�) TokenPack ����
    /// </summary>
    /// <param name="tokenPack"></param>
    public void SaveTokenPack(TokenPack tokenPack)
    {
        string savePath = Application.persistentDataPath + "/token_pack.json";
        string json = JsonUtility.ToJson(tokenPack, true);
        System.IO.File.WriteAllText(savePath, json);
        Debug.Log($"TokenPack ���� �Ϸ�: {savePath}");
    }

    /// <summary>
    /// UIDeckPhase2 Popup �ʱ�ȭ
    /// </summary>
    public void ResetUIDeckPhase2()
    {
        //��� ��ū �̹��� ����
        foreach (var img in imgCharacterMap.Values) {
            img.sprite = null;
            img.enabled = false;
        }

        //��� HexTile �ʱ�ȭ
        foreach (var tile in hexTileMap.Values)
            tile.ClearToken();

        //��ġ ��� ����
        tokenPosMap.Clear();

        //���� ����
        var tokens = ControllerRegister.Get<CharacterTokenController>().GetAllCharacterToken();
        foreach (var token in tokens)
            if (token.IsSelect) token.Unselect();

        //���� �ʱ�ȭ
        ControllerRegister.Get<FilterController>().ResetFilter();
    }
}