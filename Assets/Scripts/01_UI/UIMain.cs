using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [SerializeField] DeckGenerator deckGenerator;
    [SerializeField] GameObject createDeckPhase1, createDeckPhase2;

    [SerializeField] RectTransform hexParantRt /*map*/, battleFieldRt;
    [SerializeField] GameObject hexPrefab;  //�簢�� �ȿ� ������ �̹��� �ִ� UI ������

    private void Awake()
    {
        DataManager.GetInstance().LoadGamePlayData();
        DataManager.GetInstance().LoadCharacterCardData();
        DataManager.GetInstance().LoadSkillCardData();

        BackendManager.GetInstance().InitBackend();
    }

    private void Start()
    {
        GridManager.GetInstance().CreateHexGrid(battleFieldRt, hexPrefab, hexParantRt);
    }

    public void OnClickAddDeck()
    {
         //deckGenerator.CreateDeck();
    }
}