using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardManager CM;
    public Image cardImage;
    public TextMeshProUGUI Content;
    public BattleManager BM;
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
    public int DeckNo;// start->0 Q->1 스파키->2 반가라->3 포르테->4 령->5
    public int type;//스타터->0 스타터x->1 특수->2
    public int cardNo;
    public int selectType; // YH : selectType의 번호가 무슨 의미인지?
    public bool iscard20Mode;

    //부여 가능한 속성들
    int drowCard;
    int weaknessCount;
    int percentDmgToAllTarget;

    public bool isRemove;

    //YH
    [HideInInspector] public bool isSelected = false;

    public void useCardInCard20() // YH : '20카드사용' 같은 주석말고, 언제 실행되는 함수인지? 무슨 역할을 하는 함수인지? 정확하게 기재 부탁드립니다
    {
        if (selectType == 1)
        {
            BM.selectedEnemy = BM.previousEnemy;
            EnemySelectCard();
        }
        else if (selectType == 5)
        {
            BM.selectedCharacter = BM.previousCharacter;
            CharacterSelectCard();
        }
        else useCard();
    }

    public void useCard() // YH : '카드사용' 같은 주석말고, 언제 실행되는 함수인지? 무슨 역할을 하는 함수인지? 정확하게 기재 부탁드립니다
    {
        if (CM.TM.turn == 1 )
        {if( BM.GD.blessbool[4]||BM.GD.blessbool[12])
            return;
        }
        BM.otherCanvasOn = false;
        if (!BM.enemySelectMode)
        {   
            if (BM.actCharacter == null)
            {
                BM.TargetOn();
                return;
            }

            if (BM.leftCost < cardcost)
            {
                HandManager.Instance.CancelToUse();
                BM.costOver();
                return;
            }        
            else if (selectType == 1) // YH : selectType의 번호가 무슨 의미인지?
            {
                //BM.goEnemySelectMode();
            }
            else if (selectType == 2) // 무덤류 -> YH : '무덤류' 라는 모호한 표현말고, 정확하게 주석 달아주세요. 무덤에서 카드를 선택해야 하는 타입이라던가,
                                      // 무덤으로 보내야하는 타입이라던가..
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
            else if (selectType == 3) // YH : selectType의 번호가 무슨 의미인지?
            {
                if (cardNo == 24)
                {
                    BM.SelectDeckCard(1);
                }
            }
            else if (selectType == 4) // YH : selectType의 번호가 무슨 의미인지?
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
                BM.UsePreviousCard();
            }
            else if (selectType == 5) // YH : selectType의 번호가 무슨 의미인지?
            {
                BM.goCharacterSelectMode();
            }
            else
            {
                BM.log.logContent.text += "\n" + BM.actCharacter.Name + "이(가) " + Name.text + " 발동!";
           
                if (cardNo == 4)
                {                
                    BM.specialDrow(2);
                    BM.SpeedChange(BM.actCharacter, -0.1f);
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
                    BM.getArmor(BM.leftCost*12+BM.actCharacter.turnDef,BM.actCharacter);
                   
                }
                if (cardNo == 9)
                {
                    BM.NextTurnArmor(20 + BM.actCharacter.turnDef,BM.actCharacter);
                }
                if (cardNo == 10)
                {
                    BM.teamTurnAtkUp(1);
                    BM.SpeedChange(BM.actCharacter, -0.1f);
                }
                if (cardNo == 12)
                {              
                    BM.useCost(cardcost,gameObject);
                   
                    BM.card12();
                   
                    return;
                }
                if (cardNo == 16)
                {
                    BM.reflectUp(1);
                   
                }
                if (cardNo == 18)
                {
                   for(int i = 0; i < BM.characters.Count; i++)
                    {
                        if (BM.characters[i].characterNo == 2)
                        {
                            BM.SpeedChange(BM.characters[i], -1.0f);
                            BM.TurnAtkUp(BM.characters[i],1);
                        }
                    }
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
                    //BM.card23();
                }
                if (cardNo == 25)
                {
                    BM.ghostRevive(BM.leftCost);
                    for(int i = 0; i < CM.field.Count; i++)
                    {
                        if (CM.field[i] != gameObject)
                        {
                            CM.field[i].GetComponent<Card>().GetDrowCardEffect();
                        }
                    }
                    for(int i = 0; i < CM.Grave.Count; i++)
                    {
                        CM.Grave[i].GetComponent<Card>().GetDrowCardEffect();
                    }
                }
                if (cardNo == 26)
                {
                    BM.ghostRevive(10);
                    BM.AllAttack(6, BM.actCharacter,1);
                }
                if (cardNo == 28)
                {
                    BM.GD.Ignum -= 50;
                    if (BM.actCharacter.characterNo == 5)
                    {
                        for (int i = 0; i < BM.characters.Count; i++)
                        {
                            BM.characters[i].myPassive.EnemyGetWeak();
                        }
                        for (int i = 0; i < BM.Enemys.Length; i++)
                        {
                            BM.Enemys[i].GetComponent<Enemy>().StatusChange((int)Status.weak, 3);
                        }
                    }
                    BM.AllAttack(2, BM.actCharacter, 1);
                }
                if (cardNo == 29)
                {
                    int statusCount = BM.Enemys[0].GetComponent<Enemy>().status.Length;
                    BM.GD.Ignum -= 50;
                    for(int i = 0; i < BM.Enemys.Length; i++)
                    {
                        for (int j = 0; j < statusCount; j++)                        
                            BM.Enemys[i].GetComponent<Enemy>().status[j] *= 2;                                                         
                    }
                }
                if (cardNo == 30)
                {
                    int statusCount = BM.Enemys[0].GetComponent<Enemy>().status.Length;
                    int removeCount = 0;
                    for (int i = 0; i < BM.Enemys.Length; i++)
                    {
                        for (int j = 0; j < statusCount; j++)
                        {
                            if (BM.Enemys[i].GetComponent<Enemy>().status[j] > 0)
                            {
                                removeCount += BM.Enemys[i].GetComponent<Enemy>().status[j];
                                BM.Enemys[i].GetComponent<Enemy>().status[j] = 0;
                            }
                        }
                    }
                    if (BM.actCharacter.characterNo == 5)//사용자가 령
                    {
                        BM.AllAttack(5,BM.actCharacter, removeCount);
                    }
                }
                if (cardNo == 31)
                {
                    if (BM.actCharacter.characterNo == 5)//사용자가 령
                    {
                        BM.AllAttack(2, BM.actCharacter, 1);
                    }
                }
                if (cardNo == 32)
                {
                    BM.OnRandomAttack(6, BM.actCharacter, 3);
                }
                if (cardNo == 34)
                { 
                    for(int i = 0; i < BM.characters.Count; i++)
                    {
                        if (BM.characters[i].characterNo == 6) BM.AtkUp(BM.characters[i],1);
                    }
                    if (BM.actCharacter.armor == 0)
                    {
                        for (int i = 0; i < BM.characters.Count; i++)
                        {
                            if (BM.characters[i].characterNo == 6)
                            {
                                BM.AtkUp(BM.characters[i], -1);
                                BM.getArmor(10, BM.characters[i]);
                            }
                        }

                    }
                }
                if (cardNo == 35)
                {

                    BM.card35(gameObject);
                    return;
                }
                if (cardNo == 36)
                {
                    BM.OnRandomAttack(BM.actCharacter.atk*3, BM.actCharacter, 1);
                }

                    if (!iscard20Mode)
                {
                    BM.useCost(cardcost, gameObject);
                   
                }               
                CardUse();
                BM.AM.Act();
            }
            
        }
        else
            BM.enemySelectMode = false;
    }
  
    public void EnemySelectCard() // YH : '적선택' 같은 주석말고, 언제 실행되는 함수인지? 무슨 역할을 하는 함수인지? 정확하게 기재 부탁드립니다
    {
      
        BM.log.logContent.text += "\n" + BM.actCharacter.Name + "이(가) " + Name.text + " 발동!";
        if (cardNo == 1)
        {          
            BM.OnDmgOneTarget(7,BM.selectedEnemy,BM.actCharacter,1);         
        }
        if (cardNo == 3)
        {
           
            BM.OnDmgOneTarget(7, BM.selectedEnemy,BM.actCharacter,1);
            BM.specialDrow(1);          
        }
     
        if (cardNo == 11)
        {
            BM.OnDmgOneTarget(15, BM.selectedEnemy,BM.actCharacter,1);
       
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
            BM.OnDmgOneTarget(q.ghost, BM.selectedEnemy,BM.actCharacter,1);
            BM.ghostRevive(-1 * q.ghost);
           BM.ghostRevive(30);
        }
        if (cardNo == 15)
        {
            BM.OnDmgOneTarget(CM.Grave.Count, BM.selectedEnemy,BM.actCharacter,1);
            BM.ghostRevive(CM.Grave.Count);
        }
   
        if (cardNo == 19)
        {
            BM.OnDmgOneTarget(4, BM.selectedEnemy,BM.actCharacter,7);                      
        }
        if (cardNo == 27)
        {
            if (BM.selectedEnemy.status[(int)Status.weak] > 0)
            {
            BM.OnDmgOneTarget(15, BM.selectedEnemy, BM.actCharacter,1);
            }
            for (int i = 0; i < BM.characters.Count; i++)
            {
                BM.characters[i].myPassive.EnemyGetWeak();
            }
            BM.selectedEnemy.GetComponent<Enemy>().StatusChange((int)Status.weak, 10);
        }
        if (cardNo == 33)
        {
            BM.OnDmgOneTarget(10, BM.selectedEnemy, BM.actCharacter, 1);
            if (BM.selectedEnemy.Hp <= BM.selectedEnemy.maxHp * 0.2f)
            {
                BM.OnDmgOneTarget(10, BM.selectedEnemy, BM.actCharacter, 1);
            }
        }
        if (!iscard20Mode)
        {
            BM.useCost(cardcost, gameObject);
          
        }
        
        CardUse();
    
        BM.AM.Act();
    }

    public void CancleRevive() // YH : '부활취소' 같은 주석말고, 언제 실행되는 함수인지? 무슨 역할을 하는 함수인지? 정확하게 기재 부탁드립니다
    {
        
    }

    public void SelectRevive() // YH : '부활선택' 같은 주석말고, 언제 실행되는 함수인지? 무슨 역할을 하는 함수인지? 정확하게 기재 부탁드립니다
    {
        if (cardNo == 7)
        {            
            BM.ghostRevive(4);
        }
        if (!iscard20Mode)
        {
            BM.useCost(cardcost, gameObject);          
        }
        CardUse();
        BM.AM.Act();
        BM.Click_GraveOff();
    }

    public void SelectDeck() // YH : '덱선택' 같은 주석말고, 언제 실행되는 함수인지? 무슨 역할을 하는 함수인지? 정확하게 기재 부탁드립니다
    {
        BM.log.logContent.text += "\n" + BM.actCharacter.Name + "이(가) " + Name.text + " 발동!";
        if (cardNo == 24)
        {
            BM.card24();
        }
        if (!iscard20Mode)
        {
            BM.useCost(cardcost, gameObject);
         
        }
        CardUse();
        BM.AM.Act();
    }

    public void CharacterSelectCard() // YH : '캐릭터선택카드' 같은 주석말고, 언제 실행되는 함수인지? 무슨 역할을 하는 함수인지? 정확하게 기재 부탁드립니다
    {
        BM.log.logContent.text += "\n" + BM.actCharacter.Name + "이(가) " + Name.text + " 발동!";
        if (cardNo == 2)
        {
            BM.getArmor(10+BM.selectedCharacter.turnDef,BM.selectedCharacter);
        }
        if (cardNo == 17)
        {
            BM.getArmor(20 + BM.selectedCharacter.turnDef,BM.selectedCharacter);
            for (int i = 0; i < BM.characters.Count; i++) {
                if(BM.characters[i].characterNo==3)//반가라 일 경우
                BM.NextTurnArmor(-30,BM.characters[i]); }
        }
        if (!iscard20Mode)
        {
            BM.useCost(cardcost, gameObject);
            
        }
      
        CardUse();
       
        BM.AM.Act();
    }

    public void decreaseCost(int amount) // YH : '코스트감소' 같은 주석말고, 언제 실행되는 함수인지? 무슨 역할을 하는 함수인지? 정확하게 기재 부탁드립니다
    {
        cardcost -= amount;
        if (cardcost < 0) cardcost = 0;
        if(cardNo!=8)
        costT.text = cardcost + "";
    }

    private void Start() // YH : Start() 함수가 굳이 이런곳에 있어야할 이유가 있을까요?
    {
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        AM = BM.AM;
    }

    public void CardUse() // YH : '카드사용' 같은 주석말고, 언제 실행되는 함수인지? 무슨 역할을 하는 함수인지? 정확하게 기재 부탁드립니다
                          // 위에 useCard() 함수가 있는데 어떤 차이가 있나요? useCard()와 CardUse() 같은 구분이 모호한 명명은 지양해주시고
                          // 함수 이름에서 함수의 역할이 어느정도 유추되는게 아닌 카드는 주석을 정확하게 적어주세요
    {
        if (GetComponent<BlackWhite>() != null)
        {        
            GetComponent<BlackWhite>().onDamage();
        }
        if (drowCard > 0)
        {
            BM.specialDrow(drowCard);
        }
        if (weaknessCount > 0)
        {
            for (int i = 0; i < BM.characters.Count; i++)
            {
                BM.characters[i].myPassive.EnemyGetWeak();
            }
            for (int i = 0; i < BM.Enemys.Length; i++)
            {
                BM.Enemys[i].GetComponent<Enemy>().StatusChange((int)Status.weak, weaknessCount);
            }
        }
        if (percentDmgToAllTarget > 0)
        {
            BM.AllAttackOnPercent(percentDmgToAllTarget, BM.actCharacter);            
        }
        if (iscard20Mode)
        {           
            BM.useCost(BM.usedInCard20.GetComponent<Card>().cardcost,gameObject);
            BM.usedInCard20.GetComponent<Card>().CardUse();
            BM.usedInCard20 = null;
            Destroy(gameObject);
            return;
        }
        CM.UseCard(gameObject);
    }

    public void textSet() // YH : '텍스트설정' 같은 주석말고, 언제 실행되는 함수인지? 무슨 역할을 하는 함수인지? 정확하게 기재 부탁드립니다
    {
       
        if (cardNo != 8)
            costT.text = cardcost + "";
        else costT.text = "N";
    }

    [HideInInspector] public Vector3 origin_Position;
    private bool isOnMouse;

    private void OnMouseDown()
    {
        if (!isGrave && !isDeck && !BM.otherCanvasOn)
        {
            if (BM.otherCorIsRun) return;
           
            if (isSelected) return;

            if (!HandManager.Instance.isEnableOtherButton)
                return;

            HandManager.Instance.isEnableOtherButton = false;
            BM.isSelectedCardinHand = true; //YH
            isSelected = true; //YH
            HandManager.Instance.SelectCard(this);
            HandManager.Instance.InputToOriginText(this); //YH  
            HandManager.Instance.CardMouseEnter(this); //YH

            if (!BM.enemySelectMode && !BM.otherCanvasOn)
            {
                if (BM.selectedCard != gameObject)
                    BM.SetCard(gameObject);
                else
                    BM.cancleCard();
            }
            //else if (isGrave || isDeck)
            //    CM.ClickInGraveOrDeck(gameObject);

            if (selectType == 1)
            {
                BM.enemySelectMode = true;
                Debug.Log("Enemy select mode on");
            }
            else
            {
                BM.enemySelectMode = false;
                Debug.Log("Enemy select mode off");
            }

            HandManager.Instance.CardMouseDown();
            //gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, 1000f, 0f);
        }
        else
            CM.ClickInGraveOrDeck(gameObject);
    }

    private void OnMouseUp()
    {
        if (!BM.otherCanvasOn)
            HandManager.Instance.CardMouseUp();
    }

    private void OnMouseEnter()
    {
        if (!HandManager.Instance.isSelectedCard && !BM.otherCanvasOn)
        {
            if (!BM.enemySelectMode && !BM.otherCanvasOn && !isDeck)
                HandManager.Instance.CardMouseEnter(this);
            if (BM.otherCanvasOn && (BM.isGraveWindowOn || BM.isDeckWindowOn))
            {
                //if (isGrave)
                //    HandManager.Instance.CardTooltipOn(this);
                // if(isGrave||isDeck) HandManager.Instance.CardMouseEnter(this);
                HandManager.Instance.CardTooltipOn(this);
            }
        }
    }

    private void OnMouseOver()
    {
        isOnMouse = true;
    }

    private void OnMouseExit()
    {
        if (!HandManager.Instance.isSelectedCard)
        {
            if (!BM.otherCanvasOn)
            {
                isOnMouse = false;
                HandManager.Instance.CardMouseExit(this);
            }
            else
            {
                if (BM.isGraveWindowOn || BM.isDeckWindowOn)
                    HandManager.Instance.go_SelectedCardTooltip.SetActive(false);
            }
        }
    }

    public void SavePosition(float x, float y, float z)
    {
        origin_Position = new Vector3(x, y, z);
    }

    public void GetDrowCardEffect()
    {
        if (drowCard == 0)
        {
            drowCard = 1;
            Content.text += "\n-[드로우:1]";
        }
        else
        {
            drowCard++;
            string newstring = Content.text;
            newstring = newstring.Replace("[드로우:"+(drowCard-1)+"]", "[드로우:" + drowCard  + "]");
            Content.text = newstring;
        }
    }

    public void GetWeaknessEffect()
    {
        if (weaknessCount == 0)
        {
            weaknessCount = 1;
            Content.text += "\n-[적 전체에게 약화를 1부여한다.]";
        }
        else
        {
            weaknessCount++;
            string newstring = Content.text;
            newstring = newstring.Replace("[적 전체에게 약화를 " + (weaknessCount - 1) + "부여한다.]", "[적 전체에게 약화를 " + weaknessCount + "부여한다.]");
            Content.text = newstring;
        }
    }

    public void GetRemove()
    {
        isRemove = true;
        Content.text += "\n-[소멸]";
    }

    public void RemoveThisCardInDeck()
    {
        for(int i = 0; i < BM.characters.Count; i++)
        {
            BM.characters[i].myPassive.CardRemove();
        }
        
        CM.Deck.Remove(gameObject);
        CM.RemoveCard.Add(gameObject);
        transform.parent = CM.RemoveContent.transform;
        gameObject.SetActive(false);
    }

    public void RemoveThisCardInField()
    {
     
        for (int i = 0; i < BM.characters.Count; i++)
        {
            BM.characters[i].myPassive.CardRemove();
        }
        CM.field.Remove(gameObject);
        CM.RemoveCard.Add(gameObject);
        transform.parent = CM.RemoveContent.transform;
    }

    public void GetPercentDmgToAllTarget(int dmg)
    {
        if (percentDmgToAllTarget== 0)
        {
            percentDmgToAllTarget = dmg;
            Content.text += "\n-[적 전체에게 데미지:적 현재 HP의" +dmg+"%]";
        }
        else
        {
            int previous = percentDmgToAllTarget;
            percentDmgToAllTarget += dmg;
            string newstring = Content.text;
            newstring = newstring.Replace("[적 전체에게 데미지:적 현재 HP의" +previous+"%]", "[적 전체에게 데미지:적 현재 HP의" + percentDmgToAllTarget + "%]");
            Content.text = newstring;
        }
    }
}
