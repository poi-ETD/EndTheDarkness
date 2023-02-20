using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Jang010 : Enemy
{


    public Image image_character;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;
    [SerializeField] TextMeshProUGUI NameT;
    public int myAct;
    public int phase = 1;

    
    public override void Start()
    {
        base.Start();
        Name = "장";
        NameT.text = Name;
    }

    public override void EnemySelectPattern()
    {
        base.EnemySelectPattern();
        StartPattern();
    }



    void StartPattern()
    {
        if (BM.teamDieCount < BM.characters.Count)
        {

            if (!isDie)
            {
                if (phase == 1)
                {
                    int rand = Random.Range(0, 3);
                    if (rand == 0)
                    {
                        for (int i = 0; i < 7; i++)
                            BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 0));

                    }
                    if (rand==1)
                    {
                        for (int i = 0; i < 3; i++)
                            BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 1));
                    }
                    if (rand==2)
                    {
                        for (int i = 0; i < TM.turn*2; i++)
                            BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 0));
                    }

                }
                else//2페이즈
                {
                    int rand = Random.Range(0, 3);
                    if (rand == 0)
                    {
                        for (int i = 0; i < TM.turn; i++)
                            BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 0));

                    }
                    if (rand == 1)
                    {
                        for (int i = 0; i < 3; i++)
                            BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 1));
                        BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 1));
                    }
                    if (rand == 2)
                    {
                        for (int i = 0; i < TM.turn * 2; i++)
                            BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 0));
                    }
                }
            }
            BM.AM.EnemyAct();
        }
    }
}