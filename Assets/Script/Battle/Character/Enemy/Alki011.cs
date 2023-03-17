using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Alki011 : Enemy
{

    public int curTurn;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;
    [SerializeField] TextMeshProUGUI NameT;
    private bool startPattern;
    private int phase = 1;
  private  int pattern = 1;
    private bool pattern5;
    private bool[] myAct=new bool[4];
    public override void Start()
    {
        base.Start();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        NameT.text = Name;
    }

    public override void EnemySelectPattern()
    {
        if (phase == 2 && Hp < 50) {
            phase = 3;
        }
        if(phase==1&&status[0]>=10)
        {
            pattern5 = true;
        }
        base.EnemySelectPattern();
        StartPattern();

    }


    void StartPattern()
    {
        if (BM.teamDieCount < BM.characters.Count)
        {

            if (!isDie)
            {
                if(phase==1)
                {

                    if (pattern5)
                    {

                    }
                    else
                    {
                        int rand = Random.Range(0, 4);
                        while (myAct[rand])
                        {
                            rand = Random.Range(0, 4);
                        }
                        if (rand == 0)
                        {

                        }
                        if (rand == 1)
                        {

                        }
                        if (rand == 2)
                        {

                        }
                        if (rand == 3)
                        {

                        }
                      
                        if (myAct[0] && myAct[1] && myAct[3] && myAct[2])
                        {
                            for (int i = 0; i < 4; i++) myAct[rand] = false;
                        }
                    }
                }
                if(phase==2)
                {
                    if(startPattern)
                    {
                        startPattern = false;
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

                        }
                        if (rand == 1)
                        {

                        }
                        if (rand == 2)
                        {

                        }
                     
                        if (myAct[0] && myAct[1] && myAct[2])
                        {
                            for (int i = 0; i < 3; i++) myAct[rand] = false;
                        }
                    }
                }
                if(phase==3)
                {
                    int rand = Random.Range(0, 4);
                    while (myAct[rand])
                    {
                        rand = Random.Range(0, 4);
                    }
                    if (rand == 0)
                    {

                    }
                    if (rand == 1)
                    {

                    }
                    if (rand == 2)
                    {

                    }
                    if (rand == 3)
                    {

                    }

                    if (myAct[0] && myAct[1] && myAct[3] && myAct[2])
                    {
                        for (int i = 0; i < 4; i++) myAct[rand] = false;
                    }
                }
            }
            BM.AM.EnemyAct();
        }
    }
}