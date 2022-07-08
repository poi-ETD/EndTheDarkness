using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    bool smallmode;
    [SerializeField] Text smallt;
    [SerializeField] Text ContinueT;

    [SerializeField] private GameObject go_Main;
    [SerializeField] private GameObject go_New;
    public void SmallMode()
    {
        if (smallmode == false)
        {
            smallt.text = "전체화면";
            smallmode = true;
            Screen.SetResolution(1920, 1080, smallmode);
        }
        else
        {
            smallt.text = "창모드";
            smallmode = false;
            Screen.SetResolution(1920, 1080, smallmode);
        }

    }
    private void Awake()
    {
        //string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CharacterDatas[GameManager.Instance.nowSlot]);

        //if (!File.Exists(path))
        //    ContinueT.color = new Color(0.4f, 0.4f, 0.4f);
    }
    public void ContinueGame()
    {
        string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CharacterDatas[GameManager.Instance.nowSlot]);
        if (!File.Exists(path))
            return;
        else
            StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene3_Lobby"));
    }
    public void NewGame()
    {
        go_Main.SetActive(false);
        go_New.SetActive(true);
    }

    public void Click_Button_NewSlot()
    {
        if (GameManager.Instance.numberOfSlot < 10)
        {
            //string path = Path.Combine(Application.persistentDataPath, "CharacterData.json");
            //string path2 = Path.Combine(Application.persistentDataPath, "CardData.json");
            string path_GameData = Path.Combine(Application.persistentDataPath, "GameData.json");
            //if (File.Exists(path))
            //    File.Delete(path);
            //if (File.Exists(path2))
            //    File.Delete(path2);
            if (File.Exists(path_GameData))
                File.Delete(path_GameData);

            StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene2_Character"));
            //SceneManager.LoadScene("character");
        }
    }

    public void Click_Button_ContinueSlot()
    {

    }

    /* public GameObject[,] Enemys;
     public BattleData bd=new BattleData();
     float wtime;
     [SerializeField] GameObject warn;

     [SerializeField] Text Ignum;
     [SerializeField] GameObject RecoverPopUp;
     [SerializeField] Text[] RecoverT;
     [SerializeField] InputField Recover;
     public void SetBattleScene(int p)
     {
         bd.battleNo = p;
         SaveBattleDataToJson();
         SceneManager.LoadScene("Main");  
     }
     private void Awake()
     {

         string path = Path.Combine(Application.persistentDataPath, "BattleData.json");
         if (File.Exists(path))
         {
             string battleData = File.ReadAllText(path);
             bd= JsonUtility.FromJson<BattleData>(battleData);
             if(Ignum!=null)
             Ignum.text = "이그넘 : " + bd.Ignum;
         }
     }

     public void GoBattleScene()
     {
         string path1 = Path.Combine(Application.persistentDataPath, "CardData.json");
         string path2 = Path.Combine(Application.persistentDataPath, "CharacterData.json");
         string path3 = Path.Combine(Application.persistentDataPath, "BattleData.json");
         string path4 = Path.Combine(Application.persistentDataPath, "BlessData.json");
         if (File.Exists(path1) && File.Exists(path2)&&File.Exists(path3) && File.Exists(path4))
             SceneManager.LoadScene("battle");
         else
         {
             warnon();
         }
     }
     public void resetSetting()
     {
         string path1 = Path.Combine(Application.persistentDataPath, "CardData.json");
         string path2 = Path.Combine(Application.persistentDataPath, "CharacterData.json");
         string path3 = Path.Combine(Application.persistentDataPath, "BattleData.json");
         string path4 = Path.Combine(Application.persistentDataPath, "BlessData.json");
         if(File.Exists(path1))
         System.IO.File.Delete(path1);
         if (File.Exists(path2))
             System.IO.File.Delete(path2);
         if (File.Exists(path3))
             System.IO.File.Delete(path3);
         if (File.Exists(path4))
             System.IO.File.Delete(path4);

     }
     public void GoBlessScene()
     {
         string path1 = Path.Combine(Application.persistentDataPath, "CardData.json");
         string path2 = Path.Combine(Application.persistentDataPath, "CharacterData.json");
         string path3 = Path.Combine(Application.persistentDataPath, "BattleData.json");

         if (File.Exists(path1) && File.Exists(path2) && File.Exists(path3))
             SceneManager.LoadScene("Bless");
         else
         {
             warnon();
         }

     }
     public void GoBattleSetScene()
     {
         SceneManager.LoadScene("SetBattle");
     }
     public void GoCharacterScene()
     {
         SceneManager.LoadScene("character");
     }
     public void GoCardScene()
     {
         SceneManager.LoadScene("card");
     }
     public void SaveBattleDataToJson()
     {
         string battleData = JsonUtility.ToJson(bd);
         string path = Path.Combine(Application.persistentDataPath, "battleData.json");
         File.WriteAllText(path, battleData);
     }
    /* public void RecoverAll()
     {
         string path = Path.Combine(Application.persistentDataPath, "CharacterData.json");
         if (File.Exists(path))
         {
             string characterData = File.ReadAllText(path);
           CharacterData cd = JsonUtility.FromJson<CharacterData>(characterData);
             for (int i = 0; i < cd.SumCharacter; i++)
             {

                 cd.curHp[i] = cd.CharaterHp[cd.RotateCharacter[i]];

             }
             characterData = JsonUtility.ToJson(cd);
             File.WriteAllText(path, characterData);
         }
     }
     int rcount;
     public void Recover1()
     {
         c = -1;
         rcount = 0;
         RecoverPopUp.SetActive(true);
         RecoverT[0].text = "회복시킬 캐릭터의 번호를 입력하세요.\n1~4";
     }
     int c;
     public void Recover2()
     {

         int i = int.Parse(Recover.text);

         if (rcount == 0)
         {
             if (i <= 4 && i >= 1)
             {

                 c = i;
                 Recover.text = "";
                 RecoverT[0].text = "회복시킬 수치를 입력하세요.\n최대체력이상을 입력 시 최대체력이 됩니다.";
                 rcount++;
             }
             else
             {
                 c = -1;
                 RecoverPopUp.SetActive(false);
             }
         }
       else  if (rcount == 1)
         {
             string path = Path.Combine(Application.persistentDataPath, "CharacterData.json");
             if (File.Exists(path))
             {

                 string characterData = File.ReadAllText(path);
                 CharacterData cd = JsonUtility.FromJson<CharacterData>(characterData);
                 cd.curHp[c-1] = i;
                 if (cd.curHp[c - 1] > cd.CharaterHp[cd.RotateCharacter[c - 1]])
                     cd.curHp[c - 1] = cd.CharaterHp[cd.RotateCharacter[c - 1]];
                characterData = JsonUtility.ToJson(cd);
                 File.WriteAllText(path, characterData);
             }
             rcount = 0;
             Recover.text = "";
             RecoverPopUp.SetActive(false);
         }
     }

     private void Update()
     {
         if (wtime < 0)
         {
             warn.SetActive(false);
         }
         if (wtime > 0)
         {
             wtime -= Time.deltaTime;
         }
     }
     void warnon()
     {
         warn.SetActive(true);
         wtime += 1;
     }
     public void goExit() {
         Application.Quit();
     }

     */
}
public class BattleData
{
    public int battleNo;
    public int Ignum;
   
}
