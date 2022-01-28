﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Card15 : MonoBehaviour
{
    public BattleManager BM;
    public TurnManager TM;
    public CardManager CM;
    public int dmg;
    [SerializeField] Card myCard;
    bool isNotCancle;

    private void Update()
    {

        if (myCard.use)
        {
            if (BM.character != null)
            {
                if (BM.cost >= myCard.cardcost && BM.character.Act > 0)
                {
                    if (BM.cost >= myCard.cardcost)
                    {
                        isNotCancle = true;
                        BM.goEnemySelectMode();
                      

                    }
                    else if (BM.character.Act > 0)
                    {
                        myCard.use = false;
                        BM.costOver();
                    }
                    else
                    {
                        myCard.use = false;
                        BM.overAct();
                    }
                }
            }
            else
            {
                myCard.use = false;
                BM.TargetOn();
            }
            
        }
        if (BM.EnemySelectMode && BM.enemy != null && myCard.use)
        {
            isNotCancle = false;
            BM.log.logContent.text += "\n" + BM.character.Name + "이(가) " + myCard.Name.text + "발동!";
            BM.character.Act--;
            BM.OnDmgOneTarget(CM.Grave.Count);
            BM.ghostRevive(CM.Grave.Count);
            myCard.isUsed = true;
            BM.cost -= myCard.cardcost;
        }
        else if (!BM.EnemySelectMode)
        {
            isNotCancle = false;
            myCard.use = false;
        }


    }
    private void Awake()
    {
        myCard = GetComponent<Card>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
    }

}
