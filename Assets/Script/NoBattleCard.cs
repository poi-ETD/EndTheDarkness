using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoBattleCard : MonoBehaviour //대략적으로 배틀이 아닐 때 카드를 관리하기 위함 ex)로비나, 세팅
{
    int no=1;
    [SerializeField] TextMeshProUGUI[] CardInfoText;
   
    CardSetManager csm;
    public int deckNo;
    public bool select;
    public void setCost(int cost)
    {
        CardInfoText[3].text = "" + cost;//코스트
    }
    public void setCardInfo(int i)
    {
        csm = GameObject.Find("CardSetManager").GetComponent<CardSetManager>();   
        no = CardInfo.Instance.cd[i].No;
        CardInfoText[0].text=CardInfo.Instance.cd[i].Name;//이름
        CardInfoText[1].text = CardInfo.Instance.cd[i].Content;//내용
        CardInfoText[2].text="NO."+ CardInfo.Instance.cd[i].No.ToString("D3");//넘버
        CardInfoText[3].text = "" + CardInfo.Instance.cd[i].Cost;//코스트
        string Name="";
        if (CardInfo.Instance.cd[i].Deck==0)
        {
            Name = "BASE";
        }
        if (CardInfo.Instance.cd[i].Deck == 1)
        {
            Name = "Q";
        }
        if (CardInfo.Instance.cd[i].Deck ==2)
        {
            Name = "SPARKY";
        }
        if (CardInfo.Instance.cd[i].Deck == 3)
        {
            Name = "VANGARA";
        }
        if (CardInfo.Instance.cd[i].Deck == 4)
        {
            Name = "PORTE";
        }
        if (CardInfo.Instance.cd[i].Deck == 5)
        {
            Name = "RHYNG";
        }
        CardInfoText[4].text = Name;
        CardInfoText[5].text = csm.CardCount[no] + "";
    }
    public void SelectInBless() //축복 5나 12 캔버스에서 선택.
    {       
        if (select) //선택되면 크게
        {
            GameObject.Find("Bless").GetComponent<Bless>().blesscount--;
            transform.localScale /= 1.2f;
            select = false;
        }
        else
        {if (GameObject.Find("Bless").GetComponent<Bless>().curBless == 5)
            {
                if (GameObject.Find("Bless").GetComponent<Bless>().blesscount < 6) //최대 6장 가능하기 때문
                {
                    GameObject.Find("Bless").GetComponent<Bless>().blesscount++;
                    transform.localScale *= 1.2f;
                    select = true;
                }
            }
            if (GameObject.Find("Bless").GetComponent<Bless>().curBless == 12)
            {
                if (GameObject.Find("Bless").GetComponent<Bless>().blesscount < 3) //최대 3장이 가능하기 떄문
                {
                    GameObject.Find("Bless").GetComponent<Bless>().blesscount++;
                    transform.localScale *= 1.2f;
                    select = true;
                }
            }
        }
        if (GameObject.Find("Bless").GetComponent<Bless>().curBless == 5)
            GameObject.Find("Bless").GetComponent<Bless>().bless5countt.text = "선택된 카드 수 :"+GameObject.Find("Bless").GetComponent<Bless>().blesscount;
        if (GameObject.Find("Bless").GetComponent<Bless>().curBless == 12)
            GameObject.Find("Bless").GetComponent<Bless>().bless12countt.text = "선택된 카드 수 :" + GameObject.Find("Bless").GetComponent<Bless>().blesscount;
   //축복 5일떄, 12일 때
    
    }
    public void inRemove() //카드 삭제 캔버스에서
    {
        if (select)
        {
            GameObject.Find("LobbyManager").GetComponent<LobbyManager>().removeCount--;
            transform.localScale /= 1.2f;
            select = false;
        }
        else
        {
           
                if (GameObject.Find("LobbyManager").GetComponent<LobbyManager>().removeCount< GameObject.Find("LobbyManager").GetComponent<LobbyManager>().maxRemoveCount)
                {
                GameObject.Find("LobbyManager").GetComponent<LobbyManager>().removeCount++;
                    transform.localScale *= 1.2f;
                    select = true;
                }
            }
          
        }
    public void inReward() //배틀 승리 후 보상 선택 캔버스에서.
    {
        if (select)
        {
            GameObject.Find("BattleManager").GetComponent<BattleManager>().SelectedRewardCount--;
            transform.localScale /= 1.2f;
            select = false;
        }
        else
        {

            if (GameObject.Find("BattleManager").GetComponent<BattleManager>().SelectedRewardCount<1)
            {
                GameObject.Find("BattleManager").GetComponent<BattleManager>().SelectedRewardCount++;
                transform.localScale *= 1.2f;
                select = true;
            }
        }
    }
   
    public void setCardInfoInLobby(int num,int d) //로비에서 카드 목록을 볼 때
    {
        Debug.Log(num);
        deckNo = d;
        no = CardInfo.Instance.cd[num].No;
        CardInfoText[0].text = CardInfo.Instance.cd[num].Name;//제목
        CardInfoText[1].text = CardInfo.Instance.cd[num].Content;//내용
        CardInfoText[2].text = "NO." + CardInfo.Instance.cd[num].No.ToString("D3");//넘버
        string Name = "";
        if (CardInfo.Instance.cd[num].Deck == 0)
        {
            Name = "BASE";
        }
        if (CardInfo.Instance.cd[num].Deck == 1)
        {
            Name = "Q";
        }
        if (CardInfo.Instance.cd[num].Deck == 2)
        {
            Name = "SPARKY";
        }
        if (CardInfo.Instance.cd[num].Deck == 3)
        {
            Name = "VANGARA";
        }
        if (CardInfo.Instance.cd[num].Deck == 4)
        {
            Name = "PORTE";
        }
        if (CardInfo.Instance.cd[num].Deck == 4)
        {
            Name = "RHYNG";
        }
        CardInfoText[4].text = Name;
    }
    public void CardPlus(int i) //서브 세팅에서 캐릭터별 카드 고를 때 PLUS혹은 MINUS
    {
        csm.AllCard -= csm.CardCount[no]; 
        if (i == 1)
        {
            if (csm.CardCount[no] < 5 && csm.curCardCount < csm.maxiumCardCount)
            {
                csm.curCardCount++;
                csm.CardCount[no]++;
            }
            else
            {
                csm.CardOver();
            }
        }
        else
        {
            if (csm.CardCount[no] >0 && csm.curCardCount >0)
            {
                csm.curCardCount--;
                csm.CardCount[no]--;
            }
            else
            {
                csm.CardOver();
            }
        }
        CardInfoText[5].text = csm.CardCount[no] + "";
        csm.AllCard += csm.CardCount[no];
    }
    public void CardSee() //로비 카드 보기에서 누르면 크게 적용
    {
        GameObject.Find("LobbyManager").GetComponent<LobbyManager>().ThisCardSee(deckNo);
    }
    public void CardSeeInShop() //상점에서 선택할 시
    {
        GameObject.Find("LobbyManager").GetComponent<LobbyManager>().CardSelectInShop(gameObject,no);
    }
}
