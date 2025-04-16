using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [SerializeField] DeckGenerator deckGenerator;
    [SerializeField] GameObject createDeckPhase1, createDeckPhase2;

    [SerializeField] RectTransform mainRt, parantRt;
    [SerializeField] GameObject hexPrefab;  //�簢�� �ȿ� ������ �̹��� �ִ� UI ������

    private void Awake()
    {
        BackendManager.GetInstance().InitBackend();
    }

    private void Start()
    {
        GridManager.GetInstance().CreateHexGrid(mainRt, hexPrefab, parantRt);
    }

    public void OnClickAddDeck()
    {
         //deckGenerator.CreateDeck();
    }
}
