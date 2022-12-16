using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Ifrin006 : Enemy
{

    public int curTurn;
    public Image image_character;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;
    [SerializeField] TextMeshProUGUI NameT;
    [SerializeField] GameObject obj_Paul;
    public int myAct;
    private int phase=1;
    public bool glassinDie;
    private bool phase2start;
    [SerializeField] Glassin006 glassin;
    public override void Start()
    {
        base.Start();
        Name = "이프린";
        NameT.text = Name;

    }

    public override void EnemySelectPattern()
    {
        base.EnemySelectPattern();
        StartPattern();
    }
    public override void EnemyStartTurn()
    {
        base.EnemyStartTurn();
        if (phase==1&&glassinDie)
        {
            phase = 2;
            phase2start = true;
        }
    }
 
  
 
    public override void die()
    {
        base.die();
        glassin.IfrinDieTrigger();

    }
    void StartPattern()
    {
        if (BM.teamDieCount < BM.characters.Count)
        {

            if (!isDie)
            {
                if (phase == 1)
                {
                    if (myAct == 1 || myAct == 3)
                    {
                        BM.EnemyStateChange(this, 0);
                    }
                    if (myAct == 0)
                    {
                        List<Character> list_character = BM.SelectCharacterListInEnemyTurn(2);
                        for (int i = 0; i < list_character.Count; i++)
                        {
                            BM.EnemyAttack(2, this, list_character[i]);
                        }
                    }
                    if (myAct == 2)
                    {
                        BM.EnemyAttack(4, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyAttack(4, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(1, 0));

                    }
                    myAct++;
                    if (speed <= 3.3)
                    {
                        if (myAct == 3)
                        {
                            myAct = 0;
                        }
                    }
                    else if (myAct == 4)
                    {
                        myAct = 0;
                    }
                }
                else//2페이즈
                {
                    if (phase2start)
                    {
                        List<Character> list_character = BM.SelectCharacterListInEnemyTurn(2);
                        for (int i = 0; i < list_character.Count; i++)
                        {
                            int mount = 5;
                            mount = Mathf.Min(mount, list_character[i].Hp + list_character[i].armor - 1);
                            BM.EnemyAttack(mount, this, list_character[i]);

                        }
                    }
                    else
                    {
                        int rand = Random.Range(0, 3);
                        if (rand == 0)
                        {
                            List<Character> list_character = BM.SelectCharacterListInEnemyTurn(2);
                            for (int i = 0; i < list_character.Count; i++)
                            {
                                BM.EnemyAttack(3, this, list_character[i]);

                            }
                        }
                        if (rand == 1)
                        {
                            BM.EnemyAttack(6, this, BM.SelectCharacterInEnemyTurn(0, 0));
                            BM.EnemyAttack(6, this, BM.SelectCharacterInEnemyTurn(0, 0));
                            BM.EnemyAttack(6, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        }
                        if (rand == 2)
                        {
                            Character target = BM.SelectCharacterInEnemyTurn(1, 2);
                            BM.EnemyAttack(5, this, target);
                            BM.EnemyActStatusChange(this, 3, 100, target);
                        }
                    }
                }
            }
            BM.AM.EnemyAct();
        }
    }
}