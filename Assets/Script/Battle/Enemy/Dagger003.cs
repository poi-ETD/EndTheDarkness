using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dagger003 : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    public BattleManager BM;
    [SerializeField] Text t;
    [SerializeField] int plusname;
    public int pattern = 2;
    public int Dmg=2; 
    private void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy.Name = "단검" + plusname;
        StartPattern();
    }
    void Update()
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
            if (!myEnemy.isDie)
            {
                if (curTurn != 0)
                {
                   
                    int rand = Random.Range(0, pattern);
                    if (rand == 0)
                    {
                        BM.HitFront(Dmg, 1, myEnemy, false);
                        BM.HitFront(Dmg, 1, myEnemy, false);
                        BM.HitFront(Dmg, 1, myEnemy, false);
                    }
                    if (rand == 1)
                    {
                        BM.HitBack(Dmg, 0, myEnemy, false);
                        BM.HitBack(Dmg, 0, myEnemy, false);
                    }
                    if (rand == 2)
                    {
                        BM.HitFront(Dmg, 0, myEnemy, false);
                        BM.HitBack(Dmg, 0, myEnemy, false);
                        if (myEnemy.CanShadow())
                            BM.EnemyStateChange(myEnemy, 0);
                        else
                        {
                           
                              
                                    BM.HitAll(2, 4, myEnemy, false);
                            
                        }
                    }
                }
                 
            }
            curTurn++;
        }
        myEnemy.EnemyEndTurn();
    }
}