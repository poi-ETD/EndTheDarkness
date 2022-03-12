using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Poly0 : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    public BattleManager BM;
    private void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy.Name = "폴리";
        StartPattern();
    }
    void Update()
    {
        if (myEnemy.Hp <= 100)
        {
            BM.Victory();
        }
        if (curTurn != TM.t)
        {
            StartPattern();
        }
    }
    bool phase2start;
    int phase2;
    void StartPattern()
    {
      
        if (BM.diecount <BM.characters.Count)
        {
            
            if (myEnemy.Hp > 200)
            {
                int rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    BM.HitFront(4
                        , 0, myEnemy.Name, false);
                    BM.HitFront(4, 0, myEnemy.Name, false);
                }
                else if (rand == 1)
                {
                    myEnemy.GetArmor(10, myEnemy.Name);
                }
                else if (rand == 2)
                {
                    BM.HitFront(3, 0, myEnemy.Name, false);
                    myEnemy.GetArmor(5, myEnemy.Name);
                }      
            }
            if (myEnemy.Hp <= 200)
            {
                if (!phase2start)
                {
                    phase2start = true;
                    myEnemy.GetArmor(20, myEnemy.Name);
                    BM.FormationCollapse(myEnemy.Name);
                    for(int i = 0; i < BM.characters.Count; i++)
                    {
                        if (!BM.characters[i].isDie) BM.characters[i].NextTurnMinusAct+=5;
                    }
                }
                else
                {
                    phase2++;
                    if (phase2 % 2 == 0)
                    {
                        BM.HitFront(myEnemy.Armor*2, 0, myEnemy.Name, false);
                        myEnemy.Armor = 0;
                        myEnemy.GetArmor((myEnemy.Hp)/10,myEnemy.Name);
                    }
                    else
                    {
                        int k = (myEnemy.maxHp - myEnemy.Hp) / 10;
                        int n = Random.Range(1, k);
                        int m = k - n;
                        BM.HitFront(n, 0, myEnemy.Name, false);
                        BM.HitFront(m, 0, myEnemy.Name, false);
                    }
                }
            }
            myEnemy.EnemyEndTurn();
            curTurn++;
        }
    }
}