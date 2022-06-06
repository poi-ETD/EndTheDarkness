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
        myEnemy.Name = "레인저";
        StartPattern();
    }
    private void Update()
    {
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
                if (pattern[0] && pattern[1])
                {
                    pattern[0] = false;
                    pattern[1] = false;
                    BM.HitFront(15, 1, myEnemy, false);
                }
                else
                {
                    int rand = Random.Range(0, 2);
                    pattern[rand] = true;
                    if (rand == 0)
                    {
                        BM.HitFront(5, 0, myEnemy, false);
                        BM.HitBack(1, 2, myEnemy, false);
                    }
                    if (rand == 1)
                    {
                        BM.EnemyGetAromor(3, myEnemy, myEnemy);
                        BM.HitFront(5, 0, myEnemy, false);
                    }
                }
            }

        }
        myEnemy.EnemyEndTurn();
        curTurn++;
    }
}