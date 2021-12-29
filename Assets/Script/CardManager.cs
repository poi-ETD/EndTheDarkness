using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardManager : MonoBehaviour
{
    public List<GameObject> Deck = new List<GameObject>();
    public List<GameObject> Grave = new List<GameObject>();
    public List<GameObject> field = new List<GameObject>();
    [SerializeField] int deckSize;
    public Text graveT;
    public Text deckT;
    GameObject[] fieldCard=new GameObject[100];
    private void Update()
    {

        graveT.text = "" + Grave.Count;
        deckT.text = "" + Deck.Count;
    }
    private void Awake()
    {
        
    }
    void Rebatch()
    {
        for (int i = 0; i < fieldCard.Length; i++)
            Destroy(fieldCard[i]);
        for(int i = 0; i < field.Count; i++)
        {
            fieldCard[i]=Instantiate(field[i],
            new Vector3(-3 + 1.5f * i, -3.5f,-5+i),transform.rotation);
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
    public void UseCard(GameObject usingCard)
    {
        for (int i = field.Count - 1; i >= 0; i--)
        {
            if (usingCard == fieldCard[i])
            {
                Destroy(fieldCard[i]);
                Grave.Add(field[i]);
                field.RemoveAt(i);
                break;
            }
        }
        Rebatch();
    }
    public void FieldOff()
    {  
        for(int i = field.Count-1; i >=0; i--)
        {            
            Destroy(fieldCard[i]);
            Deck.Add(field[i]);
            field.RemoveAt(i);
          
        }
    }
}
