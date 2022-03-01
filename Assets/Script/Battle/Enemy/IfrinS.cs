using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IfrinS : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    int phase1;
    int phase2;
    int phase3;
    public BattleManager BM;
    public List<Character> HaveArmor = new List<Character>();
    public List<Character> ForwardHaveArmor = new List<Character>();
    int count;
    [SerializeField] Text t;
    [SerializeField] Enemy Glassin;
    int rand;
    bool[] randCount;
    int myHp;
    int glassinHp;
    bool p4;
    private void Start()
    {
        myHp = myEnemy.Hp;
        glassinHp = Glassin.Hp;
        randCount = new bool[3];
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy.Name = "이프린";
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
        if (!myEnemy.isDie)
        {
            if (myEnemy.Hp > 10)
            {
                if (BM.diecount < BM.characters.Count)
                {
                    myEnemy.immortal = true;
                    if ((curTurn + 1) % 4 != 0)
                    {

                        rand = Random.Range(0, 3);
                        while (randCount[rand])
                        {
                            rand = Random.Range(0, 3);
                        }
                        if (rand == 0)
                        {
                            randCount[0] = true;
                            for (int i = 0; i < 2; i++)
                            {
                                BM.HitFront(5, 0, myEnemy.Name, false);
                        

                            }
                        }
                        if (rand == 1)
                        {
                            randCount[1] = true;
                            BM.HitAll(3, 4, myEnemy.Name, false);
                        }
                        if (rand == 2)
                        {
                            randCount[2] = true;
                            if (BM.forward.Count > 0)
                            {
                                int rand2 = Random.Range(0, BM.forward.Count);
                                while (BM.characters[rand2].isDie)
                                    rand2 = Random.Range(0, BM.forward.Count);
                                BM.characters[rand2].StatusAbnom(0, 2);
                            }
                            else
                            {
                                int rand2 = Random.Range(0, BM.characters.Count);
                                while (BM.characters[rand2].isDie)
                                    rand2 = Random.Range(0, BM.characters.Count);
                                BM.characters[rand2].StatusAbnom(0, 2);
                            }
                        }
                    }
                    else
                    {
                        BM.HitFront((myEnemy.maxHp - myEnemy.Hp) / 10, 4, myEnemy.Name, false);
                        BM.HitBack((myEnemy.maxHp - myEnemy.Hp) / 20, 4, myEnemy.Name, false);
                        randCount[0] = false;
                        randCount[1] = false;
                        randCount[2] = false;
                    }

                }
            }
            else
            {
                myEnemy.onShadow();
            }
                myEnemy.EnemyEndTurn();
                curTurn++;
            }
        
    }
}