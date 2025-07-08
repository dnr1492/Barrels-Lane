using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumClass;

public class FilterData : MonoBehaviour
{
    public FilterType filterType;

    //Enum ��� ���Ϳ�
    public CharacterJob job;
    public CharacterTierAndCost tier;
    public SkillCardRankAndMpConsum skillCardRank;
    public SkillCardType skillCardType;

    //���� ��� ���Ϳ�
    public int intValue;  //Hp, Mp�� ���
}
