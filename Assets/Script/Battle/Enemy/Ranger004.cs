using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ranger004 : Enemy
{
    public int curTurn;

    [SerializeField] int plusname;
    // YH
    public Image image_character;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;
    private Enemy[] enemyArray = new Enemy[4];
    private int phase = 1;
    [SerializeField] TextMeshProUGUI NameT;
    private int myAction = 1;
    public override void Start()
    {
        base.Start();
        this.Name = "레인저" + plusname;
        NameT.text = this.Name;
       
      
    }
    public override void EnemySelectPattern()
    {
        Debug.Log("A");
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
        for (int i = 0; i < 4; i++)
        {
            if (!enemyArray[i].isDie) alive++;
        }
        if (alive <= 2)
        {
            myAction = 0;
            phase = 2;
        }
    }
    void StartPattern()
    {
        if (BM.teamDieCount < BM.characters.Count)
        {
            if (phase == 1)
            {
                if (myAction == 3)
                {
                    myAction = 0;
                    BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(2, 2));
                    BM.EnemyAttack(3, this, BM.SelectCharacterInEnemyTurn(0, 0));
                    BM.EnemyGetAromor(7, this, this);
                }
                else
                {
                    myAction++;
                    int rand = Random.Range(0, 2);
                    if (rand == 0)
                    {
                        BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 1));
                    }
                    if (rand == 1)
                    {
                        BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyGetAromor(5, this, this);
                    }
                }
            }
            else
            {
                if (myAction == 3)
                {
                    myAction = 0;
                    BM.EnemyAttack(3, this, BM.SelectCharacterInEnemyTurn(0, 0));
                    BM.EnemyAttack(5, this, BM.SelectCharacterInEnemyTurn(0, 0));
                    BM.EnemyGetAromor(10, this, this);
                }
                else
                {
                    myAction++;
                    int rand = Random.Range(0, 2);
                    if (rand == 0)
                    {
                        BM.EnemyAttack(4, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyAttack(3, this, BM.SelectCharacterInEnemyTurn(0, 1));
                    }
                    if (rand == 1)
                    {
                        BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 1));
                        BM.EnemyGetAromor(10, this, this);
                    }
                }
            }


            this.BM.AM.EnemyAct();




        }

    }
}

