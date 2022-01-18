using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class CardManager : MonoBehaviour
{
    public List<GameObject> Deck = new List<GameObject>();
    public List<GameObject> Grave = new List<GameObject>();
    public List<GameObject> field = new List<GameObject>();
    public Text graveT;
    public Text deckT;
    public TurnManager TM; 
    public int FiledCardCount;
    public int specialDrow;
    public int[] CardCount = new int[100];
    public GameObject[] startCard = new GameObject[100];
    public int cardKind;
    [SerializeField] GameObject CardCanvas;
    CardData CD;
    private void Update()
    {
        graveT.text = "" + Grave.Count;
        deckT.text = "" + Deck.Count;
    }
    private void Awake()
    {
        string path = Path.Combine(Application.dataPath, "CardData.json");
        if (File.Exists(path))
        {
            string cardData = File.ReadAllText(path);
            CD = JsonUtility.FromJson<CardData>(cardData);
            for (int i = 0; i < cardKind; i++)
            {             
               CardCount[i] = CD.CardCount[i];               
            }
        }
        for (int i = 0; i < cardKind; i++) {
            for (int j = 0; j < CardCount[i]; j++) {
                GameObject newCard = Instantiate(startCard[i], new Vector3(100, 100, 0), transform.rotation,CardCanvas.transform);
                Deck.Add(newCard);    
            }
      }
        for(int i = 0; i < Deck.Count; i++)
        {
            Deck[i].SetActive(false);
        }
    }
   public void Rebatch()
    {          
        for(int i = 0; i < field.Count; i++)
        {
            field[i].GetComponent<RectTransform>().anchoredPosition=new Vector3(-300 + 150f * i, -520, 0);
                //transform.position = new Vector3(-300 + 150f * i, -520, 0);
        field[i].SetActive(true);
        }
    }
    public void CardToField()
    {
        if (Deck.Count > 0)
        {
            int rand = Random.Range(0, Deck.Count);
            field.Add(Deck[rand]);
            Deck.RemoveAt(rand);
            Rebatch();
        }            
    }
    public void TurnStartCardSet()
    {
        for(int i = 0; i < Deck.Count; i++)
        {
            Deck[i].SetActive(true);
            Deck[i].GetComponent<Card>().cardcost = Deck[i].GetComponent<Card>().realcost;
            Deck[i].SetActive(false);
        }
    }

    public void SpecialCardToField()
    {
   
        if (Deck.Count > 0)
        {
            specialDrow++;
            int rand = Random.Range(0, Deck.Count);
            field.Add(Deck[rand]);          
            Deck.RemoveAt(rand);
            Rebatch();            
        }
    }
    public void PlusCard(int i)
    {
        GameObject newCard = Instantiate(startCard[i], new Vector3(100, 100, 0), transform.rotation, CardCanvas.transform);
        Deck.Add(newCard);
        newCard.SetActive(false);
    }
    public void UseCard(GameObject usingCard)
    {
        for (int i = field.Count - 1; i >= 0; i--)
        {
            if (usingCard == field[i])
            {               
               
                Grave.Add(field[i]);
                field[i].transform.parent = GameObject.Find("GraveContent").transform;
                field.RemoveAt(i);              
                break;
            }
        }
        usingCard.GetComponent<Card>().isGrave = true;
        usingCard.SetActive(false);
        TM.BM.cancleCard();
        TM.BM.pcard = usingCard;
        TM.BM.penemy = TM.BM.enemy;
        if(usingCard.GetComponent<Card>().Name.text!="스케치 반복")
        TM.BM.allClear();  
        TM.turnCard++;
        Rebatch();
    }

    public void GraveOn() {
        for (int i = 0; i < Grave.Count; i++)
        {        
            Grave[i].SetActive(true);
        }
    }
    public void GraveOff()
    {
        for (int i = 0; i < Grave.Count; i++)
        {
            
            Grave[i].SetActive(false);
        }
    }
    public void FieldOff()
    {
        FiledCardCount = field.Count;
        for(int i = field.Count-1; i >=0; i--)
        {
            field[i].transform.position = new Vector3(100, 100, 0);
            field[i].SetActive(false);
            Deck.Add(field[i]);
            field.RemoveAt(i);        
        }
        Rebatch();
    }
    public void GraveToField(GameObject Gcard)
    {
        TM.BM.log.logContent.text += "\n" + Gcard.GetComponent<Card>().Name.text + "이(가) 묘지에서 패로 이동합니다.";
        for(int i = 0; i < Grave.Count; i++)
        {
            if (Gcard == Grave[i])
            {
                field.Add(Grave[i]);
                Grave[i].SetActive(true);
                Grave[i].GetComponent<Card>().isGrave = false;
                Grave[i].GetComponent<Card>().isUsed = false;
                Grave[i].transform.parent = CardCanvas.transform;
                Grave[i].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
                Grave[i].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                Grave.RemoveAt(i);
                break;
            }
        }
        Rebatch();
    
    }
    public void FieldToDeck(GameObject FieldCard)
    {
       for(int i = 0; i < field.Count; i++)
        {
            if (field[i] == FieldCard)
            {
                Deck.Add(FieldCard);
                field[i].SetActive(false);
                field.RemoveAt(i);
                break;
            }
        }
        Rebatch();
    }
}
