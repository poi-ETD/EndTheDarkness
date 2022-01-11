using System.Collections;
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
    void StartPattern()
    {
        myEnemy.Board.text = "";
        if (BM.diecount < 4)
        {
            if (myEnemy.power)
            {
                if (BM.forward.Count > 0)
                {

                    for (int i = 0; i < TM.t; i++)
                    {
                        int rand = Random.Range(0, BM.forward.Count);
                        while (BM.characters[rand].isDie)
                            rand = Random.Range(0, ForwardHaveArmor.Count);
                        BM.characters[rand].onHit(1, myEnemy.Name);
                    }
                }
                else
                {

                    for (int i = 0; i < TM.t; i++)
                    {
                        int rand = Random.Range(0, 4);
                        while (BM.characters[rand].isDie)
                            rand = Random.Range(0, 4);
                        BM.characters[rand].onHit(1, myEnemy.Name);
                    }

                }
            }
            else
            {               
                    if (onecounter == 4)
                    {
                        done[0] = false;
                        done[1] = false;
                        done[2] = false;
                        onecounter = 0;
                      
                            for (int i = 0; i < 4; i++)
                            {
                               if(!BM.characters[i].isDie)
                                BM.characters[i].onHit(TM.t/2, myEnemy.Name);
                            }
                      
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
                        if (BM.forward.Count > 0)
                        {
                            for (int i = 0; i <4; i++)
                            {
                                int rand2 = Random.Range(0, BM.forward.Count);
                                while (BM.characters[rand2].isDie)
                                    rand2 = Random.Range(0, ForwardHaveArmor.Count);
                                BM.characters[rand2].onHit(2, myEnemy.Name);
                            }

                        }
                        else
                        {
                            for (int i = 0; i <4; i++)
                            {
                                int rand2 = Random.Range(0, 4);
                                while (BM.characters[rand2].isDie)
                                    rand2 = Random.Range(0, 4);
                                BM.characters[rand2].onHit(2, myEnemy.Name);
                            }

                        }
                    }
                        else
                        {
                        if (BM.forward.Count > 0)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                int rand2 = Random.Range(0, BM.forward.Count);
                                while (BM.characters[rand2].isDie)
                                    rand2 = Random.Range(0, ForwardHaveArmor.Count);
                                BM.characters[rand2].onHit(3, myEnemy.Name);

                            }
                        }
                        else
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                int rand2 = Random.Range(0, 4);
                                while (BM.characters[rand2].isDie)
                                    rand2 = Random.Range(0, 4);
                                BM.characters[rand2].onHit(3, myEnemy.Name);
                            }

                        }
                            onecounter++;
                        }
                    }
                }
            
        myEnemy.EnemyEndTurn();
            curTurn++;
        }
    }
}