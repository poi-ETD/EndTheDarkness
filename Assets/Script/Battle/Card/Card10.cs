using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card10 : MonoBehaviour
{
    public BattleManager BM;
    public TurnManager TM;
    public CardManager CM;


    public int atk=1;
   [SerializeField] Card myCard;

    private void Update()
    {

        if (myCard.use)
        {

            if (BM.character != null)
            {
                if (BM.cost >= myCard.cardcost && BM.character.Act > 0)
                {
                    BM.log.logContent.text += "\n" + BM.character.Name + "이(가) " + myCard.Name.text + "발동!";
                    BM.character.Act--;
                    BM.teamTurnAtkUp(atk);

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
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
        myCard = GetComponent<Card>();
    }

}
