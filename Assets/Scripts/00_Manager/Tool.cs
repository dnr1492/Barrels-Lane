using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static EnumClass;

public class Tool
{
#if UNITY_EDITOR
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

        list.characterCardDatas.Add(new CharacterCardData { id = 0, name = "��� ���к�", tier = CharacterTierAndCost.Low, hp = 4, mp = 1, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1001, 3001 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 1, name = "�������� â��", tier = CharacterTierAndCost.Low, hp = 3, mp = 1, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1002, 2001, 3002 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 2, name = "�׸��ڼ� ��ȣ��", tier = CharacterTierAndCost.Low, hp = 2, mp = 2, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1003, 3003, 3004 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 3, name = "ħ���� �����", tier = CharacterTierAndCost.Middle, hp = 4, mp = 2, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1004, 3005, 2002 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 4, name = "������ ������", tier = CharacterTierAndCost.Middle, hp = 3, mp = 3, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1005, 3006 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 5, name = "�������и� �θ� ��ȣ��", tier = CharacterTierAndCost.Middle, hp = 2, mp = 4, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1006, 4001, 3007 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 6, name = "�걺 -����-", tier = CharacterTierAndCost.High, hp = 6, mp = 3, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1007, 3008, 2003, 3009 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 7, name = "������ ���� -���-", tier = CharacterTierAndCost.High, hp = 2, mp = 6, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1008, 5001, 5002, 3010, 3011 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 8, name = "���尡�� -�ΰ�-", tier = CharacterTierAndCost.High, hp = 6, mp = 3, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1009, 4002, 2004, 3012 } });

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

        list.skillCardDatas.Add(new SkillCardData { id = 1001, name = "��� ���к� (�̵�)", rank = (int)SkillCardRankAndMpConsum.Zero, effect = "", cardType = SkillCardType.Move, rangeType = SkillRangeType.None });

        LoadDataFromJSON(list, "skillCard_data.json");
    }
    #endregion
#endif
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
    public int id;  //CharacterCardData�� skills�� Ű�� ex) 1001
    public string name;
    public int rank;
    //public int mpConsum;  //MP�Ҹ��� Rank�� ����
    public string effect;
    public SkillCardType cardType;
    public SkillRangeType rangeType;
    public List<(int dq, int dr, Color color)> customOffsetRange;

    // ���� Damage, Count, �̵� �Ұ� (enum) ������ �߰��ϱ� ���� //
    // ���� Damage, Count, �̵� �Ұ� (enum) ������ �߰��ϱ� ���� //
    // ���� Damage, Count, �̵� �Ұ� (enum) ������ �߰��ϱ� ���� //
}
#endregion