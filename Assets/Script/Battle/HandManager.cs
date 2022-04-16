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

    public GameObject cardSample;
    private GameObject card;
    private List<GameObject> list_Card;

    private int count_Card;
    public bool isInited = false;
    private List<float> list_XPosition;
    private List<float> list_Rotation;
    CardManager CM;

    //[SerializeField] private InputField inputField_RemoveCard;

    [SerializeField] private GameObject go_SelectedCard;
    [SerializeField] private TextMeshProUGUI text_CardName;
    [SerializeField] private TextMeshProUGUI text_CardNo;
    [SerializeField] private TextMeshProUGUI text_CardContext;
    BattleManager BM;
    private int selectedCardStack = 0; // prevent to too much fast select error between cards

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
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            BM.specialDrow(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
           
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            if (CM.field.Count > 0)
            {
                CM.FieldToGrave(CM.field[CM.field.Count - 1]);
                CM.Rebatch();
            }
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

    private void ArrangeCard()
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
        if (BM.turnStarting) return;
       
        if (selectedCardStack <= 2&&isInited)
        {           
            go_SelectedCard.SetActive(true);
            text_CardName.text = card.Name.text;
            text_CardNo.text = card.NoT.text;
            text_CardContext.text = card.Content.text;
            if (!card.isGrave && !card.isDeck)
            {
                card.transform.DOMove(new Vector3(card.gameObject.transform.position.x, card.gameObject.transform.position.y + 0.5f,
                card.gameObject.transform.position.z), 0.3f).SetEase(Ease.OutExpo);
                selectedCardStack++;
                if (selectedCardStack == 1)
                    Invoke("initStack", 0.15f);
            }
            // TODO:if first card select too much fast, add below
            // if (firstcard) -> Invoke("initStack", 1f~);
        }
    }

    public void CardMouseExit(Card card)
    {
        //Debug.Log(card.origin_Position);
        if (!card.isGrave && !card.isDeck && isInited)
        {
            card.transform.DOMove(card.origin_Position, 0.3f).SetEase(Ease.OutExpo);
        }
        go_SelectedCard.SetActive(false); // if be unpleasant because of too much fast flicker of selected card, add below
        // 1. add polygon collider -> 2. if mouse pointer in collider go_SelectedCard.SetActive(true), else go_SelectedCard.SetActive(false)
    }

    private void initStack()
    {
        selectedCardStack = 0;
    }
}