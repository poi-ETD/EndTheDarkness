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
        myEnemy.Name = "단검" +1;
        StartPattern();
    }
    void Update()
    {
        if (myEnemy.isDie)
        {
            myEnemy.Hp = 0;
            if (another.isDie)
            {
                BM.Victory();
            }
        }
        if (curTurn != TM.t)
        {
            StartPattern();
        }
    }
    void StartPattern()
    {
        if (BM.diecount < 4)
        {
            if (!myEnemy.isDie)
            {
                if (pattern)
                {
                    myEnemy.Shadow = false;
                    pattern = false;
                    myEnemy.GetArmor(5,myEnemy.Name);
                }
                else
                {
                    int rand = Random.Range(0, 2);
                    if (rand == 0)
                    {
                        BM.HitFront(3, 1, myEnemy.Name, true);
                        BM.HitFront(3, 1, myEnemy.Name, true);
                    }
                    if (rand == 1)
                    {
                        myEnemy.onShadow();
                        BM.HitBack(1, 0, myEnemy.Name, false);
                        pattern = true;
                    }
                }
            }                
        }                
        curTurn++;
    }
}