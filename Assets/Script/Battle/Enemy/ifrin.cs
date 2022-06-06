using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ifrin : MonoBehaviour
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
    [SerializeField] Enemy Glassin;
    int rand;
    bool[] randCount;
    int myHp;
    int glassinHp;
    bool p4;
    private void Start()
    {
        myHp = myEnemy.Hp;
        glassinHp = Glassin.Hp;
        randCount = new bool[3];
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy.Name = "이프린";
        StartPattern();
    }
    void Update()
    {
        if (myEnemy.isDie)
        {
            myEnemy.Hp = 0;
            if (Glassin.isDie) BM.Victory();
        }
        if (curTurn != TM.turn)
        {
            StartPattern();
        }
        if (p4 && myEnemy.Hp != myHp || p4 && glassinHp != Glassin.Hp)
        {
            glassinHp = Glassin.Hp;
            myHp = myEnemy.Hp;
            for (int i = 0; i < BM.characters.Count; i++)
            {
                if (!BM.characters[i].isDie)
                {
                   // BM.characters[i].onDynamicHit(Mathf.Abs(myHp - Glassin.Hp), myEnemy.Name);
                }
            }
        }
    }
    void StartPattern()
    {
        if (!myEnemy.isDie)
        {
            if (BM.teamDieCount < BM.characters.Count)
            {
                if (!Glassin.isDie)
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
                            for (int i = 0; i < 2; i++)
                            {
                                if (BM.forward.Count > 0)
                                {
                                    int rand2 = Random.Range(0, BM.forward.Count);
                                    while (BM.characters[rand2].isDie)
                                        rand2 = Random.Range(0, BM.forward.Count);
                                  //  BM.characters[rand2].onHit(5, myEnemy.Name);

                                }
                                else
                                {
                                    int rand2 = Random.Range(0, BM.characters.Count);
                                    while (BM.characters[rand2].isDie)
                                        rand2 = Random.Range(0, BM.characters.Count);
                                  //  BM.characters[rand2].onHit(5, myEnemy.Name);

                                }
                            }
                        }
                        if (rand == 1)
                        {
                            randCount[1] = true;
                            for (int i = 0; i < BM.characters.Count; i++)
                            {
                                if (!BM.characters[i].isDie)
                                {
                                   // BM.characters[i].onHit(3, myEnemy.Name);
                                }
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
                                BM.characters[rand2].StatusAbnom(0, 2);

                            }
                            else
                            {
                                int rand2 = Random.Range(0, BM.characters.Count);
                                while (BM.characters[rand2].isDie)
                                    rand2 = Random.Range(0, BM.characters.Count);
                                BM.characters[rand2].StatusAbnom(0, 2);
                            }
                        }
                    }
                    else
                    {
                        p4 = true;
                        myHp = myEnemy.Hp;
                        glassinHp = Glassin.Hp;
                        for (int i = 0; i < BM.characters.Count; i++)
                        {
                            if (!BM.characters[i].isDie)
                            {
                               // BM.characters[i].onHit(Mathf.Abs(Glassin.Hp - myHp), myEnemy.Name);
                            }
                        }
                        randCount[0] = false;
                        randCount[1] = false;
                        randCount[2] = false;
                    }

                }
                if (Glassin.isDie)
                {
                    for (int i = 0; i < BM.characters.Count; i++)
                    {
                        if (!BM.characters[i].isDie)
                        {
                          //  BM.characters[i].onHit(20, myEnemy.Name);
                            BM.characters[i].NextTurnMinusAct+=5;
                        }
                    }
                }
                myEnemy.EnemyEndTurn();
                curTurn++;
            }
        }
    }
}