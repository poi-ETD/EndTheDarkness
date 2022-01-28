﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card13 : MonoBehaviour
{
    public BattleManager BM;
    public TurnManager TM;
    public CardManager CM;
    public int specialDrow;
    public int revive;
    [SerializeField] Card myCard;
    bool isU;

    private void Update()
    {

        if (myCard.use)
        {
            if (BM.character != null)
            {
                if (BM.cost >= myCard.cardcost && BM.character.Act > 0 && !isU)
                {

                    BM.log.logContent.text += "\n" + BM.character.Name + "이(가) " + myCard.Name.text + "발동!";
                    isU = true;
                    BM.ReviveToField(revive);                  
                }
                else if (BM.character.Act > 0 && !isU)
                {
                    myCard.use = false;
                    BM.costOver();
                }
                else if (!isU)
                {
                    myCard.use = false;
                    BM.overAct();
                }
            }
            else
            {
                myCard.use = false;
                BM.TargetOn();
            }

        }
        if (isU)
        {
            if (BM.CancleReviveMode)
            {
                myCard.use = false;
                BM.CancleReviveMode = false;
                isU = false;               
            }
            if (BM.ReviveMode)
            {
                isU = false;
                BM.character.Act--;
                BM.cost -= myCard.cardcost;
                BM.specialDrow(specialDrow);
                BM.ReviveMode = false;
                myCard.isUsed = true;
            }
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
