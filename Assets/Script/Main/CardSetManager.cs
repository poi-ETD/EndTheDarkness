using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using Newtonsoft.Json;
public class CardSetManager : MonoBehaviour
{
    
    public Text AllCardT;
    CardData CD=new CardData();
    public int[] CardCount=new int[100];
    public int AllCard;
    public GameObject Canvas;
    public GameObject CardPrefebs;
    CardData2 cd = new CardData2();
    public int curCardCount;
   public int maxiumCardCount;
    [SerializeField] TextMeshProUGUI cardCount;
    private void Awake()
    {
        string filepath = Application.persistentDataPath + "/CardData.json";
        string path = Path.Combine(Application.persistentDataPath, "CardData.json");
        if (File.Exists(path))
        {           
            string cardData = File.ReadAllText(path);
            CD = JsonConvert.DeserializeObject<CardData>(cardData);

        }
        AllCard = CD.cardNo.Count;
        for (int i = 0; i < AllCard; i++)
        {
            CardCount[CD.cardNo[i]]++;
        }
      /*  for (int i = 1; i < cd.cd.Length; i++)
        {
           
           
            GameObject newCard = Instantiate(CardPrefebs, Canvas.transform);
            newCard.GetComponent<NoBattleCard>().setCardInfo(i);
        }
    */
    }
    public void getStarterCard(int no,int counter) //->no==0이면 모든 카드, type==1->스타트 2->직업
    {
        curCardCount = 0;
        maxiumCardCount = counter;
        cardCount.text = "선택 가능한 최대 카드 수 : " + maxiumCardCount;
        for (int i = 1; i < cd.cd.Length; i++)
        {if (cd.cd[i].Deck == no && cd.cd[i].type == 0)
            {
                GameObject newCard = Instantiate(CardPrefebs, Canvas.transform);
                newCard.GetComponent<NoBattleCard>().setCardInfo(i);
            }
        }
    }
    public void CardOver()
    {
        
    }
    private void Update()
    {
      
    }
 /*   public void ToMain()
    {
        CD.AllCard = AllCard;
        int counter = 0;
        for(int i = 0; i < cd.cd.Length; i++)
        {
            while (CardCount[i] != 0)
            {
                CardCount[i]--;
                CD.cardNo[counter] = i;
                CD.cardCost[counter] = cd.cd[i].Cost;
                Debug.Log(cd.cd[i].Cost);
                counter++;
            }
        }
        string cardData = JsonConvert.SerializeObject(CD);
        string path = Path.Combine(Application.persistentDataPath, "CardData.json");
        File.WriteAllText(path,cardData);
        SceneManager.LoadScene("Main");
    }*/
    public void clear()
    {
        Transform[] childList = Canvas.GetComponentsInChildren<Transform>();

      for(int i = 1; i < childList.Length; i++)
        {
            Destroy(childList[i].gameObject);
        }
    }
public void SaveCard()
    {     
  for(int i = 1; i <= 4; i++)
        {for (int j = 0; j < 5; j++)
            {
                CD.cardNo.Add(i);
                CD.cardCost.Add(cd.cd[i].Cost);
                CD.cardGet.Add(CD.get);
                CD.get++;
            }
        }
        for (int i = 0; i < cd.cd.Length; i++)
        {
            while (CardCount[i] != 0)
            {
                CardCount[i]--;
                CD.cardNo.Add(i);
                CD.cardCost.Add(cd.cd[i].Cost);
                CD.cardGet.Add(CD.get);
                CD.get++;

            }
        }
       
        string cardData = JsonConvert.SerializeObject(CD);
        string path = Path.Combine(Application.persistentDataPath, "CardData.json");
        File.WriteAllText(path, cardData);
    }  
}

public class CardData
{
    public List<int> cardNo = new List<int>();
    public List<int> cardCost = new List<int>();
    public List<int> cardGet = new List<int>();
    public int get;
    
}