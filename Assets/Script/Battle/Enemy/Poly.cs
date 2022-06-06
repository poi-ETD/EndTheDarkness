using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Poly : MonoBehaviour
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
    private void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy.Name = "폴리";
        StartPattern();
    }
    void Update()
    {
        if (myEnemy.isDie)
        {
            myEnemy.Hp = 0;
            BM.Victory();
        }
        if (curTurn != TM.turn)
        {
            StartPattern();
        }
    }
    void StartPattern()
    {
    
        if (BM.teamDieCount < BM.characters.Count)
        {
            if (myEnemy.Hp > 150)
            {
                myEnemy.immortal = true;
                phase1++;
                if (phase1 % 2 == 1)
                {
                    BM.HitFront(8, 1, myEnemy, false);
                    BM.HitFront(8, 1, myEnemy, false);
                }
                else if (phase1 % 2 == 0)
                {
                    myEnemy.GetArmor(5, myEnemy.Name);
                    BM.HitFront(3, 1, myEnemy, false);
                }
            }
            else if (myEnemy.Hp <= 150 && myEnemy.Hp > 50)
            {
                myEnemy.immortal = true;

                phase2++;
                if (phase2 == 1)
                {

                    for (int i = 0; i < BM.characters.Count; i++)
                    {
                        BM.characters[i].NextTurnMinusAct += 5;
                    }

                    myEnemy.GetArmor(30, myEnemy.Name);
                }
                else if (phase2 % 2 == 0)
                {

                    BM.HitFront(10, 1, myEnemy, true);

                }
                else if (phase2 % 2 == 1)
                {
                    myEnemy.GetArmor(10, myEnemy.Name);
                }
            }
            else if (myEnemy.Hp <= 50)
            {

                myEnemy.noDie = false;
                phase3++;

                if (phase3 == 1)
                {
                    for (int i = 0; i < BM.characters.Count; i++)
                    {

                        BM.characters[i].NextTurnMinusAct++;
                    }
                    count = 6;
                    myEnemy.GetArmor(100, myEnemy.Name);
                }
                else if (phase3 < 7)
                {
                    count--;
                    t.text = "" + count;
                }
                else if (phase3 == 7)
                {
                    t.text = "";
                    for (int i = 0; i < BM.characters.Count; i++)
                    {
                        if (!BM.characters[i].isDie)
                        {
                          //  BM.characters[i].onHit(myEnemy.Armor, myEnemy.Name);
                        }
                    }
                }
                else
                {
                    myEnemy.GetArmor(10, myEnemy.Name);
                    BM.HitFront(13, 1, myEnemy, false);
                }
            }
            myEnemy.EnemyEndTurn();

            curTurn++;
        }
    }
}