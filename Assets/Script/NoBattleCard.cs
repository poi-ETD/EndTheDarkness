using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoBattleCard : MonoBehaviour
{
    int no=1;
    [SerializeField] TextMeshProUGUI[] CardInfo;
    CardData2 cd=new CardData2();
    CardSetManager csm;
    public int deckNo;
    public bool select;
    public void setCost(int i)
    {
        CardInfo[3].text = "" + i;//코스트
    }
    public void setCardInfo(int i)
    {
        csm = GameObject.Find("CardSetManager").GetComponent<CardSetManager>();   
        no = cd.cd[i].No;
        CardInfo[0].text=cd.cd[i].Name;//제목
        CardInfo[1].text = cd.cd[i].Content;//내용
        CardInfo[2].text="NO."+ cd.cd[i].No.ToString("D3");//넘버
        CardInfo[3].text = "" + cd.cd[i].Cost;//코스트
        string Name="";
        if (cd.cd[i].Deck==0)
        {
            Name = "BASE";
        }
        if (cd.cd[i].Deck == 1)
        {
            Name = "Q";
        }
        if (cd.cd[i].Deck ==2)
        {
            Name = "SPARKY";
        }
        if (cd.cd[i].Deck == 3)
        {
            Name = "VANGARA";
        }
        if (cd.cd[i].Deck == 4)
        {
            Name = "PORTE";
        }
        CardInfo[4].text = Name;
        CardInfo[5].text = csm.CardCount[no] + "";
    }
    public void SelectInBless5()
    {       
        if (select)
        {
            GameObject.Find("Bless").GetComponent<Bless>().blesscount--;
            transform.localScale /= 1.2f;
            select = false;
        }
        else
        {if (GameObject.Find("Bless").GetComponent<Bless>().curBless == 5)
            {
                if (GameObject.Find("Bless").GetComponent<Bless>().blesscount < 6)
                {
                    GameObject.Find("Bless").GetComponent<Bless>().blesscount++;
                    transform.localScale *= 1.2f;
                    select = true;
                }
            }
            if (GameObject.Find("Bless").GetComponent<Bless>().curBless == 12)
            {
                if (GameObject.Find("Bless").GetComponent<Bless>().blesscount < 3)
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
    }
    public void setCardInfoInLobby(int i,int d)
    {
        deckNo = d;
        no = cd.cd[i].No;
        CardInfo[0].text = cd.cd[i].Name;//제목
        CardInfo[1].text = cd.cd[i].Content;//내용
        CardInfo[2].text = "NO." + cd.cd[i].No.ToString("D3");//넘버
        string Name = "";
        if (cd.cd[i].Deck == 0)
        {
            Name = "BASE";
        }
        if (cd.cd[i].Deck == 1)
        {
            Name = "Q";
        }
        if (cd.cd[i].Deck == 2)
        {
            Name = "SPARKY";
        }
        if (cd.cd[i].Deck == 3)
        {
            Name = "VANGARA";
        }
        if (cd.cd[i].Deck == 4)
        {
            Name = "PORTE";
        }
        CardInfo[4].text = Name;
    }
    public void CardPlus(int i)
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
        CardInfo[5].text = csm.CardCount[no] + "";
        csm.AllCard += csm.CardCount[no];
    }
    public void CardSee()
    {
        GameObject.Find("LobbyManager").GetComponent<LobbyManager>().ThisCardSee(deckNo);
    }
    public void CardSeeInShop()
    {
        GameObject.Find("LobbyManager").GetComponent<LobbyManager>().CardSelectInShop(gameObject,no);
    }
}
