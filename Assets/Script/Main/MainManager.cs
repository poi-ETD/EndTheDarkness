using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
using Newtonsoft.Json;
public class MainManager : MonoBehaviour
{
    bool smallmode;
    [SerializeField] Text smallt;
    [SerializeField] Text ContinueT;

    [SerializeField] private GameObject go_Main;
    [SerializeField] private GameObject go_New;
    [SerializeField] private GameObject go_StartEditDelete;
    [SerializeField] private TextMeshProUGUI[] texts_SlotName;

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
    public void Click_DeleteMemory()
    {
       
        string path_GameData = Path.Combine(Application.persistentDataPath, "GameData.json");
        if (File.Exists(path_GameData))
            File.Delete(path_GameData);
        string path_UserData = Path.Combine(Application.persistentDataPath, "UserMainData");
        if (File.Exists(path_UserData))
        {

            string fileName = "UserMainData";
            string path = Application.persistentDataPath + "/" + fileName;
            string json = File.ReadAllText(path);
            UserSaveData userData = JsonUtility.FromJson<UserSaveData>(json);
            userData.isInit = false;
            for (int i = 0; i < 10; i++) userData.slot_Names[i] = "";
            userData.nowPlayingSlot = 0;
            json = JsonUtility.ToJson(userData);
            File.WriteAllText(path, json);
        }
        for (int i = 0; i < 10; i++)
        {
            string path_CharacterData = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CharacterDatas[i]);
            if (File.Exists(path_CharacterData))
                File.Delete(path_CharacterData);
            string path_CardData = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CardDatas[i]);
            if (File.Exists(path_CardData))
                File.Delete(path_CardData);
        }
        SceneManager.LoadScene(0);
    }
    private void Awake()
    {
        {
            if (SceneManager.sceneCount==1)
                SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
        Debug.Log(Resources.Load<Sprite>("CardSprite/Card-13"));
        //string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CharacterDatas[GameManager.Instance.nowSlot]);

        //if (!File.Exists(path))
        //    ContinueT.color = new Color(0.4f, 0.4f, 0.4f);
    }

    public void Click_Button_NewGame()
    {
        for (int i = 0; i < 10; i++)
            texts_SlotName[i].text = GameManager.Instance.slot_Names[i];

        go_Main.SetActive(false);
        go_New.SetActive(true);
    }

    public void Click_Button_ContinueGame()
    {
        string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CharacterDatas[GameManager.Instance.nowPlayingSlot]);
        if (!File.Exists(path))
            return;
        else
            StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene3_Lobby"));
    }
    
    //public void Click_Button_NewSlot()
    //{
    //    if (/*GameManager.Instance.numberOfSlot < 10*/true)
    //    {
    //        //string path = Path.Combine(Application.persistentDataPath, "CharacterData.json");
    //        //string path2 = Path.Combine(Application.persistentDataPath, "CardData.json");
    //        string path_GameData = Path.Combine(Application.persistentDataPath, "GameData.json");
    //        //if (File.Exists(path))
    //        //    File.Delete(path);
    //        //if (File.Exists(path2))
    //        //    File.Delete(path2);
    //        if (File.Exists(path_GameData))
    //            File.Delete(path_GameData);

    //        StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene2_Character"));
    //        //SceneManager.LoadScene("character");
    //    }
    //}

    public void Click_Button_Slot(int slotCode) // 슬롯 클릭시 실행되는 함수
    {
        GameManager.Instance.selectedSlot_Main = slotCode;

        if (GameManager.Instance.slot_Names[slotCode].Equals("")) // 클릭한 슬롯의 이름이 비어있다면 빈 슬롯으로 판단하고 NewSlot() 함수 실행
            NewSlot();
        else // 클릭한 슬롯의 이름이 비어있지 않다면 존재하는 슬롯으로 판단하고 ExistingSlot(int slotCode) 함수 실행
            ExistingSlot(slotCode);
    }

    private void NewSlot() // 기존 새로하기 버튼 클릭시 실행되는 루틴이 담긴 함수
    {
        string path_GameData = Path.Combine(Application.persistentDataPath, "GameData.json");

        if (File.Exists(path_GameData))
            File.Delete(path_GameData);

        StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene2_Character"));
    }

    private void ExistingSlot(int slotCode) // 이미 데이터가 존재하는 슬롯으로 시작할지, 수정할지, 삭제할지 고르기 전 실행되는 함수
    {
        RectTransform rect = go_StartEditDelete.GetComponent<RectTransform>();
        float y = 0;

        if (slotCode == 0) y = 450f;
        else if (slotCode == 1) y = 350f;
        else if (slotCode == 2) y = 250f;
        else if (slotCode == 3) y = 150f;
        else if (slotCode == 4) y = 50f;
        else if (slotCode == 5) y = -50f;
        else if (slotCode == 6) y = -150f;
        else if (slotCode == 7) y = -250f;
        else if (slotCode == 8) y = -350f;
        else if (slotCode == 9) y = -450f;

        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);
        go_StartEditDelete.SetActive(true);
    }

    public void Click_Button_Start()
    {
        GameManager.Instance.nowPlayingSlot = GameManager.Instance.selectedSlot_Main; // 게임을 시작하기 전 현재 슬롯 번호를 초기화함. 이후 게임 내에선 이 현재 슬롯번호를 이용함

        GameManager.Instance.Save();

        StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene3_Lobby"));
    }

    public void Click_Button_Edit()
    {

    }

    public void Click_Button_Delete()
    {
        GameManager.Instance.slot_Names[GameManager.Instance.selectedSlot_Main] = "";
        texts_SlotName[GameManager.Instance.selectedSlot_Main].text = "";

        GameManager.Instance.Save();

        go_StartEditDelete.SetActive(false);
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
