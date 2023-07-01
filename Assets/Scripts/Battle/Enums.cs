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
	charming,	//매혹 , 가하는 모든 데미지 감소
}

public enum Owner // YH : owner character of card
{
    standard = 0,
    q,
    sparky,
    vangara,
    porte,
    ryeong,
    hyuwnggwi
}

public enum Thema
{
    starter = 0,
    standard,
    additional,
    token
}