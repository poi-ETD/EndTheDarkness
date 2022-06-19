using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
public class GMmode : MonoBehaviour
{
    public CardData CD;
    public CharacterData ChD;
    public GameData GD = new GameData();
    CharacterData2 ChaInfo = new CharacterData2();
    CardData2 CaInfo = new CardData2();
    [SerializeField] GameObject CardView;
    [SerializeField] GameObject CharacterView;
    int[] CardCount = new int[200];
    [SerializeField]TextMeshProUGUI[] passiveCounts;
    // Start is called before the first frame update
    void Start()
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
        for (int i = 0; i < CD.cardNo.Count; i++)
        {
            CardCount[CD.cardNo[i]]++;
        }
    }

public void GoLobby()
    {
        string characterData = JsonConvert.SerializeObject(ChD);
        string path = Path.Combine(Application.persistentDataPath, "CharacterData.json");
        File.WriteAllText(path, characterData);
        SaveCard();
        string cardData = JsonConvert.SerializeObject(CD);
        path = Path.Combine(Application.persistentDataPath, "CardData.json");
        File.WriteAllText(path, cardData);
        string gameData = JsonConvert.SerializeObject(GD);
        path = Path.Combine(Application.persistentDataPath, "GameData.json");
        File.WriteAllText(path, gameData);
        SceneManager.LoadScene("Lobby");
    }   
    void SaveCard()
    {
        CD.cardCost.Clear();
        CD.cardGet.Clear();
        CD.cardNo.Clear();
        int get = 0;
        for(int i = 1; i < CardCount.Length; i++)
        {
            int count = 0;
            while (count < CardCount[i])
            {
                CD.cardNo.Add(i);
                CD.cardCost.Add(CaInfo.cd[i].Cost);
                CD.cardGet.Add(get);
                CD.get = get;
                get++;
                count++;
            }
        }
    }
    public void OpenCardView()
    {  
        for(int i = 0; i < CD.cardNo.Count; i++)
        {
            CardCount[CD.cardNo[i]]++;
        }
        for(int i = 0; i < CaInfo.cd.Length-1; i++)
        {
            CardView.transform.GetChild(i).gameObject.SetActive(true);
            CardView.transform.GetChild(i).GetComponent<SetCardInGM>().set(i+1,this,CardCount[i+1]);
        }
    }
    public void OpenPassiveView()
    {
        for(int i = 0; i < ChD.size; i++)
        {
            CharacterView.transform.GetChild(i).gameObject.SetActive(true);
            CharacterView.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = ChD.characterDatas[i].Name;
            for (int j = 0; j < 4; j++)
            {
                CharacterView.transform.GetChild(i).GetChild(1 + j).GetComponent<TextMeshProUGUI>().text = ChaInfo.cd[ChD.characterDatas[i].No].passive[j];
                CharacterView.transform.GetChild(i).GetChild(1 + j).GetChild(0).GetComponent<TextMeshProUGUI>().text = ChD.characterDatas[i].passive[j]+"";
            }
        }
    }
    public void CardChange(int no,int size)
    {
        CardCount[no] = size;
    }
    public void ChangePassivePlus(int k)
    {
        ChD.characterDatas[k / 4].passive[k % 4]++;
        if (ChD.characterDatas[k / 4].passive[k % 4] < 0) ChD.characterDatas[k / 4].passive[k % 4] = 0;
        passiveCounts[k].text = ChD.characterDatas[k / 4].passive[k % 4]+"";
    }
    public void ChangePassiveMinus(int k)
    {
        ChD.characterDatas[k / 4].passive[k % 4]--;
        if (ChD.characterDatas[k / 4].passive[k % 4] < 0) ChD.characterDatas[k / 4].passive[k % 4] = 0;
        passiveCounts[k].text = ChD.characterDatas[k / 4].passive[k % 4] + "";
    }
}

