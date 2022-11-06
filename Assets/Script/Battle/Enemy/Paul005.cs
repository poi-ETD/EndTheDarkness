using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Paul005 : Enemy
{

    public int curTurn;
    public Enemy myEnemy;
    private int myTurn;
    private bool[] myAct = new bool[2];
    // YH
    public Image image_character;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;

    [SerializeField] TextMeshProUGUI NameT;

    public override void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy = GetComponent<Enemy>();
        myEnemy.Name = "파울";
        NameT.text = myEnemy.Name;

    }

    private void Update()
    {
        if (myEnemy.isAct)
        {
            StartPattern();
            myEnemy.isAct = false;
        }
    }
    private void Escape()
    {
  
        BM.Victory();
    }
   
    public override void onHit(int dmg)
    {

        base.onHit(dmg);
        if (Hp <= 20)
        Escape();

    }
    void StartPattern()
    {
        if (BM.teamDieCount < BM.characters.Count)
        {

            if (!myEnemy.isDie)
            {
                curTurn++;
                if (curTurn % 3 == 0)
                {
                    BM.EnemyGetHp((maxHp - Hp) *100/20, this, this);
                }
                else
                {
                    int rand = Random.Range(0, 3);
                    if (rand == 0)
                    {
                        BM.EnemyAttack(8, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyAttack(8, this, BM.SelectCharacterInEnemyTurn(0, 0));
                    }
                    else if (rand == 1)
                    {
                        BM.EnemyAttack(10, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyGetAromor(10, this, this);
                    }
                    else if (rand == 2)
                    {
                        BM.EnemyAttack(5, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyAttack(5, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyGetAromor(5, this, this);
                    }
                }
            }
            myEnemy.BM.AM.EnemyAct();
        }
    }
}