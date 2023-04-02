using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Jang007 : Enemy
{

    public int curTurn;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;
    [SerializeField] TextMeshProUGUI NameT;
    bool[] myAct = new bool[3];
    private int phase = 1;
    [SerializeField] private Alki007 alki;
    private int pattern;
    private bool patternStart;
    public override void Start()
    {
        base.Start();
        Name = "장";
        NameT.text = Name;
        power = true;

    }
    public override void EnemyStartTurn()
    {
        base.EnemyStartTurn();
        if (alki.isDie&&phase==1)
        {
            phase = 2;
            patternStart = true;
           
           
        }
     
        pattern = 0;
    }
    public override void EnemySelectPattern()
    {
        base.EnemySelectPattern();
        StartPattern();
        
    
    }


    public override void die()
    {
        base.die();
  

    }
  
    void StartPattern()
    {
        if (BM.teamDieCount < BM.characters.Count)
        {

            if (!isDie)
            {
                if (phase == 1)
                {
                    int rand = Random.Range(0, 3);
                    while (myAct[rand])
                    {
                        rand = Random.Range(0, 3);
                    }
                    myAct[rand] = true;
              
                    if (rand == 0)
                    {
                     for(int i = 0; i < 5 + TM.turn; i++)
                        {
                            BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        }

                    }
                    else if (rand == 1)
                    {
                        for (int i = 0; i <3;i++)
                        {
                            BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 0));

                        }
                        BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 1));
                    }
                    else if (rand == 2)
                    {
                        for (int i = 0; i < 2*TM.turn; i++)
                        {
                            BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        }
                    }
                    if (myAct[0] && myAct[1] && myAct[2])
                    {
                        myAct[0] = false;
                        myAct[1] = false;
                        myAct[2] = false;
                    }
                }
                else
                {
                    if (patternStart)
                    {
                        BM.EnemySpeedDown(this, Mathf.Min(200, atk * 100), this);
                        BM.EnemyAtkUp(this, -atk, this);
                        
                    }
                    else
                    {
                        if (pattern == 0)
                        {
                            List<Character> list_character = BM.SelectCharacterListInEnemyTurn(2);
                            for (int i = 0; i < list_character.Count; i++)
                            {
                               for(int j = 0; j < 4; j++)
                                {
                                    BM.EnemyAttack(1, this, list_character[j]);
                                }
                            }
                        }
                        else if (pattern == 1)
                        {
                            for(int i = 0; i < TM.turn; i++)
                            {
                                BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 0));
                            }
                            for(int i = 0; i < 3; i++)
                            {
                                BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 1));
                            }
                        }
                        else if (pattern == 2)
                        {
                            for(int i = 0; i < 7 + TM.turn; i++)
                            {
                                BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 0));
                            }
                        }
                        else
                        {
                            for(int i = 0; i < 2 * TM.turn; i++)
                            {
                                BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 0));
                            }
                        }
                    }
                }
                BM.AM.EnemyAct();
            }
        
        }
    }
}