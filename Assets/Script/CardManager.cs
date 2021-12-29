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
    GameObject[] fieldcard;
    public Text graveT;
    public Text deckT;
    private void Update()
    {

        graveT.text = "" + Grave.Count;
        deckT.text = "" + Deck.Count;
    }
    private void Awake()
    {
        fieldcard = new GameObject[5];
    }
    public void Suffle()
    {
        for (int i = 0; i < Deck.Count; i++)
        {
            int rand = Random.Range(i, Deck.Count);
            GameObject imsi = Deck[rand];
            Deck[rand] = Deck[i];
            Deck[i] = imsi;
        }
        for (int i = 0; i < 5; i++)
        {
            if(i<Deck.Count)
            fieldcard[i] = Instantiate(Deck[i], new Vector2(-4 + 2 * i, -3), gameObject.transform.rotation);
        }    
    }
    public void UseCard(GameObject usingCard)
    {
       for(int i = 0; i < 5; i++)
        {
            if (fieldcard[i] == usingCard)
            {
               
                if (Deck.Count > 5)
                {
                    Destroy(fieldcard[i]);
                    Grave.Add(Deck[i]);
                    Deck.RemoveAt(i);
                }
                else
                {
                    int count = 0;
                    int fcount = 0;
                    while (fieldcard[fcount] != fieldcard[i])
                    {
                        if (fieldcard[fcount] != null) count++;
                        fcount++;
                    }
                    Grave.Add(Deck[count]);
                    Deck.RemoveAt(count);
                    Destroy(fieldcard[i]);
                }
                break;
            }
        }
    }
    public void FieldOff()
    {
        for(int i = 0; i < 5; i++)
        {
            if (fieldcard[i] != null)
                Destroy(fieldcard[i]);
        }
    }
}
