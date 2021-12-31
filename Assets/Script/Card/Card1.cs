using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card1 : MonoBehaviour
{
    public BattleManager BM;
    public TurnManager TM;
    [SerializeField] int cardcost;
    public CardManager CM;
    [SerializeField] Text costT;
    [SerializeField] Card myCard;
    private void Awake()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
    }
        private void Update()
    {
        costT.text = cardcost + "";
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (BM.character != null&&BM.card!=gameObject)
                    {
                        BM.EnemySelectMode = true;
                        if(BM.card!=null)
                        BM.cancleCard();
                        BM.SetCard(gameObject);
                    }
                    else if (BM.character != null && BM.card == gameObject)
                    {
                        BM.cancleCard();
                    }
                    else
                    {
                        Debug.Log("카드 설명");
                    }
                }
            }
           
        }
        if (BM.card == gameObject && BM.character != null && BM.enemy != null)
        {
            if (BM.cost >= cardcost)
            {
                BM.enemy.onHit(5 + BM.character.Atk);
                BM.character.Act--;
                BM.cost -= cardcost;
                BM.character = null;
                BM.enemy = null;
                BM.card = null;
                BM.CharacterCancle();
                myCard.isUsed = true;
                BM.Setting();
            }
            else
            {
                Debug.Log("코스트가 부족합니다");
                BM.cancleCard();
            }
        }
    }
}
