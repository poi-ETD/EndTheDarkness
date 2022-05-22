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
   
    public List<equipment> EquipmentList = new List<equipment>();
    public int[] curEquip = new int[4];//-1일 때는 장착 x,초기 값으로 넣어주자.
}
public struct equipment
{
    public int type;
    public string equipName;
    public List<int> improveStat; //증가시키는 스탯의 종류,중첩이 가능하다.
    public List<int> improveMount; //증가시키는 스탯의 양
    public int degradeStat; //저하시키는 스탯의 종류
    public int degradeMount; //저하시키는 스탯의 양
    public int equipNum;

    public equipment(int type, string equipName, List<int> improveStat, List<int> improveMount, int degradeStat, int degradeMount, int equipNum)
    {
        this.type = type;
        this.equipName = equipName;
        this.improveStat = improveStat;
        this.improveMount = improveMount;
        this.degradeStat = degradeStat;
        this.degradeMount = degradeMount;
        this.equipNum = equipNum;
    }
}