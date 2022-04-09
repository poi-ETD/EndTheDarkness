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

        if (BM.diecount < BM.characters.Count)
        {
            if (!myEnemy.isDie)
            {
                if (curTurn != 0)
                {if (myEnemy.Shadow) myEnemy.Shadow = false;
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
                    }
                    if (rand == 2)
                    {
                        BM.HitFront(Dmg, 0, myEnemy, false);
                        BM.HitBack(Dmg, 0, myEnemy, false);
                        if(myEnemy.CanShadow())
                        myEnemy.onShadow();
                        else
                        {
                            for(int i = 0; i < BM.characters.Count; i++)
                            {if (!BM.characters[i].isDie)
                                    BM.HitAll(2, 4, myEnemy, false);
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