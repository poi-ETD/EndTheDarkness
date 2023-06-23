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
    public int selectType; // 해당 카드가 발동되는 조건, 0->선택 없이 발동, 1->적 선택 시 발동 2->무덤에 있는 카드 선택 시 발동 ...
    public bool iscard20Mode;

    //부여 가능한 속성들
    int drowCard;
    int weaknessCount;
    int percentDmgToAllTarget;

    public bool isRemove;
    [SerializeField] private SO_CardList so_CardList;
    //YH
    [HideInInspector] public bool isSelected = false;

    private void Start() // YH : Start() 함수가 굳이 이런곳에 있어야할 이유가 있을까요?
    {
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        AM = BM.AM;
        Sprite sprite = so_CardList.cardDetails[cardNo].sprite_Card;
        if (sprite != null)
            cardImage.sprite = sprite;
        else cardImage.sprite = so_CardList.cardDetails[1].sprite_Card;
    }

    public void useCardInCard20() // 특수 케이스 : 스케치 반복 사용 로직 중 호출
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

    public void useCard() // 카드를 내면 호출되는 함수, 카드의 SelectType에 따라서 다음 행동이 결정된다.
    {
        if (CM.TM.turn == 1 )
        {
			if (BM.GD.blessbool[4]||BM.GD.blessbool[12])
			{
				BM.WarnOn("축복의 효과로 첫 턴에는 행동이 불가능합니다.");
				return;
			}
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
            else if (selectType == 1) // 적을 선택해야 하는 카드,현재는 다른 로직으로 실행됨
            {
                //BM.goEnemySelectMode();
            }
            else if (selectType == 2) // 무덤에 있는 카드를 선택해야 하는 카드
            {
                if (cardNo == 7)
                {
                    BM.ReviveToField(2);
                }
                if (cardNo == 13)
				{
					for (int i = 0; i < BM.Enemys.Length; i++)
					{
						BM.Enemys[i].GetComponent<Enemy>().StatusChange((int)Status.charming, 1);
					}

					BM.ReviveToField(1);
                }
            }
            else if (selectType == 3) //덱에 있는 카드를 선택해야 하는 카드
            {
                if (cardNo == 24)
                {
                    BM.SelectDeckCard(1);
                }
            }
            else if (selectType == 4) // 특수 케이스 : 스케치 반복에서만 사용
            {
                if (BM.previousSelectedCard == null)
                {
					BM.WarnOn("이전에 사용한 카드가 없습니다.");
                    return;
                }
                if (BM.previousSelectedCard.GetComponent<Card>().cardNo == 20)
                {
					BM.WarnOn("스케치 반복은 연속해서 사용 할 수 없습니다.");
                    return;
                }
                BM.UsePreviousCard();
            }
            else if (selectType == 5) // 아군 캐릭터를 선택해야 하는 카드
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
                UseCardResult();
                BM.AM.Act();
            }
            
        }
        else
            BM.enemySelectMode = false;
    }
  
    public void EnemySelectCard() // 해당 카드가 적을 선택해서 사용하는 카드일 때, 캐릭터가 선택 될 시 카드가 발동되면서 호출되는 함수
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
        
        UseCardResult();
    
        BM.AM.Act();
    }

    public void SelectRevive() //  해당 카드가 무덤에 있는 카드를 선택해서 사용하는 카드일 때, 캐릭터가 선택 될 시 카드가 발동되면서 호출되는 함수
    {
        if (cardNo == 7)
        {            
            BM.ghostRevive(4);
        }
        if (!iscard20Mode)
        {
            BM.useCost(cardcost, gameObject);          
        }
        UseCardResult();
        BM.AM.Act();
        BM.Click_GraveOff();
    }

    public void SelectDeck() //  해당 카드가 덱에 있는 카드를 선택해서 사용하는 카드일 때, 캐릭터가 선택 될 시 카드가 발동되면서 호출되는 함수
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
        UseCardResult();
        BM.AM.Act();
    }

    public void CharacterSelectCard() // 해당 카드가 캐릭터를 선택해서 사용하는 카드일 때, 캐릭터가 선택 될 시 카드가 발동되면서 호출되는 함수
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
      
        UseCardResult();
       
        BM.AM.Act();
    }

    public void decreaseCost(int amount) // "코스트 감소" 키워드가 발동될 때, 해당 카드의 코스트를 감소하는 함수
    {
        cardcost -= amount;
        if (cardcost < 0) cardcost = 0;
        if(cardNo!=8)
        costT.text = cardcost + "";
    }

    public void UseCardResult() // 카드 사용 시퀀스의 마지막, 카드가 사용되면서 효과가 설정된다.
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
            BM.usedInCard20.GetComponent<Card>().UseCardResult();
            BM.usedInCard20 = null;
            Destroy(gameObject);
            return;
        }
        CM.UseCard(gameObject);
    }

    public void costTextSet() // 카드의 효과로 인해 코스트가 변경될 경우, 해당 카드의 코스트 표기를 변경하는 함수
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

            if (selectType == 1&&!BM.porte3mode)
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
