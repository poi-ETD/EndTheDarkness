using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Card : MonoBehaviour
{
    CardManager CM;
    public bool isUsed;
    public bool isDestroy;
    public TextMeshProUGUI Content;
    public bool use;
    BattleManager BM;
    public int cardcost;
    [SerializeField] TextMeshProUGUI costT;
    public TextMeshProUGUI Name;
    public bool isGrave;
    public int realcost;
    public bool isDeck;
    public void useCard()
    {
     
        use = true;
    }
    private void Awake()
    {
      
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        
    }
    private void Update()
    {      
        costT.text = cardcost + "";
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hit = Physics2D.RaycastAll(pos, Vector2.zero, 0f);
            if (hit != null)
            {
                bool isCardOn = false; ;
                for(int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].collider.gameObject == gameObject)
                        isCardOn = true;
                }
                if (isCardOn)
                {
                    if (!BM.EnemySelectMode && !BM.otherCanvasOn)
                    {
                      
                            if (BM.card != gameObject)
                            {
                                BM.SetCard(gameObject);
                            }
                            else
                            {
                                BM.cancleCard();
                            }                       
                    }
                    else if(isGrave||isDeck)
                        CM.ClickInGrave(gameObject);
                }
            }
        }
        if (isUsed&&!isGrave)
        {          
            CM.UseCard(gameObject);            
            if (GetComponent<BlackWhite>()!= null)
            {
                GetComponent<BlackWhite>().onDamage();
            }
            use = false;        
            cardcost = realcost;
        }
    }
    
}
