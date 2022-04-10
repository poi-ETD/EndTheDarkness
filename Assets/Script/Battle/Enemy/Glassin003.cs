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
    bool dieCheck;

    private void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy.Name = "글래신";
        StartPattern();
    }
    void Update()
    {
        if (myEnemy.isDie&&!dieCheck)
        {           
            Daggers[0].Dmg =  1;
            Daggers[1].Dmg = 1;
            Daggers[0].pattern++;
            Daggers[1].pattern++;           
            gameObject.SetActive(false);
            GameObject b1 = BM.Enemys[0];
            GameObject b2 = BM.Enemys[2];
            BM.Enemys = new GameObject[2];
            BM.Enemys[0] = b1;
            BM.Enemys[1] = b2;
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
                    myEnemy.EnemyEndTurn();
                    gameObject.SetActive(false);
                    GameObject b1 = BM.Enemys[0];
                    GameObject b2 = BM.Enemys[2];
                    BM.Enemys = new GameObject[2];
                    BM.Enemys[0] = b1;
                    BM.Enemys[1] = b2;
                }
                if (curTurn == 0)
                {

                    BM.HitAll(5, 5, myEnemy, true);
                    BM.EnemyGetAromor(10, myEnemy, myEnemy);
                }
                else
                {
                    for(int i = 0; i < BM.forward.Count; i++)
                    {
                        BM.HitFront(1, 5, myEnemy, true);
                    }
                    if (curTurn != 3)
                    {
                        Daggers[0].Dmg++;
                        Daggers[1].Dmg++;
                    }
                }
            }
            curTurn++;
            myEnemy.EnemyEndTurn();
        }
      
    }
}