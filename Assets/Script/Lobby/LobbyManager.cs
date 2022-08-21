using System.Collections;
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
    public GameData GD = new GameData();
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
    [SerializeField] TextMeshProUGUI TributeT;
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
    [SerializeField] GameObject CardRemovePopup;
    [SerializeField] TextMeshProUGUI[] cardRemoveText;
    [SerializeField] GameObject RemoveCardPrefebs;
    [SerializeField] GameObject RemoveContent;
    [SerializeField] GameObject PassivePopup;
    [SerializeField] GameObject noIgnumInPassive;
    [SerializeField] GameObject noStackInPassive;
    [SerializeField] TextMeshProUGUI curStack;
    [SerializeField] GameObject ResetCanvas;
    public int ResetMaraCount;
    [SerializeField] GameObject ResetBless;
    [SerializeField] GameObject ResetItem;


    [SerializeField] GameObject GetEquipmentCanvas;
    [SerializeField] TextMeshProUGUI[] equipStrings;
    equipment curEquip;

    [SerializeField] GameObject EquipmentView;
    [SerializeField] GameObject EquipmentContent;
    int curEquipNumber;
    [SerializeField] GameObject EquipCharacterView;
    bool equipmode;
    [SerializeField] GameObject EquipManageView;
    [SerializeField] GameObject EquipManageContent;
    [SerializeField] List<int> SelectedEquipList = new List<int>();
    [SerializeField] GameObject[] SelectedEquipImage;
    [SerializeField] GameObject EquipManageButton;

    [SerializeField] GameObject tributeView; //공물

    private void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CardDatas[GameManager.Instance.nowPlayingSlot]);
        string path2 = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CharacterDatas[GameManager.Instance.nowPlayingSlot]);
        string path3 = Path.Combine(Application.persistentDataPath, "GameData.json");
        if (File.Exists(path))
        {
            Debug.Log(path);
            string cardData = File.ReadAllText(path);
            CD = JsonConvert.DeserializeObject<CardData>(cardData);
        }
        if (File.Exists(path2))
        {
            string characterData = File.ReadAllText(path2);
            ChD = JsonConvert.DeserializeObject<CharacterData>(characterData);
        }
        if (File.Exists(path3))
        {
            string gameData = File.ReadAllText(path3);
            GD = JsonConvert.DeserializeObject<GameData>(gameData);
        }
        else
        { //path3->이 없다면, 첫 시작이기 때문에 리세마라 시작
            GD.Day = 1; 
            Resetmara(ResetMaraCount);

        }
        IgnumT.text = GD.Ignum + "";
        TributeT.text = GD.tribute + "";
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
            if (GD.isAct)
            {
                for (int i = 0; i < mainNightAct.Length; i++)
                {
                    mainNightAct[i].color = new Color(0.5f, 0.5f, 0.5f);
                }
            }
            day.SetActive(false);
            night.SetActive(true);
        }
        setDay();
    }

    public void Resetmara(int rc)
    {
        if (rc < ChD.size)
        {
            canvasOn = true;
            ResetCanvas.SetActive(true);
            resetmara = true;
            for (int i = 1; i < CardInfo.Instance.cd.Length; i++)
            {
                if (CardInfo.Instance.cd[i].Deck == ChD.characterDatas[rc].No)
                {
                    if (CardInfo.Instance.cd[i].type != 2)
                        RandomCardList.Add(i);//
                }
            }
            int r = Random.Range(0, RandomCardList.Count);
            
            GameObject newCard = Instantiate(ShopPrefebs, GameObject.Find("ResetShop").transform.GetChild(0).transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(RandomCardList[r], 0);
            int f = r;
            while (r == f) r = Random.Range(0, RandomCardList.Count);
            newCard = Instantiate(ShopPrefebs, GameObject.Find("ResetShop").transform.transform.GetChild(0).transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(RandomCardList[r], 0);
            int s = r;
            if (RandomCardList.Count > 2)
            {
                while (r == f || s == r) r = Random.Range(0, RandomCardList.Count);
                newCard = Instantiate(ShopPrefebs, GameObject.Find("ResetShop").transform.transform.GetChild(0).transform);
                newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(RandomCardList[r], 0);
            }
        }
        else if (rc == ChD.size)
        {
            canvasOn = false;
            GameObject.Find("Bless").GetComponent<Bless>().BlessPopupOn();
            GameObject.Find("Bless").GetComponent<Bless>().GetBless();
        }
        else if (rc == 5)
        {
            canvasOn = true;
            GetRandomEquipment();
        }
        else if (rc == 6)
        {
            canvasOn = true;
            ResetMaraCount = 5;
            ResetBless.SetActive(false);
            ResetItem.SetActive(true);
        }
        else if (rc == 7)
        {
            canvasOn = false;
            GD.Ignum += 500;
            GD.tribute += 300;
            IgnumT.text = "" + GD.Ignum;
            resetmara = false;          
            canvasOn = false;
            ResetCanvas.SetActive(false);
            save();
        }
    }

    public bool resetmara;

    public void CardSelectInReset(bool t)
    {
        if (ShopSelectedCard == null && t) return;
        if (t)
        {
            CD.cardNo.Add(ShopSelectedCardNo);
            CD.cardCost.Add(CardInfo.Instance.cd[ShopSelectedCardNo].Cost);
            CD.cardGet.Add(CD.get);
            CD.get++;
        }            
        Transform[] childList = GameObject.Find("ShopList").GetComponentsInChildren<Transform>();
        for (int i = 1; i < childList.Length; i++)
        {
            Destroy(childList[i].gameObject);
        }
        RandomCardList.Clear();
        if(ResetMaraCount>ChD.size-2)GameObject.Find("ResetShop").SetActive(false);    
        ResetMaraCount++;
        Resetmara(ResetMaraCount);
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
                string s1 = "";
                string s2 = "";
                Sprite spr = Resources.Load<Sprite>("temporal/x_01");
                if (ChD.characterDatas[i].curEquip == -1)
                {
                    s1 = "장비 없음";
                   
                }
                else
                {
                    equipment e = GD.EquipmentList[ChD.characterDatas[i].curEquip];
                    List<string> sList = EquipmentManager.Instance.equipmentStrings(e);
                    s1 = sList[0];
                    if (e.special == 0)
                        s2 = sList[1] + '\n' + sList[2] + '\n' + sList[3];
                    else s2 = sList[1];
                    spr = EquipmentManager.Instance.equipSpr[e.equipNum];
                }
                CharacterView.transform.GetChild(i).gameObject.SetActive(true);
                CharacterView.transform.GetChild(i).gameObject.GetComponent<CharacterSetting>().SetCharacterInLobby(ChD.characterDatas[i].No, CharacterInfo.Instance.cd[ChD.characterDatas[i].No].characterSprtie,
                    ChD.characterDatas[i].Atk, ChD.characterDatas[i].def, ChD.characterDatas[i].Cost, ChD.characterDatas[i].curHp, ChD.characterDatas[i].maxHp,
                    ChD.characterDatas[i].curFormation, ChD.characterDatas[i].passive,s1,s2,spr
                    );
            }
        }
    }


    public void ShowEquipment()
    {
        if (canvasOn) return;
        canvasOn = true;
        PopUpCanvas.SetActive(true);
        EquipmentView.SetActive(true);
        equipmode = false;
        for(int i = 0; i < GD.EquipmentList.Count; i++)
        {
            equipment e = GD.EquipmentList[i];
            List<string> sList = EquipmentManager.Instance.equipmentStrings(e);
            EquipmentContent.transform.GetChild(i).gameObject.SetActive(true);
            EquipmentContent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = EquipmentManager.Instance.equipSpr[e.equipNum];
            EquipmentContent.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = sList[0];
            if (e.special == 0)
                EquipmentContent.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = sList[1] + '\n'
                    + sList[2] + '\n' + sList[3];
            else EquipmentContent.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = sList[1] + '\n'


;            EquipmentContent.transform.GetChild(i).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "착용";
        }
        for(int i = GD.EquipmentList.Count; i < 50; i++)
        {
            EquipmentContent.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < ChD.size; i++)
        {
            if (ChD.characterDatas[i].curEquip != -1)
            {
                EquipmentContent.transform.GetChild(ChD.characterDatas[i].curEquip).GetChild(3).GetChild(0).
                    GetComponent<TextMeshProUGUI>().text = ChD.characterDatas[i].Name+" 착용 해제";
            }
        }
    }
    public void EquipThis(GameObject me)
    {
        int num = me.name[7]-48;
        if (me.name[8] != 41)
        {
            num *= 10;
            num += me.name[8] - 48;
        }
        if (equipmode) return;
        bool isEquip = false;
        for(int i = 0; i < ChD.size; i++)
        {
            if (ChD.characterDatas[i].curEquip == num) isEquip = true;
        }
        if (isEquip)
        {
            for (int i = 0; i < ChD.size; i++)
            {
                if (ChD.characterDatas[i].curEquip == num) {
                    ChD.characterDatas[i].curEquip = -1;
                }
               
            }
            canvasOn = false;
            ShowEquipment();
        }
        else
        {
            equipmode = true;
            curEquipNumber = num;
            EquipCharacterView.SetActive(true);
            for (int i = 0; i < ChD.size; i++)
            {
                EquipCharacterView.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                EquipCharacterView.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = ChD.characterDatas[i].Name;
            }
        }
    }
    public void CancleEquip()
    {
        EquipCharacterView.SetActive(false);
        canvasOn = false;
        ShowEquipment();
    }
    public void EquipToThisCharacter(int num)
    {
        ChD.characterDatas[num].curEquip = curEquipNumber;
        CancleEquip();
    }
    public void clear()
    {
        Transform[] childList = cardContent.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childList.Length; i++)
        {
            Destroy(childList[i].gameObject);
        }
        
    }
    public void ThisCardSee(int i)
    {
        OneCardCanvas.SetActive(true);
        OneCard.GetComponent<NoBattleCard>().setCardInfoInLobby(CD.cardNo[i], 0);
        OneCard.GetComponent<NoBattleCard>().setCost(CD.cardCost[i]);
    }
    public void GetPassiveInGM() //개발용
    {
        if (canvasOn) return;

        StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene4_GMmode"));
        //SceneManager.LoadScene("GMmode");

    }
    public void GetPassive()
    {
        if (GD.isAct) return;
        if (canvasOn) return;
        PassiveView.SetActive(true);
        canvasOn = true;
        PopUpCanvas.SetActive(true);
        for (int i = 0; i < ChD.size; i++)
        {
            ByPassive[i].SetActive(true);
            ByPassiveNames[i].text = ChD.characterDatas[i].Name;
            ByPassiveButtons[i * 4].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].No].passive[0];
            ByPassiveButtons[i * 4 + 1].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].No].passive[1];
            ByPassiveButtons[i * 4 + 2].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].No].passive[2];
            ByPassiveButtons[i * 4 + 3].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].No].passive[3];
        }
    }
    public void GetItem()
    {
        if (canvasOn) return;
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
        GD.passiveStack -= 15;
        canvasOn = false;
        DayAct();
    }
    public void DayAct()
    {
        if (!GD.isNight)
        {
            if (!GD.blessbool[11])
            { GD.isAct = true; }
            else
            {
                if (GD.isActInDay)
                {
                    GD.isAct = true;
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
        else
        {
            GD.isAct = true;
            IgnumT.text = GD.Ignum + "";
            save();
            if (GD.isAct)
            {
                for (int i = 0; i < mainNightAct.Length; i++)
                {
                    mainNightAct[i].color = new Color(0.5f, 0.5f, 0.5f);
                }
            }
        }
    }

    public void save()
    {
        string characterData = JsonConvert.SerializeObject(ChD);
        string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CharacterDatas[GameManager.Instance.nowPlayingSlot]);
        File.WriteAllText(path, characterData);

        string cardData = JsonConvert.SerializeObject(CD);
        path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CardDatas[GameManager.Instance.nowPlayingSlot]);
        File.WriteAllText(path, cardData);

        string gameData = JsonConvert.SerializeObject(GD);
        path = Path.Combine(Application.persistentDataPath, "GameData.json");
        File.WriteAllText(path, gameData);

        IgnumT.text = GD.Ignum + "";
        TributeT.text = GD.tribute + "";
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
        StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene1_Main"));
        //SceneManager.LoadScene("Scene1_Main");
    }
    public void Actfalse()
    {
        day.SetActive(true);
        night.SetActive(false);
        GD.isNight = false;
        GD.isAct = false;
        GD.isActInDay = false;
        setDay();
        for (int i = 0; i < mainNightAct.Length; i++)
        {
            mainNightAct[i].color = new Color(1, 1, 1);
        }
        for (int i = 0; i < mainDayAct.Length; i++)
        {
            mainDayAct[i].color = new Color(1, 1, 1);
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
    {
        if (isNoIgnum) return;
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
    public void CardSelectInShop(GameObject C, int no)
    {
        if (ShopSelectedCard == C)
        {
            C.transform.localScale /= 1.2f;
            ShopSelectedCard = null;
            return;
        }
        if (ShopSelectedCard != null)
            ShopSelectedCard.transform.localScale /= 1.2f;
        C.transform.localScale *= 1.2f;
        ShopSelectedCard = C;
        ShopSelectedCardNo = no;

    }
    public void CardSelectInList(bool t)
    {
        if (ShopSelectedCard == null && t) return;
        if (t)
        {
            CD.cardNo.Add(ShopSelectedCardNo);
            CD.cardCost.Add(CardInfo.Instance.cd[ShopSelectedCardNo].Cost);
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
        if (type == 10)
        {
            cancleInShop.SetActive(false);
            for (int i = 1; i < CardInfo.Instance.cd.Length; i++)
            {
                for (int j = 0; j < ChD.characterDatas.Length; j++)
                {
                    if (CardInfo.Instance.cd[i].Deck == ChD.characterDatas[j].No)
                    {
                        if (CardInfo.Instance.cd[i].type != 2)
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
            for (int i = 1; i < CardInfo.Instance.cd.Length; i++)
            {
                for (int j = 0; j < ChD.characterDatas.Length; j++)
                {
                    if (CardInfo.Instance.cd[i].Deck == ChD.characterDatas[j].No || CardInfo.Instance.cd[i].Deck == 0)
                    {
                        if (CardInfo.Instance.cd[i].type != 2)
                            RandomCardList.Add(i);//
                        break;
                    }
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
        else if (type == 300)
        {
            for (int i = 1; i < CardInfo.Instance.cd.Length; i++)
            {

                if (CardInfo.Instance.cd[i].Deck == ChD.characterDatas[SelectedCharacter].No)
                {
                    if (CardInfo.Instance.cd[i].type != 2)
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
    {if (canvasOn) return;
        if (!GD.isNight)
        {
            day.SetActive(false);
            night.SetActive(true);
            GD.isAct = false;
            GD.isNight = true;
            setDay();
            save();
        }
        else
        {
            GD.isAct = true;
            save();
            StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene5_Battle"));
            //SceneManager.LoadScene("Scene3_Battle");
        }
    }
    public void click_button_GMmode()
    {
        StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene4_GMmode"));
    }
    public void getMoney()
    {
        GD.Ignum += 50000;
        IgnumT.text = GD.Ignum + "";
    }
    public void setDay()
    {
        string n = "DAY";
        if (GD.isNight) n = "Night";
        curDay.text = n + GD.Day;
    }
    public GameObject removeCard;
    public int removeCount;
    public int maxRemoveCount;
    Dictionary<int, int[]> removeCardList = new Dictionary<int, int[]>(); //key->넘버 value->코스트
    List<GameObject> objRemovecardList = new List<GameObject>();
    public void removeCardPopupOn(int n)
    {
        if (canvasOn) return;
        if (GD.isAct) return;
        canvasOn = true;
        CardRemovePopup.SetActive(true);
        cardRemoveText[0].text = "삭제할 카드를 " + n + "장 선택해 주세요.";
        maxRemoveCount = n;
        for (int i = 0; i < CD.cardNo.Count; i++)
        {
            GameObject newCard = Instantiate(RemoveCardPrefebs, RemoveContent.transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(CD.cardNo[i], i);
            newCard.GetComponent<NoBattleCard>().setCost(CD.cardCost[i]);
            int[] k = { CD.cardNo[i], CD.cardCost[i] };
            removeCardList.Add(i, k);
            objRemovecardList.Add(newCard);
        }
    }
    public void OrderByCostinRemove()
    {
        var queryAsc = CardList.OrderBy(x => x.Value[1]);
        int count = 0;
        foreach (var dictionary in queryAsc)
        {

            objRemovecardList[dictionary.Key].transform.SetSiblingIndex(count);
            count++;
        }
    }
    public void OrderByNoinRemove()
    {
        var queryAsc = CardList.OrderBy(x => x.Value[0]);
        int count = 0;
        foreach (var dictionary in queryAsc)
        {

            objRemovecardList[dictionary.Key].transform.SetSiblingIndex(count);
            count++;
        }
    }
    public void OrderByGetinRemove()
    {
        var queryAsc = CardList.OrderBy(x => x.Key);
        int count = 0;
        foreach (var dictionary in queryAsc)
        {

            objRemovecardList[dictionary.Key].transform.SetSiblingIndex(count);
            count++;
        }
    }
    public void Remove()
    {
        List<int> removeList = new List<int>();
        if (removeCount < maxRemoveCount) return;
        for (int i = 0; i < CD.cardNo.Count; i++)
        {

            if (RemoveContent.transform.GetChild(i).gameObject.GetComponent<NoBattleCard>().select)
                removeList.Add(RemoveContent.transform.GetChild(i).gameObject.GetComponent<NoBattleCard>().deckNo);
        }
        removeList.Sort();

        for (int i = maxRemoveCount - 1; i >= 0; i--)
        {
            CD.cardNo.RemoveAt(removeList[i]);
            CD.cardCost.RemoveAt(removeList[i]);
            CD.cardGet.RemoveAt(removeList[i]);
        }
        canvasOn = false;
        DayAct();
        CardRemovePopup.SetActive(false);
        removeCardList.Clear();
        objRemovecardList.Clear();
        Transform[] childList = RemoveContent.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childList.Length; i++)
        {
            Destroy(childList[i].gameObject);
        }
    }
    public void exitRemove()
    {
        Transform[] childList = RemoveContent.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childList.Length; i++)
        {
            Destroy(childList[i].gameObject);
        }
        CardRemovePopup.SetActive(false);
        canvasOn = false;
        removeCardList.Clear();
        objRemovecardList.Clear();
    }
    public void GetPassivePopup()
    {
        if (canvasOn) return;
        if (GD.isAct) return;
        canvasOn = true;
        PassivePopup.SetActive(true);
        PopUpCanvas.SetActive(true);
        curStack.text = "현재스택:" + GD.passiveStack;
    }
    public void GetStack(int n)
    {
        if (n == 1)
        {
            if (GD.tribute < 500)
            {
                noIgnumInPassive.SetActive(true);
                return;
            }
            GD.tribute-=500;
            GD.passiveStack += n;
        }
        if (n == 3)
        {
            if (GD.tribute < 1500)
            {
                noIgnumInPassive.SetActive(true);
                return;
            }
            GD.tribute -= 1500;
            GD.passiveStack += n;
        }
        canvasOn = false;
        DayAct();
        PassivePopup.SetActive(false);
        PopUpCanvas.SetActive(false);
        noStackInPassive.SetActive(false);
        noStackInPassive.SetActive(false);
    }
    public void getRandomPassive()
    {
        if (GD.passiveStack < 5)
        {
            noStackInPassive.SetActive(true);
            return;
        }
        GD.passiveStack -= 5;
        int rand = Random.Range(0, ChD.size * 4);
        ChD.characterDatas[rand / 4].passive[rand % 4]++;
        canvasOn = false;
        DayAct();
        PassivePopup.SetActive(false);
        PopUpCanvas.SetActive(false);
        noStackInPassive.SetActive(false);
        noStackInPassive.SetActive(false);
    }
    public void GetRandomEquipment()
    {
        canvasOn = true;
        GetEquipmentCanvas.SetActive(true);
        curEquip=EquipmentManager.Instance.makeEquipment();
        GetEquipmentCanvas.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = EquipmentManager.Instance.equipSpr[curEquip.equipNum];
        List<string> sList = EquipmentManager.Instance.equipmentStrings(curEquip);
        equipStrings[0].text = sList[0];
        equipStrings[1].text = sList[1] + '\n' + sList[2] + '\n' + sList[3];
        GD.EquipmentList.Add(curEquip);
    }
    public void CloseRandomEquipment()
    {
        canvasOn = false;
        GetEquipmentCanvas.SetActive(false);
        if (resetmara) Resetmara(6);
    }
    public void GetSelectPassive() //개발용
    {
        if (GD.passiveStack < 15)
        {
            noStackInPassive.SetActive(true);
            return;
        }
        PassivePopup.SetActive(false);
        PassiveView.SetActive(true);
        PopUpCanvas.SetActive(true);
        for (int i = 0; i < ChD.size; i++)
        {
            ByPassive[i].SetActive(true);
            ByPassiveNames[i].text = ChD.characterDatas[i].Name;
            ByPassiveButtons[i * 4].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].No].passive[0];
            ByPassiveButtons[i * 4 + 1].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].No].passive[1];
            ByPassiveButtons[i * 4 + 2].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].No].passive[2];
            ByPassiveButtons[i * 4 + 3].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].No].passive[3];
        }
    }
    
    public void EquipManageViewOn()
    {
        if (canvasOn) return;
        if (GD.isAct) return;
        canvasOn = true;
        SelectedEquipList.Clear();
        EquipManageView.SetActive(true);
        PopUpCanvas.SetActive(true);
        SetEquipListImage();
        for (int i = 0; i < GD.EquipmentList.Count; i++)
        {
            equipment e = GD.EquipmentList[i];
            List<string> sList = EquipmentManager.Instance.equipmentStrings(e);
            EquipManageContent.transform.GetChild(i).gameObject.SetActive(true);
            EquipManageContent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = EquipmentManager.Instance.equipSpr[e.equipNum];
            EquipManageContent.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = sList[0];
            if (e.special == 0)
            {
                EquipManageContent.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = sList[1] + '\n'
                      + sList[2] + '\n' + sList[3];
                EquipManageContent.transform.GetChild(i).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "선택";
            }
            else
            {
                EquipManageContent.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = sList[1];
                EquipManageContent.transform.GetChild(i).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "선택불가";
            }
        }
        for (int i = GD.EquipmentList.Count; i < 50; i++)
        {
            EquipManageContent.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < ChD.size; i++)
        {
            if (ChD.characterDatas[i].curEquip != -1)
            {
                EquipManageContent.transform.GetChild(ChD.characterDatas[i].curEquip).GetChild(3).GetChild(0).
                    GetComponent<TextMeshProUGUI>().text = ChD.characterDatas[i].Name + " 착용 중";
            }
        }
    }
    public void EquipmentManageSelect(GameObject me)
    {
        int num = me.name[7] - 48;
        if (me.name[8] != 41)
        {
            num *= 10;
            num += me.name[8] - 48;
        }
        bool isEquip = false;
        for (int i = 0; i < ChD.size; i++)
        {
            if (ChD.characterDatas[i].curEquip == num) isEquip = true;
        }
        if (isEquip) return;
        if (GD.EquipmentList[num].special != 0) return;
        string myCondition = me.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text;
        if (myCondition == "선택")
        {
            if (SelectedEquipList.Count > 1) return;
            me.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "선택 됨";
            SelectedEquipList.Add(num);
        }
        else
        {
            me.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "선택";
            SelectedEquipList.Remove(num);
        }
        SetEquipListImage();
    }
    void SetEquipListImage()
    {
        if (SelectedEquipList.Count == 0)
        {
            SelectedEquipImage[0].SetActive(false);
            SelectedEquipImage[1].SetActive(false);
            EquipManageButton.SetActive(false);
        }
        else if (SelectedEquipList.Count == 1)
        {
            SelectedEquipImage[0].SetActive(true);
            SelectedEquipImage[1].SetActive(false);
            SelectedEquipImage[0].GetComponent<Image>().sprite = EquipmentManager.Instance.equipSpr[GD.EquipmentList[SelectedEquipList[0]].equipNum];
            SelectedEquipImage[0].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = GD.EquipmentList[SelectedEquipList[0]].equipName;
            EquipManageButton.SetActive(true);
            EquipManageButton.transform.GetChild(0).GetComponent<Text>().text = "초기화(700이그넘)";
        }
        else
        {
            SelectedEquipImage[0].SetActive(true);
            SelectedEquipImage[1].SetActive(true);
            SelectedEquipImage[0].GetComponent<Image>().sprite = EquipmentManager.Instance.equipSpr[GD.EquipmentList[SelectedEquipList[0]].equipNum];
            SelectedEquipImage[0].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = GD.EquipmentList[SelectedEquipList[0]].equipName;
            SelectedEquipImage[1].GetComponent<Image>().sprite = EquipmentManager.Instance.equipSpr[GD.EquipmentList[SelectedEquipList[1]].equipNum];
            SelectedEquipImage[1].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = GD.EquipmentList[SelectedEquipList[1]].equipName;
            EquipManageButton.SetActive(true);
            EquipManageButton.transform.GetChild(0).GetComponent<Text>().text = "합성(300이그넘)";
        }
    }
    public void SelectEquipManageButton()
    {   
        if (SelectedEquipList.Count == 1)
        {
            if (GD.Ignum < 700) return;
            GD.Ignum -= 700;
            equipment e = GD.EquipmentList[SelectedEquipList[0]];
            e = EquipmentManager.Instance.ResetEquipment(e);
            GD.EquipmentList[SelectedEquipList[0]] = e;
            SelectedEquipList.Clear();
            DayAct();
            EquipManageView.SetActive(false);
            PopUpCanvas.SetActive(false);
            GetEquipmentCanvas.SetActive(true);           
            GetEquipmentCanvas.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = EquipmentManager.Instance.equipSpr[e.equipNum];
            List<string> sList = EquipmentManager.Instance.equipmentStrings(e);
            equipStrings[0].text = sList[0];
            equipStrings[1].text = sList[1] + '\n' + sList[2] + '\n' + sList[3];
        }
        if (SelectedEquipList.Count == 2)
        {
            if (GD.Ignum < 300) return;
            GD.Ignum -= 300;
            equipment e1 = GD.EquipmentList[SelectedEquipList[0]];
            equipment e2 = GD.EquipmentList[SelectedEquipList[1]];
            equipment newE = EquipmentManager.Instance.AddEquipments(e1,e2);
            GD.EquipmentList.RemoveAt(SelectedEquipList[0]);
            GD.EquipmentList.RemoveAt(SelectedEquipList[1]);
            GD.EquipmentList.Add(newE);
            for(int i = 0; i < ChD.size; i++)
            {
                if (ChD.characterDatas[i].curEquip > SelectedEquipList[0]) ChD.characterDatas[i].curEquip--;
                if (ChD.characterDatas[i].curEquip > SelectedEquipList[1]) ChD.characterDatas[i].curEquip--;
            }
            SelectedEquipList.Clear();
            DayAct();
            EquipManageView.SetActive(false);
            PopUpCanvas.SetActive(false);
            GetEquipmentCanvas.SetActive(true);
            GetEquipmentCanvas.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = EquipmentManager.Instance.equipSpr[newE.equipNum];
            List<string> sList = EquipmentManager.Instance.equipmentStrings(newE);
            equipStrings[0].text = sList[0];
            equipStrings[1].text = sList[1] + '\n' + sList[2] + '\n' + sList[3];
        }
    }



    public void TributeViewOn()
    {
        if (canvasOn) return;
        if (GD.isAct) return;
        canvasOn = true;
        tributeView.SetActive(true);
        int cur = GD.tributeStack;
        int ig = 0;
        if (cur == 0) ig = 100;
        else if (cur == 1) ig = 300;
        else if (cur == 2) ig = 600;
        tributeView.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "현재 공급로:" + cur;
        if (cur == 3) tributeView.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        else tributeView.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "공급로 확보\n(" + ig + "이그넘 필요)";
    }

    public void TributeViewOff()
    {
        canvasOn = false;
        tributeView.SetActive(false);
    }

    public void GetTributeStack()
    {
        int cur = GD.tributeStack;
        int ig = 0;
        if (cur == 0) ig = 100;
        else if (cur == 1) ig = 300;
        else if (cur == 2) ig = 600;
        if (GD.Ignum < ig) return;
        GD.Ignum -= ig;
        GD.tributeStack++;
        DayAct();
        save();
        TributeViewOff();
    }
    public void GetTribute()
    {
        int cur = GD.tributeStack;
        int tri = 100;
        if (cur == 1) tri = Random.Range(1, 4) * 100;
        else if (cur == 2) tri = Random.Range(3, 6) * 100;
        else if (cur == 3) tri = 500;
        GD.tribute += tri;
        DayAct();
        save();
        TributeViewOff();

    }
}