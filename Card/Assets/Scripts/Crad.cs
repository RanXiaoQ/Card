public class Card
{
    public int m_Id;
    public string m_CardName;
    public Card(int _id,string _cardName)
    {
        this.m_Id = _id;
        this.m_CardName = _cardName;
    }
}

/// <summary>
/// 怪物卡
/// </summary>
public class MonsterCard:Card
{
    public int m_Attack;   //攻击
    public int m_HealthPoint;  //当前血量
    public int m_HealthPointMax;   //最大血量
    public int m_AttackNumber;

    public MonsterCard(int _id, string _cardName, int _attack,int _healthPointMax) : base(_id, _cardName)
    {
        this.m_Attack = _attack;
        this.m_HealthPoint = _healthPointMax;
        this.m_HealthPointMax = _healthPointMax;
        m_AttackNumber = 2;
        //this.m_AttackNumber = _attackNumber;
    }
}

/// <summary>
/// 技能卡
/// </summary>
public class SpellCard : Card
{
    public string m_Effect;   //技能介绍
    public int m_Attack;

    public SpellCard(int _id, string _cardName,string _effect,int _attack):base(_id, _cardName)
    {
        this.m_Effect = _effect;
        this.m_Attack = _attack;
    }
}

