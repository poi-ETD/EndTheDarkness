using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Alki007 : Enemy
{

    public int curTurn;
    public Image image_character;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;
    [SerializeField] TextMeshProUGUI NameT;
    bool[] myAct = new bool[3];
    private int phase = 1;
    [SerializeField] Jang007 jang;
    public override void Start()
    {
        base.Start();
        Name = "알키트라제";
        NameT.text = Name;

    }
    public override void EnemyStartTurn()
    {
        base.EnemyStartTurn();
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
                if (TM.turn % 2 == 0)
                {
                    BM.EnemyAtkUp(jang, 1, this);
                   
                }
                else
                {
                    
                    int rand = Random.Range(0, 3);
                    if (rand == 0)
                    {
                        status[0] += 5;
                        for(int i = 0; i < 2; i++)
                        {
                            BM.EnemyActStatusChange(this, 2,100, BM.SelectCharacterInEnemyTurn(0, 0));
                        }
                    }
                    else if (rand == 1)
                    {
                        status[0] += 2;
                        BM.EnemyGetAromor(5, this, this);
                        BM.EnemyGetAromor(5, this, jang);
                    }
                    else
                    {
                        List<Character> list_character = BM.SelectCharacterListInEnemyTurn(2);
                        for (int i = 0; i < list_character.Count; i++)
                        {
                            BM.EnemyActStatusChange(this, 2, 100, list_character[i]);
                        }
                    }
                }
                BM.AM.EnemyAct();
            }
        }
    }
}