﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
public class LobbyManager : MonoBehaviour
{
   public CardData CD;
   public CharacterData ChD;
   public GameData GD=new GameData();
    CharacterData2 ChaInfo = new CharacterData2();
    CardData2 CaInfo = new CardData2();
    [SerializeField] GameObject PopUpCanvas;
    [SerializeField] GameObject day;
    [SerializeField] GameObject night;
    [SerializeField] GameObject cardView;
    [SerializeField] GameObject cardPrefebs;
    [SerializeField] GameObject cardContent;
    [SerializeField] GameObject OneCard;
    [SerializeField] GameObject OneCardCanvas;
    [SerializeField] GameObject CharacterView;
    public bool canvasOn;
    [SerializeField] TextMeshProUGUI[] mainDayAct;
    [SerializeField] TextMeshProUGUI[] mainNightAct;
    [SerializeField] Sprite[] CharacterSprtie;
    [SerializeField] GameObject LobbyCharacterPrefebs;
    [SerializeField] GameObject PassiveView;
    [SerializeField] GameObject[] ByPassive;
    [SerializeField] Text[] ByPassiveNames;
    [SerializeField] Text[] ByPassiveButtons;
    Dictionary<int, int[]> CardList = new Dictionary<int, int[]>(); //key->넘버 value->코스트
    [SerializeField] TextMeshProUGUI IgnumT;
    [SerializeField] TextMeshProUGUI GetItemIgnum;
    [SerializeField] GameObject ItemCanvas;
    [SerializeField] GameObject CardShopCanvas;
    bool isNoIgnum;
    [SerializeField] GameObject CardShopNoIgnum;
    GameObject ShopSelectedCard;
    [SerializeField] GameObject SelectCardCanvas;
    int ShopSelectedCardNo;
    [SerializeField] GameObject ShopPrefebs;
    List<int> RandomCardList = new List<int>();
    [SerializeField] GameObject cancleInShop;
    int SelectedCharacter;
    [SerializeField] GameObject[] ShopButtons;
    [SerializeField] TextMeshProUGUI curDay;
  
    private void Awake()
    {
        string path = Path.Combine(Application.persistentDataPath, "CardData.json");
        string path2 = Path.Combine(Application.persistentDataPath, "CharacterData.json");
        if (File.Exists(path))
        {
            string cardData = File.ReadAllText(path);
            CD = JsonConvert.DeserializeObject<CardData>(cardData);
        }
        if (File.Exists(path2))
        {
            string characterData = File.ReadAllText(path2);
            ChD = JsonConvert.DeserializeObject<CharacterData>(characterData);
        }
        string path3 = Path.Combine(Application.persistentDataPath, "GameData.json");
        if (File.Exists(path3))
        {
            string gameData = File.ReadAllText(path3);
            GD = JsonConvert.DeserializeObject<GameData>(gameData);
        }
        else
        {
            GD.Day = 1;
            save();
            Resetmara();
         
        }
        IgnumT.text = GD.Ignum + "";
        if (GD.isAct && !GD.isNight)
        {
            for (int i = 0; i < mainDayAct.Length; i++)
            {
                mainDayAct[i].color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
        if (!GD.isNight)
        {
            day.SetActive(true);
            night.SetActive(false);
        }
        else
        {
            day.SetActive(false);
            night.SetActive(true);
        }
        setDay();
    }
    void Resetmara()
    {

    }
    public void ShowCardList()
    {
        if (!canvasOn)
        {
            cardContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            canvasOn = true;
            PopUpCanvas.SetActive(true);
            cardView.SetActive(true);
            for (int i = 0; i < CD.cardNo.Count; i++)
            {
                GameObject newCard = Instantiate(cardPrefebs, cardContent.transform);
                newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(CD.cardNo[i], i);
                newCard.GetComponent<NoBattleCard>().setCost(CD.cardCost[i]);
                int[] k = { CD.cardNo[i], CD.cardCost[i] };
                CardList.Add(i, k);
            }
        }
    }//public void SetCharacterInLobby(int i, Sprite s,int curHp,int formation,int passive1,int passive2,int passive3,int passive4)
    public void OrderByCost()
    {
        var queryAsc = CardList.OrderBy(x => x.Value[1]);
        clear();        
        foreach (var dictionary in queryAsc)
        {
            GameObject newCard = Instantiate(cardPrefebs, cardContent.transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(dictionary.Value[0], dictionary.Key);
            newCard.GetComponent<NoBattleCard>().setCost(dictionary.Value[1]);
        }
    }
    public void OrderByNo()
    {
        var queryAsc = CardList.OrderBy(x => x.Value[0]);
        clear();
        foreach (var dictionary in queryAsc)
        {
            GameObject newCard = Instantiate(cardPrefebs, cardContent.transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(dictionary.Value[0], dictionary.Key);
            newCard.GetComponent<NoBattleCard>().setCost(dictionary.Value[1]);
        }
    }
    public void OrderByGet()
    {
        var queryAsc = CardList.OrderBy(x => x.Key);
        clear();
        foreach (var dictionary in queryAsc)
        {
            GameObject newCard = Instantiate(cardPrefebs, cardContent.transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(dictionary.Value[0], dictionary.Key);
            newCard.GetComponent<NoBattleCard>().setCost(dictionary.Value[1]);
        }
    }
    public void ShowCharacters()
    {
        if (!canvasOn)
        {
            canvasOn = true;
            PopUpCanvas.SetActive(true);
            CharacterView.SetActive(true);


            for (int i = 0; i < ChD.size; i++)
            {

                GameObject LC = Instantiate(LobbyCharacterPrefebs, CharacterView.transform);
                LC.GetComponent<CharacterSetting>().SetCharacterInLobby(ChD.characterDatas[i].No, CharacterSprtie[ChD.characterDatas[i].No],
                    ChD.characterDatas[i].Atk, ChD.characterDatas[i].Cost, ChD.characterDatas[i].curHp, ChD.characterDatas[i].maxHp,
                    ChD.characterDatas[i].curFormation, ChD.characterDatas[i].passive
                    ) ;
            }
        }
    }
    public void clear()
    {
        Transform[] childList = cardContent.GetComponentsInChildren<Transform>();

        for (int i = 1; i < childList.Length; i++)
        {
            Destroy(childList[i].gameObject);
        }
        childList = CharacterView.GetComponentsInChildren<Transform>();

        for (int i = 1; i < childList.Length; i++)
        {
            Destroy(childList[i].gameObject);
        }
    }
    public void ThisCardSee(int i)
    {
        
        OneCardCanvas.SetActive(true);
        OneCard.GetComponent<NoBattleCard>().setCardInfoInLobby(CD.cardNo[i],0);
        OneCard.GetComponent<NoBattleCard>().setCost(CD.cardCost[i]);
    }
    public void GetPassive()
    {   if (GD.isAct) return;
        if (canvasOn) return;
        PassiveView.SetActive(true);
        canvasOn = true;
        PopUpCanvas.SetActive(true);
       for(int i = 0; i < ChD.size; i++)
        {
            ByPassive[i].SetActive(true);
            ByPassiveNames[i].text = ChD.characterDatas[i].Name;
            ByPassiveButtons[i * 4].text = ChaInfo.cd[ChD.characterDatas[i].No].passive[0];
            ByPassiveButtons[i * 4+1].text = ChaInfo.cd[ChD.characterDatas[i].No].passive[1];
            ByPassiveButtons[i * 4+2].text = ChaInfo.cd[ChD.characterDatas[i].No].passive[2];
            ByPassiveButtons[i * 4+3].text = ChaInfo.cd[ChD.characterDatas[i].No].passive[3];
        }

    }
    public void GetItem()
    {   if (canvasOn) return;
        if (GD.isAct) return;
        canvasOn = true;
        ItemCanvas.SetActive(true);
        int Ig = 150 + GD.victory * 20;
        GetItemIgnum.text = Ig + "";      
    }
    public void SelectGetItem(bool t)
    {
        if (t)
        {
            int Ig = 150 + GD.victory * 20;
            GD.Ignum += Ig;
            IgnumT.text = GD.Ignum + "";
            DayAct();
        }
        ItemCanvas.SetActive(false);
        canvasOn = false;
    }
    public void GetPassiveButton(int i)
    {
        GameObject.Find("PassiveView").SetActive(false);
        PopUpCanvas.SetActive(false);
        ChD.characterDatas[i / 4].passive[i % 4]++;
        canvasOn = false;
        DayAct();
    }
    public void DayAct()
    {if (!GD.blessbool[11])
        { GD.isAct = true; }
        else
        {
            if (GD.isActInDay) { GD.isAct = true;
               GD.isActInDay = false;
            }
            else GD.isActInDay = true;
        }
        IgnumT.text = GD.Ignum + "";
        save();
        if (GD.isAct)
        {
            for (int i = 0; i < mainDayAct.Length; i++)
            {
                mainDayAct[i].color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
    }
    public void save()
    {
        string characterData = JsonConvert.SerializeObject(ChD);
        string path = Path.Combine(Application.persistentDataPath, "CharacterData.json");
        File.WriteAllText(path, characterData);
        string cardData = JsonConvert.SerializeObject(CD);
        path = Path.Combine(Application.persistentDataPath, "CardData.json");
        File.WriteAllText(path, cardData);
        string gameData = JsonConvert.SerializeObject(GD);
        path = Path.Combine(Application.persistentDataPath, "GameData.json");
        File.WriteAllText(path, gameData);
        IgnumT.text = GD.Ignum + "";
    }
    public void PopupOff()
    {
        CardList.Clear();
        clear();
        canvasOn = false;
    }
    public void goHome()
    {
        save();
        SceneManager.LoadScene("Main");
    }
    public void Actfalse()
    {
        day.SetActive(true);
        night.SetActive(false);
        GD.isNight = false;
        GD.isAct = false;
        GD.isActInDay = false;
        setDay();
        for (int i = 0; i < mainDayAct.Length; i++)
        {
            mainDayAct[i].color = new Color(1,1,1);
        }
    } //테스트용 실제 x
    public void CardShopPopupOn()
    {
        if (canvasOn) return;
        if (GD.isAct) return;
        for (int i = 0; i < ChD.size; i++)
        {
        
            ShopButtons[i].SetActive(true);
            ShopButtons[i].transform.GetChild(0).GetComponent<Text>().text = ChD.characterDatas[i].Name;
        }
        for (int i = ChD.size; i < 4; i++)
        {
            ShopButtons[i].SetActive(false);
        }
        canvasOn = true;
        CardShopCanvas.SetActive(true);
        PopUpCanvas.SetActive(true);
    }
    public void SelectCardShop(int ignum)
    {   if (isNoIgnum) return;
        if (GD.Ignum < ignum)
        {
            CardShopNoIgnum.SetActive(true);
            isNoIgnum = true;           
            return;
        }
  
        
        GD.Ignum -= ignum;
        IgnumT.text = GD.Ignum + "";
        PopUpCanvas.SetActive(false);
        CardShopCanvas.SetActive(false);
        canvasOn = false;
        CardShopSetting(ignum);
       // DayAct(); ->저장 포인트를 어디에 둘까?
    }
    public void CancleNoIgnum()
    {
        isNoIgnum = false;
    }
    public void CardSelectInShop(GameObject C,int no)
    {
        if (ShopSelectedCard == C)
        {
            C.transform.localScale /= 1.2f;
            ShopSelectedCard = null;
            return;
        }
        if(ShopSelectedCard!=null)
        ShopSelectedCard.transform.localScale /= 1.2f;
        C.transform.localScale *= 1.2f;
        ShopSelectedCard = C;
        ShopSelectedCardNo = no;
        
    }
    public void CardSelectInList(bool t)
    {if (ShopSelectedCard == null&&t) return;
        if (t)
        {
            CD.cardNo.Add(ShopSelectedCardNo);
            CD.cardCost.Add(CaInfo.cd[ShopSelectedCardNo].Cost);
            CD.cardGet.Add(CD.get);
            CD.get++;

        }
       cancleInShop.SetActive(true);
        Transform[] childList = GameObject.Find("ShopList").GetComponentsInChildren<Transform>();
        for (int i = 1; i < childList.Length; i++)
        {
            Destroy(childList[i].gameObject);
        }
        RandomCardList.Clear();
        SelectCardCanvas.SetActive(false);
        DayAct();
    }
    public void SelectCardShop3(int c)
    {
        SelectedCharacter = c;
    }
    public void CardShopSetting(int type)
    {
        SelectCardCanvas.SetActive(true);
        if (type == 10) { cancleInShop.SetActive(false);
        for(int i = 1; i < CaInfo.cd.Length; i++)
            {
                for(int j = 0; j < ChD.characterDatas.Length; j++)
                {
                    if (CaInfo.cd[i].Deck == ChD.characterDatas[j].No)
                    {if(CaInfo.cd[i].type!=2)
                        RandomCardList.Add(i);//
                        break;
                    }
                }
            }
            int r = Random.Range(0, RandomCardList.Count);
            GameObject newCard = Instantiate(ShopPrefebs, GameObject.Find("ShopList").transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(RandomCardList[r], 0);
        }
        else if (type == 100)
        {
            for (int i = 1; i < CaInfo.cd.Length; i++)
            {
                for (int j = 0; j < ChD.characterDatas.Length; j++)
                {
                    if (CaInfo.cd[i].Deck == ChD.characterDatas[j].No||CaInfo.cd[i].Deck==0)
                    {                      
                            if (CaInfo.cd[i].type != 2)
                                RandomCardList.Add(i);//
                        break;
                    }
                }
            }
            int r = Random.Range(0, RandomCardList.Count);
            GameObject newCard = Instantiate(ShopPrefebs, GameObject.Find("ShopList").transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(RandomCardList[r], 0);
            int f = r;
            while(r==f) r = Random.Range(0, RandomCardList.Count);
            newCard = Instantiate(ShopPrefebs, GameObject.Find("ShopList").transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(RandomCardList[r], 0);
            int s = r;
            while (r == f||s==r) r = Random.Range(0, RandomCardList.Count);
            newCard = Instantiate(ShopPrefebs, GameObject.Find("ShopList").transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(RandomCardList[r], 0);
        }
        else if (type == 300)
        {
            for (int i = 1; i < CaInfo.cd.Length; i++)
            {

                if (CaInfo.cd[i].Deck == ChD.characterDatas[SelectedCharacter].No)
                {
                    if (CaInfo.cd[i].type != 2)
                        RandomCardList.Add(i);//


                }
            }
            int r = Random.Range(0, RandomCardList.Count);
            GameObject newCard = Instantiate(ShopPrefebs, GameObject.Find("ShopList").transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(RandomCardList[r], 0);
            int f = r;
            while (r == f) r = Random.Range(0, RandomCardList.Count);
            newCard = Instantiate(ShopPrefebs, GameObject.Find("ShopList").transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(RandomCardList[r], 0);
            int s = r;
            while (r == f || s == r) r = Random.Range(0, RandomCardList.Count);
            newCard = Instantiate(ShopPrefebs, GameObject.Find("ShopList").transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(RandomCardList[r], 0);
        }
    }
    public void NextTurn()
    {
        if (!GD.isNight)
        {
            save();
            day.SetActive(false);
            night.SetActive(true);
            GD.isAct = false;
            GD.isNight = true;
            setDay();
        }
        else
        {
            save();
            SceneManager.LoadScene("battle");

        }
    }
    public void setDay()
    { string n = "DAY";
        if (GD.isNight) n = "Night";
        curDay.text = n + GD.Day;
    }
}