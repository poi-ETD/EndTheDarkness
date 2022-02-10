using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dagger001 : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    public BattleManager BM;
    public List<Character> HaveArmor = new List<Character>();
    public List<Character> ForwardHaveArmor = new List<Character>();
    [SerializeField] Text t;
    [SerializeField] int plusname;
    [SerializeField] Dagger001 anotherDagger;
    int pattern;
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
            if (anotherDagger.myEnemy.isDie)
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
                if ((curTurn + 1) % 5 != 0)
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
                        if (BM.forward.Count > 0)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                int rand2 = Random.Range(0, BM.forward.Count);
                                while (BM.characters[rand2].isDie)
                                    rand2 = Random.Range(0, BM.forward.Count);
                                BM.characters[rand2].onHit(3, myEnemy.Name);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                int rand2 = Random.Range(0, 4);
                                while (BM.characters[rand2].isDie)
                                    rand2 = Random.Range(0, 4);
                                BM.characters[rand2].onHit(3, myEnemy.Name);


                            }
                        }
                    }
                    if (pattern == 1)
                    {
                        myEnemy.GetArmor(3, myEnemy.Name);
                        if (BM.forward.Count > 0)
                        {
                            int rand2 = Random.Range(0, BM.forward.Count);
                            while (BM.characters[rand2].isDie)
                                rand2 = Random.Range(0, BM.forward.Count);
                            BM.characters[rand2].onHit(3, myEnemy.Name);

                        }
                        else
                        {

                            int rand2 = Random.Range(0, 4);
                            while (BM.characters[rand2].isDie)
                                rand2 = Random.Range(0, 4);
                            BM.characters[rand2].onHit(3, myEnemy.Name);
                        }
                    }
                    if (pattern == 2)
                    {
                        int rand2 = Random.Range(0, BM.back.Count);
                        BM.characters[BM.line + rand2].NextTurnMinusAct++;
                        BM.characters[BM.line + rand2].onHit(1, myEnemy.Name);
                    }
                }
                else
                {
                    int rand2;
                    if (BM.forward.Count > 0)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            rand2 = Random.Range(0, BM.forward.Count);
                            while (BM.characters[rand2].isDie)
                                rand2 = Random.Range(0, BM.forward.Count);
                            BM.characters[rand2].onHit(3, myEnemy.Name);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            rand2 = Random.Range(0, 4);
                            while (BM.characters[rand2].isDie)
                                rand2 = Random.Range(0, 4);
                            BM.characters[rand2].onHit(3, myEnemy.Name);


                        }
                    }
                    rand2 = Random.Range(0, BM.back.Count);
                    BM.characters[BM.line + rand2].onHit(1, myEnemy.Name);
                }
            }
        }
      
        if (plusname == 1)
        {
            myEnemy.EnemyEndTurn();         
        }
        curTurn++;
    }
}