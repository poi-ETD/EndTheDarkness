using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Bless5Button : MonoBehaviour
{
    public int CardNo;
    
    CardData CardD;
    public void removeCard()
    {
        BlessSetManager BLM = GameObject.Find("BlessManager").GetComponent<BlessSetManager>();
        BLM.removecount--;
        BLM.removeCard.text = "제거 가능한 카드 : "+BLM.removecount;
        string cardData;
        string path3 = Path.Combine(Application.persistentDataPath, "CardData.json");
        if (File.Exists(path3))
        {
            cardData = File.ReadAllText(path3);
            CardD = JsonUtility.FromJson<CardData>(cardData);
        }
        CardD.CardCount[CardNo]--;
        cardData = JsonUtility.ToJson(CardD);
        File.WriteAllText(path3, cardData);
        if (BLM.removecount == 0)
        {
            BLM.resetScene();
            BLM.bless5PopUp.SetActive(false);
        }
        Destroy(gameObject);
    }
}
