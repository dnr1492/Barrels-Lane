public class EnumClass
{
    public enum CharacterRace { None = -1, Primordial, Earthbound, Seaborne, Verdant, Skyborne, Mythkin, Divinite, Morphid, Undying, Automaton }  //����, ����, ����, �Ĺ�, õ��, ȯ��, ����, ����, �һ�, ���
    public enum CharacterTokenState { None = -1, Cancel, Select, Confirm }
    public enum CharacterTokenDirection { None = -1, Right = 0, UpRight, UpLeft, Left, DownLeft, DownRight }
    public enum CharacterTier { None = -1, Boss, High, Middle, Low }
    public enum CharacterJob { None = -1, Dealer, Tanker, Supporter }
    public enum FilterType
    {
        Job,
        Tier,
        Hp,
        Mp,
        SkillCardRank,
        SkillCardType
    }
    public enum CardState { None = -1, Front, Back }
    public enum CardType { None = -1, CharacterCard, SkillCard }
    public enum SkillCardType { None = -1, Attack, Defense, Move, Buff, Debuff }
    public enum SkillCardRankAndMpConsum { None = -1, Zero = 0, Rank1 = 1, Rank2 = 2, Rank3 = 3, Rank4 =4, Rank5 = 5 }
    public enum SkillRangeType
    {
        None,
        LineForward1,  //���� 1ĭ
        LineForward2,  //���� 2ĭ
        LineForward3,  //���� 3ĭ
        Ring1,         //���� 1ĭ
        Custom
    }
    public enum PhotonEventCode : byte { SendDeck = 1 }
}
