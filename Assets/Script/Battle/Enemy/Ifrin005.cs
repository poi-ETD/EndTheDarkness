using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Ifrin005 : Enemy
{

    public int curTurn;
    public Enemy myEnemy;
    private int phase=1;
    private bool phase2start;
    public Image image_character;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;
    bool[] myAct=new bool[3];
    [SerializeField] TextMeshProUGUI NameT;
    [SerializeField] GameObject obj_Paul;

    public override void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy = GetComponent<Enemy>();
        myEnemy.Name = "이프린";
        NameT.text = myEnemy.Name;

    }

    public override void EnemySelectPattern()
    {
        base.EnemySelectPattern();
        StartPattern();
    }
    private void Escape()
    {
        BM.GD.isTriggerOn = true;
        
    }
    public override void EnemyStartTurn()
    {
        base.EnemyStartTurn();
        if (Hp <= 30&&phase==1)
        {
            myAct[0] = false;
            myAct[1] = false;
            myAct[2] = false;
            phase = 2;
        }
      
    }
    public override void die()
    {
        base.die();
        Instantiate(obj_Paul);
        Destroy(gameObject);
    }
    void StartPattern()
    {
        if (BM.teamDieCount < BM.characters.Count)
        {

            if (!myEnemy.isDie)
            {
                if (phase == 1)
                {

                    int rand = Random.Range(0, 3);
                    while (myAct[rand])
                    {
                        rand = Random.Range(0, 3);
                    }
                    if (rand == 0)
                    {
                        BM.EnemyAttack(3, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyAttack(3, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyAttack(3, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyAttack(3, this, BM.SelectCharacterInEnemyTurn(0, 0));
                    }
                    else if (rand == 1)
                    {
                        BM.EnemyGetAromor(5, this, this);
                        BM.EnemyAttack(5, this, BM.SelectCharacterInEnemyTurn(0, 0));

                    }
                    else if (rand == 2)
                    {
                        List<Character> list_character = BM.SelectCharacterListInEnemyTurn(2);
                        for(int i = 0; i < list_character.Count; i++)
                        {
                            BM.EnemyAttack(2, this, list_character[i]);
                        }
                    }
                    if (myAct[0] && myAct[1] && myAct[2])
                    {
                        myAct[0] = false;
                        myAct[1] = false;
                        myAct[2] = false;
                    }
                }
                else if (phase == 2)
                {
                    if (phase2start)
                    {
                        phase2start = false;
                        List<Character> list_character = BM.SelectCharacterListInEnemyTurn(2);
                        for (int i = 0; i < list_character.Count; i++)
                        {
                            BM.EnemyActStatusChange(this, 2, 100, list_character[i]);
                        }
                        BM.EnemySpeedDown(this, 50, this);
                    }
                    else
                    {
                        int rand = Random.Range(0, 3);
                        while (myAct[rand])
                        {
                            rand = Random.Range(0, 3);
                        }
                        if (rand == 0)
                        {
                            Character a=BM.SelectCharacterInEnemyTurn(0, 0);
                            Character b= BM.SelectCharacterInEnemyTurn(0, 0);
                            BM.EnemyActStatusChange(this, 1, 100, a);
                            BM.EnemyActStatusChange(this, 2, 100,b);
                            BM.EnemyAttack(2, this, a);
                            BM.EnemyAttack(2, this, b);
                        }
                        else if (rand == 1)
                        {
                            BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 0));
                            BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 0));
                            BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 0));
                            BM.EnemyGetAromor(10, this, this);
                        }
                        else if (rand == 2)
                        {
                            Character a = BM.SelectCharacterInEnemyTurn(1, 2);
                            BM.EnemyAttack(2, this, a);
                            BM.EnemyAttack(2, this, a);
                            BM.EnemyIncreaseSpeed(50, this, a);

                        }
                        if (myAct[0] && myAct[1] && myAct[2])
                        {
                            myAct[0] = false;
                            myAct[1] = false;
                            myAct[2] = false;
                        }
                    }
                }

            }
            myEnemy.BM.AM.EnemyAct();
        }
    }
}