using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranger002 : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    public BattleManager BM;
    [SerializeField] Text t;
    [SerializeField] Enemy another;
    bool[] pattern = new bool[2];
    private void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy.Name = "레인저" + 1;
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
        if (BM.diecount < BM.characters.Count)
        {
            if (!myEnemy.isDie)
            {
                if (pattern[0] && pattern[1])
                {
                    pattern[0] = false;
                    pattern[1] = false;
                    BM.HitFront(15, 1, myEnemy.Name, false);
                }
                else
                {
                    int rand = Random.Range(0, 2);
                    pattern[rand] = true;
                    if (rand == 0)
                    {
                        BM.HitFront(5, 0, myEnemy.Name, false);
                        BM.HitBack(1, 2, myEnemy.Name, false);
                    }
                    if (rand == 1)
                    {
                        myEnemy.GetArmor(3, myEnemy.Name);

                        BM.HitFront(5, 0, myEnemy.Name, false);
                    }
                }
            }

        }
        myEnemy.EnemyEndTurn();
        curTurn++;
    }
}