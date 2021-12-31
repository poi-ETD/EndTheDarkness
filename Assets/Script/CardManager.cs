using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardManager : MonoBehaviour
{
    public List<GameObject> Deck = new List<GameObject>();
    public List<GameObject> Grave = new List<GameObject>();
    public List<GameObject> field = new List<GameObject>();
    public Text graveT;
    public Text deckT;
    [SerializeField] TurnManager TM; 
    public int FiledCardCount;
    public int specialDrow;
    public int[] CardCount = new int[100];
    public GameObject[] startCard = new GameObject[100];
    public int cardKind;
    private void Update()
    {
        graveT.text = "" + Grave.Count;
        deckT.text = "" + Deck.Count;
    }
    private void Awake()
    {
        for (int i = 0; i < cardKind; i++) {
            for (int j = 0; j < CardCount[i]; j++) {
                GameObject newCard = Instantiate(startCard[i], new Vector3(100, 100, 0), transform.rotation);
                Deck.Add(newCard);    
            }
      }
        for(int i = 0; i < Deck.Count; i++)
        {
            Deck[i].SetActive(false);
        }
    }
    void Rebatch()
    {    
        for(int i = 0; i < field.Count; i++)
        {

            field[i].transform.position = new Vector3(-3 + 1.5f * i, -3.5f, -5 + i);
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
    public void UseCard(GameObject usingCard)
    {
        for (int i = field.Count - 1; i >= 0; i--)
        {
            if (usingCard == field[i])
            {

                field[i].transform.position = new Vector3(100, 100, 0);
                field[i].SetActive(false);
                Grave.Add(field[i]);
                field.RemoveAt(i);
                break;
            }
        }
        TM.BM.enemy = null;    
        TM.turnCard++;
        Rebatch();
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
}
