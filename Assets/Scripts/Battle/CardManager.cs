using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using TMPro;

public class CardManager : SingletonMonobehaviour<MonoBehaviour>
{
    public List<GameObject> Deck = new List<GameObject>(); //덱에 있는 카드들의 리스트
    public List<GameObject> Grave = new List<GameObject>(); //무덤에 있는 카드들의 리스트
    public List<GameObject> field = new List<GameObject>(); //필드에 있는 카드들의 리스트
    public List<GameObject> ReviveCard = new List<GameObject>(); //무덤에서 부활로 선택된 카드들의 리스트
    public List<GameObject> SelectedCard = new List<GameObject>(); //덱에서 선택된 카드들의 리스트
    public List<GameObject> RemoveCard = new List<GameObject>();
    public Text graveT;
    public Text deckT;
    [SerializeField] GameObject graveWarn;
    [SerializeField] GameObject selectedWarn;
    [SerializeField] GameObject CardPrefebs;

    [SerializeField] GameObject GraveContent;
    public GameObject RemoveContent;
    public TurnManager TM;
    public int FiledCardCount;//현재 필드에 카드가 몇 장 있나
    public int specialDrow;//카드를 통한 드로우
    public int cardKind;//카드의 종류
    public GameObject CardCanvas;
    public GameObject HandCanvas;
    public GameObject DeckCanvas;
    public CardData CD;
    BattleManager BM;
    HandManager HM;
    ActManager AM;

    string[] deckText = new string[7];

    [SerializeField] private GameObject go_Content_Deck;
	[SerializeField] private Transform deckContentTrans;
	public SO_CardList cardList;
	//매 프레임마다 하는게 괜찮다면야..
	private void Update()
    {
        graveT.text = "" + Grave.Count;
        deckT.text = "" + Deck.Count;
    }
    private void Start()
    {
		//raetic 여기 부분 스크립터블 오브젝트로 수정하자.
		deckText[0] = "기본";
        deckText[1] = "Q";
        deckText[2] = "스파키";
        deckText[3] = "반가라";
        deckText[4] = "포르테";
        deckText[5] = "령";
        deckText[6] = "흉귀";
        //string filepath = Application.persistentDataPath + "/CardData.json";
        string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CardDatas[GameManager.Instance.nowPlayingSlot]);
        if (File.Exists(path))
        {
            string cardData = File.ReadAllText(path);
            CD = JsonConvert.DeserializeObject<CardData>(cardData);

        }
		CD.SetSO(cardList);

		for (int i = 0; i < CD.cardDetails.Count; i++)
        {
			//카드 데이터에 있는 카드들로 내 덱을 꾸려준다.
            GameObject newCard = Instantiate(CardPrefebs, CardCanvas.transform);
			int cardNo = CD.cardDetails[i].no;
			string ClassName = "Card"+ cardNo;
			var type = System.Type.GetType(ClassName);
			if (type != null)
			{
				newCard.AddComponent(type);
			}
			else
			{
				newCard.AddComponent<RenewalCard>();
			}

			RenewalCard newCardComponent = newCard.GetComponent<RenewalCard>();
			newCardComponent.SetInfo(cardNo, CD.cardDetails[i].cost, CD.cardDetails[i].values);

			Deck.Add(newCard);
        }
        //내 카드 데이터를 가지고 카드들을 생성하여 모든 카드들을 덱에 넣는다.
        for (int i = 0; i < Deck.Count; i++)
        {
            Deck[i].SetActive(false); //덱에 들어간 카드들은 당장 비 활성화
        }
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        HM = GameObject.Find("HandManager").GetComponent<HandManager>();
        AM = GameObject.Find("ActManager").GetComponent<ActManager>();
    }

    public void CardToField() //랜덤한 카드를 덱에서 필드로 이동시키는 함수
    {
        if (Deck.Count > 0)
        {
            int rand = Random.Range(0, Deck.Count);

            field.Add(Deck[rand]);
            HM.AddCard(Deck[rand]);
            Deck.RemoveAt(rand);
        }
    }
    public void TurnStartCardSet()  //해당 턴에서 적용 됐던 설정들을 초기화 하는 과정
    {
        for (int i = 0; i < Deck.Count; i++)
        {
            Deck[i].SetActive(true);
            Deck[i].GetComponent<RenewalCard>().cardcost = Deck[i].GetComponent<RenewalCard>().realCost;
            Deck[i].GetComponent<RenewalCard>().costTextSet(Deck[i].GetComponent<RenewalCard>().cardcost);
            Deck[i].SetActive(false);

        }
        for (int i = 0; i < Grave.Count; i++)
        {
            Grave[i].SetActive(true);
            Grave[i].GetComponent<RenewalCard>().cardcost = Grave[i].GetComponent<RenewalCard>().realCost;
            Grave[i].GetComponent<RenewalCard>().costTextSet(Grave[i].GetComponent<RenewalCard>().cardcost);
            Grave[i].SetActive(false);
        }
        StartCoroutine("turnStartDrow");
    }
    IEnumerator turnStartDrow()//턴 시작시 드로우
    {
        BM.otherCorIsRun = true;
  
        for (int i = 0; i < BM.TurnCardCount; i++)
        {
            CardToField();
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(1.2f);

        GameObject.Find("HandManager").GetComponent<HandManager>().InitCard();
        TM.TurnStartPassive();
        BM.otherCorIsRun = false;

    }
    public void SpecialCardToField(GameObject card)//카드를 통한 드로우
    {
        HM.AddCard(card);
        //Rebatch();   
    }
    public void PlusCard(int i)//i번 카드를 덱에 넣는 함수(현재는 큐의 백옥의 왕에서 적용)
    {
        //GameObject newCard = Instantiate(CardPrefebs, CardCanvas.transform);

        //newCard.GetComponent<RenewalCard>().NoT.text = "NO." + CardInfo.Instance.cd[i].No.ToString("D3");//넘버
        //newCard.GetComponent<RenewalCard>().cardNo = CardInfo.Instance.cd[i].No;
        //newCard.GetComponent<RenewalCard>().DeckT.text = deckText[CardInfo.Instance.cd[i].Deck];
        //newCard.GetComponent<RenewalCard>().Content.text = CardInfo.Instance.cd[i].Content;
        //newCard.GetComponent<RenewalCard>().Name.text = CardInfo.Instance.cd[i].Name;
        //newCard.GetComponent<RenewalCard>().cardcost = CardInfo.Instance.cd[i].Cost;
        //newCard.GetComponent<RenewalCard>().realCost = CardInfo.Instance.cd[i].Cost;
        //newCard.GetComponent<RenewalCard>().selectType = CardInfo.Instance.cd[CD.cardCode[i]].select;

        //Deck.Add(newCard);
        //newCard.SetActive(false);
    }
    public void UseCard(GameObject usingCard)//사용된 카드를 무덤에 넣는 과정
    {
        if (usingCard.GetComponent<RenewalCard>().isRemove)
        {

            usingCard.GetComponent<RenewalCard>().RemoveThisCardInField();
            usingCard.transform.parent = RemoveContent.transform;
            usingCard.GetComponent<RenewalCard>().isGrave = true;
            usingCard.SetActive(false);
        }
        else
        {
            for (int i = field.Count - 1; i >= 0; i--)
            {
                if (usingCard == field[i])
                {
                    field.RemoveAt(i);
                    break;
                }
            }
            bool InGrave = false;
            for (int i = 0; i < Grave.Count; i++)
            {
                if (Grave[i] == usingCard)
                {
                    InGrave = true;
                    break;
                }
            }

            if (!InGrave) Grave.Add(usingCard);
            usingCard.transform.parent = GraveContent.transform;
            usingCard.GetComponent<RenewalCard>().isGrave = true;
            usingCard.SetActive(false);
        }
      
        BM.previousCharacter = null;
        BM.previousEnemy = null;
        if (usingCard.GetComponent<RenewalCard>().selectType == 1)
        {
            BM.previousEnemy = BM.selectedEnemy;
        }
        if (usingCard.GetComponent<RenewalCard>().selectType == 5)
        {
            BM.previousCharacter = BM.selectedCharacter;
        }
        HandManager.Instance.CancelToUse(false);
        TM.BM.previousSelectedCard = usingCard; //스케치 반복을 위해 이전 카드를 기록함

        BM.actCharacter.SelectBox.SetActive(false);
        Character curC = BM.actCharacter;
        curC.myPassive.CardUse(usingCard.GetComponent<RenewalCard>());
        curC.GetComponent<CharacterPassive>().myAct();
        for (int i = 0; i < BM.ChD.size; i++)
        {
            BM.characters[i].myPassive.CardUseInTeam();
        }

        if (usingCard.GetComponent<RenewalCard>().Name.text != "스케치 반복")
        {
            BM.cancleCard();
            TM.turnCardPlus();
        }
        // StartCoroutine("CardUseCor",curC);
        HandManager.Instance.ArrangeCard();

    }

    public void FieldToGrave(GameObject c) //사용하지 않고 바로 필드에서 무덤으로 버리는 함수
    {
        for (int i = field.Count - 1; i >= 0; i--)
        {
            if (c == field[i])
            {
                field.RemoveAt(i);
                break;
            }
        }
        Grave.Add(c);
        c.transform.parent = GraveContent.transform;
        c.GetComponent<RenewalCard>().isGrave = true;
        c.SetActive(false);
    }

    public void GraveOn() //무덤 팝업을 킴
    {
        for (int i = 0; i < Grave.Count; i++)
        {
            Grave[i].SetActive(true);//무덤에 있는 카드들을 활성화
            Grave[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

    public void DeckOn() //덱 팝업을 킴
    {
        for (int i = 0; i < Deck.Count; i++)
        {
            Deck[i].GetComponent<RenewalCard>().isDeck = true;
            Deck[i].transform.parent = go_Content_Deck.transform;
            Deck[i].SetActive(true);//덱에 있는 카드들을 활성화
            Deck[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

    public void GraveOff()
    {
        for (int i = 0; i < Grave.Count; i++)
        {
            Grave[i].SetActive(false);
        }
        BM.GraveReviveMode = false;
    }

    public void DeckOff()
    {
        SelectedCard.Clear();
        for (int i = 0; i < Deck.Count; i++)
        {
            Deck[i].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            Deck[i].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            Deck[i].GetComponent<RenewalCard>().isDeck = false;
            Deck[i].transform.parent = CardCanvas.transform;
            Deck[i].SetActive(false);
        }
    }
    public void FieldOff() //필드에 있는 모든 카드를 덱으로 보냄 턴 종료시에 발동
    {
        FiledCardCount = field.Count;
        for (int i = field.Count - 1; i >= 0; i--)
        {
            field[i].transform.position = new Vector3(100, 100, 0);
            field[i].SetActive(false);
            field[i].transform.parent = CardCanvas.transform;
            Deck.Add(field[i]);
            field.RemoveAt(i);
        }
        HandManager.Instance.ArrangeCard();
    }

    public void GraveToField(GameObject Gcard) //선택된 카드를 무덤에서 필드로 부활시키는 함수
    {
        TM.BM.log.logContent.text += "\n" + Gcard.GetComponent<RenewalCard>().Name.text + "이(가) 묘지에서 패로 이동합니다.";
        for (int i = 0; i < Grave.Count; i++)
        {
            if (Gcard == Grave[i])
            {
                field.Add(Grave[i]);
                HM.AddCard(Grave[i]);
                Grave[i].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 225);
                Grave[i].GetComponentInChildren<Image>().color = new Color(1, 1, 1);
                Grave[i].SetActive(true);
                Grave[i].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                Grave[i].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);


                Grave[i].GetComponent<RenewalCard>().isGrave = false;
                Grave[i].GetComponent<Transform>().localScale = new Vector2(1, 1);
                if (BM.curSelectedCardInSelectOthers!=null&&BM.curSelectedCardInSelectOthers.cardNo == 13)
                {
                    Grave[i].GetComponent<RenewalCard>().changeCardCost(-2);
                }
                Grave.RemoveAt(i);

                break;
            }
        }



    }
    public void FieldToDeck(GameObject FieldCard) //선택된 카드를 필드에서 덱으로 보내는 함수, 필드 배열에서는 밖에서 처리해줘야 함.
    {
        for (int i = 0; i < field.Count; i++)
        {
            if (field[i] == FieldCard)
            {
                Deck.Add(FieldCard);
                FieldCard.transform.parent = DeckCanvas.transform;
                field[i].SetActive(false);
                //field.RemoveAt(i);
                break;
            }
        }

        HandManager.Instance.ArrangeCard();
    }
    public bool Revive() //부활시키는 애니메이션 용
    {
		if(ReviveCard.Count != BM.reviveCount)
		{
			BM.WarnOn("부활시킬 카드의 숫자가 맞지 않습니다.");
			return false;
		}

        StartCoroutine("ReviveC");
		return true;
    }
    IEnumerator ReviveC()
    {
		//락걸자.
		HandManager.Instance.isAnimationDoing = true;
        for (int i = ReviveCard.Count - 1; i >= 0; i--)
        {
			RenewalCard card = BM.selectedCard.GetComponent<RenewalCard>();
			if (card.cardNo==13)
			{
				ReviveCard[i].GetComponent<RenewalCard>().changeTurnCardCost(-(int)card.values[1]);
			}
            GraveToField(ReviveCard[i]);
            ReviveCard.RemoveAt(i);
            yield return new WaitForSeconds(0.5f);
        }
		HandManager.Instance.isAnimationDoing = false;
	}
    public void ReviveCountOver() //무덤에서 너무 많이 고를 때
    {
        graveWarn.SetActive(true);
        Invoke("gWarnOff", 1f);
    }
    public void SelectedCountOver()
    {
        selectedWarn.SetActive(true);
        Invoke("sWarnOff", 1f);
    }
    void gWarnOff()
    {
        graveWarn.SetActive(false);
    }
    void sWarnOff()
    {

        selectedWarn.SetActive(false);
    }
    public void ClickInGraveOrDeck(GameObject g) //무덤이나 덱에서 카드를 선택 할 시, 어떤 카드가 선택되엇는지 시각적으로 보여주기 위함
    {
        if (BM.GraveReviveMode)
        {
            bool isClicked = false;
            for (int i = 0; i < ReviveCard.Count; i++)
            {
                if (g == ReviveCard[i]) { isClicked = true; break; }
            }
            if (isClicked)
            {
                g.GetComponent<Transform>().localScale = new Vector2(1, 1f);
                for (int i = 0; i < ReviveCard.Count; i++)
                {
                    if (g == ReviveCard[i]) { ReviveCard.RemoveAt(i); break; }
                }
            }
            else
            {
                if (BM.reviveCount > ReviveCard.Count)
                {
                    g.GetComponent<Transform>().localScale = new Vector2(1.1f, 1.1f);
                    ReviveCard.Add(g);
                }
                else
                {
                    //ReviveCountOver();
                }
            }
        }
        if (BM.DeckSelectMode)
        {
            bool isClicked = false;
            for (int i = 0; i < SelectedCard.Count; i++)
            {
                if (g == SelectedCard[i]) { isClicked = true; break; }
            }
            if (isClicked)
            {
                g.GetComponent<Transform>().localScale = new Vector2(1, 1f);
                for (int i = 0; i < SelectedCard.Count; i++)
                {
                    if (g == SelectedCard[i]) { SelectedCard.RemoveAt(i); break; }
                }
            }
            else
            {
                if (BM.SelectDeckCount > SelectedCard.Count)
                {
                    g.GetComponent<Transform>().localScale = new Vector2(1.1f, 1.1f);
                    SelectedCard.Add(g);
                }
                else
                {
                    SelectedCountOver();
                }
            }

        }
    }
    public void DeckToField()//덱에서 선택된 카드들을 필드로 보내는 함수
    {
        for (int j = 0; j < SelectedCard.Count; j++)
        {
            TM.BM.log.logContent.text += "\n" + SelectedCard[j].GetComponent<RenewalCard>().Name.text + "이(가) 덱에서 패로 이동합니다.";
            for (int i = 0; i < Deck.Count; i++)
            {
                if (SelectedCard[j] == Deck[i])
                {
                    field.Add(SelectedCard[j]);
                    HM.AddCard(SelectedCard[j]);
                    SelectedCard[j].GetComponent<RenewalCard>().isDeck = false;
                    SelectedCard[j].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                    SelectedCard[j].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                    SelectedCard[j].SetActive(true);
                    SelectedCard[j].transform.parent = CardCanvas.transform;
                    SelectedCard[j].GetComponent<Transform>().localScale = new Vector2(1, 1);
                    Deck.RemoveAt(i);
                    break;
                }
            }
        }
        HandManager.Instance.ArrangeCard();
    }
    public void GraveToDeck(GameObject c) //무덤에서 덱으로 부활시키는 함수
    {
        TM.BM.log.logContent.text += "\n" + c.GetComponent<RenewalCard>().Name.text + "이(가) 무덤에서 덱으로 이동합니다.";
        for (int i = 0; i < Grave.Count; i++)
        {
            if (c == Grave[i])
            {               
                Deck.Add(c);
                c.GetComponent<RenewalCard>().isDeck = true;
                c.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                c.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
				c.transform.parent =deckContentTrans;
				c.GetComponent<RenewalCard>().isGrave = false;
                c.SetActive(false);
                c.GetComponent<RenewalCard>().isSelected = false;
                Grave.RemoveAt(i);
                break;
            }
        }
        HandManager.Instance.ArrangeCard();
    }
    public void DeckToGrave(GameObject c) //덱에서 무덤으로 선택된 카드를 보내는 함수
    {
        TM.BM.log.logContent.text += "\n" + c.GetComponent<RenewalCard>().Name.text + "이(가) 덱에서 무덤으로 이동합니다.";
        for (int i = 0; i < Deck.Count; i++)
        {
            if (c == Deck[i])
            {
                Grave.Add(c);
                c.GetComponent<RenewalCard>().isDeck = false;
                c.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                c.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                c.transform.parent = GraveContent.transform;
                c.GetComponent<RenewalCard>().isGrave = true;
                c.SetActive(false);
                Deck.RemoveAt(i);
                break;
            }
        }
        HandManager.Instance.ArrangeCard();
    }
    public void Drow()
    {
        StartCoroutine("DrowCor");
    }
    IEnumerator DrowCor()
    {
        BM.otherCorIsRun = true;


        CardToField();
        yield return new WaitForSeconds(0.25f);


        HandManager.Instance.InitCard();
        BM.otherCorIsRun = false;
    }
    public void RemoveCardRemove()
    {
        for (int i = 0; i < field.Count; i++)
        {
            if (field[i].GetComponent<RenewalCard>().isRemove)
            {
                field[i].GetComponent<RenewalCard>().RemoveThisCardInField();
            }
        }
        for (int i = 0; i < Deck.Count; i++)
        {
            if (Deck[i].GetComponent<RenewalCard>().isRemove)
            {
                Deck[i].GetComponent<RenewalCard>().RemoveThisCardInDeck();
            }
        }
    }
}