using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card24 : MonoBehaviour
{
    public BattleManager BM;
    public TurnManager TM;
    public CardManager CM;
    [SerializeField] Card myCard;
    bool isU;
    public int DeckCount=1;

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
                    BM.SelectDeckCard(DeckCount);
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
            if (BM.DeckSelectCancle)
            {
                BM.DeckSelectCancle = false;
                isU = false;
                myCard.use = false;
            }
            if (BM.DeckSelect)
            {
                isU = false;
                BM.character.Act--;
                BM.cost -= myCard.cardcost;
              
                BM.DeckSelect = false;
                myCard.isUsed = true;
                BM.card24();

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
