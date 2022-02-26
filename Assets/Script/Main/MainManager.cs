using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject[,] Enemys;
    public BattleData bd=new BattleData();
    float wtime;
    [SerializeField] GameObject warn;
    bool smallmode;
    [SerializeField] Text smallt;
    [SerializeField] Text Ignum;
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
        if (File.Exists(path1) && File.Exists(path2)&&File.Exists(path3))
            SceneManager.LoadScene("battle");
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
    public void SmallMode()
    {if (smallmode == false)
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
}
public class BattleData
{
    public int battleNo;
    public int Ignum;
}
