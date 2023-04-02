public enum ActTarget
{
    character,
    enemy
}

public enum EquipmentStat
{
    atk = 0,
    def,
    maxHp,
    cost,
    act,
}

public enum Status
{
    blood, // -> YH : 포션 오타난건지 포이즌 오타난건지 변경 필요해보입니다 //->출혈로 변경
    weak,
}

public enum Owner // YH : owner character of card
{
    standard = 0,
    q,
    sparky,
    vangara,
    porte,
    reong,
    tiger
}

public enum Thema
{
    starter = 0,
    standard,
    additional,
    token
}