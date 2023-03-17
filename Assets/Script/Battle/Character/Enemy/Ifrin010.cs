using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Ifrin010 : Enemy
{ 
    public Image image_character;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;
    [SerializeField] TextMeshProUGUI NameT;
    public int myAct;
    public int phase = 1;
    [SerializeField] Enemy[] TeamEnemy;

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
                        List<Character> lists = BM.SelectCharacterListInEnemyTurn(0);
                        if (lists.Count == 1)
                        {
                            BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 1));
                        }
                        for (int i = 0; i < lists.Count; i++)
                        {
                            BM.EnemyAttack(3, this, lists[i]);
                        }
                    }
                    if (rand == 1)
                    {
                        if(Shadow)
                        {
                            BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 0));
                            TeamEnemy[Random.Range(0, 3)].onHit(3);
                        }
                        else
                        BM.EnemyStateChange(this, 0);
                     }
                    if (rand == 2)
                    {
                        BM.EnemyAttack(1, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        TeamEnemy[Random.Range(0, 3)].onHit(3);
                    }

                }
                else//2페이즈
                {
                    int rand = Random.Range(0, 3);
                    if (rand == 0)
                    {
                        List<Character> lists = BM.SelectCharacterListInEnemyTurn(0);
                        if (lists.Count == 1)
                        {
                            BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 1));
                        }
                        for (int i = 0; i < lists.Count; i++)
                        {
                            BM.EnemyAttack(4, this, lists[i]);
                        }
                    }
                    if (rand == 1)
                    {
                        BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 1));
                        BM.EnemyAttack(2, this, BM.SelectCharacterInEnemyTurn(0, 1));
                    }
                    if (rand == 2)
                    {
                        BM.EnemyAttack(3, this, BM.SelectCharacterInEnemyTurn(0, 0));
                        TeamEnemy[Random.Range(0, 3)].onHit(5);
                    }

                }
            }
            BM.AM.EnemyAct();
        }
    }
}