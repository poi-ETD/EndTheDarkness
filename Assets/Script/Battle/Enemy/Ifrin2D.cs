using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ifrin2D : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    int phase1;
    int phase2;
    int phase3;
    public BattleManager BM;
    int posionCount;
    [SerializeField] Text t;

    private void Start()
    {
        myEnemy = GetComponent<Enemy>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy.Name = "이프린";
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
        if (!myEnemy.isDie)
        {
            if (BM.diecount < BM.characters.Count)
            {
                if (posionCount < 5)
                {
            
                    int rand = Random.Range(0, 3);
                    if (rand == 0)
                    {
                        for(int i = 0; i < curTurn+1; i++)
                        {
                            BM.HitFront(3, 0, myEnemy, false);
                        }
                    }
                    if (rand == 1)
                    {
                        if (myEnemy.CanShadow())
                            myEnemy.onShadow();
                        else
                        {
                            posionCount++;
                            for (int i = 0; i < BM.characters.Count; i++)
                            {
                                if (!BM.characters[i].isDie)
                                {                           
                                    BM.characters[i].StatusAbnom(0, 1);
                                }
                            }
                        }
                        posionCount+=2;
                        for (int i = 0; i < BM.characters.Count; i++)
                        {
                            if (!BM.characters[i].isDie)
                            {                           
                                BM.characters[i].StatusAbnom(0, 2);
                            }
                        }
                    }
                    if (rand == 2)
                    {
                        BM.HitFront(5, 0, myEnemy, false);
                        BM.HitFront(5, 0, myEnemy, false);
                        posionCount++;
                        for (int i = 0; i < BM.characters.Count; i++)
                        {
                            if (!BM.characters[i].isDie)
                            {
                                BM.characters[i].StatusAbnom(0, 1);
                            }
                        }
                    }
                }
                else
                {
                    for(int i = 0; i < BM.characters.Count; i++)
                    {
                        if (!BM.characters[i].isDie)
                        {
                            BM.characters[i].Status[0] = 0;
                        }
                    }
                    BM.HitAll(posionCount, 4, myEnemy, false);
                    posionCount = 0;
                }
         
                curTurn++;
            }
        }
        myEnemy.EnemyEndTurn();
    }
}