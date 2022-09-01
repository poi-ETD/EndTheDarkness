using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameData
{
    public int Ignum;
    public int tribute;
    public bool isAct;
    public bool isNight;
    public int Day;
    public int victory;
    public bool[] blessbool=new bool[21];
    public int BattleNo;
    public int[] bless12 = new int[3];
    public int bless3count;
    public bool isActInDay;
    public int passiveStack;
    public int tributeStack;
    public int ryungPassive2;
    public int blessSelect;
    public List<equipment> EquipmentList = new List<equipment>();
    public int[] curEquip = new int[4];//-1일 때는 장착 x,초기 값으로 넣어주자.
}
public struct equipment
{
    public int type;//type 0->전방,1->후방,2->전체
    public int special;//0일 경우에 일반 장비, 그 외에 캐릭터 NO의 장비
    public string equipName;
    public string equipContent; //장비의 설명, 일반 장비는 신경 x
    public List<int> improveStat; //증가시키는 스탯의 종류,중첩이 가능하다., 전용장비는 신경 X
    public List<int> improveMount; //증가시키는 스탯의 양 ,전용장비는 신경 X
    public int degradeStat; //저하시키는 스탯의 종류, 전용장비는 신경 X
    public int degradeMount; //저하시키는 스탯의 양 ,전용장비는 신경 X
    public int equipNum;

    public equipment(int type,int special, string equipName,string equipContent, List<int> improveStat, List<int> improveMount, int degradeStat, int degradeMount, int equipNum)
    {
        this.type = type;
        this.special = special;
        this.equipName = equipName;
        this.equipContent = equipContent;
        this.improveStat = improveStat;
        this.improveMount = improveMount;
        this.degradeStat = degradeStat;
        this.degradeMount = degradeMount;
        this.equipNum = equipNum;
    }
}