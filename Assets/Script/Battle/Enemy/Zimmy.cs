using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zimmy : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    public BattleManager BM;
    public int n;
    bool[] p = new bool[2];
    public int atk=1;
    private void Start()
    {
        myEnemy = GetComponent<Enemy>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy.Name = "지미";
        StartPattern();
      
    }
    void Update()
    {if (myEnemy.Hp <= 0) BM.Victory();
        if (curTurn != TM.turn)
        {
            StartPattern();
        }
       
    }
    void StartPattern()
    {

        if (BM.teamDieCount < BM.characters.Count)
        {
            if (!myEnemy.isDie)
            {
                if (n < 3)
                {
                    if (p[0] && p[1])
                    {
                        p[0] = false;
                        p[1] = false;
                        n++;
                    }
                    else if (p[0])
                    {
                        p[1] = true;
                        for (int i = 0; i < 3 + n; i++)
                        {
                            BM.HitFront(1, 0, myEnemy, false);
                        }
                        for (int i = 0; i <  n; i++)
                        {
                            BM.HitBack(1, 0, myEnemy, false);
                        }
                    }
                    else if (p[1]) {
                        p[0] = true;
                        for (int i = 0; i < 5 + 2 * n; i++)
                        {
                            BM.HitFront(1, 0, myEnemy, false);
                        }
                    }
                    else
                    {
                        int rand = Random.Range(0, 2);
                        p[rand] = true;
                        if (rand == 0)
                        {
                            for(int i = 0; i < 5 + 2 * n; i++)
                            {
                                BM.HitFront(1, 0, myEnemy, false);
                            }
                        }
                        if (rand == 1)
                        {
                            for (int i = 0; i < 3 +   n; i++)
                            {
                                BM.HitFront(1, 0, myEnemy, false);
                            }
                            for (int i = 0; i <  n; i++)
                            {
                                BM.HitBack(1, 0, myEnemy, false);
                            }
                        }
                    }
                }
                else
                {
                    if (p[0] && p[1])
                    {
                        p[0] = false;
                        p[1] = false;
                        n++;
                        for(int i = 0; i < n; i++)
                        {
                            BM.HitAll(atk, 0, myEnemy, false);
                        }
                    }
                    else if (p[0])
                    {
                        p[1] = true;
                        for (int i = 0; i < (3*n)/2; i++)
                        {
                            BM.HitAll(atk, 0, myEnemy, false);
                        }
                        
                    }
                    else if (p[1])
                    {
                        p[0] = true;
                        for (int i = 0; i < 3*n; i++)
                        {
                            BM.HitFront(atk, 0, myEnemy, false);
                        }
                    }
                    else
                    {
                        int rand = Random.Range(0, 2);
                        p[rand] = true;
                        if (rand == 0)
                        {
                            for (int i = 0; i < 3 * n; i++)
                            {
                                BM.HitFront(atk, 0, myEnemy, false);
                            }
                        }
                        if (rand == 1)
                        {
                            for (int i = 0; i < (3 * n) / 2; i++)
                            {
                                BM.HitAll(atk, 0, myEnemy, false);
                            }
                        }
                    }
                }
            }
            
        }
        curTurn++;
    }
}