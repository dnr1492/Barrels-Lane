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

    [SerializeField] RectTransform contentRt;

    private void Awake()
    {
        for (int i = 0; i < arrFilterButton.Length; i++)
        {
            var btn = arrFilterButton[i].GetComponent<Button>();
            var filterBtn = arrFilterButton[i];
            if (btn != null) btn.onClick.AddListener(() => OnClickFilter(filterBtn));
        }
    }

    private void Start()
    {
        ResizeFilterButtonCellSize();
        ResizeContent();
    }

    private void OnClickFilter(FilterButton filterButton)
    {
        string key = filterButton.FilterKey;

        bool isJob = key is "D" or "T" or "S";
        bool isTier = key is "C" or "H" or "M" or "L";

        if (isJob)
        {
            if (selectedJobKey.Contains(key)) selectedJobKey.Clear();
            else {
                selectedJobKey.Clear();
                selectedJobKey.Add(key);
            }
        }
        else if (isTier)
        {
            if (selectedTierKey.Contains(key)) selectedTierKey.Clear();
            else {
                selectedTierKey.Clear();
                selectedTierKey.Add(key);
            }
        }

        //����, Ƽ�� ��ư ���¸� ������Ʈ
        foreach (var btn in arrFilterButton)
        {
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
        foreach (var btn in arrFilterButton)
        {
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
        var allCharacterCards = DataManager.GetInstance().dicCharacterCardData;

        //���� ���ǿ� �´� ĳ���� ����Ʈ ����
        var filtered = allCharacterCards.Where(ch =>
        {
            var job = ch.Value.job;
            var tier = ch.Value.tier;

            bool isValidJob = !string.IsNullOrEmpty(job.ToString());
            bool isValidTier = !string.IsNullOrEmpty(tier.ToString());

            string jobKey = isValidJob ? job.ToString().Substring(0, 1).ToUpper() : null;
            string tierKey = isValidTier ? tier.ToString().Substring(0, 1).ToUpper() : null;

            //Ƽ��, ����
            return (selectedTierKey.Count == 0 || (tierKey != null && selectedTierKey.Contains(tierKey))) &&
                   (selectedJobKey.Count == 0 || (jobKey != null && selectedJobKey.Contains(jobKey)));
        }).ToList();

        //����� �α׷� �̸� ���
        foreach (var ch in filtered) {
            Debug.Log($"Matched Character - ID: {ch.Value.tier}{ch.Value.job}, Name: {ch.Value.name}");
        }

        // ============================================== ���� �� =============================================== //
        // ============================================== ���� �� =============================================== //
        // ============================================== ���� �� =============================================== //
        //ID�� ���ڿ��� ��ȯ�ؼ� ���� ��ȸ�� Set
        var resultSet = filtered.Select(ch => ch.Key.ToString()).ToHashSet();
        var dicCharacterCards = DataManager.GetInstance().dicCharacterCardData;
        foreach (var kvp in dicCharacterCards)
        {
            string characterKey = kvp.Key.ToString();

            //���� ����� ���Ե��� ������ continue
            if (!resultSet.Contains(characterKey)) continue;

            CharacterCardData cardData = kvp.Value;
            //���⼭ �ش� ī�� UI�� �����ͼ� ������Ʈ (Ȱ��ȭ, ������ ���ε� ��)
            Debug.Log($"ĳ���� �̸�: {cardData.name}");
            
        }
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

    // ============================================== ���� �� =============================================== //
    // ============================================== ���� �� =============================================== //
    // ============================================== ���� �� =============================================== //
    private void ResizeContent()
    {
        int itemCount = gridLayoutGroup.GetComponent<RectTransform>().childCount;

        int columnCount = gridLayoutGroup.constraintCount;
        int rowCount = Mathf.CeilToInt((float)itemCount / columnCount);

        float cellHeight = gridLayoutGroup.cellSize.y;
        float spacingY = gridLayoutGroup.spacing.y;
        float paddingTop = gridLayoutGroup.padding.top;
        float paddingBottom = gridLayoutGroup.padding.bottom;

        float totalHeight = paddingTop + paddingBottom + (cellHeight * rowCount) + (spacingY * (rowCount - 1));

        Vector2 sizeDelta = contentRt.sizeDelta;
        sizeDelta.y = totalHeight;
        contentRt.sizeDelta = sizeDelta;
    }
}