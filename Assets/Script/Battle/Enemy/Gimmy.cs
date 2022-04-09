﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gimmy : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    int phase1;
    int phase2;
    int phase3;
    public BattleManager BM;
    public List<Character> HaveArmor = new List<Character>();
    public List<Character> ForwardHaveArmor = new List<Character>();
    int count;
    [SerializeField] Text t;
    public Enemy[] R;
    [SerializeField] GameObject PowerLine;
    int onecounter;
    bool[] done = new bool[3];
    int PhaseTurn;
    private void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy.Name = "지미";
        myEnemy.power = true;
        StartPattern();
    }
    void Update()
    {
        if (R[0].isDie && R[1].isDie)
        {
            myEnemy.power = false;
            PowerLine.SetActive(false);
        }
        else
        {
            myEnemy.Hp = 200;
            myEnemy.power = true;
        }
        if (myEnemy.Hp<100)
        {
            
            BM.Victory();
        }
        if (curTurn != TM.t)
        {
            StartPattern();
        }
    }
    bool phaseStart=false;
    void StartPattern()
    {
        myEnemy.Board.text = "";
        if (BM.diecount < BM.characters.Count)
        {
            if (myEnemy.power)
            {
                for (int i = 0; i < TM.t; i++)
                {
                    BM.HitFront(1, 0, myEnemy, false);
                }
            }
            else
            {

                if (!phaseStart)
                {
                    PhaseTurn = TM.t;
                    phaseStart = true;
                    myEnemy.GetArmor(100 - TM.t, myEnemy.Name);
                  
                }
                if (onecounter == 4)
                {
                    done[0] = false;
                    done[1] = false;
                    done[2] = false;
                    onecounter = 0;

                   
                            BM.HitAll(PhaseTurn / 2,4, myEnemy,false);
                    

                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (!done[i])
                        {
                            break;
                        }
                        if (i == 2)
                        {
                            done[0] = false;
                            done[2] = false;
                            done[1] = false;
                        }
                    }//done 3개 다 true라면 초기화
                    int rand = Random.Range(0, 3);
                    while (done[rand])
                    {
                        rand = Random.Range(0, 3);
                    }
                    done[rand] = true;
                    if (rand == 2)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            BM.HitFront(2, 0, myEnemy, false);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 2; i++)
                            BM.HitFront(3, 0, myEnemy, false);

                    }
                    onecounter++;

                }
            
                }
            
        myEnemy.EnemyEndTurn();
            curTurn++;
        }
    }
}