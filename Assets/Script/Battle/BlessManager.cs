using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
public class BlessManager : MonoBehaviour
{
    [SerializeField] BattleManager BM;
    GameData GD;
    bool[] bless=new bool[21];
    private void Start()
    {
        string path3 = Path.Combine(Application.persistentDataPath, "GameData.json");
        if (File.Exists(path3))
        {
            string gameData = File.ReadAllText(path3);
            GD = JsonConvert.DeserializeObject<GameData>(gameData);
        }
      
            bless = GD.blessbool;
        if (bless[2])
        {
            
            int rand = Random.Range(BM.line, BM.CD.size);
            Debug.Log(BM.characters[rand]);
            BM.characters[rand].bless[2] = true;
            BM.characters[rand].Atk += 2;
        }
        if (bless[3]&&GD.bless3count>0)
        {
            BM.CardCount += 3;
            BM.TurnCardCount +=3;
            GD.bless3count--;
            if (GD.bless3count == 0) GD.blessbool[3] = false;
        }
        if (bless[4])
        {
            BM.gd.blessbool[4] = true;
        }
        if (bless[7])
        {
            BM.gd.blessbool[7] = true;
        }
        if (bless[17])
        {
            for(int i = 0; i < BM.characters.Count; i++)
            {
                BM.characters[i].DefUp(1);
            }
        }
    }
}
