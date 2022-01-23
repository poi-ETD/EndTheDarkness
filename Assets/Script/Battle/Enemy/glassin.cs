using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class glassin : MonoBehaviour
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
    [SerializeField] Enemy Ifrin;
    int rand;
    bool[] randCount;
    int myHp;
    int IfrininHp;
    bool p4;
    private void Start()
    {
        myHp = myEnemy.Hp;
        IfrininHp = Ifrin.Hp;
        randCount = new bool[3];
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy.Name = "글래신";
        StartPattern();
    }
    void Update()
    {
        if (myEnemy.isDie)
        {
            myEnemy.Hp = 0;

        }
        if (curTurn != TM.t)
        {
            StartPattern();
        }

        if (p4)
        {
            if (myEnemy.Hp != myHp || IfrininHp != Ifrin.Hp)
            {
                IfrininHp = Ifrin.Hp;
                myHp = myEnemy.Hp;
                if (myHp < IfrininHp)
                {
                    myEnemy.GetDynamicHp(Mathf.Abs(myHp - IfrininHp), myEnemy.Name);
                    Ifrin.GetDynamicHp(0, myEnemy.Name);
                }
                else
                {
                    Ifrin.GetDynamicHp(Mathf.Abs(myHp - IfrininHp), myEnemy.Name);
                    myEnemy.GetDynamicHp(0, myEnemy.Name);
                }
            }
        }
    }
    void StartPattern()
    {
        if (!myEnemy.isDie)
        {
            if (BM.diecount < 4)
            {
                if (!Ifrin.isDie)
                {
                    if ((curTurn + 1) % 4 != 0)
                    {
                        p4 = false;
                        rand = Random.Range(0, 3);

                        while (randCount[rand])
                        {
                            rand = Random.Range(0, 3);
                        }

                        if (rand == 0)
                        {
                            randCount[0] = true;
                            if (myEnemy.Hp > Ifrin.Hp)
                            {
                                Ifrin.GetArmor(6, myEnemy.Name);
                            }
                            else if (myEnemy.Hp < Ifrin.Hp)
                            {
                                myEnemy.GetArmor(6, myEnemy.Name);
                            }


                        }
                        if (rand == 1)
                        {
                            randCount[1] = true;
                            if (BM.diecount >= 1)
                            {
                                int rand2 = Random.Range(0, 4);
                                while (BM.characters[rand2].isDie)
                                {
                                    rand2 = Random.Range(0, 4);
                                }
                                BM.characters[rand2].NextTurnMinusAct++;
                            }
                            else
                            {
                                int rand2 = Random.Range(0, 4);
                                while (BM.characters[rand2].isDie)
                                {
                                    rand2 = Random.Range(0, 4);
                                }
                                int rand3 = Random.Range(0, 4);
                                while (BM.characters[rand3].isDie || rand2 == rand3) rand3 = Random.Range(0, 4);
                                BM.characters[rand2].NextTurnMinusAct++;
                                BM.characters[rand3].NextTurnMinusAct++;
                            }
                        }
                        if (rand == 2)
                        {
                            randCount[2] = true;
                            if (BM.forward.Count > 0)
                            {
                                int rand2 = Random.Range(0, BM.forward.Count);
                                while (BM.characters[rand2].isDie)
                                    rand2 = Random.Range(0, BM.forward.Count);
                                BM.characters[rand2].onHit(5, myEnemy.Name);

                            }
                            else
                            {
                                int rand2 = Random.Range(0, 4);
                                while (BM.characters[rand2].isDie)
                                    rand2 = Random.Range(0, 4);
                                BM.characters[rand2].onHit(5, myEnemy.Name);
                            }
                        }
                    }
                    else
                    {
                        p4 = true;
                        myHp = myEnemy.Hp;
                        IfrininHp = Ifrin.Hp;
                        randCount[0] = false;
                        randCount[1] = false;
                        randCount[2] = false;
                        if (myHp < IfrininHp)
                        {
                            myEnemy.GetHp(Mathf.Abs(myHp - IfrininHp), myEnemy.Name);
                        }
                        else
                        {
                            Ifrin.GetHp(Mathf.Abs(myHp - IfrininHp), myEnemy.Name);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (!BM.characters[i].isDie)
                        {
                            BM.characters[i].onHit(10, myEnemy.Name);
                            BM.characters[i].NextTurnMinusAct++;
                        }
                    }
                   
                }
                if (Ifrin.isDie)
                    myEnemy.EnemyEndTurn();
                curTurn++;
            }
        }
    }
}