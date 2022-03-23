using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
using Newtonsoft.Json;
public class CharacterManager : MonoBehaviour
{
    CharacterData CD = new CharacterData();
    float timer;
    CharacterData2 CD2 = new CharacterData2();
    [SerializeField] Sprite[] CharacterImgae;
    [SerializeField] Sprite[] CharacterFaceImgae;
    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject Prefebs;
    List<int> CharacterList = new List<int>();
    [SerializeField] Image[] listImage;
    int counter;
    List<int> FrontCharacter = new List<int>();
    List<int> BackCharacter = new List<int>();
    [SerializeField] TextMeshProUGUI[] SetText;//1->이름 2~5 -> 패시브 1 6->패시브 팝업 텍스트 7->경고 팝업 텍스트
    [SerializeField] GameObject PassivePopup;
    int curPassive;
    [SerializeField] GameObject[] Arrows;
    [SerializeField] Image[] FrontButton;
    int curFormation;
    [SerializeField] CardSetManager cardManager;
    [SerializeField] Image SubImage;
    [SerializeField] GameObject warnPopup;
    public void ToMain()
    { 
        cardManager.SaveCard();
        for(int i = 0; i < CharacterList.Count; i++)
        {
            if (CD.characterDatas[i].curFormation == 0) {
                CD.line++;
              }
           
        }
        CD.size = CharacterList.Count;
        string characterData = JsonConvert.SerializeObject(CD);       
        string path = Path.Combine(Application.persistentDataPath, "CharacterData.json");                       
        File.WriteAllText(path, characterData);
        SceneManager.LoadScene("Main");
    }
    public void RemoveCharacter(int i)
    {if(i<CharacterList.Count)
        CharacterList.RemoveAt(i);
        setListImage();
    }
    private void Awake()
    {
        curPassive = -1;
        curFormation = -1;
        counter = -1;
        string path = Path.Combine(Application.persistentDataPath, "CharacterData.json");
        if (File.Exists(path))
        {
            string characterData = File.ReadAllText(path);
            CD = JsonConvert.DeserializeObject<CharacterData>(characterData);
         
        }
   for(int i = 1; i < CD2.cd.Length; i++)
        {
            GameObject cha = Instantiate(Prefebs, Canvas.transform);
            cha.GetComponent<CharacterSetting>().SetCharacter(i,CharacterImgae[i]);
        }

    }
    public void setCharacter(int n)
    {
        bool isThere=false;
        for(int i = 0; i < CharacterList.Count; i++)
        {
            if (CharacterList[i] == n)
            {
                CharacterList.RemoveAt(i);
                isThere = true;
            }
        }
        if (!isThere && CharacterList.Count < 4)
        {
          
            CharacterList.Add(n);
        }
        setListImage();
    }
    void setListImage()
    {
        for(int i = 0; i < CharacterList.Count; i++)
        {
            listImage[i].sprite = CharacterFaceImgae[CharacterList[i]];
            listImage[i].color = new Color(1, 1, 1, 1);
        }
        for(int i = CharacterList.Count; i < 4; i++)
        {
            listImage[i].color = new Color(1, 1, 1, 0);
        }
    }
    public void SetSubSetting() //1->이름 2~5 -> 패시브 1 6->패시브 팝업 텍스트
    {      
        if (counter > -1)
        { int no = CharacterList[counter];
            if (curFormation == -1)
            {
                warnPopup.SetActive(true);
                SetText[6].text = "진형이 설정되지 않았습니다.";
                return;
            }
            if (curPassive == -1)
            {
                warnPopup.SetActive(true);
                SetText[6].text = "패시브가 설정되지 않았습니다.";
                return;
            }// public curCharacterData(string name, int no, int cost, int atk, 
            //int maxHp, int curHp, int passive1, int passive2, int passive3, int passive4, int curFormation)
                CD.characterDatas[counter]=new CharacterData.curCharacterData(
                CD2.cd[no].Name,no, CD2.cd[no].Cost, CD2.cd[no].Atk, CD2.cd[no].maxHp, CD2.cd[no].maxHp,0,0,0,0,curFormation);
            if (curPassive == 0)
            {
                CD.characterDatas[counter].passive1++;
            }
            if (curPassive == 1)
            {
                CD.characterDatas[counter].passive2++;
            }
            if (curPassive == 2)
            {
                CD.characterDatas[counter].passive3++;
            }
            if (curPassive == 3)
            {
                CD.characterDatas[counter].passive4++;
            }
           
        }
      
        if (counter+1 >= CharacterList.Count)
        {
            ToMain();
            return;
        }
        counter++;
        cardManager.clear();
        if(CharacterList[counter]==1)
        cardManager.getStarterCard(CharacterList[counter],5);
        else
        {
            cardManager.getStarterCard(CharacterList[counter], 1);
        }
        SubImage.sprite = CharacterImgae[CharacterList[counter]];
        SetText[0].text = CD2.cd[CharacterList[counter]].Name;
        SetText[1].text = CD2.cd[CharacterList[counter]].passive1;
        SetText[2].text = CD2.cd[CharacterList[counter]].passive2;
        SetText[3].text = CD2.cd[CharacterList[counter]].passive3;
        SetText[4].text = CD2.cd[CharacterList[counter]].passive4;      
        Canvas.SetActive(false);
        curPassive = -1;
        curFormation = -1;
        PassivePopup.SetActive(false);
        for(int i=0;i<4;i++)
        Arrows[i].SetActive(false);
        FrontButton[0].color = new Color(1, 1, 1);
        FrontButton[1].color = new Color(1, 1, 1);
    }
    public void setPassive(int i)
    {
        if (curPassive != i)
        {          
            PassivePopup.SetActive(true);
            if (i == 0)
                SetText[5].text = CD2.cd[CharacterList[counter]].passiveContent1;
            if (i == 1)
                SetText[5].text = CD2.cd[CharacterList[counter]].passiveContent2;
            if (i == 2)
                SetText[5].text = CD2.cd[CharacterList[counter]].passiveContent3;
            if (i == 3)
                SetText[5].text = CD2.cd[CharacterList[counter]].passiveContent4;
            if (curPassive != -1)
                Arrows[curPassive].SetActive(false);
            curPassive = i;
           
            Arrows[i].SetActive(true);
        }
        else
        {
            Arrows[curPassive].SetActive(false);
            PassivePopup.SetActive(false);
            curPassive = -1;
        }
    }
    public void setFormation(int i)
    {
        if (i == curFormation)
        {
            FrontButton[i].color = new Color(1, 1, 1);
            curFormation = -1;
        }
        else
        {
            if(curFormation!=-1)
            FrontButton[curFormation].color = new Color(1, 1, 1);
            curFormation = i;
            FrontButton[curFormation].color = new Color(1, 0, 0);
        }
    }
}

public class CharacterData 
{
  public struct curCharacterData
    {
        public string Name;
        public int No;
        public int Cost;
        public int Atk;
        public int maxHp;
        public int curHp;
        public int passive1;
        public int passive2;
        public int passive3;
        public int passive4;
        public int curFormation; //0->전방 1->후방
        public curCharacterData(string name, int no, int cost, int atk, int maxHp, int curHp, int passive1, int passive2, int passive3, int passive4, int curFormation)
        {
            Name = name;
            No = no;
            Cost = cost;
            Atk = atk;
            this.maxHp = maxHp;
            this.curHp = curHp;
            this.passive1 = passive1;
            this.passive2 = passive2;
            this.passive3 = passive3;
            this.passive4 = passive4;
            this.curFormation = curFormation;
        }
    }
    public curCharacterData[] characterDatas = new curCharacterData[5];
    public int line;
    public int size;
}
