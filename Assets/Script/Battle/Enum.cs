using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    private static Enums instance;

    //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 맘껏 호출할 수 있다.
    public static Enums Instance
    {
        get
        {
            if (null == instance)
            {
                //게임 인스턴스가 없다면 하나 생성해서 넣어준다.
                instance = new Enums();
            }
            return instance;
        }
    }
    public enum ActTarget
    {
        character,
        enemy
    }
    public enum EquipmentStat
    {
        atk=0 ,
        def,
        maxHp,
        cost,
        act,
    }
    public enum Status
    {
        posion,
        weak,
    }
  
}
