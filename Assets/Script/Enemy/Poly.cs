using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Poly : MonoBehaviour
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
    private void Awake()
    {
        StartPattern();
        myEnemy.name = "폴리";
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
        if (BM.diecount < 4)
        {
            if (myEnemy.Hp > 150)
            {
                phase1++;
                HaveArmor.Clear();
                ForwardHaveArmor.Clear();
                for (int i = 0; i < 4; i++)
                {
                    if (BM.characters[i].Armor > 0)
                    {
                        HaveArmor.Add(BM.characters[i]);
                    }
                }
                for (int i = 0; i < BM.forward.Count; i++)
                {
                    if (BM.forward[i].Armor > 0)
                    {
                        ForwardHaveArmor.Add(BM.characters[i]);
                    }
                }
                if (phase1 % 2 == 1)
                {
                    if (BM.forward.Count > 0)
                    {
                        if (ForwardHaveArmor.Count > 0)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                int rand = Random.Range(0, ForwardHaveArmor.Count);
                                while (BM.characters[rand].isDie)
                                    rand = Random.Range(0, ForwardHaveArmor.Count);
                                BM.characters[rand].onHit(8);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                int rand = Random.Range(0, BM.forward.Count);
                                while (BM.characters[rand].isDie)
                                    rand = Random.Range(0, ForwardHaveArmor.Count);
                                BM.characters[rand].onHit(8);
                            }
                        }
                    }
                    else
                    {
                        if (HaveArmor.Count > 0)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                int rand = Random.Range(0, HaveArmor.Count);
                                while (BM.characters[rand].isDie)
                                    rand = Random.Range(0, HaveArmor.Count);
                                BM.characters[rand].onHit(8);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                int rand = Random.Range(0, 4);
                                while (BM.characters[rand].isDie)
                                    rand = Random.Range(0, 4);
                                BM.characters[rand].onHit(8);
                            }
                        }
                    }
                }
                else if (phase1 % 2 == 0)
                {
                    myEnemy.GetArmor(5);
                    if (BM.forward.Count > 0)
                    {
                        if (ForwardHaveArmor.Count > 0)
                        {

                            int rand = Random.Range(0, ForwardHaveArmor.Count);
                            while (BM.characters[rand].isDie)
                                rand = Random.Range(0, ForwardHaveArmor.Count);
                            BM.characters[rand].onHit(3);

                        }
                        else
                        {

                            int rand = Random.Range(0, BM.forward.Count);
                            while (BM.characters[rand].isDie)
                                rand = Random.Range(0, BM.forward.Count);
                            BM.characters[rand].onHit(3);

                        }
                    }
                    else
                    {
                        if (HaveArmor.Count > 0)
                        {

                            int rand = Random.Range(0, HaveArmor.Count);
                            while (BM.characters[rand].isDie)
                                rand = Random.Range(0, HaveArmor.Count);
                            BM.characters[rand].onHit(3);

                        }
                        else
                        {

                            int rand = Random.Range(0, 4);
                            while (BM.characters[rand].isDie)
                                rand = Random.Range(0, 4);
                            BM.characters[rand].onHit(3);

                        }
                    }
                }
              
            }
            else if (myEnemy.Hp <= 150&&myEnemy.Hp>50)
            {
                myEnemy.noDie = true;
                phase2++;
                if (phase2 == 1)
                {
                  
                        for(int i = 0; i < 4; i++)
                        {
                           
                            BM.characters[i].NextTurnMinusAct++;
                        }
                    
                    myEnemy.GetArmor(30);
                }
                else if (phase2 % 2 == 0)

                {
          
                    if (BM.forward.Count > 0)
                    {
                        if (ForwardHaveArmor.Count > 0)
                        {

                            int rand = Random.Range(0, ForwardHaveArmor.Count);
                            while (BM.characters[rand].isDie)
                                rand = Random.Range(0, ForwardHaveArmor.Count);
                            BM.characters[rand].onHit(10);
                            BM.characters[rand].NextTurnMinusAct = 1;

                        }
                        else
                        {

                            int rand = Random.Range(0, BM.forward.Count);
                            while (BM.characters[rand].isDie)
                                rand = Random.Range(0, BM.forward.Count);
                            BM.characters[rand].onHit(10);
                            BM.characters[rand].NextTurnMinusAct = 1;

                        }
                    }
                    else
                    {
                        if (HaveArmor.Count > 0)
                        {

                            int rand = Random.Range(0, HaveArmor.Count);
                            while (BM.characters[rand].isDie)
                                rand = Random.Range(0, HaveArmor.Count);
                            BM.characters[rand].onHit(10);
                            BM.characters[rand].NextTurnMinusAct = 1;

                        }
                        else
                        {

                            int rand = Random.Range(0, 4);
                            while (BM.characters[rand].isDie)
                                rand = Random.Range(0, 4);
                            BM.characters[rand].onHit(10);
                            BM.characters[rand].NextTurnMinusAct = 1;

                        }
                    }
                }
                else if (phase2 % 2 == 1)
                {
                    myEnemy.GetArmor(10);
                }
            }
            else if (myEnemy.Hp <= 50)
            {
                
                myEnemy.noDie = false;
                phase3++;
              
                if (phase3 == 1)
                {
                    for (int i = 0; i < 4; i++)
                    {

                        BM.characters[i].NextTurnMinusAct++;
                    }
                    count = 6;
                    myEnemy.GetArmor(100);
                }
                else if (phase3 < 7)
                {
                    count--;
                    t.text = "" + count;
                }
                else if(phase3==7)
                {
                    t.text = "";
                    for(int i = 0; i < 4; i++)
                    {
                        if (!BM.characters[i].isDie)
                        {
                            BM.characters[i].onHit(myEnemy.Armor);
                        }
                    }
                }
                else

                {

                    if (BM.forward.Count > 0)
                    {
                        if (ForwardHaveArmor.Count > 0)
                        {

                            int rand = Random.Range(0, ForwardHaveArmor.Count);
                            while (BM.characters[rand].isDie)
                                rand = Random.Range(0, ForwardHaveArmor.Count);
                            BM.characters[rand].onHit(13);
                            myEnemy.GetArmor(10);

                        }
                        else
                        {

                            int rand = Random.Range(0, BM.forward.Count);
                            while (BM.characters[rand].isDie)
                                rand = Random.Range(0, BM.forward.Count);
                            BM.characters[rand].onHit(13);
                            myEnemy.GetArmor(10);

                        }
                    }
                    else
                    {
                        if (HaveArmor.Count > 0)
                        {

                            int rand = Random.Range(0, HaveArmor.Count);
                            while (BM.characters[rand].isDie)
                                rand = Random.Range(0, HaveArmor.Count);
                            BM.characters[rand].onHit(13);
                            myEnemy.GetArmor(10);

                        }
                        else
                        {

                            int rand = Random.Range(0, 4);
                            while (BM.characters[rand].isDie)
                                rand = Random.Range(0, 4);
                            BM.characters[rand].onHit(13);
                            myEnemy.GetArmor(10);
                        }
                    }
                }
            }
            myEnemy.EnemyEndTurn();

            curTurn++;
        }
    }
}