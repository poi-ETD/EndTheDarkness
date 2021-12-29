using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poly : MonoBehaviour
{
    public TurnManager TM;
    int curTurn;
    public Enemy myEnemy;
    int phase1;
    public BattleManager BM;
    public List<Character> HaveArmor = new List<Character>();
    private void Awake()
    {      
        StartPattern();
    }
    void Update()
    {
        if (curTurn != TM.t)
        {
            StartPattern();
        } 
    }
    void StartPattern() {
        if (myEnemy.Hp > 150)
        {
            phase1++;
            HaveArmor.Clear();
            for(int i = 0; i < 4; i++)
            {
                if(BM.characters[i].Armor>0)
                {
                    HaveArmor.Add(BM.characters[i]);
                }
            }
            if (phase1 % 2 == 1)
            {
                
               for(int i = 0; i < 2; i++)
                {   if (HaveArmor.Count > 0)
                    {
                        int rand = Random.Range(0, HaveArmor.Count);
                        HaveArmor[rand].onHit(8);
                    }
                    else
                    {
                        int rand = Random.Range(0, 4);
                        BM.characters[rand].onHit(8);
                    }
                }
            }
            else
            {
                if (HaveArmor.Count > 0)
                {
                    int rand = Random.Range(0, HaveArmor.Count);
                    HaveArmor[rand].onHit(3);
                }
                else
                {
                    int rand = Random.Range(0, 4);
                    BM.characters[rand].onHit(3);
                }
                myEnemy.GetArmor(5);
            }
            myEnemy.EnemyEndTurn();
        }
        curTurn++;
    }
}
