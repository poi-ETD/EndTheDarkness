using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Ifrin009 : Enemy
{

    public int curTurn;
    private int phase = 1;
    private bool phase2start;
    public Image image_character;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;
    bool[] myAct = new bool[3];
    [SerializeField] TextMeshProUGUI NameT;
    [SerializeField] GameObject obj_Alki;
    int myDecreaseHp;
    bool Pattern4;
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
    private void Escape()
    {
        BM.GD.isTriggerOn = true;

    }
    public override void EnemyStartTurn()
    {
        base.EnemyStartTurn();
      

    }
    public override void onHit(int dmg)
    {
        base.onHit(dmg);
        myDecreaseHp += dmg;
        if (myDecreaseHp >= 30)
        {
            myDecreaseHp = 0;
            Pattern4 = true;
        }
    }
    public override void die()
    {
        base.die();
        GameObject p = Instantiate(obj_Alki, gameObject.transform.parent);
        gameObject.SetActive(false);
        TM.PlayerTurnEnd();
        BM.Enemys[0] = p.transform.GetChild(0).gameObject;

    }
    void StartPattern()
    {
        if (BM.teamDieCount < BM.characters.Count)
        {

            if (!isDie)
            {
                if (Pattern4)
                {
                    onHit(10);
                    List<Character> lists = BM.SelectCharacterListInEnemyTurn(0);
              
                    for (int i = 0; i < lists.Count; i++)
                    {
                        BM.EnemyAttack(8, this, lists[i]);
                    }
                    lists = BM.SelectCharacterListInEnemyTurn(1);
                    for (int i = 0; i < lists.Count; i++)
                    {
                        BM.EnemyAttack(4, this, lists[i]);
                    }
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
                        List<Character> lists = BM.SelectCharacterListInEnemyTurn(0);
                        if (lists.Count == 1)
                        {
                            lists.Add(BM.SelectCharacterInEnemyTurn(0, 1));
                        }
                        for(int i = 0; i < lists.Count; i++)
                        {
                            BM.EnemyAttack(4, this, lists[i]);
                        }
                    }
                    else if (rand == 1)
                    {
                        List<Character> lists = BM.SelectCharacterListInEnemyTurn(0);
                        
                        for (int i = 0; i < lists.Count; i++)
                        {
                            BM.EnemyActStatusChange(this, 3, 100, lists[i]);
                        }
                    }
                    else if (rand == 2)
                    {
                        List<Character> lists = BM.SelectCharacterListInEnemyTurn(2);

                        for (int i = 0; i < lists.Count; i++)
                        {
                            BM.EnemyAttack(3, this, lists[i]);
                            BM.EnemyActStatusChange(this,1, 100, lists[i]);
                        }

                    }
                    if (myAct[0] && myAct[1] && myAct[2])
                    {
                        myAct[0] = false;
                        myAct[1] = false;
                        myAct[2] = false;
                    }
                }
                   

            }
            BM.AM.EnemyAct();
        }
    }
}