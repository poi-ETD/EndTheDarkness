using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Card1 : MonoBehaviour
{
    public BattleManager BM;
    public TurnManager TM;
    public CardManager CM;
    public int dmg;
    [SerializeField] Card myCard;

    private void Update()
    {
      
        if (myCard.use)
        {

            if (BM.character != null && BM.enemy != null)
            {
                if (BM.cost >= myCard.cardcost)
                {
                    BM.log.logContent.text += "\n" + BM.character.Name + "이(가) " + myCard.Name.text + "발동!";
                    BM.character.Act--;
                    BM.OnDmgOneTarget(dmg);
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
