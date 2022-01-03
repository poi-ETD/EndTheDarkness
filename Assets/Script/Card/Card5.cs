using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card5 : MonoBehaviour
{
    public BattleManager BM;
    public TurnManager TM;
    public CardManager CM;
    public Text Content;
    public int drowCount;
    public int contentdorw;
    public int ghostCount;
    public int contentghost;
    public int copyCount;
    public int contentcopy;
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
                    BM.specialDrow(drowCount);
                        
                          BM.ghostRevive(ghostCount);
                    BM.CopyCard(copyCount);
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
        Content.text = "망자부활:"+ghostCount+"\n드로우:" + drowCount+"\n덱에 이 카드를 "+copyCount+"장 복사해 넣는다.";
        contentdorw = drowCount;
        contentghost = ghostCount;
        contentcopy = copyCount;
    }

}
