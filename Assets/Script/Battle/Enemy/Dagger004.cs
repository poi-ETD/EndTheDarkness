using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dagger004 : Enemy
{
    public int curTurn;
   
    [SerializeField] int plusname;
    // YH
    public Image image_character;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;
    private Enemy[] enemyArray=new Enemy[4];
    private int phase=1;
    [SerializeField] TextMeshProUGUI NameT;
    private int myAction = 1;
    public override void Start()
    {
        base.Start();
        this.Name = "단검" + plusname;
        NameT.text = this.Name;
    
        if (plusname == 2)
        {
            myAction = 3;
        }

        if (plusname == 1 && BM.GD.isTriggerOn)
        {
            BM.FormationCollapse();
            BM.GD.isTriggerOn = false;
        }
    }
    public override void EnemySelectPattern()
    {
      
        base.EnemySelectPattern();
   
        StartPattern();
    }
    public override void EnemyStartTurn()
    {
        base.EnemyStartTurn();
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < 4; i++)
        {
            enemyArray[i] = enemys[i].GetComponent<Enemy>();
        }
        int alive = 0;
        for(int i = 0; i < 4; i++)
        {
            if (!enemyArray[i].isDie) alive++;
        }
        if (alive <= 2) {
            phase = 2;
        }
    }
    void StartPattern()
    {
      
        if (BM.teamDieCount < BM.characters.Count)
        {
            if (phase == 1)
            {
                if (myAction == 4)
                {
                    myAction = 1;
                }
                if (myAction == 0)
                {
                    myAction = 3;
                }
                if (myAction == 1)
                {
                    if (CanShadow())
                    {
                        BM.EnemyStateChange(this, 0);
                        BM.EnemyAttack(3, this, BM.SelectCharacterInEnemyTurn(1, 0));
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        }
                    }
                }
                if (myAction == 2)
                {
                    if (CanShadow())
                    {
                        BM.EnemyStateChange(this, 0);
                        BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 0));
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        }
                    }
                }
                if (myAction == 3)
                {
                    for(int i = 0; i < 4; i++)
                    {
                        BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 0));
                    }
                }
                
                if (plusname == 1) myAction++;
                if (plusname == 2) myAction--;
                
            }
            else
            {
                int rand = Random.Range(1, 4);
                if (rand == 1)
                {

                    BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 0));
                    BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 0));
                    BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 0));
                }
                else if (rand == 2)
                {
                    if (CanShadow())
                    {
                        BM.EnemyStateChange(this, 0);
                        BM.EnemyGetAromor(5, this, this);
                    }
                    else
                    {
                        BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(1, 0));
                        BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(1, 0));
                        BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(1, 0));
                        BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(1, 0));
                    }
             
                }
                else if (rand == 3)
                {
                    BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(1, 0));
                    BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(1, 0));
                    BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(1, 0));
                    BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(1, 0));
                }
            }


            this.BM.AM.EnemyAct();




        }

    }
}

