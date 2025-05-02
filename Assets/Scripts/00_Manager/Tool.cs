using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static EnumClass;

public class Tool
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
            maxCost = 10,
            maxLeaderCount = 1,
            maxHighTierCount = 2,
            maxMiddleTierCount = 3,
            maxLowTierCount = 5,
            limitMiddle = 2,
            limitLow = 3,
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
        list.characterCardDatas.Add(new CharacterCardData { id = 0, name = "�׸��� �� �ϻ���", skills = new List<int> { 1000, 1001 }, tier = CharacterTierAndCost.Captain, race = CharacterRace.Primordial, job = CharacterJob.Dealer });
        list.characterCardDatas.Add(new CharacterCardData { id = 1, name = "�Ͼ� �˻�", skills = new List<int> { 1000, 1001 }, tier = CharacterTierAndCost.Captain, race = CharacterRace.Primordial, job = CharacterJob.Dealer });
        list.characterCardDatas.Add(new CharacterCardData { id = 2, name = "�׸��� �� ��ȣ��", skills = new List<int> { 1002, 1003 }, tier = CharacterTierAndCost.Captain, race = CharacterRace.Primordial, job = CharacterJob.Tanker });
        list.characterCardDatas.Add(new CharacterCardData { id = 3, name = "��� ���к�", skills = new List<int> { 1002, 1003 }, tier = CharacterTierAndCost.Captain, race = CharacterRace.Primordial, job = CharacterJob.Tanker });
        list.characterCardDatas.Add(new CharacterCardData { id = 4, name = "�������� â��", skills = new List<int> { 1000, 1003 }, tier = CharacterTierAndCost.Captain, race = CharacterRace.Primordial, job = CharacterJob.Tanker });

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
        list.skillCardDatas.Add(new SkillCardData { id = 1000, name = "Skill Test 1", rank = (int)SkillCardRankAndMpConsum.Rank1, type = SkillCardType.Move });
        list.skillCardDatas.Add(new SkillCardData { id = 1001, name = "Skill Test 1", rank = (int)SkillCardRankAndMpConsum.Rank2, type = SkillCardType.Attack });
        list.skillCardDatas.Add(new SkillCardData { id = 1002, name = "Skill Test 2", rank = (int)SkillCardRankAndMpConsum.Rank1, type = SkillCardType.Move });
        list.skillCardDatas.Add(new SkillCardData { id = 1003, name = "Skill Test 2", rank = (int)SkillCardRankAndMpConsum.Rank2, type = SkillCardType.Attack });
        list.skillCardDatas.Add(new SkillCardData { id = 1004, name = "Skill Test 3", rank = (int)SkillCardRankAndMpConsum.Rank1, type = SkillCardType.Buff });

        LoadDataFromJSON(list, "skillCard_data.json");
    }
    #endregion
}

#region ���� �÷��� ������
[Serializable]
public class GamePlayData
{
    public int maxCost;
    public int maxLeaderCount;
    public int maxHighTierCount;
    public int maxMiddleTierCount;
    public int maxLowTierCount;
    public int limitMiddle;
    public int limitLow;
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
    public CharacterTierAndCost tier;
    //public int cost;  //Tier�� �� Cost�� ���� ���ǵǾ� ����
    public int hp;
    public int mp;
    public CharacterRace race;
    public CharacterJob job;
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
    public int rank;
    //public int mpConsum;  //MP�Ҹ��� Rank�� ����
    public SkillCardType type;
}
#endregion