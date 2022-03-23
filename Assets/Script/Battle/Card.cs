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
    public TextMeshProUGUI costT;
    public TextMeshProUGUI DeckT;
    public TextMeshProUGUI NoT;
    public TextMeshProUGUI Name;
    public bool isGrave;
    public int realcost;
    public bool isDeck;
    public bool isIngame;
    public int DeckNo;// start->0 Q->1 스파키->2 반가라->3 포르테->4
    public int type;//스타터->0 스타터x->1 특수->2

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
            if (Name.text != "리셋")
            CM.UseCard(gameObject);            
            if (GetComponent<BlackWhite>()!= null)
            {
                GetComponent<BlackWhite>().onDamage();
            }
            use = false;        
            cardcost = realcost;
        }
    }

    [HideInInspector] public Vector3 origin_Position;
    private bool isOnMouse;

    private void OnMouseEnter()
    {
        isOnMouse = true;
        if(BM.otherCanvasOn&&isGrave||BM.otherCanvasOn&&isDeck)
        HandManager.Instance.CardMouseEnter(this);
        if (!BM.otherCanvasOn)
        {
            if(!isGrave&&!isDeck) HandManager.Instance.CardMouseEnter(this);
        }
    }

    private void OnMouseOver()
    {
     
        if (transform.position.y == origin_Position.y && !isOnMouse)
        {
            if (BM.otherCanvasOn && isGrave || BM.otherCanvasOn && isDeck)
                HandManager.Instance.CardMouseEnter(this);
            if (!BM.otherCanvasOn)
            {
                if (!isGrave && !isDeck) HandManager.Instance.CardMouseEnter(this);
            }
        }
    }

    private void OnMouseExit()
    {
        isOnMouse = false;
        HandManager.Instance.CardMouseExit(this);
    }

    public void SavePosition(float x, float y, float z)
    {
        origin_Position = new Vector3(x, y, z);
    }
}
