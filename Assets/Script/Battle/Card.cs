using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    CardManager CM;
    public Image cardImage;
    public TextMeshProUGUI Content;
    BattleManager BM;
    ActManager AM;
    public int cardcost;
    public TextMeshProUGUI costT;
    public TextMeshProUGUI DeckT;
    public TextMeshProUGUI NoT;
    public TextMeshProUGUI Name;
    public bool isGrave;
    public int realCost;
    public bool isDeck;
    public bool isIngame;
    public int DeckNo;// start->0 Q->1 스파키->2 반가라->3 포르테->4
    public int type;//스타터->0 스타터x->1 특수->2
    public int cardNo;
    public int selectType;
    public bool iscard20Mode;

    //YH
    [HideInInspector] public bool isSelected = false;

    public void useCard()
    {
        BM.otherCanvasOn = false;
        if (!BM.EnemySelectMode)
        {
            if (BM.selectedCharacter == null)
            {
                BM.TargetOn();
                return;
            }
            if (BM.leftCost < cardcost)
            {
                BM.costOver();
                return;
            }
            if (BM.selectedCharacter.turnAct <= -5)
            {
                if (cardNo != 18)
                {
                    BM.overAct();
                    return;
                }
            }
            if (selectType==1)
            {
              
                BM.goEnemySelectMode();
            }
           else if (selectType == 2) //무덤류
            {
                if (cardNo == 7)
                {
                  
                    BM.ReviveToField(2);
                  
                }
                if (cardNo == 13)
                {
                    BM.ReviveToField(1);
                }
            }
           else if (selectType == 3)
            {
                if (cardNo == 24)
                {
                    BM.SelectDeckCard(1);
                }
            }
        else   if (selectType == 4)
            {
                if (BM.previousSelectedCard == null)
                {
                    BM.WarnOn();
                    BM.warnT.text = "이전에 사용한 카드가 없습니다.";
                    return;
                }
                if (BM.previousSelectedCard.GetComponent<Card>().cardNo == 20)
                {
                    BM.WarnOn();
                    BM.warnT.text = "스케치 반복은 연속해서 사용 할 수 없습니다.";
                    return;
                }
                BM.card20Active();
            }
            else
            {
                BM.log.logContent.text += "\n" + BM.selectedCharacter.Name + "이(가) " + Name.text + " 발동!";
                if (cardNo == 2)
                {                 
                    BM.getArmor(10+BM.selectedCharacter.turnDef);                 
                }
                if (cardNo == 4)
                {                
                    BM.specialDrow(2);                 
                }
                if (cardNo == 5)
                {
                    BM.ghostRevive(8);
                    BM.CopyCard(1);
                }
                if (cardNo == 6)
                {
                    BM.nextTurnStartCost+=2;
                    BM.ghostRevive(6);
                    BM.specialDrow(1);

                }
                if (cardNo == 8)
                {
                    cardcost = BM.leftCost;
                    BM.getArmor(BM.leftCost*12+BM.selectedCharacter.turnDef);
                   
                }
                if (cardNo == 9)
                {
                    BM.NextTurnArmor(20 + BM.selectedCharacter.turnDef);
                }
                if (cardNo == 10)
                {
                    BM.teamTurnAtkUp(1);
                }
                if (cardNo == 12)
                {
               
                    BM.useCost(cardcost);
                    BM.selectedCharacter.useAct(1);
                    BM.card12remake();

                    return;
                }
                if (cardNo == 16)
                {
                    BM.reflectUp(1);
                   
                }
                if (cardNo == 17)
                {
                    BM.getArmor(20 + BM.selectedCharacter.turnDef);
                    BM.NextTurnArmor(-30);
                }
                if (cardNo == 21)
                {
                    BM.RandomReviveToField(3);
                }
                if (cardNo == 22)
                {
                    BM.card22();
                }
                if (cardNo == 23)
                {
                    BM.card23();
                }
                if (cardNo == 25)
                {
                    BM.ghostRevive(BM.leftCost);
                    BM.NextTurnArmor(7 + BM.selectedCharacter.turnDef);
                }
                if (cardNo == 26)
                {
                    BM.ghostRevive(10);
                    BM.AllAttack(2, BM.selectedCharacter,3);
                }
                if (cardNo == 28)
                {
                    BM.GD.Ignum -= 50;
                    if (BM.selectedCharacter.characterNo == 5)
                    {
                        for(int i = 0; i < BM.Enemys.Length; i++)
                        {
                            BM.Enemys[i].GetComponent<Enemy>().StatusChange((int)Enums.Status.weak, 2);
                        }
                    }
                    BM.AllAttack(2, BM.selectedCharacter, 1);
                }
                BM.useCost(cardcost);
                BM.selectedCharacter.useAct(1);               
                CardUse();
                BM.AM.Act();
            }
            
        }
        else
        {
            BM.CardUseText.text = "사용";
            BM.EnemySelectMode = false;
        }
    }
    public void EnemySelectCard()
    {
        BM.log.logContent.text += "\n" + BM.selectedCharacter.Name + "이(가) " + Name.text + " 발동!";
        if (cardNo == 1)
        {          
            BM.OnDmgOneTarget(7,BM.selectedEnemy,1);         
        }
        if (cardNo == 3)
        {                
            BM.OnDmgOneTarget(7, BM.selectedEnemy,1);
            BM.specialDrow(1);          
        }
     
        if (cardNo == 11)
        {
            BM.OnDmgOneTarget(5, BM.selectedEnemy,1);
       
        }
        if (cardNo == 14)
        {
            CharacterPassive q = null;
            for(int i = 0; i < BM.characters.Count; i++)
            {
                if (BM.characters[i].characterNo == 1)
                {
                    q = BM.characters[i].GetComponent<CharacterPassive>();
                }
            }
            BM.OnDmgOneTarget(q.ghost, BM.selectedEnemy,1);
            BM.ghostRevive(-1 * q.ghost);
           BM.ghostRevive(30);
        }
        if (cardNo == 15)
        {
            BM.OnDmgOneTarget(CM.Grave.Count, BM.selectedEnemy,1);
            BM.ghostRevive(CM.Grave.Count);
        }
        if (cardNo == 18)
        {
            BM.OnDmgOneTarget(2, BM.selectedEnemy,1);
        }
        if (cardNo == 19)
        {
            BM.OnDmgOneTarget(4, BM.selectedEnemy,7);                      
        }
        if (cardNo == 27)
        {


          

            if (BM.selectedEnemy.status[(int)Enums.Status.weak] > 0)
        {
            BM.OnDmgOneTarget(15, BM.selectedEnemy, 1);
        }
            BM.selectedEnemy.GetComponent<Enemy>().StatusChange((int)Enums.Status.weak, 10);
        }
        BM.useCost(cardcost);           
        BM.selectedCharacter.useAct(1);
        CardUse();
    
        BM.AM.Act();
    }
    public void CancleRevive()
    {
        
    }
    public void SelectRevive()
    {
        

        if (cardNo == 7)
        {            
            BM.ghostRevive(4);
        }
     
        
        BM.selectedCharacter.useAct(1);
        BM.useCost(cardcost);
        CardUse();
        BM.AM.Act();
        BM.Click_GraveOff();
    }
    public void SelectDeck()
    {
        BM.log.logContent.text += "\n" + BM.selectedCharacter.Name + "이(가) " + Name.text + " 발동!";
        if (cardNo == 24)
        {
            BM.card24();
        }     
        BM.selectedCharacter.useAct(1);
        BM.useCost(cardcost);
        CardUse();
        BM.AM.Act();
    }
    public void decreaseCost(int amount)
    {
        cardcost -= amount;
        if (cardcost < 0) cardcost = 0;
        if(cardNo!=8)
        costT.text = cardcost + "";
    }
    private void Start()
    {
      
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        AM = BM.AM;
    }
    public void CardUse()
    {
      
        if(Name.text=="리셋")return;
        

        if (GetComponent<BlackWhite>() != null)
        {        
            GetComponent<BlackWhite>().onDamage();
        }
        
        if (iscard20Mode)
        {
            Destroy(gameObject);
            CM.UseCard(BM.usedInCard20);       
            BM.log.logContent.text += "\n" + BM.selectedCharacter.Name + "이(가) " + BM.usedInCard20.GetComponent<Card>().Name.text + " 발동!";
            BM.useCost(BM.usedInCard20.GetComponent<Card>().cardcost);             
            BM.usedInCard20 = null;
            return;
        }
        CM.UseCard(gameObject);
      
    }

    public void textSet()
    {
       
        if (cardNo != 8)
            costT.text = cardcost + "";
        else costT.text = "N";
    }
    [HideInInspector] public Vector3 origin_Position;
    private bool isOnMouse;

    private void OnMouseDown()
    {
        if (!isGrave && !isDeck)
        {
            if (BM.otherCor) return;
           
            if (isSelected) return;
          
            BM.isSelectedCardinHand = true; //YH
            isSelected = true; //YH
            HandManager.Instance.SelectCard(this);
            HandManager.Instance.InputToOriginText(this); //YH  
            HandManager.Instance.CardMouseEnter(this); //YH
         
            if (!BM.EnemySelectMode && !BM.otherCanvasOn)
            {
                
                if (BM.selectedCard != gameObject)
                    BM.SetCard(gameObject);
                else
                    BM.cancleCard();
            }
            //else if (isGrave || isDeck)
            //    CM.ClickInGraveOrDeck(gameObject);

            //HandManager.Instance.CardMouseDown();
            //gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, 1000f, 0f);
        }
        else
            CM.ClickInGraveOrDeck(gameObject);
    }

    private void OnMouseUp()
    {
        HandManager.Instance.CardMouseUp();
    }

    private void OnMouseEnter()
    {
        if (!BM.EnemySelectMode && !BM.otherCanvasOn &&!isDeck)
            HandManager.Instance.CardMouseEnter(this);
        if (BM.otherCanvasOn && BM.isGraveWindowOn)
        {
            if (isGrave)
                HandManager.Instance.CardTooltipOn(this);
           // if(isGrave||isDeck) HandManager.Instance.CardMouseEnter(this); 
        }
    }

    private void OnMouseOver()
    {
        isOnMouse = true;
    }

    private void OnMouseExit()
    {
        if (!BM.otherCanvasOn)
        {
            isOnMouse = false;
            HandManager.Instance.CardMouseExit(this);
        }
        else
        {
            if (BM.isGraveWindowOn)
                HandManager.Instance.go_SelectedCardTooltip.SetActive(false);
        }
    }

    public void SavePosition(float x, float y, float z)
    {
        origin_Position = new Vector3(x, y, z);
    }
}
