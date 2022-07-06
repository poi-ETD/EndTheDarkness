using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
public class HandManager : MonoBehaviour
{
    private static HandManager instance;

    public static HandManager Instance
    {
        get
        {
            return instance;
        }
    }

    Camera camera;

    public GameObject cardSample;
    private GameObject card;
    private List<GameObject> list_Card;

    private int count_Card;
    public bool isInited = false;
    private List<float> list_XPosition;
    private List<float> list_Rotation;
    CardManager CM;

    //[SerializeField] private InputField inputField_RemoveCard;

    public GameObject go_UseButton;

    public GameObject go_SelectedCardTooltip;
    public TextMeshProUGUI text_TooltipCardName;
    public TextMeshProUGUI text_TooltipCardNo;
    public TextMeshProUGUI text_TooltipCardContext;

    private string originCardName;
    private string originCardNo;
    private string originCardContext;

    BattleManager BM;
    private int selectedCardStack = 0; // prevent to too much fast select error between cards

    private Card onPointerCard = null; // this field have script<Card> on mouse pointer
    private Card selectedCard = null; // this field have script<Card> selected

    [HideInInspector] public bool isSelectedCard = false;
    [SerializeField] private GameObject go_selectedCardImage;

    [HideInInspector] public bool isEnableOtherButton; // 패에서 카드 클릭시 카드를 사용하거나 취소하기 전까지는 다른 버튼을 사용하지 못하게 하는 변수

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
        list_Card = new List<GameObject>();
        list_XPosition = new List<float>();
        list_Rotation = new List<float>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();

        selectedCard = new Card();
    }

    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        isEnableOtherButton = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            BM.specialDrow(1);

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {

        } 

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            if (CM.field.Count > 0)
            {
                CM.FieldToGrave(CM.field[CM.field.Count - 1]);
                ArrangeCard();
            }
        }

        //if (!BM.isPointerinHand)
        //    InputToCardText();

        if(Input.GetMouseButtonDown(0))
        {
            if (isSelectedCard)
                BM.Click_useCard();
        }

        if (Input.GetMouseButtonDown(1))
        {
            //isSelectedCard = false;
            CancelToUse();
        }

        if (isSelectedCard)
        {
            CardDrag();
            go_selectedCardImage.SetActive(true);
        }
    }

    public void AddCard(GameObject newCard)
    {
        if (!isInited)
        {       
            newCard.transform.position = new Vector3(18,-7, 0);

            newCard.SetActive(true);
            int random_Rotation_Z = Random.Range(-10, 10);
            float random_Position_X = Random.Range(-0.2f, 0.2f);
            float random_Position_Y = Random.Range(-0.2f, 0.2f);
            newCard.transform.parent = GameObject.Find("HandCardCanvas").transform;       

            newCard.transform.rotation = Quaternion.Euler(new Vector3(0, 0, random_Rotation_Z));
            //newCard.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            newCard.transform.DOMove(new Vector2(random_Position_X, random_Position_Y - 8f), 1);
            newCard.transform.DOScale(new Vector3(1f, 1f, 1f), 1).SetEase(Ease.OutExpo);
            newCard.GetComponent<Image>().DOFade(1f, 1).SetEase(Ease.OutExpo); // TODO:non action function -> action function           
             
            list_Card.Add(newCard);
            count_Card++;
        }
        else
        {
            //card = Instantiate(cardSample, new Vector3(8f, 0f, 0f), Quaternion.identity);
            // card.GetComponent<HandCard>().handNumber = count_Card;
            newCard.SetActive(true);
            newCard.transform.parent = GameObject.Find("HandCardCanvas").transform;
            newCard.transform.position = new Vector3(18f, -10f, 0f);
            newCard.transform.rotation = Quaternion.identity;       
            newCard.GetComponent<Image>().DOFade(1f, 1).SetEase(Ease.OutExpo);
            ArrangeCard(); 
        }
    }

    private void RemoveCard()
    {
        Destroy(list_Card[count_Card - 1]);
        list_Card.RemoveAt(count_Card - 1);
        count_Card--;

        ArrangeCard();
    }

    private void RemoveCard(int index)
    {
        Destroy(list_Card[index]);
        list_Card.RemoveAt(index);
        count_Card--;
        for (int i = index; i < list_Card.Count; i++)
           // list_Card[i].GetComponent<HandCard>().handNumber--;

        ArrangeCard();
    }

    public void InitCard()
    { 
        ArrangeCard();

        isInited = true;
    }

    public void ArrangeCard()
    {
        float x = 16 / (float)CM.field.Count;
        float initX = -8f + x / 2;

        for (int i = 0; i < CM.field.Count; i++)
        {
            list_XPosition.Add(initX);
            list_Rotation.Add(initX * -2f);
            initX += x;
        }
        int index = 0;

        foreach (GameObject card in CM.field)
        {
            card.GetComponent<Card>().SavePosition(list_XPosition[index], FindY_CircleEquation(list_XPosition[index]) - 8f, -(index / 10f));         
            card.transform.DOMove(new Vector3(list_XPosition[index], FindY_CircleEquation(list_XPosition[index]) - 8f, -(index / 10f)), 0.3f)
                .SetEase(Ease.OutExpo);
            card.transform.DORotate(new Vector3(0f, 0f, list_Rotation[index]), 0.3f).SetEase(Ease.OutExpo);
            index++;
        }

        list_XPosition.Clear();
        list_Rotation.Clear();
    }

    private float FindY_CircleEquation(float x)
    {
        float y = -30;
        y += Mathf.Sqrt(900 - Mathf.Pow(x, 2));
        return y;
    }

    //public void click_RemoveCard()
    //{
    //    RemoveCard(int.Parse(inputField_RemoveCard.text));
    //}

    public void CardMouseEnter(Card card)
    {
        if (!card.isGrave && !BM.isGraveWindowOn)
        {
            if (BM.turnStarting) return;

            onPointerCard = card;
            BM.isPointerinHand = true;

            if (selectedCardStack <= 2 && isInited)
            {
                CardTooltipOn(card);

                if (card != selectedCard)
                {
                    card.transform.DOMove(new Vector3(card.gameObject.transform.position.x, card.gameObject.transform.position.y + 0.5f,
                        card.gameObject.transform.position.z), 0.3f).SetEase(Ease.OutExpo);
                }
                selectedCardStack++;

                if (selectedCardStack == 1)
                    Invoke("initStack", 0.15f);

                // TODO:if first card select too much fast, add below
                // if (firstcard) -> Invoke("initStack", 1f~);
            }
        }
        else
        {

        }
    }

    public void CardMouseExit(Card card)
    {
        if (!card.isGrave)
        {
            onPointerCard = null;
            BM.isPointerinHand = false;

            if (isInited)
            {
                if (card != selectedCard)
                    card.transform.DOMove(card.origin_Position, 0.3f).SetEase(Ease.OutExpo);
            }

            if (!BM.isSelectedCardinHand)
                go_SelectedCardTooltip.SetActive(false); // if be unpleasant because of too much fast flicker of selected card, add below
                                                         // 1. add polygon collider -> 2. if mouse pointer in collider go_SelectedCard.SetActive(true), else go_SelectedCard.SetActive(false)
        }
        else
        {
            go_SelectedCardTooltip.SetActive(false);
        }
    }

    public void CardMouseDown()
    {
        Invoke("On_IsSelectedCard", 0.01f);
        selectedCard.gameObject.SetActive(false);
        //selectedCard.gameObject.transform.position = new Vector3(0f, 1000f, 0f);
    }

    private void On_IsSelectedCard() // 카드 선택(왼쪽 클릭) 후 카드 사용(왼쪽 클릭)까지 격차를 주기 위한(동시에 이뤄지지 않게 하기 위한) Invoke용 함수
    {
        isSelectedCard = true;
        ArrangeCard();
    }

    public void CardMouseUp()
    {
        
    }

    private void initStack()
    {
        selectedCardStack = 0;
    }

    public void InputToOriginText(Card card)
    {
        originCardName = card.Name.text;
        originCardNo = card.NoT.text;
        originCardContext = card.Content.text;
    }

    private void InputToCardText()
    {
        text_TooltipCardName.text = originCardName;
        text_TooltipCardNo.text = originCardNo;
        text_TooltipCardContext.text = originCardContext;
    }

    public void CancelToUse()
    {
        if (selectedCard == null)
            return;

        isSelectedCard = false;
        selectedCard.gameObject.SetActive(true);
        go_selectedCardImage.SetActive(false);

        if (selectedCard != onPointerCard)
        {
            SelectCardToOriginPosition();
            go_SelectedCardTooltip.SetActive(false);
        }

        selectedCard.isSelected = false;
        BM.isSelectedCardinHand = false;
        BM.cancleCard();

        selectedCard = null;
        isEnableOtherButton = true;
    }

    public void SelectCard(Card card) // 이전에 선택된 카드의 선택상태를 취소하고 새로운 인자로 들어온 카드를 설정하는 함수
    {
        if (selectedCard != null)
        {
            selectedCard.transform.DOMove(selectedCard.origin_Position, 0.3f).SetEase(Ease.OutExpo);
            selectedCard.isSelected = false;
        }

        selectedCard = card;
    }

    public void CardTooltipOn(Card card)
    {
        text_TooltipCardName.text = card.Name.text;
        text_TooltipCardNo.text = card.NoT.text;
        text_TooltipCardContext.text = card.Content.text;

        go_SelectedCardTooltip.SetActive(true);
    }

    public void SelectCardToOriginPosition()
    {
        if (selectedCard != null)
            selectedCard.transform.DOMove(selectedCard.origin_Position, 0.3f).SetEase(Ease.OutExpo);
        //selectedCard.gameObject.transform.position = selectedCard.origin_Position;
    }

    private void CardDrag()
    {
        Vector3 mouseCursor = Input.mousePosition - new Vector3(960, 540);

        go_selectedCardImage.GetComponent<RectTransform>().localPosition = mouseCursor;
    }
}