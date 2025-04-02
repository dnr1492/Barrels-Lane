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

    #region ĳ���� ī�� ������
    [MenuItem("�����/Generate characterCard_data")]
    private static void GenerateCharacterCardData()
    {
        //JSON ������ ����
        CharacterCardDataList list = new CharacterCardDataList {
            characterCardDatas = new List<CharacterCardData>()
        };

        // ===== �ӽ� ������ =====
        list.characterCardDatas.Add(new CharacterCardData { id = 0, name = "Test 1", skills = new List<int> { 1000, 1001 }, cost = 1, tier = "Low" });
        list.characterCardDatas.Add(new CharacterCardData { id = 1, name = "Test 2", skills = new List<int> { 1000, 1001 }, cost = 1, tier = "Middle" });
        list.characterCardDatas.Add(new CharacterCardData { id = 2, name = "Test 3", skills = new List<int> { 1002, 1003 }, cost = 1, tier = "Low" });
        list.characterCardDatas.Add(new CharacterCardData { id = 3, name = "Test 4", skills = new List<int> { 1002, 1003 }, cost = 1, tier = "Middle" });
        list.characterCardDatas.Add(new CharacterCardData { id = 4, name = "Test 5", skills = new List<int> { 1000, 1003 }, cost = 2, tier = "High" });

        LoadDataFromJSON(list, "characterCard_data.json");
    }
    #endregion

    #region ��ų ī�� ������
    [MenuItem("�����/Generate skillCard_data")]
    private static void GenerateSkillCardData()
    {
        //JSON ������ ����
        SkillCardDataList list = new SkillCardDataList {
            skillCardDatas = new List<SkillCardData>()
        };

        // ===== �ӽ� ������ =====
        list.skillCardDatas.Add(new SkillCardData { id = 1000, name = "Skill Test 1", rank = 1 });
        list.skillCardDatas.Add(new SkillCardData { id = 1001, name = "Skill Test 1", rank = 2 });
        list.skillCardDatas.Add(new SkillCardData { id = 1002, name = "Skill Test 2", rank = 1 });
        list.skillCardDatas.Add(new SkillCardData { id = 1003, name = "Skill Test 2", rank = 2 });
        list.skillCardDatas.Add(new SkillCardData { id = 1004, name = "Skill Test 3", rank = 1 });

        LoadDataFromJSON(list, "skillCard_data.json");
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

#region ĳ���� ī�� ������
[Serializable]
public class CharacterCardDataList
{
    public List<CharacterCardData> characterCardDatas;
}

[Serializable]
public class CharacterCardData
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
    public List<int> skills;  //ex) 1001, 1002, 1003)
}
#endregion

#region ��ų ī�� ������
[Serializable]
public class SkillCardDataList
{
    public List<SkillCardData> skillCardDatas;
}

[Serializable]
public class SkillCardData
{
    public int id;  //CharacterCardData�� skills�� Ű��, ex) 1001
    public string name;
    public int mpConsum;
    public int rank;
}
#endregion