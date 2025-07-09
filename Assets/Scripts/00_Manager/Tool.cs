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

        list.characterCardDatas.Add(new CharacterCardData { id = 101, name = "��� ���к�", tier = CharacterTierAndCost.Low, hp = 4, mp = 1, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1001, 3001 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 102, name = "�������� â��", tier = CharacterTierAndCost.Low, hp = 3, mp = 1, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1001, 2001, 3002 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 103, name = "�׸��ڼ� ��ȣ��", tier = CharacterTierAndCost.Low, hp = 2, mp = 2, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1001, 3003, 3004 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 104, name = "ħ���� �����", tier = CharacterTierAndCost.Middle, hp = 4, mp = 2, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1001, 3005, 2002 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 105, name = "������ ������", tier = CharacterTierAndCost.Middle, hp = 3, mp = 3, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1001, 3006 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 106, name = "�������и� �θ� ��ȣ��", tier = CharacterTierAndCost.Middle, hp = 2, mp = 4, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1001, 4001, 3007 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 107, name = "�걺 -����-", tier = CharacterTierAndCost.High, hp = 6, mp = 3, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1001, 3008, 2003, 3009 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 108, name = "������ ���� -���-", tier = CharacterTierAndCost.High, hp = 2, mp = 6, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1001, 5001, 5002, 3010, 3011 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 109, name = "���尡�� -�ΰ�-", tier = CharacterTierAndCost.High, hp = 6, mp = 3, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1001, 4002, 2004, 3012 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 110, name = "���� ��� -��-", tier = CharacterTierAndCost.Boss, hp = 2, mp = 1, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1001, 3013 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 111, name = "���� ���� -����Ƽ-", tier = CharacterTierAndCost.Boss, hp = 2, mp = 1, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1001, 4003 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 112, name = "ö�� ���� -�츣-", tier = CharacterTierAndCost.Boss, hp = 3, mp = 1, race = CharacterRace.Primordial, job = CharacterJob.Tanker, skills = new List<int> { 1001, 3014 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 113, name = "�Ͼ�˻�", tier = CharacterTierAndCost.Low, hp = 3, mp = 2, race = CharacterRace.Primordial, job = CharacterJob.Dealer, skills = new List<int> { 1001, 2005 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 114, name = "�׸��� �� �ϻ���", tier = CharacterTierAndCost.Low, hp = 2, mp = 2, race = CharacterRace.Primordial, job = CharacterJob.Dealer, skills = new List<int> { 1001, 2006, 2007 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 115, name = "������ �Բ� �ȴ� �ϻ���", tier = CharacterTierAndCost.Low, hp = 2, mp = 3, race = CharacterRace.Primordial, job = CharacterJob.Dealer, skills = new List<int> { 1001, 2008 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 116, name = "������ ���", tier = CharacterTierAndCost.Middle, hp = 4, mp = 2, race = CharacterRace.Primordial, job = CharacterJob.Dealer, skills = new List<int> { 1001, 2009, 2010 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 117, name = "Ǫ������ ���ݼ�", tier = CharacterTierAndCost.Middle, hp = 2, mp = 3, race = CharacterRace.Primordial, job = CharacterJob.Dealer, skills = new List<int> { 1001, 4004, 1002, 2011 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 118, name = "������ ���Ȱ�", tier = CharacterTierAndCost.Middle, hp = 3, mp = 2, race = CharacterRace.Primordial, job = CharacterJob.Dealer, skills = new List<int> { 1001, 2012, 2013, 2014 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 119, name = "ī�̸� ������", tier = CharacterTierAndCost.High, hp = 6, mp = 3, race = CharacterRace.Primordial, job = CharacterJob.Dealer, skills = new List<int> { 1001, 2015, 2016, 2017 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 120, name = "������ -����ĭ-", tier = CharacterTierAndCost.High, hp = 9, mp = -1, race = CharacterRace.Primordial, job = CharacterJob.Dealer, skills = new List<int> { 1001, 2018, 2019, 2020 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 121, name = "���� -�ܿ�-", tier = CharacterTierAndCost.High, hp = 5, mp = 4, race = CharacterRace.Primordial, job = CharacterJob.Dealer, skills = new List<int> { 1001, 2021, 2022, 2023 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 122, name = "�����Ҳ� -û��-", tier = CharacterTierAndCost.Boss, hp = 2, mp = 1, race = CharacterRace.Primordial, job = CharacterJob.Dealer, skills = new List<int> { 1001, 2024, 2025 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 123, name = "�̰��� â -����Ʈ-", tier = CharacterTierAndCost.Boss, hp = 1, mp = 2, race = CharacterRace.Primordial, job = CharacterJob.Dealer, skills = new List<int> { 1001, 2026 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 124, name = "������ -�ڽ���-", tier = CharacterTierAndCost.Boss, hp = 2, mp = 1, race = CharacterRace.Primordial, job = CharacterJob.Dealer, skills = new List<int> { 1001, 2027 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 125, name = "������ ġ����", tier = CharacterTierAndCost.Low, hp = 1, mp = 4, race = CharacterRace.Primordial, job = CharacterJob.Supporter, skills = new List<int> { 1001, 4005 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 126, name = "����� ġ���", tier = CharacterTierAndCost.Low, hp = 2, mp = 3, race = CharacterRace.Primordial, job = CharacterJob.Supporter, skills = new List<int> { 1001, 4006 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 127, name = "���� ������ ���ֻ�", tier = CharacterTierAndCost.Low, hp = 2, mp = 2, race = CharacterRace.Primordial, job = CharacterJob.Supporter, skills = new List<int> { 1001, 5003, 5004 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 128, name = "����� ������", tier = CharacterTierAndCost.Middle, hp = 2, mp = 3, race = CharacterRace.Primordial, job = CharacterJob.Supporter, skills = new List<int> { 1001, 1003, 4007, 3015 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 129, name = "��Ȱ�� ������", tier = CharacterTierAndCost.Middle, hp = 3, mp = 3, race = CharacterRace.Primordial, job = CharacterJob.Supporter, skills = new List<int> { 1001, 4008, 1004 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 130, name = "������ ���� ���� ���ݼ���", tier = CharacterTierAndCost.Middle, hp = 2, mp = 4, race = CharacterRace.Primordial, job = CharacterJob.Supporter, skills = new List<int> { 1001, 4009, 3016 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 131, name = "�ο� ���̵�", tier = CharacterTierAndCost.High, hp = 3, mp = 5, race = CharacterRace.Primordial, job = CharacterJob.Supporter, skills = new List<int> { 1001, 4010, 5005, 4011, 4012 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 132, name = "�Ϲ��� �������� -������-", tier = CharacterTierAndCost.High, hp = 5, mp = 4, race = CharacterRace.Primordial, job = CharacterJob.Supporter, skills = new List<int> { 1001, 4013, 4014, 5006 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 133, name = "��ǳ�� ���� -�󿡳�-", tier = CharacterTierAndCost.High, hp = 3, mp = 6, race = CharacterRace.Primordial, job = CharacterJob.Supporter, skills = new List<int> { 1001, 4015, 4016, 4017 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 134, name = "�ð��� �ȴ� �� -���-", tier = CharacterTierAndCost.Boss, hp = 1, mp = 2, race = CharacterRace.Primordial, job = CharacterJob.Supporter, skills = new List<int> { 1001, 4018, 5007 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 135, name = "õ���� �︲ -��-", tier = CharacterTierAndCost.Boss, hp = 2, mp = 2, race = CharacterRace.Primordial, job = CharacterJob.Supporter, skills = new List<int> { 1001, 4019 } });
        list.characterCardDatas.Add(new CharacterCardData { id = 136, name = "���� -�紩��-", tier = CharacterTierAndCost.Boss, hp = 2, mp = 1, race = CharacterRace.Primordial, job = CharacterJob.Supporter, skills = new List<int> { 1001, 5008 } });

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

#region ���� ������

#endregion

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