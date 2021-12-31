using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reply : MonoBehaviour
{

    [SerializeField] Character myCharacter;
    public int passive;
    GameObject[] enemys;
    Enemy[] enemyScript;
    [SerializeField] int[] EnemyHp;
    TurnManager TM;
    BattleManager BM;
    bool passive1;
    private void Awake()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemyScript = new Enemy[enemys.Length];
        EnemyHp = new int[enemys.Length];
        for (int i = 0; i < enemys.Length; i++)
        {
            enemyScript[i] = enemys[i].GetComponent<Enemy>();
            EnemyHp[i] = enemyScript[i].Hp;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (passive == 1)
        {
            if (TM.turnCard % 3 == 0 && TM.turnCard > 0 && !passive1)
            {
                passive1 = true;
                myCharacter.Act++;
            }
            else if (TM.turnCard % 3 != 0 || TM.turnCard == 0)
            {
                passive1 = false;
            }
        }
        if (passive == 2)
        {
            if (myCharacter.isSet)
            {
                myCharacter.isSet = false;
                for (int i = 0; i < enemys.Length; i++)
                {
                    if (EnemyHp[i] != enemyScript[i].Hp)
                    {
                        if (EnemyHp[i] > enemyScript[i].Hp)
                        {
                            enemyScript[i].onHit(myCharacter.Atk);
                        }
                        EnemyHp[i] = enemyScript[i].Hp;
                    }
                }
            }
        }
    }
}
