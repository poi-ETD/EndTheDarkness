using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class BlessManager : MonoBehaviour
{
    BlessData bld;
    BattleManager BM;
    int[] characterHp;
    private void Start()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        string path2 = Path.Combine(Application.persistentDataPath, "BlessData.json");
        if (File.Exists(path2))
        {
            string blessData = File.ReadAllText(path2);
            bld = JsonUtility.FromJson<BlessData>(blessData);
        }
        if (bld.BlessOn[2])
        {
            
            int rand = Random.Range(0, BM.back.Count);
            BM.back[rand].Atk+=2;
            BM.back[rand].turnAtk += 2;
            BM.back[rand].bless[2] = true;
        }
        if (bld.BlessOn[3])
        {
            BM.TurnCardCount = 7;
            BM.CardCount = 7;
            bld.BlessCount[3]--;
            if (bld.BlessCount[3] == 0)
            {
                bld.BlessOn[3] = false;
            }
          
            string blessData = JsonUtility.ToJson(bld);
            File.WriteAllText(path2, blessData);
        }
        if (bld.BlessOn[4])
        {
            BM.BlessBM[4] = true;
            for(int i = 0; i < BM.characters.Count; i++)
            {
                BM.characters[i].Act = 0;
            }
        }
        if (bld.BlessOn[6])
        { 
        }
        if (bld.BlessOn[7])
        {
            
            BM.BlessBM[7] = true;
        }
    }
}
