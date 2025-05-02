using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterTokenController : MonoBehaviour
{
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] RectTransform contentRt;

    private CharacterToken[] arrAllCharacterToken;

    private readonly int columnCount = 4;
    private readonly float aspectRatio = 1.136f;  //100(����)/88(����)
    private readonly float safeMarginRatio = 0.97f;  //������ ���� 97%�� ���

    private void Awake()
    {
        ControllerRegister.Register(this);
    }

    private void Start()
    {
        ResizeCharacterTokenCellSize();
    }

    private void ResizeCharacterTokenCellSize()
    {
        float totalWidth = contentRt.rect.width;
        float paddingHorizontal = gridLayoutGroup.padding.left + gridLayoutGroup.padding.right;
        float spacingHorizontal = gridLayoutGroup.spacing.x * (columnCount - 1);

        float availableWidth = (totalWidth - paddingHorizontal - spacingHorizontal) * safeMarginRatio;

        float cellWidth = availableWidth / columnCount;
        float cellHeight = cellWidth / aspectRatio;

        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
    }

    public CharacterToken[] GetAllCharacterToken()
    {
        if (arrAllCharacterToken == null) arrAllCharacterToken = gridLayoutGroup.GetComponentsInChildren<CharacterToken>(true);
        return arrAllCharacterToken;
    }

    public void OnClickToken(CharacterToken clickedToken, EnumClass.CharacterTierAndCost curTier)
    {
        //������ ���
        if (curTier == EnumClass.CharacterTierAndCost.Captain) {
            foreach (var token in arrAllCharacterToken) {
                //�ش� ĳ���� ��ū�� �����ϰ� �ٸ� ���� ���� ����
                if (token != clickedToken) token.Deselect();
            }
        }

        //ĳ���� ��ū Ȱ��ȭ
        clickedToken.Select();

        //ĳ���� ��ū�� ��Ʋ�ʵ忡 ǥ��
        GridManager.Instance.DisplayCharacterTokenOnBattlefield(clickedToken);
    }
}
