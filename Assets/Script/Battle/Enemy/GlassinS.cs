using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlassinS : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    public BattleManager BM;
    public List<Character> HaveArmor = new List<Character>();
    public List<Character> ForwardHaveArmor = new List<Character>();
    int count;
    [SerializeField] Text t;
    [SerializeField] Enemy Ifrin;
    int rand;
    bool[] randCount;
    private void Start()
    {
        
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
            BM.Victory();
        }
        if (curTurn != TM.t)
        {
            StartPattern();
        }

        
 
    }
    void StartPattern()
    {
        if (!myEnemy.isDie)
        {
            if (BM.diecount < 4)
            {
                if (Ifrin.Hp > 10)
                {
                    if ((curTurn + 1) % 4 != 0)
                    {

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
                                if (myEnemy.Hp < Ifrin.Hp)
                                {
                                 
                                    myEnemy.GetHp(5, myEnemy.Name);
                                }
                                else if(myEnemy.Hp>Ifrin.Hp)
                                {
                                 
                                    Ifrin.GetHp(5, myEnemy.Name);
                                }
                               
                               
                            
                        }
                    }
                    else
                    {


                        Enemy lessHp;
                        if (myEnemy.Hp < Ifrin.Hp)
                        {
                            lessHp = myEnemy;
                            lessHp.GetHp(Mathf.FloorToInt((lessHp.maxHp - lessHp.Hp) * 0.3f), myEnemy.Name);
                        }
                        else if(myEnemy.Hp > Ifrin.Hp)
                        {
                            lessHp = Ifrin;
                            lessHp.GetHp(Mathf.FloorToInt((lessHp.maxHp - lessHp.Hp) * 0.3f), myEnemy.Name);
                        }
                       
                        randCount[0] = false;
                        randCount[1] = false;
                        randCount[2] = false;

                    }
                }
                else
                {
                    Ifrin.GetHp(myEnemy.Hp / 10, myEnemy.Name);
                    myEnemy.onEnemyHit(myEnemy.Hp / 10, myEnemy.Name);
                }
            }
                curTurn++;
            }
        
    }
}