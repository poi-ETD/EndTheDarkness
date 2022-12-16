using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Glassin006 : Enemy
{

    public int curTurn;
    public Image image_character;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;
    [SerializeField] TextMeshProUGUI NameT;
    bool[] myAct = new bool[3];
    [SerializeField] private Ifrin006 ifrin;
    public override void Start()
    {
        base.Start();
        Name = "글래신";
        NameT.text = Name;

    }

    public override void EnemySelectPattern()
    {
        base.EnemySelectPattern();
        StartPattern();
    }
  

    public override void die()
    {
        base.die();
        ifrin.glassinDie = true;

    }
    public void IfrinDieTrigger()
    {
        ifrin.Revive(Hp);
        onHit(Hp + Armor);
    }
    void StartPattern()
    {
        if (BM.teamDieCount < BM.characters.Count)
        {

            if (!isDie)
            {
                int rand = Random.Range(0, 3);
                while (myAct[rand])
                {
                    rand = Random.Range(0, 3);
                }
                myAct[rand] = true;
                BM.EnemySpeedDown(ifrin, 10, this);
                if (rand == 0)
                {
                    ifrin.GetAtk(1);
                    
                }
                else if (rand == 1)
                {
                    List<Character> list_character = BM.SelectCharacterListInEnemyTurn(2);
                    for (int i = 0; i < list_character.Count; i++)
                    {
                        BM.EnemyIncreaseSpeed(50, this, list_character[i]);
                    }

                }
                else if (rand == 2)
                {
                    Enemy lowerEnemy = this;
                    if (Hp > ifrin.Hp)
                    {
                        lowerEnemy = ifrin;
                    }
                    BM.EnemyGetAromor(10, this, lowerEnemy);
                }
                if (myAct[0] && myAct[1] && myAct[2])
                {
                    myAct[0] = false;
                    myAct[1] = false;
                    myAct[2] = false;
                }
                BM.AM.EnemyAct();
            }
        }
    }
}