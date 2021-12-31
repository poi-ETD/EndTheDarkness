using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vangara : MonoBehaviour
{
    [SerializeField] Character myCharacter;
    public int passive;
    [SerializeField] int[] TeamPlayerHP=new int[3];
    GameObject[] enemys;
    Enemy[] enemyScript;
    BattleManager BM;
    Character[] TeamCharacter = new Character[3];
    [SerializeField] int[] EnemyHp;
    private void Start()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        int count = 0;
        for(int i = 0; i < 4; i++)
        {
            if (BM.characters[i] != myCharacter)
            {
                TeamCharacter[count] = BM.characters[i];
                TeamPlayerHP[count] = TeamCharacter[count].Hp;
                count++;
            }
        }
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemyScript = new Enemy[enemys.Length];
        EnemyHp = new int[enemys.Length];
        for (int i = 0; i < enemys.Length; i++)
        {
            enemyScript[i] = enemys[i].GetComponent<Enemy>();
            EnemyHp[i] = enemyScript[i].Hp;
        }
     
    }
    private void Update()
    {
        if (myCharacter.isSet)
        {
           myCharacter.isSet = false;
            if (passive == 1) { 
            for(int i = 0; i < 3; i++)
            {
                if (TeamPlayerHP[i] != TeamCharacter[i].Hp)
                {
                    if (TeamPlayerHP[i] >TeamCharacter[i].Hp)
                    {
                            myCharacter.Armor += 2;
                    }
                    TeamPlayerHP[i] = TeamCharacter[i].Hp;
                }
            }
                for (int i = 0; i < enemys.Length; i++)
                {
                    if (EnemyHp[i] != enemyScript[i].Hp)
                    {
                        if (EnemyHp[i] > enemyScript[i].Hp)
                        {
                            myCharacter.Armor += 2;
                        }
                        EnemyHp[i] = enemyScript[i].Hp;
                    }
                }
            }
        }
        if (myCharacter.isTurnStart && passive == 2)
        {
            myCharacter.isTurnStart = false;
            if (myCharacter.Armor > 0) BM.cost += 1;
            else
            {
                myCharacter.Armor = 3;
            }
        }

    }
}
