using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;

public class CardManager : MonoBehaviour
{   
    public List<GameObject> Deck = new List<GameObject>();
    public List<GameObject> Grave = new List<GameObject>();
    public List<GameObject> field = new List<GameObject>();
    public List<GameObject> ReviveCard = new List<GameObject>();
    public List<GameObject> SelectedCard = new List<GameObject>();
    public Text graveT;
    public Text deckT;
    [SerializeField] GameObject graveWarn;
    [SerializeField] GameObject selectedWarn;
    [SerializeField] GameObject CardPrefebs;
    public TurnManager TM;
    public int FiledCardCount;
    public int specialDrow;
    public int cardKind;
    public GameObject CardCanvas;
    public GameObject HandCanvas;
    public GameObject DeckCanvas;
    CardData CD;
    BattleManager BM;
    HandManager HM;
    CardData2 cd = new CardData2();
    string[] deckText = new string[5];
    public ScriptableObject scrip;
    private void Update()
    {
        graveT.text = "" + Grave.Count;
        deckT.text = "" + Deck.Count;
    }
    private void Awake()
    {
        deckText[0] = "기본";
        deckText[1] = "Q";
        deckText[2] = "스파키";
        deckText[3] = "반가라";
        deckText[4] = "포르테";
        string filepath = Application.persistentDataPath + "/CardData.json";
        string path = Path.Combine(Application.persistentDataPath, "CardData.json");
        if (File.Exists(path))
        {
            string cardData = File.ReadAllText(path);
            CD = JsonConvert.DeserializeObject<CardData>(cardData);

        }
        for (int i = 0; i < CD.cardNo.Count; i++)
        {
            GameObject newCard = Instantiate(CardPrefebs, CardCanvas.transform);
            newCard.GetComponent<Card>().NoT.text = "NO." + cd.cd[CD.cardNo[i]].No.ToString("D3");//넘버
            newCard.GetComponent<Card>().cardNo = cd.cd[CD.cardNo[i]].No;
            newCard.GetComponent<Card>().DeckT.text = deckText[cd.cd[CD.cardNo[i]].Deck];
            newCard.GetComponent<Card>().Content.text = cd.cd[CD.cardNo[i]].Content;
            newCard.GetComponent<Card>().Name.text = cd.cd[CD.cardNo[i]].Name;
            newCard.GetComponent<Card>().cardcost = CD.cardCost[i];
            newCard.GetComponent<Card>().realcost = CD.cardCost[i];
            newCard.GetComponent<Card>().selectType = cd.cd[CD.cardNo[i]].select;
            //addComponent(newCard, CD.cardNo[i]);                    
            Deck.Add(newCard);
        }
        for (int i = 0; i < Deck.Count; i++)
        {
            Deck[i].SetActive(false);
        }
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        HM = GameObject.Find("HandManager").GetComponent<HandManager>();
    }
    void addComponent(GameObject obj,int i)
    {/*
        if (i == 1)
        {
           obj.AddComponent<Card1>();
        }
        if (i == 2)
        {
            obj.AddComponent<Card2>();
        }
        if (i == 3)
        {
            obj.AddComponent<Card3>();
        }
        if (i == 4)
        {
            obj.AddComponent<Card4>();
        }
        if (i == 5)
        {
            obj.AddComponent<Card5>();
        }
        if (i == 6)
        {
            obj.AddComponent<Card6>();
        }
        if (i == 7)
        {
            obj.AddComponent<Card7>();
        }
        if (i == 8)
        {
            obj.AddComponent<Card8>();
        }
        if (i == 9)
        {
            obj.AddComponent<Card9>();
        }
        if (i == 10)
        {
            obj.AddComponent<Card10>();
        }
         if (i == 11)
        {
            obj.AddComponent<Card11>();
        }
        if (i == 12)
        {
            obj.AddComponent<Card12>();
        }
        if (i == 13)
        {
            obj.AddComponent<Card13>();
        }
        if (i == 14)
        {
            obj.AddComponent<Card14>();
        }
        if (i == 15)
        {
            obj.AddComponent<Card15>();
        }
        if (i == 16)
        {
            obj.AddComponent<Card16>();
        }
        if (i == 17)
        {
            obj.AddComponent<Card17>();
        }
        if (i == 18)
        {
            obj.AddComponent<Card18>();
        }
        if (i == 19)
        {
            obj.AddComponent<Card19>();
        }
        if (i == 20)
        {
            obj.AddComponent<Card20>();
        }
        if (i == 21)
        {
            obj.AddComponent<Card21>();
        }
        if (i == 22)
        {
            obj.AddComponent<Card22>();
        }
        if (i == 23)
        {
            obj.AddComponent<Card23>();
        }
        if (i == 24)
        {
            obj.AddComponent<Card24>();
        }*/
    
    }
    public void Rebatch()
    {
        HM.InitCard();
     
    }

    public void CardToField()
    {
        if (Deck.Count > 0)
        {
            int rand = Random.Range(0, Deck.Count);
          
            field.Add(Deck[rand]);
            HM.AddCard(Deck[rand]);
            Deck.RemoveAt(rand);           
        }
    }
    public void TurnStartCardSet()
    {
        for (int i = 0; i < Deck.Count; i++)
        {
            Deck[i].SetActive(true);
            Deck[i].GetComponent<Card>().cardcost = Deck[i].GetComponent<Card>().realcost;
            Deck[i].GetComponent<Card>().costT.text=Deck[i].GetComponent<Card>().cardcost+ "";
          Deck[i].SetActive(false);
          
        }
        for (int i = 0; i < Grave.Count; i++)
        {
            Grave[i].SetActive(true);
            Grave[i].GetComponent<Card>().cardcost = Grave[i].GetComponent<Card>().realcost;
            Grave[i].GetComponent<Card>().costT.text = Grave[i].GetComponent<Card>().realcost+"";
            Grave[i].SetActive(false);
        }
        StartCoroutine("turnStartDrow");
    }
    IEnumerator turnStartDrow()
    {
        BM.otherCor = true;
        for (int i = 0; i < BM.TurnCardCount; i++)
        {
            CardToField();
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(1.2f);

        GameObject.Find("HandManager").GetComponent<HandManager>().InitCard();
        TM.TurnStartPassive();
        BM.otherCor = false;
        
    }
    public void SpecialCardToField()
    {

        if (Deck.Count > 0)
        {
            specialDrow++;
            int rand = Random.Range(0, Deck.Count);
            field.Add(Deck[rand]);
            HM.AddCard(Deck[rand]);
            Deck.RemoveAt(rand);
            Rebatch();
        }
    }
    public void PlusCard(int i)
    {
        GameObject newCard = Instantiate(CardPrefebs, CardCanvas.transform);
        newCard.GetComponent<Card>().NoT.text = "NO." + cd.cd[i].No.ToString("D3");//넘버
        newCard.GetComponent<Card>().cardNo = cd.cd[i].No;
        newCard.GetComponent<Card>().DeckT.text = deckText[cd.cd[i].Deck];
        newCard.GetComponent<Card>().Content.text = cd.cd[i].Content;
        newCard.GetComponent<Card>().Name.text = cd.cd[i].Name;
        newCard.GetComponent<Card>().cardcost = cd.cd[i].Cost;
        newCard.GetComponent<Card>().realcost = cd.cd[i].Cost;
        newCard.GetComponent<Card>().selectType = cd.cd[CD.cardNo[i]].select;
        addComponent(newCard, i);
        Deck.Add(newCard);
        newCard.SetActive(false);
    }
    public void UseCard(GameObject usingCard)
    {
        for (int i = field.Count - 1; i >= 0; i--)
        {
            if (usingCard == field[i])
            {
                field.RemoveAt(i);
                break;
            }
        }
        bool InGrave = false;
        for (int i = 0; i < Grave.Count; i++)
        {
            if (Grave[i] == usingCard)
            {
                InGrave = true;
                break;
            }
        }
        if (!InGrave) Grave.Add(usingCard);
        usingCard.transform.parent = GameObject.Find("GraveContent").transform;      
        if(BM.character!=null)
        BM.character.Acting();
        usingCard.GetComponent<Card>().isGrave = true;
        usingCard.SetActive(false);
        TM.BM.cancleCard();
        TM.BM.pcard = usingCard;
        TM.BM.penemy = TM.BM.enemy;
        StartCoroutine("CardUseCor");
        if (usingCard.GetComponent<Card>().Name.text != "스케치 반복")
        {
            BM.allClear();
            TM.turnCardPlus();
        }
        Rebatch();
      
    }
    IEnumerator CardUseCor()
    {
        BM.otherCanvasOn = true;
        int t = 0;
        t = BM.character.GetComponent<CharacterPassive>().myAct();
        yield return t;
        for(int i = 0; i < BM.CD.size; i++)
        {
            t = BM.characters[i].myPassive.CardUse();
            yield return t;
        }
        BM.otherCanvasOn = false;
    }
    public void FieldToGrave(GameObject c)
    {
        for (int i = field.Count - 1; i >= 0; i--)
        {
            if (c == field[i])
            {
                field.RemoveAt(i);
                break;
            }
        }
        Grave.Add(c);
        c.transform.parent = GameObject.Find("GraveContent").transform;
        c.GetComponent<Card>().isGrave = true;
        c.SetActive(false);
    }
    public void GraveOn()
    {
        for (int i = 0; i < Grave.Count; i++)
        {
            Grave[i].SetActive(true);
            Grave[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }
    public void DeckOn()
    {
        for (int i = 0; i < Deck.Count; i++)
        {
            Deck[i].GetComponent<Card>().isDeck = true;
            Deck[i].transform.parent = GameObject.Find("DeckContent").transform;
            Deck[i].SetActive(true);
            Deck[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }
    public void GraveOff()
    {
        for (int i = 0; i < Grave.Count; i++)
        {
            Grave[i].SetActive(false);
        }
    }
    public void DeckOff()
    {
        SelectedCard.Clear();
        for (int i = 0; i < Deck.Count; i++)
        {
            Deck[i].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            Deck[i].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            Deck[i].GetComponent<Card>().isDeck = false;
            Deck[i].transform.parent = CardCanvas.transform;
            Deck[i].SetActive(false);
        }
    }
    public void FieldOff()
    {
        FiledCardCount = field.Count;
        for (int i = field.Count - 1; i >= 0; i--)
        {
            field[i].transform.position = new Vector3(100, 100, 0);
            field[i].SetActive(false);
            field[i].transform.parent = CardCanvas.transform;
            Deck.Add(field[i]);
            field.RemoveAt(i);
        }
        Rebatch();
    }
    public void ToGrave(GameObject Fcard)
    {
        Fcard.transform.parent = GameObject.Find("GraveContent").transform;
        Fcard.SetActive(false);
    }
    public void GraveToField(GameObject Gcard)
    {
        TM.BM.log.logContent.text += "\n" + Gcard.GetComponent<Card>().Name.text + "이(가) 묘지에서 패로 이동합니다.";
        for (int i = 0; i < Grave.Count; i++)
        {
            if (Gcard == Grave[i])
            {
                field.Add(Grave[i]);
                HM.AddCard(Grave[i]);
                Gcard.GetComponent<Card>().use = false;
                Grave[i].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 225);
                Grave[i].GetComponentInChildren<Image>().color = new Color(1, 1, 1);
                Grave[i].SetActive(true);
                Grave[i].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                Grave[i].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
             
                if (BM.card7mode)
                {
                    Grave[i].GetComponent<Card>().cardcost = 0;
                    Grave[i].GetComponent<Card>().costT.text = "" + 0;


                }
                Grave[i].GetComponent<Card>().isGrave = false;
                Grave[i].GetComponent<Card>().isUsed = false;           
                Grave[i].GetComponent<Transform>().localScale = new Vector2(1, 1);
                Grave.RemoveAt(i);
                break;
            }
        }
   
        

    }
    public void FieldToDeck(GameObject FieldCard)
    {
        for (int i = 0; i < field.Count; i++)
        {
            if (field[i] == FieldCard)
            {
                Deck.Add(FieldCard);
                FieldCard.transform.parent = GameObject.Find("CardCanvas").transform;
                field[i].SetActive(false);
                field.RemoveAt(i);
                break;
            }
        }
   
        Rebatch();
    }
    public void Revive()
    {
        StartCoroutine("ReviveC");
    }
    IEnumerator ReviveC()
    {
        for (int i = ReviveCard.Count - 1; i >= 0; i--)
        {
            GraveToField(ReviveCard[i]);
            ReviveCard.RemoveAt(i);
            yield return new WaitForSeconds(0.5f);
        }
        BM.card7mode = false;
    }
    public void ReviveCountOver()
    {
        graveWarn.SetActive(true);
        Invoke("gWarnOff", 1f);
    }
    public void SelectedCountOver()
    {
        selectedWarn.SetActive(true);
        Invoke("sWarnOff", 1f);
    }
    void gWarnOff()
    {
        graveWarn.SetActive(false);
    }
    void sWarnOff()
    {

        selectedWarn.SetActive(false);
    }
    public void ClickInGrave(GameObject g)
    {
        if (BM.GraveReviveMode)
        {
            bool isClicked = false;
            for (int i = 0; i < ReviveCard.Count; i++)
            {
                if (g == ReviveCard[i]) { isClicked = true; break; }
            }
            if (isClicked)
            {
                g.GetComponent<Transform>().localScale = new Vector2(1, 1f);
                for (int i = 0; i < ReviveCard.Count; i++)
                {
                    if (g == ReviveCard[i]) { ReviveCard.RemoveAt(i); break; }
                }
            }
            else
            {
                if (BM.ReviveCount > ReviveCard.Count)
                {
                    g.GetComponent<Transform>().localScale = new Vector2(1.1f, 1.1f);
                    ReviveCard.Add(g);
                }
                else
                {
                    ReviveCountOver();
                }
            }
        }
        if (BM.DeckSelectMode)
        {
            bool isClicked = false;
            for (int i = 0; i < SelectedCard.Count; i++)
            {
                if (g == SelectedCard[i]) { isClicked = true; break; }
            }
            if (isClicked)
            {
                g.GetComponent<Transform>().localScale = new Vector2(1, 1f);
                for (int i = 0; i < SelectedCard.Count; i++)
                {
                    if (g == SelectedCard[i]) { SelectedCard.RemoveAt(i); break; }
                }
            }
            else
            {
                if (BM.SelectDeckCount > SelectedCard.Count)
                {
                    g.GetComponent<Transform>().localScale = new Vector2(1.1f, 1.1f);
                    SelectedCard.Add(g);
                }
                else
                {
                    SelectedCountOver();
                }
            }

        }
    }
    public void DeckToField()
    {
        for (int j = 0; j < SelectedCard.Count; j++)
        {
            TM.BM.log.logContent.text += "\n" + SelectedCard[j].GetComponent<Card>().Name.text + "이(가) 덱에서 패로 이동합니다.";
            for (int i = 0; i < Deck.Count; i++)
            {
                if (SelectedCard[j] == Deck[i])
                {
                    field.Add(SelectedCard[j]);
                    HM.AddCard(SelectedCard[j]);
                    SelectedCard[j].GetComponent<Card>().isDeck = false;
                    SelectedCard[j].GetComponent<Card>().use = false;
                    SelectedCard[j].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                    SelectedCard[j].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                    SelectedCard[j].SetActive(true);
                    SelectedCard[j].transform.parent = CardCanvas.transform;
                    SelectedCard[j].GetComponent<Transform>().localScale = new Vector2(1, 1);
                    Deck.RemoveAt(i);
                    break;
                }
            }
        }
        Rebatch();
    }
    public void GraveToDeck(GameObject c)
    {
        TM.BM.log.logContent.text += "\n" + c.GetComponent<Card>().Name.text + "이(가) 무덤에서 덱으로 이동합니다.";
        for (int i = 0; i < Grave.Count; i++)
        {
            if (c == Grave[i])
            {
                Deck.Add(c);
                c.GetComponent<Card>().isDeck = true;
                c.GetComponent<Card>().use = false;
                c.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                c.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                c.transform.parent = GameObject.Find("DeckContent").transform;
                c.GetComponent<Card>().isGrave = false;
                c.SetActive(false);
                Grave.RemoveAt(i);
                break;
            }
        }
        Rebatch();
    }
    public void DeckToGrave(GameObject c)
    {
        TM.BM.log.logContent.text += "\n" + c.GetComponent<Card>().Name.text + "이(가) 덱에서 무덤으로 이동합니다.";
        for (int i = 0; i < Deck.Count; i++)
        {
            if (c == Deck[i])
            {
                Grave.Add(c);
                c.GetComponent<Card>().isDeck = false;
                c.GetComponent<Card>().use = false;
                c.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                c.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                c.transform.parent = GameObject.Find("GraveContent").transform;
                c.GetComponent<Card>().isGrave = true;
                c.SetActive(false);
                Deck.RemoveAt(i);
                break;
            }
        }
        Rebatch();
    }
}