using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
public class CharacterManager : MonoBehaviour
{
    CharacterData CD = new CharacterData();
    public GameObject passiveMuch;
    public GameObject[] checkBox;
    public Sprite onCheck;
    float timer;
    public Sprite offCheck;
    public Text[] sText;
    List<int> characterNumber = new List<int>();
    public void ToMain()
    {
        CD.frontCounter = 0;
        CD.backCounter = 0;
        int line = 0;
        for(int i = 0; i < CD.FrontSelectedCharacter.Length; i++)
        {
            if (CD.FrontSelectedCharacter[i]) {
                CD.frontCounter++;
                CD.FrontRotate[line] = i;
                line++;
                characterNumber.Add(i);
            };
        }
        line = 0;
        for (int i = 0; i < CD.BackSelectedCharacter.Length; i++)
        {
            if (CD.BackSelectedCharacter[i]) {
                CD.backCounter++;
                CD.BackRotate[line] = i;
                line++;
                characterNumber.Add(i);
            }
        }
        for (int i = 0; i < CD.SumCharacter; i++)
        {
            CD.RotateCharacter[i]=characterNumber[i];
            CD.CurCharacterAtk[i] = CD.CharacterAtk[characterNumber[i]];
        }
        for (int i = 0; i < characterNumber.Count; i++)
            CD.curHp[i] = CD.CharaterHp[characterNumber[i]];

        string characterData = JsonUtility.ToJson(CD, true);
        string path = Path.Combine(Application.persistentDataPath, "CharacterData.json");
      
     
            
        File.WriteAllText(path, characterData);



        SceneManager.LoadScene("Main");
    }
    private void Awake()
    {
        string path = Path.Combine(Application.persistentDataPath, "CharacterData.json");
        if (File.Exists(path))
        {
            string characterData = File.ReadAllText(path);
            CD = JsonUtility.FromJson<CharacterData>(characterData);
            for (int i = 0; i < 16; i++)
            {
             
            if(CD.passive[i]>0)
            checkBox[i].GetComponent<Image>().sprite = onCheck;            
            }
            for(int i = 0; i < sText.Length; i++)
            {
                
                sText[i].text = "선택 안됨";
                if (CD.FrontSelectedCharacter[i])
                    sText[i].text = "전방";
                else if (CD.BackSelectedCharacter[i])
                    sText[i].text = "후방";
            }
        }
      /*for (int i = 0; i < 4; i++)
        {
            CD.passiveCount[i] = 0;
        }*/

    }
    void onMuch()
    {
        passiveMuch.SetActive(true);
        timer =1;
    }
    
    void offMuch()
    {
        passiveMuch.SetActive(false);
    }
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                offMuch();
                timer = 0;
            }
        }
    }
    public void onPassive(int i)
    {
        Debug.Log(i);
        Debug.Log(CD.passiveCount[i / 4]);
       
        if (CD.passive[i]>0)
        {          
            checkBox[i].GetComponent<Image>().sprite = offCheck;
            CD.passive[i] = 0;
            CD.passiveCount[i/4]--;
        }
        else if (CD.passive[i]==0)
        {           
            if (CD.passiveCount[i/4] < 2)
            {
                checkBox[i].GetComponent<Image>().sprite = onCheck;
                CD.passive[i] = 1;
                CD.passiveCount[i/4]++;
            }
            else
            {
                onMuch();
            }
        }

    }
    public void SetCharacter(int i) //0->선택x 1->전방 2->후방
    {
        if (!CD.FrontSelectedCharacter[i]&&!CD.BackSelectedCharacter[i])
        {
            if (CD.SumCharacter > 4)
            {
                onMuch();
            }
            else
            {
             
                sText[i].text = "전방";
                CD.FrontSelectedCharacter[i] = true;
                CD.SumCharacter++;
            }
        }
        else if (CD.FrontSelectedCharacter[i])
        {
           
            sText[i].text = "후방";
            CD.FrontSelectedCharacter[i] = false;
            CD.BackSelectedCharacter[i] = true;

        }
        else 
        {
           
            sText[i].text = "선택안됨";
            CD.FrontSelectedCharacter[i] = false;
            CD.BackSelectedCharacter[i] = false;
            CD.SumCharacter--;
        }
    }
}
public class CharacterData
{
    public int[] passive=new int[30];
    public string[] passiveName = {"백옥의 왕","군단","흑백","절망","지치지 않는 폭주","독단적인 팀플레이",
        "부서진 족쇄","몰아치기","굳건한 위치","선봉의 호령","무장","독불장군","창조의 잠재력","미라클 드로우",
        "스타키티시모","평균율"};
    public int[] passiveCount=new int[10];
    public bool[] FrontSelectedCharacter=new bool[10];
    public bool[] BackSelectedCharacter=new bool[10];
    public int[] FrontRotate = new int[4];
    public int[] BackRotate = new int[4];
    public int SumCharacter;
    public int[] RotateCharacter = new int[4];
    public string[] CharacterName = { "큐", "스파키", "반가라", "포르테" };
    public int[] CharacterAtk = { 1, 1, 0, 0 };
    public int[] CurCharacterAtk = new int[4];
    public int[] CharaterHp = { 30, 50, 60, 10 };
    public int[] curHp = new int[4];
    public int backCounter;
    public int frontCounter;
}
