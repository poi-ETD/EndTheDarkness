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
    [SerializeField] GameObject dayInside;
    [SerializeField] GameObject dayOutside;
    [SerializeField] GameObject nightInside;
    [SerializeField] GameObject nightOutside;
    [SerializeField] GameObject cardView;
    [SerializeField] GameObject cardPrefebs;
    [SerializeField] GameObject cardContent;
    [SerializeField] GameObject OneCard;
    [SerializeField] GameObject OneCardCanvas;
    [SerializeField] GameObject CharacterView;
    public bool canvasOn;

    [SerializeField] TextMeshProUGUI[] act_texts;

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
    [SerializeField] GameObject BoxCanvas;
    [SerializeField] GameObject IgnumInBox;
    [SerializeField] GameObject TributeInBox;
    [SerializeField] GameObject ChoiceInBox;

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
    [SerializeField] NoBattleCard[] resetCards;
    [SerializeField] TextMeshProUGUI[] resetBless_text;
    public bool do_Resetmara;
    private List<int> ResetCardList = new List<int>();
    private int firstBless;
    private List<int> ResetCardListSelect = new List<int>();
    [SerializeField] GameObject[] ResetObjs;

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
    [SerializeField] GameObject bless19View;
    [SerializeField] GameObject bless19Content;

    public int bless19Count;
    [SerializeField] BlessInLobby blessInLobby;
    private bool outside;

    [SerializeField] TextMeshProUGUI endButton_text;


    [SerializeField] GameObject ConversationView;

    [SerializeField] private GameObject RitualView;
    [SerializeField] private GameObject Ritual_Day;
    [SerializeField] private GameObject Ritual_Night;

    [SerializeField] private GameObject MerchantView;

    [SerializeField] private GameObject ScriptView;

    [SerializeField] private GameObject TrainView;


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
            GD.victory = 1;
            Resetmara();
        }

        IgnumT.text = GD.Ignum + "";
        TributeT.text = GD.tribute + "";

        PositionChange();
        if (GD.isAct)
        {
            for (int i = 0; i < act_texts.Length; i++)
            {
                act_texts[i].color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
        setDay();
        GameObject.Find("Bless").GetComponent<BlessInLobby>().setBlessIcon();
    }
    public void PositionChange()
    {
        nightOutside.SetActive(false);
        nightInside.SetActive(false);
        dayInside.SetActive(false);
        dayOutside.SetActive(false);
        if (GD.isNight)
        {
            if (outside)
            {
                nightInside.SetActive(true);
                outside = false;
            }
            else
            {
                nightOutside.SetActive(true);
                outside = true;
            }
        }
        else
        {
            if (outside)
            {
                dayInside.SetActive(true);
                outside = false;
            }
            else
            {
                dayOutside.SetActive(true);
                outside = true;
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&!GameObject.Find("ESC(Clone)"))//이 창은 하나만 띄우자
        {
            Instantiate(GameManager.Instance.EscViewPrefeb, GameObject.Find("Canvas").transform);
        }
    }
    public void Resetmara() //게임이 가장 처음 시작 될 때
    {
        ResetCanvas.SetActive(true);
        canvasOn = true;
        for (int i = 0; i < ResetObjs.Length; i++)
        {
            ResetObjs[i].SetActive(false);
        }
        bool[] myCharacter = Enumerable.Repeat<bool>(false, CharacterInfo.Instance.cd.Length).ToArray<bool>();
        for (int i = 0; i < ChD.size; i++)
        {
            myCharacter[ChD.characterDatas[i].code] = true;
        }
        ResetCardList.Clear();
        ResetCardListSelect.Clear();
        for (int i = 1; i < CardInfo.Instance.cd.Length; i++)
        {
            if (CardInfo.Instance.cd[i].type < 2 && myCharacter[CardInfo.Instance.cd[i].Deck])
            {
                ResetCardList.Add(i);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            int rand = Random.Range(0, ResetCardList.Count);
            resetCards[i].setCardInfoInLobby(ResetCardList[rand], 0);
            ResetCardListSelect.Add(ResetCardList[rand]);
        }
        firstBless = blessInLobby.GetBlessInResetmara();
        resetBless_text[0].text = blessInLobby.bd.bd[firstBless].Name;
        resetBless_text[1].text = blessInLobby.bd.bd[firstBless].content;
        StartCoroutine("ResetCor");
    }
    IEnumerator ResetCor()
    {
        int idx = 0;
        yield return new WaitForSeconds(1);
        while (idx < ResetObjs.Length)
        {

            ResetObjs[idx].SetActive(true);
            idx++;
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void SelectResetmara()
    {
        canvasOn = false;
        ResetCanvas.SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            CD.cardCode.Add(ResetCardListSelect[i]);
            CD.cardCost.Add(CardInfo.Instance.cd[ResetCardListSelect[i]].Cost);
            CD.cardGetOrder.Add(CD.count);
            CD.count++;
        }

        blessInLobby.BlessApplyInResetmara(firstBless);
        blessInLobby.exitBlessPopup();

        save();
    }

    public void ShowCardList()
    {
        if (!canvasOn)
        {
            cardContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            canvasOn = true;
            PopUpCanvas.SetActive(true);
            cardView.SetActive(true);
            for (int i = 0; i < CD.cardCode.Count; i++)
            {
                GameObject newCard = Instantiate(cardPrefebs, cardContent.transform);
                newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(CD.cardCode[i], i);
                newCard.GetComponent<NoBattleCard>().setCost(CD.cardCost[i]);
                int[] k = { CD.cardCode[i], CD.cardCost[i] };
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
                CharacterView.transform.GetChild(i).gameObject.GetComponent<CharacterSetting>().SetCharacterInLobby(ChD.characterDatas[i].code, CharacterInfo.Instance.cd[ChD.characterDatas[i].code].characterSprtie,
                    ChD.characterDatas[i].atk, ChD.characterDatas[i].endurance, ChD.characterDatas[i].cost, ChD.characterDatas[i].curHp, ChD.characterDatas[i].maxHp,
                    ChD.characterDatas[i].curFormation, ChD.characterDatas[i].passive, s1, s2, spr
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
        for (int i = 0; i < GD.EquipmentList.Count; i++)
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


; EquipmentContent.transform.GetChild(i).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "착용";
        }
        for (int i = GD.EquipmentList.Count; i < 50; i++)
        {
            EquipmentContent.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < ChD.size; i++)
        {
            if (ChD.characterDatas[i].curEquip != -1)
            {
                EquipmentContent.transform.GetChild(ChD.characterDatas[i].curEquip).GetChild(3).GetChild(0).
                    GetComponent<TextMeshProUGUI>().text = ChD.characterDatas[i].name + " 착용 해제";
            }
        }
    }
    public void EquipThis(GameObject me)
    {
        int num = me.name[7] - 48;
        if (me.name[8] != 41)
        {
            num *= 10;
            num += me.name[8] - 48;
        }
        if (equipmode) return;
        bool isEquip = false;
        for (int i = 0; i < ChD.size; i++)
        {
            if (ChD.characterDatas[i].curEquip == num) isEquip = true;
        }
        if (isEquip)
        {
            for (int i = 0; i < ChD.size; i++)
            {
                if (ChD.characterDatas[i].curEquip == num)
                {
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
                if (GD.EquipmentList[num].type == ChD.characterDatas[i].curFormation||GD.EquipmentList[num].type==2)
                {
                
                    EquipCharacterView.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                    EquipCharacterView.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = ChD.characterDatas[i].name;
                }
                else
                {
                    EquipCharacterView.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
                }
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
        OneCard.GetComponent<NoBattleCard>().setCardInfoInLobby(CD.cardCode[i], 0);
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
            ByPassiveNames[i].text = ChD.characterDatas[i].name;
            ByPassiveButtons[i * 4].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].code].passive[0];
            ByPassiveButtons[i * 4 + 1].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].code].passive[1];
            ByPassiveButtons[i * 4 + 2].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].code].passive[2];
            ByPassiveButtons[i * 4 + 3].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].code].passive[3];
        }
    }
    public void GetBox()
    {
        if (canvasOn) return;
        if (GD.isAct) return;
        canvasOn = true;
        BoxCanvas.SetActive(true);
        ChoiceInBox.SetActive(true);
        IgnumInBox.SetActive(false);
        TributeInBox.SetActive(false);
    }
    public void ChoiceIgnum()
    {
        ChoiceInBox.SetActive(false);
        int ignum = 150 + GD.victory * 20;
        ignum = Mathf.RoundToInt(ignum * (1 + GD.tributeStack * 0.1f));
        GD.Ignum += ignum;
        IgnumInBox.SetActive(true);
        IgnumInBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + ignum;
        Act();
    }
    public void ChocieTriubute()
    {
        ChoiceInBox.SetActive(false);
        int tribute = 100;
        if (GD.tributeStack == 1)
        {
            tribute += 100 * Random.Range(0, 3);
        }
        else if (GD.tributeStack == 2)
        {
            tribute += 100 * Random.Range(2, 5);
        }
        else if (GD.tributeStack == 3)
        {
            tribute += 400;
        }
        GD.tribute += tribute;
        TributeInBox.SetActive(true);
        TributeInBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + tribute;
        Act();
    }

    public void GetPassiveButton(int i)
    {
        GameObject.Find("PassiveView").SetActive(false);
        PopUpCanvas.SetActive(false);
        ChD.characterDatas[i / 4].passive[i % 4]++;
        GD.passiveStack -= 15;
        canvasOn = false;
        Act();
    }
    public void Act()
    {
        if (do_Resetmara)
        {
            do_Resetmara = false;
            return;
        }
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
            if (GD.Ignum < 0) GD.Ignum = 0;
            IgnumT.text = GD.Ignum + "";
            save();
            if (GD.isAct)
            {

                for (int i = 0; i < act_texts.Length; i++)
                {
                    act_texts[i].color = new Color(0.5f, 0.5f, 0.5f);
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
                for (int i = 0; i < act_texts.Length; i++)
                {
                    act_texts[i].color = new Color(0.5f, 0.5f, 0.5f);
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

    public void CardShopPopupOn()
    {
        MerchantView.SetActive(false);

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
            CD.cardCode.Add(ShopSelectedCardNo);
            CD.cardCost.Add(CardInfo.Instance.cd[ShopSelectedCardNo].Cost);
            CD.cardGetOrder.Add(CD.count);
            CD.count++;

        }
        cancleInShop.SetActive(true);
        Transform[] childList = GameObject.Find("ShopList").GetComponentsInChildren<Transform>();
        for (int i = 1; i < childList.Length; i++)
        {
            Destroy(childList[i].gameObject);
        }
        RandomCardList.Clear();
        SelectCardCanvas.SetActive(false);
        Act();
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
                    if (CardInfo.Instance.cd[i].Deck == ChD.characterDatas[j].code)
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
                    if (CardInfo.Instance.cd[i].Deck == ChD.characterDatas[j].code || CardInfo.Instance.cd[i].Deck == 0)
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
                for (int j = 0; j < ChD.characterDatas.Length; j++)
                {
                    if (CardInfo.Instance.cd[i].Deck == ChD.characterDatas[j].code || CardInfo.Instance.cd[i].Deck == 0)
                    {
                        if (CardInfo.Instance.cd[i].type == 1)
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
    }
    public void NextTurn()
    {
        if (canvasOn) return;
        if (!GD.isNight)
        {
            GD.isAct = false;
            GD.isNight = true;

            for (int i = 0; i < act_texts.Length; i++)
            {
                act_texts[i].color = new Color(1, 1, 1);
            }

            nightOutside.SetActive(false);
            nightInside.SetActive(false);
            dayInside.SetActive(false);
            dayOutside.SetActive(false);
            if (GD.isNight)
            {
                if (outside)
                {
                    nightOutside.SetActive(true);
                }
                else
                {
                    nightInside.SetActive(true);
                }
            }
            else
            {
                if (outside)
                {
                    dayOutside.SetActive(true);
                }
                else
                {
                    dayInside.SetActive(true);
                }
            }

            setDay();
            save();
        }
        else
        {
            GD.isAct = true;
            save();
            bool[] battleDay = { false,true,false,false,true,false,false,true,false,true, //10
                false,false,true,false,false,true,false, true,false,true,false, //20
                false, false, false, true, false, false, false,false,true,true};//30 후에 더 좋은 방법 있다면...
            if (battleDay[GD.Day])
            {
                StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene5_Battle"));
            }
            else
            {
                GD.Day++;
                GD.isAct = false;
                GD.isNight = false;
                GD.isActInDay = false;
                save();
                StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene3_Lobby"));
            }
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
        for (int i = 0; i < CD.cardCode.Count; i++)
        {
            GameObject newCard = Instantiate(RemoveCardPrefebs, RemoveContent.transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(CD.cardCode[i], i);
            newCard.GetComponent<NoBattleCard>().setCost(CD.cardCost[i]);
            int[] k = { CD.cardCode[i], CD.cardCost[i] };
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
        for (int i = 0; i < CD.cardCode.Count; i++)
        {

            if (RemoveContent.transform.GetChild(i).gameObject.GetComponent<NoBattleCard>().select)
                removeList.Add(RemoveContent.transform.GetChild(i).gameObject.GetComponent<NoBattleCard>().deckNo);
        }
        removeList.Sort();

        for (int i = maxRemoveCount - 1; i >= 0; i--)
        {
            CD.cardCode.RemoveAt(removeList[i]);
            CD.cardCost.RemoveAt(removeList[i]);
            CD.cardGetOrder.RemoveAt(removeList[i]);
        }
        canvasOn = false;
        Act();
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
            GD.tribute -= 500;
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
        Act();
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
        Act();
        PassivePopup.SetActive(false);
        PopUpCanvas.SetActive(false);
        noStackInPassive.SetActive(false);
        noStackInPassive.SetActive(false);
    }
    public void GetRandomEquipment()
    {

        canvasOn = true;
        GetEquipmentCanvas.SetActive(true);
        curEquip = EquipmentManager.Instance.makeEquipment();
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
        if (GameObject.Find("Bless").GetComponent<BlessInLobby>().bless18_on)
        {
            GameObject.Find("Bless").GetComponent<BlessInLobby>().Bless_18();
            return;
        }
        if (GameObject.Find("Bless").GetComponent<BlessInLobby>().bless19_on)
        {
            Bless19PopupOn();
            return;
        }


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
            ByPassiveNames[i].text = ChD.characterDatas[i].name;
            ByPassiveButtons[i * 4].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].code].passive[0];
            ByPassiveButtons[i * 4 + 1].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].code].passive[1];
            ByPassiveButtons[i * 4 + 2].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].code].passive[2];
            ByPassiveButtons[i * 4 + 3].text = CharacterInfo.Instance.cd[ChD.characterDatas[i].code].passive[3];
        }
    }

    public void EquipManageViewOn()
    {
        MerchantView.SetActive(false);
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
                    GetComponent<TextMeshProUGUI>().text = ChD.characterDatas[i].name + " 착용 중";
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
        Debug.Log(SelectedEquipList.Count);
        if (SelectedEquipList.Count == 1)
        {
            if (GD.Ignum < 700) return;
            GD.Ignum -= 700;
            equipment e = GD.EquipmentList[SelectedEquipList[0]];
            e = EquipmentManager.Instance.ResetEquipment(e);
            GD.EquipmentList[SelectedEquipList[0]] = e;
            SelectedEquipList.Clear();
            Act();
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
            equipment newE = EquipmentManager.Instance.AddEquipments(e1, e2);
            
            GD.EquipmentList.RemoveAt(Mathf.Max(SelectedEquipList[0],SelectedEquipList[1]));
            GD.EquipmentList.RemoveAt(Mathf.Min(SelectedEquipList[0], SelectedEquipList[1]));
            GD.EquipmentList.Add(newE);
            for (int i = 0; i < ChD.size; i++)
            {
                if (ChD.characterDatas[i].curEquip > SelectedEquipList[0]) ChD.characterDatas[i].curEquip--;
                if (ChD.characterDatas[i].curEquip > SelectedEquipList[1]) ChD.characterDatas[i].curEquip--;
            }
            SelectedEquipList.Clear();
            Act();
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

        canvasOn = true;
        ScriptView.SetActive(false);
        tributeView.SetActive(true);
        int cur = GD.tributeStack;
        int ig = 0;
        if (cur == 0) ig = 100;
        else if (cur == 1) ig = 300;
        else if (cur == 2) ig = 600;
        tributeView.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "현재 공급로:" + cur;
        if (cur == 3) tributeView.transform.GetChild(1).gameObject.SetActive(false);
        else tributeView.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "공급로 확보\n(" + ig + "이그넘 필요)";
    }

    public void TributeViewOff()
    {
        canvasOn = false;
        tributeView.SetActive(false);
    }
    public void ConversationViewOn()
    {
        if (canvasOn) return;
        if (GD.isAct) return;
        canvasOn = true;
        ConversationView.SetActive(true);
        for(int i = 1; i <= 4; i++)
        {
            ConversationView.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < 4; i++)
        {
            if (ChD.characterDatas[i].curHp > 0)
            {
                ConversationView.transform.GetChild(0).GetChild(i + 1).gameObject.SetActive(true);
                ConversationView.transform.GetChild(0).GetChild(i + 1).GetComponent<TextMeshProUGUI>().text = ChD.characterDatas[i].name + "과 대화하기";
            }
        }
    }
    public void ConversationViewOff()
    {
        canvasOn = false;
        ConversationView.SetActive(false);
    }
    public void ScriptVeiwOff()
    {
        canvasOn = false;
        ScriptView.SetActive(false);
    }
    public void RitualViewOn()
    {
        if (canvasOn) return;
        if (GD.isAct) return;
        canvasOn = true;
        RitualView.SetActive(true);
        if (GD.isNight)
        {
            Ritual_Day.SetActive(false);
            Ritual_Night.SetActive(true);
        }
        else
        {
            Ritual_Day.SetActive(true);
            Ritual_Night.SetActive(false);
        }
    }
    public void RitualViewOff()
    {
        canvasOn = false;
        RitualView.SetActive(false);
    }

    public void MerchantViewOn()
    {
        if (canvasOn) return;
        if (GD.isAct) return;
        canvasOn = true;
        MerchantView.SetActive(true);
    }
    public void MerchantViewOff()
    {
        canvasOn = false;
        MerchantView.SetActive(false);
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
        Act();
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
        Act();
        save();
        TributeViewOff();

    }

    public void Bless19PopupOn()
    {
        if (bless19Count < 1)
        {
            GameObject.Find("Bless").GetComponent<BlessInLobby>().bless19_on = false;
            PopUpCanvas.SetActive(false);
            bless19View.SetActive(false);
            return;
        }
        bless19Count--;
        canvasOn = true;
        PopUpCanvas.SetActive(true);
        bless19View.SetActive(true);
        equipmode = false;
        for (int i = 0; i < GD.EquipmentList.Count; i++)
        {
            equipment e = GD.EquipmentList[i];
            List<string> sList = EquipmentManager.Instance.equipmentStrings(e);
            bless19Content.transform.GetChild(i).gameObject.SetActive(true);
            bless19Content.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = EquipmentManager.Instance.equipSpr[e.equipNum];
            bless19Content.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = sList[0];
            if (e.special == 0)
                bless19Content.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = sList[1] + '\n'
                    + sList[2] + '\n' + sList[3];
            else bless19Content.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = sList[1] + '\n';
            bless19Content.transform.GetChild(i).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "선택 가능";
        }
        for (int i = GD.EquipmentList.Count; i < 50; i++)
        {
            bless19Content.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < ChD.size; i++)
        {
            if (ChD.characterDatas[i].curEquip != -1)
            {
                bless19Content.transform.GetChild(ChD.characterDatas[i].curEquip).GetChild(3).GetChild(0).
                    GetComponent<TextMeshProUGUI>().text = ChD.characterDatas[i].name + "착용 중";
            }
        }
    }
    public void Bless19_SelectThis(GameObject me)
    {

        int num = me.name[7] - 48;
        if (me.name[8] != 41)
        {
            num *= 10;
            num += me.name[8] - 48;
        }
        GD.EquipmentList.RemoveAt(num);
        GetRandomEquipment();
        PopUpCanvas.SetActive(false);
        bless19View.SetActive(false);


    }
    public void CoversationWithMyTeam(int myCharacter)//-1일 시 브루와 베릴과 대화
    {
        ScriptView.SetActive(true);
        ConversationView.SetActive(false);
        ScriptView.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
        if (myCharacter == -1)
        {
            BBConversationOn();
            return;        
        }
        ScriptView.transform.GetChild(0).GetComponent<Image>().sprite = CharacterInfo.Instance.CharacterFullSprite[ChD.characterDatas[myCharacter].code];
        ScriptView.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = ChD.characterDatas[myCharacter].name;
    }
    private void BBConversationOn() //브루 베릴과 대화
    {
        ScriptView.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterSprite/BB");
        ScriptView.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "브루와 베릴";
        ScriptView.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
    }
    public void TrainviewOn()
    {
        canvasOn = true;
        ScriptView.SetActive(false);
        TrainView.SetActive(true);      
        for (int i = 0; i < 4; i++)
        {
            TrainView.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < 4; i++)
        {
            if (ChD.characterDatas[i].curHp > 0)
            {
                TrainView.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                TrainView.transform.GetChild(0).GetChild(i).GetComponent<TextMeshProUGUI>().text = ChD.characterDatas[i].name + "를 훈련시킨다.";
            }
        }
    }
    public void TrainViewOff()
    {
        canvasOn = false;
        TrainView.SetActive(false);
    }
    public void TrainStackUp(int myCharacter)
    {
        if (ChD.characterDatas[myCharacter].trainStack == 0)
        {
            ChD.characterDatas[myCharacter].trainStack++;
        }
        else
        {
            ChD.characterDatas[myCharacter].trainStack = 0;
            int rand = Random.Range(0, 4);
            ChD.characterDatas[myCharacter].passive[rand]++;
        }
        TrainView.SetActive(false);
        canvasOn = false;
        Act();
    }
}