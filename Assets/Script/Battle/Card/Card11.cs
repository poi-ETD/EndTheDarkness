using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card11 : MonoBehaviour
{
    public BattleManager BM;
    public TurnManager TM;
    public CardManager CM;
    public int dmg;
    public int atk;
    [SerializeField] Card myCard;
    bool decrease;

    private void Update()
    {
        if (TM.turnCard >= 4&&!decrease)
        {
            decrease = true;
            myCard.cardcost -= 2;
        }
        if (myCard.use)
        {

            if (BM.character != null)
            {
                if (BM.cost >= myCard.cardcost)
                {
                    BM.log.logContent.text += "\n" + BM.character.Name + "이(가) " + myCard.Name.text + "발동!";
                    BM.character.Act--;
                    BM.OnDmgOneTarget(dmg);
                    BM.TurnAtkUp(atk);
                    myCard.isUsed = true;
                    BM.cost -= myCard.cardcost;
                    if (decrease)
                    {
                        myCard.cardcost += 2;
                        decrease = false;
                    }
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
