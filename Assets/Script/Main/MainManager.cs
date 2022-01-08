using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject[,] Enemys;
    public BattleData bd=new BattleData();
    public void GoBattleScene(int p)
    {
        bd.battleNo = p;
        SaveBattleDataToJson();
        SceneManager.LoadScene("battle");
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
        string path = Path.Combine(Application.dataPath, "battleData.json");
        File.WriteAllText(path, battleData);
    }
         
}
public class BattleData
{
    public int battleNo;
}
