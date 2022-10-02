using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class BlessInLobby : MonoBehaviour
{
    int Tribute;
    [SerializeField] LobbyManager lobby;
    [SerializeField] TextMeshProUGUI ig;
    [SerializeField] GameObject BlessPopup;
    [SerializeField] GameObject igObj;
    [SerializeField] GameObject[] But;
    [SerializeField] GameObject BlessObj;
    [SerializeField] GameObject noIgn;
    [SerializeField] TextMeshProUGUI[] blessT;
    public blessData bd = new blessData();
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
    private int bless18_number;
    public bool bless18_on;
    public bool bless19_on;
    public void blessRemovePopupOn()
    {
        int c = 0;
        for (int i = 0; i < lobby.GD.blessbool.Length; i++)
        {
            if (lobby.GD.blessbool[i]) c++;
        }
        if (c == 0) return; //삭제 할 축복이 없을 경우
        lobby.RitualViewOff();
        blessRemovePopup.SetActive(true);
        CancleRemove[0].SetActive(true);
        CancleRemove[1].SetActive(true);
        CancleRemove[2].SetActive(true);
        SelectBless.SetActive(false);
        lobby.canvasOn = true;
    }
    public void SelectRemoveOn()  //선택해서 삭제하기
    { 
        if (lobby.GD.tribute<2000) return;
        lobby.GD.tribute-=2000;

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
        if (lobby.GD.tribute<100) return;
        lobby.GD.tribute -= 100;
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
                    CardInfo carddata = new CardInfo();
                    lobby.CD.cardCost[i] = carddata.cd[lobby.CD.cardNo[i]].Cost;
                }
            }
        }
        lobby.GD.blessbool[curBlessList[rand]] = false;
        exitRemovePopup();
        lobby.Act();
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
                    CardInfo carddata = new CardInfo();
                    lobby.CD.cardCost[i] = carddata.cd[lobby.CD.cardNo[i]].Cost;
                }
            }
        }
         if (a == 20)
        {
            for (int i = 0; i < lobby.ChD.size; i++)
            {
                if (lobby.ChD.characterDatas[i].curFormation == 0)
                {
                    lobby.ChD.characterDatas[i].Atk++;
                }
                else
                {
                    lobby.ChD.characterDatas[i].Atk--;
                }
            }
        }
        exitRemovePopup();
        lobby.Act();
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
    public void TributeSet(int i)
    {
        noIgn.SetActive(false);
        Tribute+= i;
        if (Tribute > 2000) Tribute = 2000;
        if (Tribute < 200) Tribute = 200;
        ig.text = Tribute + "";
    }
    public void BlessPopupOn()
    {

        lobby.RitualViewOff();
        igObj.SetActive(true);
        Tribute = 200;
        ig.text = Tribute + "";
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
            if (blessNum == 19) {                
                lobby.bless19Count = 3;
                lobby.Bless19PopupOn();
                BlessPopup.SetActive(false);
                bless19_on = true;
            }
            blessNum = 0;
        }
        else
        {
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
        if (lobby.GD.tribute < Tribute&&!lobby.do_Resetmara)
        {
            noIgn.SetActive(true);
            return;
        }
        lobby.GD.tribute -= Tribute;
        noIgn.SetActive(false);
        int[] randSize = new int[]{ 800, 150, 50 };
       But[0].SetActive(false);
       But[1].SetActive(false);
       
       igObj.SetActive(false);
        BlessObj.SetActive(true);
        while (Tribute >= 1200)
        {
            Tribute -= 100;
            randSize[0] -= 20;
            randSize[1] += 10;
            randSize[2] += 10;
        }
        while(Tribute >= 400){
            Tribute -= 100;
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
       
        if (lobby.GD.blessSelect == 0)
        {
            blessT[0].text = bd.bd[randList[rand]].Name + "";
            blessT[1].text = bd.bd[randList[rand]].content + "";
            BlessApply(randList[rand]);
        }
        else
        {
            blessT[0].text = bd.bd[lobby.GD.blessSelect].Name + "";
            blessT[1].text = bd.bd[lobby.GD.blessSelect].content + "";
            BlessApply(lobby.GD.blessSelect);
        }
       // BlessApply(rand);
    }
    /*randList[rand]*/

    public void b6()
    {
        BlessPopup.SetActive(true);
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
                    bless6.transform.GetChild(i).GetChild(j).GetChild(0).GetComponent<Text>().text = CharacterInfo.Instance.cd[lobby.ChD.characterDatas[i].No].passive[j];
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
        lobby.GD.Ignum = 0;
    
        lobby.Act();
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

        lobby.Act();
        bless12.SetActive(false);
        b5list.Clear();
        b5cardList.Clear();
        setBlessIcon();
    }
    public void BlessApplyInResetmara(int newbless)
    {
        if (newbless==1)
        {
            lobby.GD.Ignum += 1000;
            for (int i = 0; i < lobby.ChD.size; i++)
            {
                lobby.ChD.characterDatas[i].curHp -= Mathf.FloorToInt(lobby.ChD.characterDatas[i].curHp * 0.3f);
            }

        }
        else if (newbless == 5)
        {
            blessNum = 5;
        }
        else if (newbless == 6)
        {           
            blessNum = 6;
        }
        else if (newbless == 8)
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
        }
        else if (newbless == 12)
        {
            blessNum = 12;
            lobby.GD.blessbool[12] = true;
        }
        else if (newbless == 14)
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
        else if (newbless == 18)
        {
            lobby.GD.Ignum -= 300;
            bless18_number = 0;
            Bless_18();
        }
        else if (newbless == 19)
        {
            blessNum = 19;
        }
        else if (newbless == 20)
        {
            for (int i = 0; i < lobby.ChD.size; i++)
            {
                if (lobby.ChD.characterDatas[i].curFormation == 0)
                {
                    lobby.ChD.characterDatas[i].Atk--;
                }
                else
                {
                    lobby.ChD.characterDatas[i].Atk++;
                }
            }
        }
        else {
            if (newbless == 3)
            {
                lobby.GD.bless3count = 2;
            }
            lobby.GD.blessbool[newbless] = true;
        }
        setBlessIcon();
    }
    public void BlessApply(int newbless)
    {
        if (newbless == 1)
        {
            lobby.GD.Ignum += 1000;
            for (int i = 0; i < lobby.ChD.size; i++)
            {
                lobby.ChD.characterDatas[i].curHp -= Mathf.FloorToInt(lobby.ChD.characterDatas[i].curHp * 0.3f);
                But[2].SetActive(true);
            }

        }
        else if (newbless == 5)
        {
            But[2].SetActive(true);
            blessNum = 5;
        }
        else if (newbless == 6)
        {
            But[2].SetActive(true);
            blessNum = 6;
        }

        else if (newbless == 8)
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
        else if (newbless == 12)
        {
            But[2].SetActive(true);
            blessNum = 12;
            lobby.GD.blessbool[12] = true;
        }
        else if (newbless == 14)
        {
            bless14.SetActive(true);
            for (int i = 0; i < 4; i++)
            {
                if (lobby.ChD.characterDatas[i].Name != null)
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
        else if (newbless == 20)
        {
            for (int i = 0; i < lobby.ChD.size; i++)
            {
                if (lobby.ChD.characterDatas[i].curFormation == 0)
                {
                    lobby.ChD.characterDatas[i].Atk--;
                }
                else
                {
                    lobby.ChD.characterDatas[i].Atk++;
                }
            }
            But[2].SetActive(true);
        }
        else if (newbless == 18)
        {
            lobby.GD.Ignum -= 300;
            exitBlessPopup();
            bless18_number = 0;
            Bless_18();
        }
        else if (newbless == 19)
        {
            But[2].SetActive(true);
            blessNum = 19;
        }
        else
        {
            if (newbless == 3)
            {
                lobby.GD.bless3count = 2;
            }
            lobby.GD.blessbool[newbless] = true;
            But[2].SetActive(true);
        }

        lobby.Act();
        setBlessIcon();
    }
    public void bless14But(int i)
    {
        lobby.ChD.characterDatas[i].Atk++;
        lobby.ChD.characterDatas[i].curHp = 1;
        exitBlessPopup();



            lobby.Act();
        bless14.SetActive(false);
        setBlessIcon();

    }
    public void Bless_18()
    {
        if (bless18_number < 3)
        {
            bless18_on = true;
            lobby.GetRandomEquipment();
            
            bless18_number++;
        }
        else
        {
            bless18_on = false;
            bless18_number = 0;
        }
    }
  public void SelectMorePassive(int i)
    {
  
        bless6.SetActive(false);
        exitBlessPopup();
        lobby.ChD.characterDatas[i / 4].passive[i % 4]++;
        lobby.ChD.characterDatas[i / 4].Atk -= 2;

            lobby.Act();
        setBlessIcon();
    }
    public int GetBlessInResetmara()
    {

        List<int> blessList = new List<int>();
        for(int i = 1; i < bd.bd.Length; i++)
        {
            if (bd.bd[i].level <= 3)
            {
                blessList.Add(i);
            }
        }
        return blessList[Random.Range(0,blessList.Count)];
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
    public BData[] bd = new BData[21]
    {
        new BData(0,"축복이름","축복내용",0,false),
        new BData(1,"찬란한 수확","1000 이그넘을 획득한다.\n모든 아군이 현재 체력의 30%를  잃는다.",1,false),
        new BData(2,"과도한 영접","전투 동안 , 무작위 후방 아군 한명의 공격력을 2올린다.\n해당 아군의 방어도는 올라가지 않는다.",1,true),
        new BData(3,"숨겨진 약점","턴 시작시 드로우를 8장 한다.\n전투가 2번 끝나고 해당 축복은 사라진다.",1,true),
        new BData(4,"은밀한 준비","첫 턴동안 모든 아군은 카드를 사용할 수 없다.\n두번째 턴부터 모든 아군의 SP-1.0.",2,true),
        new BData(5,"가벼운 몸짓","카드를 6장 선택해 소멸시킨다..\n모든 이그넘을 잃는다.",3,false),
        new BData(6,"인위적인 극복","아군 한명을 선택해 패시브를 늘린다. \n	해당 아군의 공격력 -2.",3,false),
        new BData(7,"역류하는 고통","전방에 있는 아군의 체력이 감소할 때마다 그 수치만큼 적 전체에게 데미지를 준다.",3,true),
        new BData(8,"정제된 결정","덱이 절반 소멸된다.",1,false),
        new BData(9,"깊어진 생각","카드 선택 중 취소버튼이 생긴다.\n이그넘의 절반을 잃는다.",1,true),
        new BData(10,"친절한 영기","아군 회복시 2명을 선택할 수 있다.",1,true),
        new BData(11,"성실한 행동","낮에 로비에서 한번 더 행동할 수 있다.",2,true),
        new BData(12,"무리한 예시","카드를 3장 선택해 코스트를 0으로 한다\n첫 턴동안 모든 아군은 카드를 사용할 수 없다.",1,true),
        new BData(13,"엮어낸 바늘","아군의 부활 비용이 0이그넘으로 변경한다.",2,true),
        new BData(14,"합리적인 저울","아군 한명의 공격력: +1\n해당 아군의 현재 체력이 1이 된다.",1,false),
        new BData(15,"쏟아지는 금화","전투로 얻는 이그넘을 3배 받는다.	\n파티원 회복/부활 기능을 사용할 수 없다.",1,true),
        new BData(16,"넓어진 시선","카드 선택시 보기가 최대 4장이 된다.",3,true),
        new BData(17,"무거운 어깨","모든 아군의 지구력이 1증가한다.\n모든 아군의 SP +0.2",2,true),
        new BData(18,"버려진 더미","무작위 장비 3개를 획득한다.\n300 이그넘을 잃는다.",1,false),
        new BData(19,"무구한 의도","장비를 3개까지 선택하고 무작위로 변경한다.",2,false),
        new BData(20,"확실한 위치","전방의 아군의 공격력을 1 감소한다.\n후방의 아군의 공격력을 1올린다.",3,true),
    };

    
}