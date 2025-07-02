public class EnumClass
{
    public enum CharacterRace { None = -1, Primordial, Earthbound, Seaborne, Verdant, Skyborne, Mythkin, Divinite, Morphid, Undying, Automaton }  //����, ����, ����, �Ĺ�, õ��, ȯ��, ����, ����, �һ�, ���
    public enum CharacterTokenState { None = -1, Cancel, Select, Confirm }
    public enum CharacterTokenDirection { Right = 0, UpRight, UpLeft, Left, DownLeft, DownRight }
    public enum CharacterTierAndCost { None = -1, Boss = 100, High = 4, Middle = 3, Low = 2 }
    public enum CharacterJob { Dealer, Tanker, Supporter }
    public enum CardState { None = -1, Front, Back }
    public enum CardType { None = -1, CharacterCard, SkillCard }
    public enum SkillCardType { None = -1, Move, Attack, Buff, Debuff, Trap }  // ===== ���� ���� ��� ===== //
    public enum SkillCardRankAndMpConsum { None = -1, Rank1 = 1, Rank2 = 2, Rank3 = 3 }
    public enum SkillRangeType
    {
        None,
        LineForward1,  //���� 1ĭ
        LineForward2,  //���� 2ĭ
        LineForward3,  //���� 3ĭ
        Ring1,         //���� 1ĭ
        Custom
    }
}
