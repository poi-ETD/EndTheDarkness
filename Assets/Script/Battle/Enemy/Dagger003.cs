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
    public int Dmg=1;
  
    private void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy.Name = "단검" + plusname;
        StartPattern();
    }
    void Update()
    {
        if (myEnemy.isDie)
        {
            myEnemy.Hp = 0;

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
                if (curTurn != 0)
                {if (myEnemy.Shadow) myEnemy.Shadow = false;
                    int rand = Random.Range(0, pattern);
                    if (rand == 0)
                    {
                        BM.HitFront(Dmg, 1, myEnemy.Name, false);
                        BM.HitFront(Dmg, 1, myEnemy.Name, false);
                        BM.HitFront(Dmg, 1, myEnemy.Name, false);
                    }
                    if (rand == 1)
                    {
                        BM.HitBack(Dmg, 0, myEnemy.Name, false);
                    }
                    if (rand == 2)
                    {
                        BM.HitFront(Dmg, 0, myEnemy.Name, false);
                        BM.HitBack(Dmg, 0, myEnemy.Name, false);
                        if(myEnemy.CanShadow())
                        myEnemy.onShadow();
                        else
                        {
                            for(int i = 0; i < 4; i++)
                            {if (!BM.characters[i].isDie)
                                    BM.characters[i].onHit(2,myEnemy.Name);
                            }
                        }
                    }
                }
                 
            }
            curTurn++;
        }
        if (plusname == 1) myEnemy.EnemyEndTurn();
    }
}