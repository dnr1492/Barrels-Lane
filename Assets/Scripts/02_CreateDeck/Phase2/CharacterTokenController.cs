using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static EnumClass;

public class CharacterTokenController : MonoBehaviour
{
    [SerializeField] RectTransform contentRt;

    private GridLayoutGroup gridLayoutGroup;
    private CharacterToken[] arrAllCharacterToken;

    private readonly int columnCount = 4;
    private readonly float aspectRatio = 1.136f;  //100(����)/88(����)
    private readonly float safeMarginRatio = 0.97f;  //������ ���� 97%�� ���

    private void Awake()
    {
        gridLayoutGroup = contentRt.GetComponent<GridLayoutGroup>();

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

    public void OnClickToken(CharacterToken clickedToken, CharacterCard characterCard)
    {
        //Ȯ����(Confirm) ��ū�� ������ ���
        if (clickedToken.State == CharacterTokenState.Confirm)
        {
            foreach (var token in GetAllCharacterToken())
            {
                if (token != clickedToken && token.State == CharacterTokenState.Select)
                    token.SetTokenState(CharacterTokenState.Cancel);
            }
            return;
        }

        //������(Select) ��ū�� �ٽ� ������ ���
        if (clickedToken.State == CharacterTokenState.Select)
        {
            clickedToken.SetTokenState(CharacterTokenState.Cancel);

            //ĳ���� ī�� �޸����� ����
            characterCard.SetCardState(CardState.Back);
            return;
        }

        //������(Select) ��ū �ܿ� �ٸ� ��ū�� ������(Select) ���
        foreach (var token in GetAllCharacterToken())
        {
            if (token.State == CharacterTokenState.Select)
                token.SetTokenState(CharacterTokenState.Cancel);
        }

        clickedToken.SetTokenState(CharacterTokenState.Select);
    }

    public void OnClickConfirm(CharacterToken clickedToken)
    {
        //Confirm ���¿��� �ٽ� ������ Cancel
        if (clickedToken.State == CharacterTokenState.Confirm)
        {
            clickedToken.SetTokenState(CharacterTokenState.Select);

            //������ ĳ���� ��ū�� ��Ʋ�ʵ忡 ǥ�� ����
            GridManager.Instance.RemoveTokenFromBattlefield(clickedToken);
            return;
        }

        if (clickedToken.State != CharacterTokenState.Select) return;

        //Captain�� ���� �ϳ��� Confirm ����
        if (clickedToken.Tier == CharacterTierAndCost.Captain)
        {
            foreach (var t in GetAllCharacterToken())
            {
                if (t.Tier == CharacterTierAndCost.Captain && t.State == CharacterTokenState.Confirm)
                    t.SetTokenState(CharacterTokenState.Cancel);
            }
        }

        clickedToken.SetTokenState(CharacterTokenState.Confirm);

        //������ ĳ���� ��ū�� ��Ʋ�ʵ忡 ǥ��
        GridManager.Instance.DisplayTokenOnBattlefield(clickedToken);
    }
}
