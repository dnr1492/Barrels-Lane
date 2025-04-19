public class EnumClass
{
    public enum CharacterRace { None = -1, Primordial, Earthbound, Seaborne, Verdant, Skyborne, Mythkin, Divinite, Morphid, Undying, Automaton }  //����, ����, ����, �Ĺ�, õ��, ȯ��, ����, ����, �һ�, ���
    public enum CharacterTierAndCost { None = -1, Leader = 100, High = 4, Middle = 3, Low = 2 }
    public enum CharacterJob { Dealer, Tanker, Supporter }
    public enum State { None = -1, Front, Back }
    public enum CardType { None = -1, CharacterCard, SkillCard }
    public enum SkillCardType { None = -1, Move, Attack, Buff }
    public enum SkillCardRankAndMpConsum { None = -1, Rank1 = 1, Rank2 = 2, Rank3 = 3 }
}
