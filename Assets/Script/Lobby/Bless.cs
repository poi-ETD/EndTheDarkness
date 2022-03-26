using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Bless : MonoBehaviour
{
    int Ignum;
    [SerializeField] LobbyManager lobby;
    [SerializeField] TextMeshProUGUI ig;
    [SerializeField] GameObject BlessPopup;
    [SerializeField] GameObject igObj;
    [SerializeField] GameObject[] But;
    [SerializeField] GameObject BlessObj;
    [SerializeField] GameObject noIgn;
    [SerializeField] TextMeshProUGUI[] blessT;
    blessData bd = new blessData();
    public void IgnumSet(int i)
    {
        noIgn.SetActive(false);
        Ignum += i;
        if (Ignum > 1000) Ignum = 1000;
        if (Ignum < 100) Ignum = 100;
        ig.text = Ignum+"";
    }
    public void BlessPopupOn()
    {
       
        if (lobby.GD.isAct) return;
        if (lobby.canvasOn) return;
        igObj.SetActive(true);
        Ignum = 100;
        ig.text = Ignum + "";
        BlessPopup.SetActive(true);
        BlessObj.SetActive(false);
        But[0].SetActive(true);
        But[1].SetActive(true);
        But[2].SetActive(false);
        lobby.canvasOn = true;     
    }
    public void exitBlessPopup()
    {
        noIgn.SetActive(false);
        lobby.canvasOn = false;
        BlessPopup.SetActive(false);
    }
   public void GetBless()
    {
        if (lobby.GD.Ignum < Ignum)
        {
            noIgn.SetActive(true);
            return;
        }
        lobby.GD.Ignum -= Ignum;
        noIgn.SetActive(false);
        int[] randSize = new int[]{ 800, 150, 50 };
       But[0].SetActive(false);
       But[1].SetActive(false);
       
       igObj.SetActive(false);
        BlessObj.SetActive(true);
        while (Ignum>= 700)
        {
            Ignum -= 100;
            randSize[0] -= 20;
            randSize[1] += 10;
            randSize[2] += 10;
        }
        while(Ignum >= 200){
            Ignum -= 100;
            randSize[0] -= 10;
            randSize[1] += 5;
            randSize[2] += 5;
        }
        int level = 0;
        int rand = Random.Range(0,1000);
        if (randSize[0] > rand) level = 1;
        else
        {
            rand -= randSize[0];
            if (randSize[1] < rand) level = 2;
            else level = 3;
        }
        List<int> randList = new List<int>();
       for(int i = 1; i < bd.bd.Length; i++)
        {
            if (bd.bd[i].level == level)
            {
                randList.Add(bd.bd[i].no);
            }
        }
        rand = Random.Range(0, randList.Count);
        blessT[0].text = bd.bd[randList[rand]].Name + "";
        blessT[1].text = bd.bd[randList[rand]].content + "";

        BlessApply(1);//rand
    }
    void BlessApply(int b)
    {
        if (b==1)
        {
            lobby.GD.Ignum += 1000;
            for (int i = 0; i < lobby.ChD.size; i++)
            {
                lobby.ChD.characterDatas[i].curHp -= Mathf.FloorToInt(lobby.ChD.characterDatas[i].curHp * 0.3f);
                But[2].SetActive(true);
            }

        }
        else if (b == 5)
        {
           
        }
        else if (b == 6)
        {

        }

        else if (b == 8)
        {  int length = lobby.CD.cardNo.Count;
            int[] randArray = new int[lobby.CD.cardNo.Count/2];
            bool isSame;
            for(int i = 0; i < length/2; ++i)
            {
                while (true)
                {

                    randArray[i] = Random.Range(0, length);
                    isSame = false;
                    for(int j = 0; j < i; ++j)
                    {
                        if (randArray[j] == randArray[i]){
                            isSame = true;
                            break;
                        }
                    }
                    if (!isSame) break;
                }
            }
            for(int i = length/2-1; i > 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    if (randArray[j] > randArray[j + 1])
                    {
                        int temp = randArray[j];
                       randArray[j] = randArray[j + 1];
                        randArray[j + 1] = temp;
                    }
                }
            }
            for(int i = length/2-1; i >=0; i--)
            {
                lobby.CD.cardCost.RemoveAt(randArray[i]);
                lobby.CD.cardNo.RemoveAt(randArray[i]);
               
            }
            But[2].SetActive(true);
        }
        else if (b == 14)
        {

        }
       else {
            lobby.GD.blessbool[b] = true;
            But[2].SetActive(true); }
        lobby.DayAct();
    }
  
}
public class blessData
{

    public struct BData
    {
        public int no;
        public string Name;
        public string content;
        public int level;
        public bool per;

        public BData(int no, string Name, string content, int level, bool per)
        {
            this.no = no;
            this.Name = Name;
            this.content = content;
            this.level = level;  //c->1 b->2 a->3
            this.per = per; //지속이면 트루
        }
    }
    public BData[] bd = new BData[17]
    {
        new BData(0,"축복이름","축복내용",0,false),
        new BData(1,"찬란한 수확","1000 이그넘을 획득한다.\n모든 아군이 현재 체력의 30%를  잃는다.",1,false),
        new BData(2,"과도한 영접","전투 동안 , 임의의 후방 아군 한명의 공격력을 2올린다.\n해당 아군의 방어도는 올라가지 않는다.",1,true),
        new BData(3,"숨겨진 약점","시작 드로우를 8장 한다.\n전투가 2번 끝나고 해당 축복은 사라진다.",1,true),
        new BData(4,"은밀한 준비","전투의 첫 턴동안 모든 아군의 행동력이 0이 된다.\n그 이후 모든 아군의 행동력이 2로 고정된다.",2,true),
        new BData(5,"가벼운 몸짓","(패시브 빈칸이 있는) 아군 한명을 선택해 패시브를 고른다. \n	해당 아군의 공격력은 1로 고정된다.",3,false),
        new BData(6,"인위적인 극복","전방에 있는 아군의 체력이 감소할 때마다 그 수치만큼 적 전체에게 데미지를 준다.",3,false),
        new BData(7,"역류하는 고통","덱이 절반 소멸된다.",3,true),
        new BData(8,"a","덱이 절반 소멸된다.",1,false),
        new BData(9,"b","카드 선택 중 취소버튼이 생긴다.\n	이그넘의 절반을 잃는다.",1,true),
        new BData(10,"c","아군 회복시 2명을 선택할 수 있다.",1,true),
        new BData(11,"d","낮에 로비에서 한번 더 행동할 수 있다.",2,true),
        new BData(12,"e","소지하고 있는 카드의 3장을 선택해서 코스트를 0으로 한다\n전투의 첫 턴에 모든 아군의 행동력이 0이 된다.",1,true),
        new BData(13,"f","파티원 부활 비용이 0이그넘이 된다.",2,true),
        new BData(14,"g","아군 1명의 공격력을 1올린다.\n해당 아군의 현재 체력이 1이 된다.",1,false),
        new BData(15,"h","전투로 얻는 이그넘을 3배 받는다.	\n파티원 회복/부활 기능을 사용할 수 없다.",1,true),
        new BData(16,"i","카드 선택시 보기가 최대 4장이 된다.",3,true)
,
    };

    
}