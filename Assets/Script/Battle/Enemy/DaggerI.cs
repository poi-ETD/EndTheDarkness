using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaggerI : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    public BattleManager BM;
    public List<Character> HaveArmor = new List<Character>();
    public List<Character> ForwardHaveArmor = new List<Character>();
    [SerializeField] Text t;
    [SerializeField] int plusname;
    public DaggerI anotherDagger;
    public int pattern;
    bool[] done = new bool[3];

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
            pattern = -1;
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

                pattern = Random.Range(0, 3);
                if (plusname == 2)
                {
                    while (pattern == anotherDagger.pattern)
                    {
                        pattern = Random.Range(0, 3);
                    }
                }
                if (pattern == 0)
                {
                    BM.HitAll(1, 4, myEnemy.Name, false);
                }
                if (pattern == 1)
                {
                    BM.HitFront(3, 0, myEnemy.Name, false);
                    BM.HitFront(3, 0, myEnemy.Name, false);
                    BM.HitBack(1, 0, myEnemy.Name, false);
                }
                if (pattern == 2)
                {
                    if (myEnemy.CanShadow())
                    {
                       myEnemy.onShadow();
                    }
                    else
                    {
                        BM.HitFront(3, 0, myEnemy.Name, false);
                        BM.HitFront(3, 0, myEnemy.Name, false);
                    }
                    BM.HitBack(0, 0, myEnemy.Name, true);
                }



            }

            curTurn++;
        }

  
    }
}