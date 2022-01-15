using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Card14 : MonoBehaviour
{
    public BattleManager BM;
    public TurnManager TM;
    public CardManager CM;
    public int dmg;
    [SerializeField] Card myCard;
    Q q;

    private void Update()
    {

        if (myCard.use)
        {

            if (BM.character != null && BM.enemy != null)
            {
                if (BM.cost >= myCard.cardcost && BM.character.Act > 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (BM.characters[i].characterNo == 1)
                        {
                            q = BM.characters[i].GetComponent<Q>();
                            break;
                        }
                    }
                    BM.log.logContent.text += "\n" + BM.character.Name + "이(가) " + myCard.Name.text + "발동!";
                    BM.character.Act--;
                    BM.OnDmgOneTarget(q.Ghost);
                    q.Ghost = 0;
                    q.turnStartGhost = 0;
               
                    BM.ghostRevive(30);
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
    }

}
