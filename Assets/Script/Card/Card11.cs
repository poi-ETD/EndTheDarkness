using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card11 : MonoBehaviour
{
    public BattleManager BM;
    public TurnManager TM;
    public CardManager CM;
    public Text Content;

    public int dmg;
    public int contentdmg;
    public int atk;
    public int contentatk;
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
                    BM.character.Act--;
                    BM.OnDmgOneTarget(dmg);
                    BM.AtkUp(atk);
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
        Content.text = "이번 턴 카드를 4장 이상 사용 했을 시 이 카드의 코스트 2감소\n자신에게 공격력+" + atk + "\n적 한명에게 데미지:" + dmg;
        contentatk = atk;
        contentdmg = dmg;
    }

}
