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
    }
    // Update is called once per frame
    void passive1()
    {
        if (TM.turnCard % 3 == 0 && TM.turnCard > 0 && !Passive1)
        {
            Passive1 = true;
            myCharacter.Act++;
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
