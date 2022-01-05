﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card8 : MonoBehaviour
{
    public BattleManager BM;
    public TurnManager TM;
    public CardManager CM;

    public int armor;

    public int nextarmor;

    [SerializeField] Card myCard;

    private void Update()
    {

        if (myCard.use)
        {

            if (BM.character != null)
            {
                if (BM.cost >= myCard.cardcost)
                {
                    BM.character.Act--;
                    BM.getArmor(armor);
                    BM.card8(nextarmor);
                    myCard.isUsed = true;
                    BM.cost -= myCard.cardcost;
                    
                }
                else
                {
                    myCard.use = false;
                    BM.costOver();
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
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();

    }

}