using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Alki009 : Enemy
{

    public int curTurn;
    private bool[] myAct = new bool[2];
    // YH
    public Sprite sprite_idle;
    public Sprite sprite_highlight;
    private int pattern4;
    [SerializeField] TextMeshProUGUI NameT;
    private bool startPattern;

    public override void Start()
    {
        base.Start();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        NameT.text = Name;

    }

    public override void EnemySelectPattern()
    {
        base.EnemySelectPattern();
        StartPattern();
    }
    private void Escape()
    {

        BM.Victory();
    }

 
    void StartPattern()
    {
        if (BM.teamDieCount < BM.characters.Count)
        {

            if (!isDie)
            {
                if (startPattern)
                {
                    startPattern = false;
                    BM.EnemyGetAromor(20, this, this);
                    status[0] += 5;
                }
                else if(status[0]>=10)
                {
                    for(int i = 0; i < BM.characters.Count; i++)
                    {
                        BM.EnemyAttack(BM.characters[i].status[0], this, BM.characters[i]);
                        BM.characters[i].status[0] = 0;
                    }
                    BM.EnemyGetHp(status[0],this,this);
                    status[0] = 0;
                }
                else
                {
                   
                    int rand = Random.Range(0, 3);
                    if (rand == 0)
                    {
                        BM.EnemyGetAromor(10, this, this);
                        Character target = BM.SelectCharacterInEnemyTurn(2, 0);
                        BM.EnemyAttack(3, this, target);
                        BM.EnemyAttack(3, this, target);
                        BM.EnemyActStatusChange(this, 2, 100, target);
                        BM.EnemyActStatusChange(this, 2, 100, target);
                    }
                    else if (rand == 1)
                    {
                        BM.EnemyAttack(6, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyAttack(6, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        status[0] += 3;
                        BM.EnemyGetAromor(15, this, this);

                    }
                    else
                    {
                        status[0] += 5;
                        for(int i = 0; i < BM.characters.Count; i++)
                        {
                            BM.EnemyActStatusChange(this, 2, 100, BM.characters[i]);
                        }
                    }
                }
                
            }
            BM.AM.EnemyAct();
        }
    }
}