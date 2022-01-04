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
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject)
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
            }
        }
        if (isUsed)
        {           
            CM.UseCard(gameObject);
            if (GetComponent<BlackWhite>()!= null)
            {
                GetComponent<BlackWhite>().onDamage();
            }
            use = false;
        }
    }
    
}
