using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
public class CardSetManager : MonoBehaviour
{
    public Text[] CardCountT;
    public Text AllCardT;
    CardData CD=new CardData();
    public int[] CardCount;
    public int AllCard;
    [SerializeField] TextMeshProUGUI cardCat;
    private void Awake()
    {
       string filepath = Application.persistentDataPath + "/CardData.json";
        string path = Path.Combine(Application.persistentDataPath, "CardData.json");
        if (File.Exists(path))
        {
            string cardData = File.ReadAllText(path);
            CD = JsonUtility.FromJson<CardData>(cardData);         
            for (int i = 0; i < 100; i++)
            {
                if (CardCountT[i] != null)
                {
                    CardCountT[i].text = CD.CardCount[i] + "";
                    CardCount[i] = CD.CardCount[i];
                    AllCard += CardCount[i];
                }
            }
        }
        AllCardT.text = "총 카드 장 수 : "+AllCard;
    }
    private void Update()
    {
        
    }
    public void ToMain()
    {
        for(int i = 0; i < 100; i++)
        {
            CD.CardCount[i] = CardCount[i];
        }
        string cardData = JsonUtility.ToJson(CD,true);
        string path = Path.Combine(Application.persistentDataPath, "CardData.json");
        File.WriteAllText(path,cardData);
        SceneManager.LoadScene("Main");
    }
    public void CardPlus(int i)
    {
        if (CardCount[i] < 5)
        {   CardCount[i]++;
            CardCountT[i].text = CardCount[i].ToString();
            AllCard++;
            AllCardT.text = "총 카드 장 수 : " + AllCard;
        }
    }
    public void CardMinus(int i)
    {
        if (CardCount[i] >0)
        {   CardCount[i]--;
            CardCountT[i].text = CardCount[i].ToString();
            AllCard--;
            AllCardT.text = "총 카드 장 수 : " + AllCard;
        }
    }
}
class CardData
{
    public int[] CardCount=new int[100];
}