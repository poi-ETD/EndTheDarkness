using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
    CharacterData2 chainfo = new CharacterData2();
    [SerializeField] GameObject bless6;
    [SerializeField] GameObject bless5;
    [SerializeField] GameObject bless5Content;
    int blessNum;
    public int blesscount;
    public TextMeshProUGUI bless5countt;
    [SerializeField] GameObject bless5RemoveCard;
    List<GameObject> b5cardList = new List<GameObject>();
    [SerializeField] GameObject bless14;
    [SerializeField] GameObject bless12;
    [SerializeField] GameObject bless12Content;
    public int bless12count;
    public TextMeshProUGUI bless12countt;
    public int curBless;
    List<int> blessList= new List<int>();
    [SerializeField] GameObject[] blessIcon;
    [SerializeField] GameObject blessRemovePopup;
    [SerializeField] GameObject[] CancleRemove;
    [SerializeField] GameObject SelectBless;

    private void Start()
    {
        setBlessIcon();

    }
    public void blessRemovePopupOn()
    {
        if (lobby.GD.isAct) return;
        if (lobby.canvasOn) return;
        blessRemovePopup.SetActive(true);
        CancleRemove[0].SetActive(true);
        CancleRemove[1].SetActive(true);
        CancleRemove[2].SetActive(true);
        SelectBless.SetActive(false);
        lobby.canvasOn = true;
    }
    public void SelectRemoveOn()  //선택해서 삭제하기
    {if (lobby.GD.Ignum < 1000) return;
        lobby.GD.Ignum -= 1000;

        CancleRemove[0].SetActive(false);
        CancleRemove[1].SetActive(false);
        CancleRemove[2].SetActive(false);
        SelectBless.SetActive(true);
 for(int i = 0; i < lobby.GD.blessbool.Length; i++)
        {
            if (lobby.GD.blessbool[i])
            {
                blessRemovePopup.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
                blessRemovePopup.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = bd.bd[i].Name;
            }
            else
            {
                blessRemovePopup.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    public void RemoveRandom() //랜덤으로 삭제하기
    {
        if (lobby.GD.Ignum < 50) return;
        lobby.GD.Ignum -= 50;
        List<int> curBlessList = new List<int>();
        for(int i = 0; i < lobby.GD.blessbool.Length; i++)
        {
            if (lobby.GD.blessbool[i]) curBlessList.Add(i);
        }
        int rand = Random.Range(0, curBlessList.Count);
        if (curBlessList[rand] == 12)
        {
            for(int i = 0; i < lobby.CD.cardCost.Count; i++)
            {
                if(lobby.CD.cardGet[i]==lobby.GD.bless12[0]|| lobby.CD.cardGet[i] == lobby.GD.bless12[1]|| lobby.CD.cardGet[i] == lobby.GD.bless12[2])
                {
                    CardData2 carddata = new CardData2();
                    lobby.CD.cardCost[i] = carddata.cd[lobby.CD.cardNo[i]].Cost;
                }
            }
        }
        lobby.GD.blessbool[curBlessList[rand]] = false;
        exitRemovePopup();
        lobby.DayAct();
        setBlessIcon();
    }
    public void blessRemoveButton(int a)
    {
        lobby.GD.blessbool[a] = false;
        if (a == 12)
        {
            for (int i = 0; i < lobby.CD.cardCost.Count; i++)
            {
                if (lobby.CD.cardGet[i] == lobby.GD.bless12[0] || lobby.CD.cardGet[i] == lobby.GD.bless12[1] || lobby.CD.cardGet[i] == lobby.GD.bless12[2])
                {
                    CardData2 carddata = new CardData2();
                    lobby.CD.cardCost[i] = carddata.cd[lobby.CD.cardNo[i]].Cost;
                }
            }
        }
        exitRemovePopup();
        lobby.DayAct();
        setBlessIcon();
    }
    public void exitRemovePopup()
    {
        lobby.canvasOn = false;
        blessRemovePopup.SetActive(false);
    }
    public void setBlessIcon()
    {
       for(int i = 1; i < lobby.GD.blessbool.Length; i++)
        {if (blessIcon[i] == null) continue;
            if (lobby.GD.blessbool[i])
            {
             
                blessIcon[i].SetActive(true);
            }
            else
            {
                blessIcon[i].SetActive(false);
            }
        }
    }
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
        if (blessNum >0)
        {
          if(blessNum==6)
            b6();
            if (blessNum == 5)
                b5();
            if (blessNum == 12) b12();
            blessNum = 0;
        }
        else
        {if(lobby.ResetMaraCount!=5)
             lobby.Resetmara(5);
            noIgn.SetActive(false);
            lobby.canvasOn = false;
            BlessPopup.SetActive(false);
        }
    }
    Dictionary<int, int[]> CardList = new Dictionary<int, int[]>(); //key->넘버 value->코스트
    void b12()
    {
        blesscount = 0;
        curBless = 12;
        CardList.Clear();
        BlessPopup.SetActive(false);
        BlessObj.SetActive(false);
        bless12.SetActive(true);
        bless12Content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        for (int i = 0; i < lobby.CD.cardNo.Count; i++)
        {
            GameObject newCard = Instantiate(bless5RemoveCard, bless12Content.transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(lobby.CD.cardNo[i], i);
            newCard.GetComponent<NoBattleCard>().setCost(lobby.CD.cardCost[i]);
            int[] k = { lobby.CD.cardNo[i], lobby.CD.cardCost[i] };
            CardList.Add(i, k);
            b5cardList.Add(newCard);
        }
        setBlessIcon();
    }
    void b5()
    {
        blesscount = 0;
        curBless = 5;
        CardList.Clear();
        BlessPopup.SetActive(false);
        BlessObj.SetActive(false);
        bless5.SetActive(true);
        bless5Content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);         
        for (int i = 0; i < lobby.CD.cardNo.Count; i++)
        {
            GameObject newCard = Instantiate(bless5RemoveCard, bless5Content.transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(lobby.CD.cardNo[i], i);
            newCard.GetComponent<NoBattleCard>().setCost(lobby.CD.cardCost[i]);
            int[] k = { lobby.CD.cardNo[i], lobby.CD.cardCost[i] };
            CardList.Add(i, k);
            b5cardList.Add(newCard);
        }
        setBlessIcon();
    }
    public void OrderByCost()
    {
        var queryAsc = CardList.OrderBy(x => x.Value[1]);
        int count = 0;
        foreach (var dictionary in queryAsc)
        {

            b5cardList[dictionary.Key].transform.SetSiblingIndex(count);
                count++;
        }
    }
    public void OrderByNo()
    {
        var queryAsc = CardList.OrderBy(x => x.Value[0]);
        int count = 0;
        foreach (var dictionary in queryAsc)
        {

            b5cardList[dictionary.Key].transform.SetSiblingIndex(count);
                count++;
        }
    }
    public void OrderByGet()
    {
        var queryAsc = CardList.OrderBy(x => x.Key);
        int count = 0;
        foreach (var dictionary in queryAsc)
        {

            b5cardList[dictionary.Key].transform.SetSiblingIndex(count);
                count++;
        }
    }
   
    public void GetBless()
    {
        if (lobby.GD.Ignum < Ignum&&!lobby.resetmara)
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
        int count = 0;
        while (lobby.GD.blessbool[rand])
        {
            rand = Random.Range(0, randList.Count);
            count++;
            if (count > 100) break;
        }
        while (lobby.GD.blessbool[rand])
        {
            rand = Random.Range(0, randList.Count);
           
        }
        blessT[0].text = bd.bd[randList[rand]].Name + "";
        blessT[1].text = bd.bd[randList[rand]].content + "";
        
        BlessApply(randList[rand]);//rand
    }
    /*randList[rand]*/

    public void b6()
    {
        BlessObj.SetActive(false);
        bless6.SetActive(true);
        for (int i = 0; i < lobby.ChD.size; i++)
        {
            bless6.transform.GetChild(i).gameObject.SetActive(true);
            bless6.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = lobby.ChD.characterDatas[i].Name;
            for (int j = 0; j < 4; j++)
            {
                if (lobby.ChD.characterDatas[i].passive[j] == 0)
                {
                    bless6.transform.GetChild(i).GetChild(j).gameObject.SetActive(false);
                }
                else
                {
                    bless6.transform.GetChild(i).GetChild(j).gameObject.SetActive(true);
                    bless6.transform.GetChild(i).GetChild(j).GetChild(0).GetComponent<Text>().text = chainfo.cd[lobby.ChD.characterDatas[i].No].passive[j];
                }
            }
        }
        for (int i = lobby.ChD.size; i < 4; i++)
        {
            bless6.transform.GetChild(i).gameObject.SetActive(false);
        }

    }
    List<int> b5list = new List<int>();
    public void RemoveB5()
    {if (blesscount < 6) return;      
        for(int i = 0; i <lobby.CD.cardNo.Count; i++)
        {
        
            if(bless5Content.transform.GetChild(i).gameObject.GetComponent<NoBattleCard>().select)
            b5list.Add(bless5Content.transform.GetChild(i).gameObject.GetComponent<NoBattleCard>().deckNo);
        }
        b5list.Sort();
        for(int i = 5; i >=0; i--)
        { 
            lobby.CD.cardNo.RemoveAt(b5list[i]);
            lobby.CD.cardCost.RemoveAt(b5list[i]);
            lobby.CD.cardGet.RemoveAt(b5list[i]);
        }
        exitBlessPopup();
        if (lobby.resetmara) lobby.Resetmara(5);
        else lobby.DayAct();
        bless5.SetActive(false);
        b5list.Clear();
        b5cardList.Clear();
    }
    public void B12Apply()
    {
        if (blesscount < 3) return;
        for (int i = 0; i < lobby.CD.cardNo.Count; i++)
        {

            if (bless12Content.transform.GetChild(i).gameObject.GetComponent<NoBattleCard>().select)
                b5list.Add(bless12Content.transform.GetChild(i).gameObject.GetComponent<NoBattleCard>().deckNo) ;
        }
       for(int i = 0; i < 3; i++)
        {
            lobby.CD.cardCost[b5list[i]] = 0;
            lobby.GD.bless12[i] = lobby.CD.cardGet[b5list[i]];
        }
        exitBlessPopup();
        if (lobby.resetmara) lobby.Resetmara(5);
        else lobby.DayAct();
        bless12.SetActive(false);
        b5list.Clear();
        b5cardList.Clear();
        setBlessIcon();
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
            But[2].SetActive(true);
            blessNum = 5;
        }
        else if (b == 6)
        {           
            But[2].SetActive(true);
            blessNum = 6;
        }

        else if (b == 8)
        {
            int length = lobby.CD.cardNo.Count;
            int[] randArray = new int[lobby.CD.cardNo.Count / 2];
            bool isSame;
            for (int i = 0; i < length / 2; ++i)
            {
                while (true)
                {

                    randArray[i] = Random.Range(0, length);
                    isSame = false;
                    for (int j = 0; j < i; ++j)
                    {
                        if (randArray[j] == randArray[i])
                        {
                            isSame = true;
                            break;
                        }
                    }
                    if (!isSame) break;
                }
            }
            for (int i = length / 2 - 1; i > 0; i--)
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
            for (int i = length / 2 - 1; i >= 0; i--)
            {
                lobby.CD.cardCost.RemoveAt(randArray[i]);
                lobby.CD.cardNo.RemoveAt(randArray[i]);
                lobby.CD.cardGet.RemoveAt(randArray[i]);
            }
            But[2].SetActive(true);
        }
        else if (b == 12)
        {
            But[2].SetActive(true);
            blessNum = 12;
            lobby.GD.blessbool[12] = true;
        }
        else if (b == 14)
        {
            bless14.SetActive(true);
            for(int i = 0; i < 4; i++)
            {
                if (lobby.ChD.characterDatas[i].Name!= null)
                {
                    bless14.transform.GetChild(i).gameObject.SetActive(true);
                    bless14.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = lobby.ChD.characterDatas[i].Name;
                }
                else
                {
                    bless14.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
          
        }
       else {
          
            lobby.GD.blessbool[b] = true;
            But[2].SetActive(true); }
        if (!lobby.resetmara) 
       lobby.DayAct();
        setBlessIcon();
    }
 
    public void bless14But(int i)
    {
        lobby.ChD.characterDatas[i].Atk++;
        lobby.ChD.characterDatas[i].curHp = 1;
        exitBlessPopup();


        if (lobby.resetmara) lobby.Resetmara(lobby.ChD.size + 1);
        else lobby.DayAct();
        bless14.SetActive(false);
        setBlessIcon();

    }
  public void SelectMorePassive(int i)
    {
  
        bless6.SetActive(false);
        exitBlessPopup();
        lobby.ChD.characterDatas[i / 4].passive[i % 4]++;
        if (lobby.resetmara) lobby.Resetmara(5);
        else lobby.DayAct();
        setBlessIcon();
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
        new BData(5,"가벼운 몸짓","소지하고 있는 카드의 6장을 선택해 소멸시킨다.",3,false),
        new BData(6,"인위적인 극복","아군 한명을 선택해 패시브를 늘린다. \n	해당 아군의 공격력을 -2로 감소시킨다.",3,false),
        new BData(7,"역류하는 고통","전방에 있는 아군의 체력이 감소할 때마다 그 수치만큼 적 전체에게 데미지를 준다.",3,true),
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