using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    private static void LoadDataFromJSON<T>(T data, string fileName)
    {
        //Resources/Datas ���
        string path = Path.Combine(Application.dataPath, "Resources/Datas");
        string jsonPath = Path.Combine(path, fileName);

        //JSON ���� ����
        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(jsonPath, jsonData);

        Debug.Log($"JSON data saved at: {jsonPath}");
        AssetDatabase.Refresh();
    }

    #region ���� �÷��� ������
    [MenuItem("�����/Generate gamePlay_data")]
    private static void GenerateGamePlayData()
    {
        GamePlayData gamePlayData = new GamePlayData {
            maxCost = 10
        };

        LoadDataFromJSON(gamePlayData, "gamePlay_data.json");
    }
    #endregion

    #region ī�� ������
    [MenuItem("�����/Generate card_data")]
    private static void GenerateCardData()
    {
        //JSON ������ ����
        CardDataList list = new CardDataList {
            cardDatas = new List<CardData>()
        };

        list.cardDatas.Add(new CardData { });

        LoadDataFromJSON(list, "card_data.json");
    }
    #endregion

    #region ��ų ������
    [MenuItem("�����/Generate skill_data")]
    private static void GenerateSkillData()
    {
        //JSON ������ ����
        SkillDataList list = new SkillDataList {
            skillDatas = new List<SkillData>()
        };

        list.skillDatas.Add(new SkillData { });

        LoadDataFromJSON(list, "skill_data.json");
    }
    #endregion

    #region ��ų ��ũ ������
    [MenuItem("�����/Generate skillRank_data")]
    private static void GenerateSkillRankData()
    {
        //JSON ������ ����
        SkillRankDataList list = new SkillRankDataList {
            skillRankDatas = new List<SkillRankData>()
        };

        list.skillRankDatas.Add(new SkillRankData { });

        LoadDataFromJSON(list, "skillRank_data.json");
    }
    #endregion
}

#region ���� �÷��� ������
[Serializable]
public class GamePlayData
{
    public int maxCost;
}
#endregion

#region ī�� ������
[Serializable]
public class CardDataList
{
    public List<CardData> cardDatas;
}

[Serializable]
public class CardData
{
    public int id;
    public string name;
    public string tier;
    public int cost;
    public int hp;
    public int mp;
    public string race;
    public string job;
    public int attackRange;
    public int attackDirection;
    public List<int> skills;  //ex) 101, 102, 103)
}
#endregion

#region ��ų ������
[Serializable]
public class SkillDataList
{
    public List<SkillData> skillDatas;
}

[Serializable]
public class SkillData
{
    public int id;  //CardData�� skills�� Ű��, ex) 101
    public string name;
    public int mpConsum;
    public int rank;
}
#endregion

#region ��ų ��ũ ������
[Serializable]
public class SkillRankDataList
{
    public List<SkillRankData> skillRankDatas;
}

[Serializable]
public class SkillRankData
{
    public int id;  //SkillData�� rank�� Ű��
    public string name;
    public string type;
    public string effect;
    public int range;
    public int direction;
}
#endregion