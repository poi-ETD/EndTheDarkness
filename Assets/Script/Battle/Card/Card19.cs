﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Card19 : MonoBehaviour
{
    public BattleManager BM;
    public TurnManager TM;
    public CardManager CM;
    public int dmg;
    public int drow;
    public int ActUpCharacter;
    [SerializeField] Card myCard;
    
    private void Update()
    {

        if (myCard.use)
        {
            if (BM.character != null && BM.enemy != null)
            {
                if (BM.cost >= myCard.cardcost && BM.character.Act > 0)
                {
                    BM.log.logContent.text += "\n" + BM.character.Name + "이(가) " + myCard.Name.text + "발동!";
                    BM.character.Act--;
                    BM.OnDmgOneTarget(dmg);
                    BM.OnDmgOneTarget(dmg);
                    BM.ActUpCharacter(ActUpCharacter);
                    BM.specialDrow(drow);
                    myCard.isUsed = true;
                    BM.cost -= myCard.cardcost;

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
            else
            {
                myCard.use = false;
                BM.TargetOn();
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
