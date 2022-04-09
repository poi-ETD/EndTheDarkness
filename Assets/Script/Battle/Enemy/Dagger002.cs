using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dagger002 : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    public BattleManager BM;
    [SerializeField] Text t;
    [SerializeField] Enemy another;
    bool pattern;

    private void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy.Name = "단검";
        StartPattern();
    }
    private void Update()
    {
        if (curTurn != TM.t)
        {
            StartPattern();
        }
    }

    void StartPattern()
    {
        if (BM.diecount < BM.characters.Count)
        {
            myEnemy.goingShadow = false;
            if (!myEnemy.isDie)
            {
                if (pattern)
                {
                    
                    pattern = false;
                    BM.EnemyGetAromor(5, myEnemy, myEnemy);
                }
                else
                {
                    int rand = Random.Range(0, 2);
                    if (rand == 0)
                    {
                        BM.HitFront(3, 1, myEnemy, true);
                        BM.HitFront(3, 1, myEnemy, true);
                    }
                    if (rand == 1)
                    {if (myEnemy.CanShadow())
                            BM.EnemyStateChange(myEnemy, 0);
                        else
                        {
                            BM.HitFront(3, 0, myEnemy, false);
                            BM.HitFront(3, 0, myEnemy, false);
                            BM.HitFront(3, 0, myEnemy, false);

                        }
                        BM.HitBack(1, 0, myEnemy, false);
                        pattern = true;
                    }
                }
            }                
        }
        myEnemy.EnemyEndTurn();
        curTurn++;
    }
}