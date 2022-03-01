using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Glassin003 : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    public BattleManager BM;
    [SerializeField] Text t;
    [SerializeField] int plusname;
    public int pattern = 2;
    public Dagger003[] Daggers;

    private void Start()
    {
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
            Daggers[0].Dmg=1;
            Daggers[1].Dmg = 1;
            Daggers[0].pattern++;
            Daggers[1].pattern++;
            gameObject.SetActive(false);
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
                if (curTurn >= 4)
                {
                    Daggers[0].pattern++;
                    Daggers[1].pattern++;
                    gameObject.SetActive(false);
                }
                if (curTurn == 0)
                {
                  for(int i = 0; i < BM.characters.Count; i++)
                    {
                        if (!BM.characters[i].isDie)
                            BM.characters[i].NextTurnMinusAct+=5;
                    }
                    myEnemy.GetArmor(10, myEnemy.Name);
                }
                else
                {
                    for(int i = 0; i < BM.forward.Count; i++)
                    {
                        BM.forward[i].NextTurnMinusAct++;
                    }
                    if (curTurn != 3)
                    {
                        Daggers[0].Dmg++;
                        Daggers[1].Dmg++;
                    }
                }
            }
            curTurn++;
        }
    }
}