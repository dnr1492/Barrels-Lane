using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FilterController : MonoBehaviour
{
    [SerializeField] FilterButton[] arrFilterButton;
    private HashSet<string> selectedJobKey = new();   //D, T, S
    private HashSet<string> selectedTierKey = new();  //C, H, M, L

    [SerializeField] GridLayoutGroup gridLayoutGroup;
    private int columns = 4;
    private int rows = 5;
    private float paddingLeft = 30f;    //�� ���� ����
    private float paddingRight = 30f;   //�� ������ ����
    private float paddingTop = 30f;     //�� ���� ����
    private float paddingBottom = 30f;  //�� �Ʒ��� ����

    private void Awake()
    {
        ControllerRegister.Register(this);
    }

    private void Start()
    {
        SetCharacterToken();

        for (int i = 0; i < arrFilterButton.Length; i++)
        {
            var btn = arrFilterButton[i].GetComponent<Button>();
            var filterBtn = arrFilterButton[i];
            if (btn != null) btn.onClick.AddListener(() => OnClickFilter(filterBtn));
        }

        ResizeFilterButtonCellSize();
    }

    private void OnClickFilter(FilterButton filterButton)
    {
        string key = filterButton.FilterKey;
        bool isJob = key is "D" or "T" or "S";
        bool isTier = key is "C" or "H" or "M" or "L";

        if (isJob) {
            if (selectedJobKey.Contains(key)) selectedJobKey.Clear();
            else {
                selectedJobKey.Clear();
                selectedJobKey.Add(key);
            }
        }
        else if (isTier) {
            if (selectedTierKey.Contains(key)) selectedTierKey.Clear();
            else {
                selectedTierKey.Clear();
                selectedTierKey.Add(key);
            }
        }

        //����, Ƽ�� ��ư ���¸� ������Ʈ
        foreach (var btn in arrFilterButton) {
            key = btn.FilterKey;
            if (key.Length == 1)
            {
                //���� ����
                if (key is "D" or "T" or "S") btn.SetSelected(selectedJobKey.Contains(key));
                //Ƽ�� ����
                else if (key is "C" or "H" or "M" or "L") btn.SetSelected(selectedTierKey.Contains(key));
            }
        }

        //��� ���� ��ư�� ���� ó��
        foreach (var btn in arrFilterButton) {
            if (!btn.FilterKey.Contains("/")) continue;

            string[] splitKey = btn.FilterKey.Split('/');
            if (splitKey.Length != 2) {
                btn.SetSelected(false);
                continue;
            }

            string tier = splitKey[0];
            string job = splitKey[1];

            if (selectedJobKey.Count > 0 && selectedTierKey.Count > 0) btn.SetSelected(selectedJobKey.Contains(job) && selectedTierKey.Contains(tier));
            else if (selectedJobKey.Count > 0) btn.SetSelected(selectedJobKey.Contains(job));
            else if (selectedTierKey.Count > 0) btn.SetSelected(selectedTierKey.Contains(tier));
            else btn.SetSelected(false);
        }

        ApplyCharacterFilter();
    }

    private void ApplyCharacterFilter()
    {
        var allCharacterCardDatas = DataManager.Instance.dicCharacterCardData;

        //���� ���ǿ� �´� ĳ���� ����Ʈ ����
        List<KeyValuePair<int, CharacterCardData>> filtered;

        //��ü
        if (selectedJobKey.Count == 0 && selectedTierKey.Count == 0) filtered = allCharacterCardDatas.ToList();
        //Ư��
        else {
            filtered = allCharacterCardDatas.Where(ch =>
            {
                var job = ch.Value.job;
                var tier = ch.Value.tier;

                bool isValidJob = !string.IsNullOrEmpty(job.ToString());
                bool isValidTier = !string.IsNullOrEmpty(tier.ToString());

                string jobKey = isValidJob ? job.ToString().Substring(0, 1).ToUpper() : null;
                string tierKey = isValidTier ? tier.ToString().Substring(0, 1).ToUpper() : null;

                return (selectedTierKey.Count == 0 || (tierKey != null && selectedTierKey.Contains(tierKey))) &&
                       (selectedJobKey.Count == 0 || (jobKey != null && selectedJobKey.Contains(jobKey)));
            }).ToList();
        }

        FindCharacterToken(filtered.ToDictionary(pair => pair.Key, pair => pair.Value));
    }

    private void ResizeFilterButtonCellSize()
    {
        var grid = gridLayoutGroup;
        var rect = grid.GetComponent<RectTransform>().rect;

        float availableWidth = rect.width - paddingLeft - paddingRight;
        float availableHeight = rect.height - paddingTop - paddingBottom;

        float cellWidth = availableWidth / columns;
        float cellHeight = availableHeight / rows;

        grid.cellSize = new Vector2(cellWidth, cellHeight);  //�� ũ�� ����
        grid.padding = new RectOffset((int)paddingLeft, (int)paddingRight, (int)paddingTop, (int)paddingBottom);  //�� ���� ����
    }

    private void SetCharacterToken()
    {
        var allCharacterCardDatas = DataManager.Instance.dicCharacterCardData;
        var arrCharacterToken = ControllerRegister.Get<CharacterTokenController>().GetAllCharacterToken();
        var dicCharacterEnlargeSprite = SpriteManager.Instance.dicCharacterEnlargeSprite;
        int tokenIndex = 0;

        //��� ĳ���� ��ū ��Ȱ��ȭ
        foreach (var token in arrCharacterToken) token.gameObject.SetActive(false);

        //��� ĳ���� ��ū ������ ����
        foreach (var characterCardData in allCharacterCardDatas)
        {
            var spriteKey = $"{characterCardData.Value.race}_{characterCardData.Value.job}_{characterCardData.Value.tier}_{characterCardData.Value.name}";
            arrCharacterToken[tokenIndex].Init(dicCharacterEnlargeSprite[spriteKey], characterCardData.Value);
            tokenIndex++;
        }
    }

    private void FindCharacterToken(Dictionary<int, CharacterCardData> targetCharacterCardData)
    {
        var arrCharacterToken = ControllerRegister.Get<CharacterTokenController>().GetAllCharacterToken();

        //��� ĳ���� ��ū ��Ȱ��ȭ
        foreach (var token in arrCharacterToken) token.gameObject.SetActive(false);

        //�ش� Filter�� �ش��ϴ� ĳ���� ��ū�� key�� ��� ĳ���� ����� key ��
        foreach (var key in targetCharacterCardData.Keys) {
            foreach (var token in arrCharacterToken) {
                if (token.Key == key) {
                    token.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}