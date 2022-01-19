using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Card20 : MonoBehaviour
{
    public BattleManager BM;
    public TurnManager TM;
    public CardManager CM;
    [SerializeField] Card myCard;

    private void Update()
    {

        if (myCard.use)
        {
            if (BM.character != null)
            {
                if (BM.cost >= myCard.cardcost && BM.character.Act > 0)
                {
                    if (BM.pcard != null&&BM.pcard.GetComponent<Card>().Name.text!="스케치 반복")
                    {
                        BM.log.logContent.text += "\n" + BM.character.Name + "이(가) " + myCard.Name.text + "발동!";        
                        BM.enemy = BM.penemy;               
                        BM.card20Active();
                        myCard.isUsed = true;
                        BM.cost -= myCard.cardcost;
                    }
                    else
                    {
                        myCard.use = false;
                        BM.warntext.text = "이전에 사용한 카드가 없습니다.";
                        BM.WarnOn();
                    }

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
