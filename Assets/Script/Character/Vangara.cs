using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vangara : MonoBehaviour
{
    [SerializeField] Character myCharacter;
    public bool[] passive;
    [SerializeField] int[] TeamStack=new int[3];
    GameObject[] enemys;
    Enemy[] enemyScript;
    [SerializeField] int[] EnemyStack;
    BattleManager BM;
    Character[] TeamCharacter = new Character[3];
  
    private void Start()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        int count = 0;
        for(int i = 0; i < 4; i++)
        {
            if (BM.characters[i] != myCharacter)
            {
                TeamCharacter[count] = BM.characters[i];
                
                count++;
            }
        }
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemyScript = new Enemy[enemys.Length];
        EnemyStack = new int[enemys.Length];
        for (int i = 0; i < enemys.Length; i++)
        {
            enemyScript[i] = enemys[i].GetComponent<Enemy>();
           
        }
     
    }
    void passive1()
    {
        for (int i = 0; i < 3; i++)
        {
            if (TeamStack[i] != TeamCharacter[i].dmgStack)
            {
                while (TeamStack[i] != TeamCharacter[i].dmgStack)
                {

                    TeamStack[i]++;
                    myCharacter.Armor += 2;
                }
            }
        }
        for(int i = 0; i < enemys.Length; i++)
        {
            if (EnemyStack[i] != enemyScript[i].dmgStack)
            {
                while (EnemyStack[i] != enemyScript[i].dmgStack)
                {
                    EnemyStack[i]++;
                    myCharacter.Armor += 2;
                }
            }
        }
    }
    void passive2()
    {
        if (myCharacter.Armor > 0)
            BM.cost++;
        else
        {
            myCharacter.Armor += 3;
        }
    }
    private void Update()
    {
        if (!myCharacter.isDie) { 
        if (myCharacter.isSet)
        {
            if (passive[1])
            {
                passive1();
            }
            myCharacter.isSet = false;
        }
        if (myCharacter.isTurnEnd)
        {
            myCharacter.isTurnEnd = false;
        }
        if (myCharacter.isTurnStart)
        {
            if (passive[2])
                passive2();
            myCharacter.isTurnStart = false;
        }

    }
}}
