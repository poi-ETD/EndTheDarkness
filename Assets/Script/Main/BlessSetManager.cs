using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BlessSetManager : MonoBehaviour
{
    BattleData bd=new BattleData();
    BlessData bld=new BlessData();
    CardData CardD=new CardData();
    CharacterData CharD=new CharacterData();
    [SerializeField] Text[] OnOff;
    public GameObject[] cardlist;
    public GameObject cardButton;
    public Text removeCard;
    public GameObject bless5PopUp;
    public int removecount;
    public GameObject bless6PopUp;
    public Text[] Cname;
    public GameObject[] PassiveButton;
    public Text[] Pname;
    // Start is called before the first frame update
    void Start()
    {

        string path = Path.Combine(Application.persistentDataPath, "BattleData.json");
        if (File.Exists(path))
        {
            string battleData = File.ReadAllText(path);
            bd = JsonUtility.FromJson<BattleData>(battleData);
        }
        string path2 = Path.Combine(Application.persistentDataPath, "BlessData.json");
        if (File.Exists(path2))
        {
            string blessData = File.ReadAllText(path2);
            bld = JsonUtility.FromJson<BlessData>(blessData);
           
                for (int i = 1; i < bld.BlessOn.Length; i++)
                {
                    if (OnOff[i] != null)
                    {
                        if (bld.BlessOn[i]) OnOff[i].text = "On";
                        else OnOff[i].text = "Off";
                    }
                }
            
        }
        string path3 = Path.Combine(Application.persistentDataPath, "CardData.json");
        if (File.Exists(path3))
        {
            string cardData = File.ReadAllText(path3);
            CardD = JsonUtility.FromJson<CardData>(cardData);
        }
        string path4 = Path.Combine(Application.persistentDataPath, "CharacterData.json");
        if (File.Exists(path4))
        {
            string charData = File.ReadAllText(path4);
            CharD = JsonUtility.FromJson<CharacterData>(charData);
        }
      
            
        
    }
  /*  public void bless1()
    {
        bd.Ignum += 1000;
        for(int i = 0; i < CharD.SumCharacter; i++)
        {
            CharD.curHp[i] = Mathf.FloorToInt(CharD.curHp[i] * 0.7f);
        }
    }*/
    public void bless2()
    {
        if (bld.BlessOn[2])
        {
            OnOff[2].text = "Off";
            bld.BlessOn[2] = false;
        }
        else
        {
            OnOff[2].text = "On";
            bld.BlessOn[2] = true;
        }
    }
    public void bless3()
    {
        if (bld.BlessOn[3])
        {
            bld.BlessCount[3] = 0;
            OnOff[3].text = "Off";
            bld.BlessOn[3] = false;
        }
        else
        {
            bld.BlessCount[3] = 2;
            OnOff[3].text = "On";
            bld.BlessOn[3] = true;
        }
    }
    public void bless4()
    {
        if (bld.BlessOn[4])
        {
           
            OnOff[4].text = "Off";
            bld.BlessOn[4] = false;
        }
        else
        {
        
            OnOff[4].text = "On";
            bld.BlessOn[4] = true;
        }
    }
   /* public void bless5()
    {
        bd.Ignum = 0;
        removecount = 6;
        removeCard.text = "제거 가능한 카드 : 6";
        bless5PopUp.SetActive(true);
        for(int i = 0; i < CardD.CardCount.Length; i++)
        {
            if (CardD.CardCount[i] > 0)
            {
                for(int j = 0; j < CardD.CardCount[i]; j++)
                {
                    GameObject newbutton = Instantiate(cardButton, new Vector2(0, 0), transform.rotation, GameObject.Find("removeCard").transform.transform);
                    newbutton.GetComponent<Bless5Button>().CardNo = i;
                    newbutton.transform.Find("Text").GetComponent<Text>().text = cardlist[i].GetComponent<Card>().Name.text;
                }
            }
        }

    }*/
    public void resetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    [SerializeField] GameObject[] B6Buttons;
 /*   public void bless6()
    {
        bless6PopUp.SetActive(true);
        for(int i = 0; i < CharD.SumCharacter; i++)
        {
            B6Buttons[i].SetActive(true);
            int c = CharD.RotateCharacter[i];            
            Cname[i].text = CharD.CharacterName[c];
            int k = 0;
            for(int j = c * 4; j < c * 4 + 4; j++)
            {
                if (CharD.passive[j]==0) PassiveButton[i * 4 + k].SetActive(false);
                Pname[i * 4 + k].text = CharD.passiveName[j];
                k++;
            }
        }
    }
    public void bless6Select(int i)
    {
        bld.BlessOn[6] = true;
        int a = i / 4;
        int b = i % 4;
        int c = CharD.RotateCharacter[a];
        CharD.passive[c * 4 + b]++;
        Debug.Log(a);
        CharD.CurCharacterAtk[a] -= 3;
        bless6PopUp.SetActive(false);
    }
    public void bless6off()
    {
        for (int i = 0; i < 4; i++)
        {
            CharD.CurCharacterAtk[i] = CharD.CharacterAtk[CharD.RotateCharacter[i]];
        }
    }*/
    public void bless7()
    {
        if (bld.BlessOn[7])
        {

            OnOff[7].text = "Off";
            bld.BlessOn[7] = false;
        }
        else
        {

            OnOff[7].text = "On";
            bld.BlessOn[7] = true;
        }
    }
    public void GoMain()
    {
        string path = Path.Combine(Application.persistentDataPath, "BattleData.json");
        string battleData = JsonUtility.ToJson(bd);
        File.WriteAllText(path, battleData);
        string path2 = Path.Combine(Application.persistentDataPath, "BlessData.json");
        string blessData = JsonUtility.ToJson(bld);
        File.WriteAllText(path2, blessData);
        string path4 = Path.Combine(Application.persistentDataPath, "CharacterData.json");
        string CharacterData = JsonUtility.ToJson(CharD);
        File.WriteAllText(path4, CharacterData);
        SceneManager.LoadScene("Main");
    }
}
public class BlessData
{
    public bool[] BlessOn = new bool[20];
    public int[] BlessCount = new int[20];
}
