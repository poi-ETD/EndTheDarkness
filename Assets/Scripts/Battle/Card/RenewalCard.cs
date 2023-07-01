using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RenewalCard : MonoBehaviour
{
	//모든 카드의 공통 사항은 여기에서 처리
	public CardManager CM;
	public Image cardImage;
	public TextMeshProUGUI Content;
	public BattleManager BM;
	public ActManager AM;
	public int cardcost;
	public TextMeshProUGUI costT;
	public TextMeshProUGUI DeckT;
	public TextMeshProUGUI NoT;
	public TextMeshProUGUI Name;
	public bool isGrave;
	public int realCost;
	public bool isDeck;
	public bool isIngame;
	public int DeckNo;      // start->0 Q->1 스파키->2 반가라->3 포르테->4 령->5
	public int type;        //스타터->0 스타터x->1 특수->2
	public int cardNo;
	public int selectType; // 해당 카드가 발동되는 조건, 0->선택 없이 발동, 1->적 선택 시 발동 2->무덤에 있는 카드 선택 시 발동 ...
	public bool iscard20Mode;
	public float[] values;
	protected int graveReviveValue;

	//부여 가능한 속성들
	int drowCard;
	int weaknessCount;
	int percentDmgToAllTarget;

	public bool isRemove;
	[SerializeField] private SO_CardList so_CardList;
	//YH
	[HideInInspector] public bool isSelected = false;
	// Start is called before the first frame update
	public virtual void Start()
    {

	}
	public virtual void TurnCardUse(int count)
	{

	}
	public virtual void LateUseCard()
	{
		UseCardResult();
	}

	public virtual void SetInfo(int no, int cost, float[] values)
	{
		CM = GameObject.Find("CardManager").GetComponent<CardManager>();
		BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
		AM = BM.AM;
		so_CardList = CM.cardList;

		//각 요소를 달아주자,
		cardImage = transform.GetChild(0).GetComponent<Image>();
		costT = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		NoT = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
		Name = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
		DeckT = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
		Content = transform.GetChild(5).GetComponent<TextMeshProUGUI>();
		//변하지 않는 값
		cardNo = no;
		CardDetails cardDetails = so_CardList.cardDetails[no];
		DeckNo = (int)cardDetails.ownerCharacter;
		selectType = cardDetails.selectType;
		name = cardDetails.name;
		Name.text= name;
		Content.text = cardDetails.description_KR;


		Content.text = so_CardList.cardDetails[cardNo].description_KR;
		Sprite sprite = so_CardList.cardDetails[cardNo].sprite_Card;
		if (sprite != null)
			cardImage.sprite = sprite;
		else
			cardImage.sprite = so_CardList.cardDetails[1].sprite_Card;

		//각종 변수
		this.values = values;
		if(values.Length == 1)
		{
			Content.text = string.Format(Content.text, values[0]);
		}
		else if(values.Length == 2) 
		{
			Content.text = string.Format(Content.text, values[0], values[1]);
		}
		else if(values.Length==3)
		{
			Content.text = string.Format(Content.text, values[0], values[1], values[2]);
		}
		else if(values.Length ==4)
		{
			Content.text = string.Format(Content.text, values[0], values[1], values[2], values[3]);
		}
		else if(values.Length==5)
		{
			Content.text = string.Format(Content.text, values[0], values[1], values[2], values[3], values[4]);
		}

		//코스트
		costTextSet(cost);
		cardcost = cost;
		realCost = cost;
	}
	public virtual void changeCardCost(int amount) // 
	{
		realCost += amount;
		cardcost += amount;
		if (cardcost < 0) cardcost = 0;
		costTextSet(cardcost);
	}
	public virtual void costTextSet(int cost) // 카드의 효과로 인해 코스트가 변경될 경우, 해당 카드의 코스트 표기를 변경하는 함수
	{
		costT.text = cost.ToString();
	}
	public virtual void changeTurnCardCost(int amount)
	{
		cardcost += amount;
		if (cardcost < 0) cardcost = 0;
		costTextSet(cardcost);
	}

	public virtual bool UseCard()
	{
		//간단한 검사는 여기서 처리하자~
		if (CM.TM.turn == 1)
		{
			if (BM.GD.blessbool[4] || BM.GD.blessbool[12])
			{
				BM.WarnOn("축복의 효과로 첫 턴에는 행동이 불가능합니다.");
				return false;
			}
		}

		if (BM.leftCost < realCost)
		{
			HandManager.Instance.CancelToUse(false);
			BM.costOver();
			return false;
		}
		return true;
	}
	virtual public void UseCardResult() // 카드 사용 성공!
	{
		BM.selectedCard= null;
		AM.Act();
		//여기 하드코딩도...변경하자
		//if (GetComponent<BlackWhite>() != null)
		//{
		//	GetComponent<BlackWhite>().onDamage();
		//}

		//if (drowCard > 0)
		//{
		//	BM.specialDrow(drowCard);
		//}

		//if (weaknessCount > 0)
		//{
		//	for (int i = 0; i < BM.characters.Count; i++)
		//	{
		//		BM.characters[i].myPassive.EnemyGetWeak();
		//	}
		//	for (int i = 0; i < BM.Enemys.Length; i++)
		//	{
		//		BM.Enemys[i].GetComponent<Enemy>().StatusChange((int)Status.weak, weaknessCount);
		//	}
		//}

		//if (percentDmgToAllTarget > 0)
		//{
		//	BM.AllAttackOnPercent(percentDmgToAllTarget, BM.actCharacter);
		//}
		////스케치 모드중이냥?
		//if (iscard20Mode)
		//{
		//	BM.useCost(BM.usedInCard20.GetComponent<RenewalCard>().cardcost, gameObject);
		//	BM.usedInCard20.GetComponent<RenewalCard>().UseCardResult();
		//	BM.usedInCard20 = null;
		//	Destroy(gameObject);
		//	return;
		//}
		BM.useCost(cardcost);
		CM.UseCard(gameObject);
	}

	[HideInInspector] public Vector3 origin_Position;
	private bool isOnMouse;
	private void OnMouseDown()
	{
		if (HandManager.Instance.isAnimationDoing)
			return;

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

			if (selectType == 1 && !BM.porte3mode)
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
			if(isGrave||isDeck)
			CM.ClickInGraveOrDeck(gameObject);
	}

	private void OnMouseUp()
	{
		if (!BM.otherCanvasOn)
			HandManager.Instance.CardMouseUp();
	}

	private void OnMouseEnter()
	{
		if (HandManager.Instance.isAnimationDoing)
			return;

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
			newstring = newstring.Replace("[드로우:" + (drowCard - 1) + "]", "[드로우:" + drowCard + "]");
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

	public void GetGhostRevive(int amount)
	{
		//망자 부활
	}

	public void GetRemove()
	{
		isRemove = true;
		Content.text += "\n-[소멸]";
	}

	public void RemoveThisCardInDeck()
	{
		for (int i = 0; i < BM.characters.Count; i++)
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
		if (percentDmgToAllTarget == 0)
		{
			percentDmgToAllTarget = dmg;
			Content.text += "\n-[적 전체에게 데미지:적 현재 HP의" + dmg + "%]";
		}
		else
		{
			int previous = percentDmgToAllTarget;
			percentDmgToAllTarget += dmg;
			string newstring = Content.text;
			newstring = newstring.Replace("[적 전체에게 데미지:적 현재 HP의" + previous + "%]", "[적 전체에게 데미지:적 현재 HP의" + percentDmgToAllTarget + "%]");
			Content.text = newstring;
		}
	}
}
