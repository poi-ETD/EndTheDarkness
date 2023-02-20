using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Alki010 : Enemy
{

    public int curTurn;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;
    [SerializeField] TextMeshProUGUI NameT;
    private bool startPattern;
    [SerializeField] private Enemy[] TeamEnemy;
    int phase=1;
    int pattern = 1;
    bool patternStart;
    public override void Start()
    {
        base.Start();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        NameT.text = Name;
    }

    public override void EnemySelectPattern()
    {
        if(status[0]>=12&&phase==1)
        {
            phase = 2;
            TeamEnemy[1].GetComponent<Jang010>().phase = 2;
            TeamEnemy[2].GetComponent<Ifrin010>().phase = 2;
        }
        base.EnemySelectPattern();
        StartPattern();
        pattern++;
        if (pattern == 4)
            pattern = 1;
    }


    void StartPattern()
    {
        if (BM.teamDieCount < BM.characters.Count)
        {

            if (!isDie)
            {
                if (phase == 1)
                {
                    if (pattern == 1)
                    {
                        BM.EnemyActStatusChange(this, 3, 100, BM.SelectCharacterInEnemyTurn(0, 0));
                        status[0] += 3;
                    }
                    if (pattern == 2)
                    {
                        int minimum = 1000;
                        Enemy temp = this;
                        for (int i = 0; i < 3; i++)
                        {
                            if (!TeamEnemy[i].isDie)
                            {
                                if (TeamEnemy[i].Hp < minimum)
                                {
                                    minimum = TeamEnemy[i].Hp;
                                    temp = TeamEnemy[i];
                                }
                            }
                        }
                        BM.EnemyGetAromor(7, this, temp);
                        status[0] += 3;
                    }
                    if (pattern == 3)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            BM.EnemyGetAromor(5, this, TeamEnemy[i]);
                        }
                        status[5] += 3;
                    }
                }
                else
                {
                    if (patternStart)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            BM.EnemyGetAromor(status[0], this, TeamEnemy[i]);
                        }
                        status[0] = 0;
                        for(int i=0;i<BM.characters.Count;i++)
                        {
                            BM.EnemyActStatusChange(this, -BM.characters[i].status[0], 100, BM.characters[i]);
                        }
                        patternStart = false;
                    }
                    else
                    {
                        if (pattern == 1)
                        {
                            List<Character> lists = BM.SelectCharacterListInEnemyTurn(0);
                            for(int i=0;i<lists.Count;i++)
                            {
                                BM.EnemyActStatusChange(this, 4, 100, lists[i]);
                            }
                            status[0] += 3;
                        }
                        if (pattern == 2)
                        {
                            int minimum = 1000;
                            Enemy temp = this;
                            for (int i = 0; i < 3; i++)
                            {
                                if (!TeamEnemy[i].isDie)
                                {
                                    if (TeamEnemy[i].Hp < minimum)
                                    {
                                        minimum = TeamEnemy[i].Hp;
                                        temp = TeamEnemy[i];
                                    }
                                }
                            }
                            BM.EnemyGetAromor(12, this, temp);
                            status[0] += 3;
                        }
                        if (pattern == 3)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                BM.EnemyGetAromor(8, this, TeamEnemy[i]);
                            }
                            status[5] += 3;
                        }
                    }
                }
            }
            BM.AM.EnemyAct();
        }
    }
}