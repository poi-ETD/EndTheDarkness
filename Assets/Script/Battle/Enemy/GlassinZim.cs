using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlassinZim : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    public BattleManager BM;
    [SerializeField] Zimmy z;


    private void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy.Name = "글래신";
        StartPattern();
       
    }
    void Update()
    {
        if (curTurn != TM.t)
        {
            StartPattern();
        }
        myEnemy.power = true;
    }
    void StartPattern()
    {

        if (BM.diecount < BM.characters.Count)
        {
            if (!myEnemy.isDie)
            {
                myEnemy.onShadow();
                if (z.n < 3)
                {
                    int rand = Random.Range(0, 2);
                    if (rand == 0)
                    {   if(z!=null)
                        z.myEnemy.GetArmor(10, myEnemy.Name);
                        int rand2 = Random.Range(0, BM.characters.Count);
                        while (BM.characters[rand2].isDie)
                        {
                            rand2 = Random.Range(0, BM.characters.Count);
                        }
                        BM.characters[rand2].NextTurnMinusAct++;
                    }
                    if (rand == 1)
                    {
                        if (z != null)
                           // z.myEnemy.GetHp(8 + z.n * 2, myEnemy.Name);
                        BM.HitAll(1 + z.n, 0, myEnemy, false);
                    }
                }
                else
                {
                    int rand = Random.Range(0, 3);
                    if (rand == 0)
                    {
                        z.atk = 1;
                        z.n++;
                        int rand2 = Random.Range(0, BM.characters.Count);
                        while (BM.characters[rand2].isDie)
                        {
                            rand2 = Random.Range(0, BM.characters.Count);
                        }
                        BM.characters[rand2].NextTurnMinusAct++;
                    }
                    if (rand == 1)
                    {
                        z.atk = 1;
                        z.myEnemy.GetArmor(10 + 2 * z.n, myEnemy.Name);
                    }
                    if (rand == 2)
                    {
                        z.atk = 2;
                    }
                }
            }
         
            myEnemy.EnemyEndTurn();
            curTurn++;
        }
    }
}