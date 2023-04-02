using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Paul008 : Enemy
{

    public int curTurn;
    public Image image_character;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;
    [SerializeField] TextMeshProUGUI NameT;
    bool[] myAct = new bool[3];
    private int phase = 1;
    private bool phase2start;
    private int phase2count;
    private bool phase3start;

    public override void Start()
    {
        base.Start();
        Name = "파울";
        NameT.text = Name;

    }
    public override void EnemyStartTurn()
    {
        base.EnemyStartTurn();
        if (phase == 1 && Hp <= 50)
        {
            phase = 2;
            phase2start = true;
        }
        else if (phase == 2)
        {
            if (Armor <= 0)
            {
                phase = 3;
            }
            else if (phase2count == 5)
            {
                phase = 3;
                phase3start = true;
            }
        }
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
                    if (phase % 4 != 0)
                    {
                       
                            int rand = Random.Range(0, 3);
                            while (myAct[rand])
                            {
                                rand = Random.Range(0, 3);
                            }
                            myAct[rand] = true;

                            if (rand == 0)
                            {
                            BM.EnemyAttack(12, this, BM.SelectCharacterInEnemyTurn(0, 0));
                            BM.EnemyGetAromor(15, this, this);
                        }
                            else if (rand == 1)
                            {
                            BM.EnemyAttack(8, this, BM.SelectCharacterInEnemyTurn(0, 0));
                            BM.EnemyAttack(8, this, BM.SelectCharacterInEnemyTurn(0, 0));
                            BM.EnemyGetAromor(10, this, this);
                        }
                            else if (rand == 2)
                            {
                            List<Character> list_characters = BM.SelectCharacterListInEnemyTurn(0);
                            for(int i = 0; i < list_characters.Count; i++)
                            {
                                BM.EnemyAttack(8, this, list_characters[i]);
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
                        BM.EnemyGetHp((maxHp - Hp) * 3 / 10, this, this);
                    }
                    BM.AM.EnemyAct();
                }
                else if (phase == 2)
                {
                    if (phase2start)
                    {
                        BM.EnemyGetAromor(100, this, this);
                        phase2start = false;
                        BM.AM.EnemyAct();
                    }
                    else
                    {
                        BM.AM.EnemyJustAct(this);
                        phase2count++;
                    }
                }
                else
                {
                    if (phase3start)
                    {
                        phase3start = false;
                        BM.EnemyGetHp(Armor, this, this);
                        List<Character> list_characters = BM.SelectCharacterListInEnemyTurn(2);
                        for (int i = 0; i < list_characters.Count; i++)
                        {
                            BM.EnemyAttack(Armor, this, list_characters[i]);
                        }
                        BM.FormationCollapse();
                    }
                    else
                    {
                        int rand = Random.Range(0, 3);
                        if (rand == 0)
                        {
                            BM.EnemyAttack((maxHp-Hp)/10, this, BM.SelectCharacterInEnemyTurn(0, 0));
                            BM.EnemyAttack((maxHp - Hp) / 10, this, BM.SelectCharacterInEnemyTurn(0, 0));
                            BM.EnemyGetAromor(15, this, this);
                        }
                        if (rand == 1)
                        {
                            BM.EnemyAttack((maxHp - Hp) *3/20, this, BM.SelectCharacterInEnemyTurn(0, 0));
                            BM.EnemyGetAromor((maxHp - Hp) * 3 / 10, this, this);
                        }
                        if (rand == 2)
                        {
                            List<Character> list_characters = BM.SelectCharacterListInEnemyTurn(0);
                            for (int i = 0; i < list_characters.Count; i++)
                            {
                                BM.EnemyAttack(8, this, list_characters[i]);
                                BM.EnemyIncreaseSpeed(100, this, list_characters[i]);
                            }
                        }
                    }

                    BM.AM.EnemyAct();
                }
               
            }
        }
    }
}