using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card7 : MonoBehaviour
{
    public BattleManager BM;
    public TurnManager TM;
    public CardManager CM;
    public int ghostRevive;
    public int revive;
    public bool oneTimeUse;
    [SerializeField] Card myCard;

    private void Update()
    {
        Debug.Log(BM.ReviveCount+" "+oneTimeUse+" "+ myCard.isGrave);
        if (myCard.use)
        {

            if (BM.character != null)
            {
                if (BM.cost >= myCard.cardcost)
                {
                    BM.character.Act--;
                    BM.ghostRevive(ghostRevive);
                    BM.ReviveToField(revive);
                    BM.card7mode = true;
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
