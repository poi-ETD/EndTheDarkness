using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reply : MonoBehaviour
{

    [SerializeField] Character myCharacter;
    public bool[] passive;
    GameObject[] enemys;
    Enemy[] enemyScript;
    [SerializeField] int[] EnemyStack;
    TurnManager TM;
    BattleManager BM;
    bool Passive1;
    private void Awake()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemyScript = new Enemy[enemys.Length];
        EnemyStack = new int[enemys.Length];
        for (int i = 0; i < enemys.Length; i++)
        {
            enemyScript[i] = enemys[i].GetComponent<Enemy>();
            
        }
        myCharacter.Name = "스파키";
    }
    // Update is called once per frame
    void passive1()
    {
        if (TM.turnCard % 3 == 0 && TM.turnCard > 0 && !Passive1)
        {
            Passive1 = true;
            myCharacter.Act++;
            BM.log.logContent.text += "\n지치지 않는 폭주!스파키의 현재 행동력이 추가됩니다.";
        }
        else if (TM.turnCard % 3 != 0 || TM.turnCard == 0)
        {
            Passive1 = false;
        }
    }
    void passive2()
    {
        for (int i = 0; i < enemys.Length; i++)
        {
            if (EnemyStack[i] != enemyScript[i].dmgStack)
            {
              
                enemyScript[i].onHit(myCharacter.turnAtk);
                EnemyStack[i]++;
                EnemyStack[i]++;
                BM.log.logContent.text += "\n독단적인 팀플레이!"+enemyScript[i].Name+"에게 "+ myCharacter.turnAtk+"의 데미지가 주어집니다.";
            }
        }
    }
    void Update()
    {
        if (passive[2])
        {
            passive2();
        }
        if (!myCharacter.isDie)
        {
            if (passive[1])
            {
                passive1();
            }
            if (myCharacter.isSet)
            {
                myCharacter.isSet = false;
            }
            if (myCharacter.isTurnEnd)
            {
                myCharacter.isTurnEnd = false;
            }
            if (myCharacter.isTurnStart)
            {
                for(int i = 0; i < enemyScript.Length; i++)
                {
                    EnemyStack[i] =
                       enemyScript[i].dmgStack;
                }
                myCharacter.isTurnStart = false;
            }
        }
    }
}
