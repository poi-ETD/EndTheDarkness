using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card6 : MonoBehaviour
{
    public BattleManager BM;
    public TurnManager TM;
    public CardManager CM;
    public Text Content;
    public int nextCost;
    public int contentnextCost;
    public int ghostCount;
    public int contentghost;
    public int dmg;
    public int contentDmg;
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
                    BM.nextTurnStartCost++;
                    BM.ghostRevive(ghostCount);
                    BM.OnDmgOneTarget(dmg);
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
        Content.text = "망자부활:" + ghostCount + "\n적 한명에게 2번 데미지:" + dmg + "\n다음 턴 시작에 코스트+" + nextCost;
        contentDmg = dmg;
        contentghost = ghostCount;
        contentnextCost = nextCost;
    }

}
