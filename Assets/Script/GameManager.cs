using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UserSaveData
{
    public bool isInit;

    public string[] slot_Names;
    public int nowPlayingSlot;
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    [SerializeField] public GameObject EscViewPrefeb;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

 
    [HideInInspector] public bool isInit; // 게임 최초 실행자일시 false지만 한번 실행하고 난뒤로부터는 항상 true
                                          // 즉, path에 UserMainData.json이 없을 시 false, 있을 시 true

    [HideInInspector] public string[] slot_Names; // 각 슬롯의 이름을 담는 문자열 배열
    [HideInInspector] public string[] slot_CharacterDatas; // 각 슬롯의 캐릭터 데이터가 담긴 json 파일의 이름을 담는 문자열 배열
    [HideInInspector] public string[] slot_CardDatas; // 각 슬롯의 카드 데이터가 담긴 json 파일의 이름을 담는 문자열 배열

    [HideInInspector] public int selectedSlot_Main;
    [HideInInspector] public int nowPlayingSlot; // 현재 게임에 이용하고 있는 슬롯의 번호 (0~9)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        slot_Names = new string[10];
        slot_CharacterDatas = new string[10];
        slot_CardDatas = new string[10];
    }
  
    void Start()
    {
        slot_CharacterDatas[0] = "CharacterData1.json";
        slot_CharacterDatas[1] = "CharacterData2.json";
        slot_CharacterDatas[2] = "CharacterData3.json";
        slot_CharacterDatas[3] = "CharacterData4.json";
        slot_CharacterDatas[4] = "CharacterData5.json";
        slot_CharacterDatas[5] = "CharacterData6.json";
        slot_CharacterDatas[6] = "CharacterData7.json";
        slot_CharacterDatas[7] = "CharacterData8.json";
        slot_CharacterDatas[8] = "CharacterData9.json";
        slot_CharacterDatas[9] = "CharacterData10.json";
        slot_CardDatas[0] = "CardData1.json";
        slot_CardDatas[1] = "CardData2.json";
        slot_CardDatas[2] = "CardData3.json";
        slot_CardDatas[3] = "CardData4.json";
        slot_CardDatas[4] = "CardData5.json";
        slot_CardDatas[5] = "CardData6.json";
        slot_CardDatas[6] = "CardData7.json";
        slot_CardDatas[7] = "CardData8.json";
        slot_CardDatas[8] = "CardData9.json";
        slot_CardDatas[9] = "CardData10.json";

        if (File_Exist_Examine())
            Load();
        else
        {
            Init();
            Save();
        }
    }


    private void Init()
    {
        for (int i = 0; i < 10; i++)
            slot_Names[i] = "";

        isInit = true;
    }

    public void Save()
    {
        UserSaveData userData = new UserSaveData();

        userData.isInit = isInit;

        userData.slot_Names = slot_Names;
        userData.nowPlayingSlot = nowPlayingSlot;

        string json = JsonUtility.ToJson(userData);

        string fileName = "UserMainData";
        string path = Application.persistentDataPath + "/" + fileName;

        File.WriteAllText(path, json);

        Debug.Log("로컬에 데이터저장 완료");
    }

    public void Load()
    {
        string fileName = "UserMainData";
        string path = Application.persistentDataPath + "/" + fileName;

        string json = File.ReadAllText(path);

        UserSaveData userData = JsonUtility.FromJson<UserSaveData>(json);

        isInit = userData.isInit;

        slot_Names = userData.slot_Names;
        nowPlayingSlot = userData.nowPlayingSlot;

        Debug.Log("로컬로부터 데이터불러오기 완료");
    }

    public bool File_Exist_Examine()
    {
        string fileName = "UserMainData";
        string path = Application.persistentDataPath + "/" + fileName;

        return File.Exists(path);
    }
}
