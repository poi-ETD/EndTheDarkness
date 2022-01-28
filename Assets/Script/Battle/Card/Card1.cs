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
    bool isNotCancle;
    private void Update()
    {      
        if (myCard.use)
        {          
            if (BM.character != null)
            {
                if (BM.cost >= myCard.cardcost&&BM.character.Act>0&&!isNotCancle)
                {
                    isNotCancle = true;
                   
                    BM.goEnemySelectMode();
                                     
                }
                else if(BM.character.Act>0&&!isNotCancle)
                {
                    myCard.use = false;
                    BM.costOver();
                }
                else if(!isNotCancle)
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
            if (BM.EnemySelectMode && BM.enemy != null && myCard.use)
            {
                isNotCancle = false;
                BM.log.logContent.text += "\n" + BM.character.Name + "이(가) " + myCard.Name.text + "발동!";
              
                BM.OnDmgOneTarget(dmg);
                myCard.isUsed = true;
                BM.character.Act--;
                BM.cost -= myCard.cardcost;
            }
            else if(!BM.EnemySelectMode)
            {
                isNotCancle = false;
                myCard.use = false;
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
