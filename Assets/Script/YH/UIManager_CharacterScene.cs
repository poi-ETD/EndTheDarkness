using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;

public class UIManager_CharacterScene : MonoBehaviour
{
    private static UIManager_CharacterScene instance;

    public static UIManager_CharacterScene Instance
    {
        get
        {
            return instance;
        }
    }

    private Character_Party[] characters_Party; // CharacterData(json) 파일에 담길 (파티)캐릭터 배열[4] :
                                                // 인덱스의 순서가 진형의 순서는 아님. 즉, 배열[3]에 담긴 캐릭터의 진형이 꼭 후방이지는 않음

    [SerializeField] private UI_Character_Party_CharacterScene[] ui_Party; // CharacterScene의 좌측 파티에 나열된 캐릭터 UI 배열[4] :
                                                                           // 인덱스의 순서가 곧 보여지는 진형의 순서임
    [SerializeField] private UI_Character_Party_CharacterScene_Card[] ui_Party_Card; // CharacterScene(카드 선택 창)의 좌측 파티에 나열된 캐릭터 UI 배열[4]

    [SerializeField] private SO_CharacterList so_CharacterList;
    [SerializeField] private SO_CardList so_CardList;

    private int selectedPartyCode; // 선택된 파티 코드. 즉, 왼쪽 파티 목록중 선택된 자리에 대한 코드(0~3)
    private int selectedCharacterCode; // 선택된 캐릭터 코드. 즉, 현재 가운데 화면에 띄워져 있는 캐릭터 코드
    private int selectedCharacterIndex; // 선택된 캐릭터 인덱스. 즉, 파티에 포함된 캐릭터들의 정보를 담고 있는 배열 characters_Party의 인덱스

    [SerializeField] private GameObject go_Characters;
    [SerializeField] private GameObject go_SelectedCharacter;

    [SerializeField] private GameObject go_Window_SelectRevivor;
    [SerializeField] private GameObject go_Window_SelectContractor;
    [SerializeField] private GameObject go_Window_SelectCard;

    // # 선택된 캐릭터 세부 정보에 쓰이는 변수 목록
    [Space(10f)]
    [SerializeField] private TextMeshProUGUI text_Name_SelectedCharacter;
    [SerializeField] private TextMeshProUGUI text_Aka_SelectedCharacter;

    [SerializeField] private TextMeshProUGUI text_Hp;
    [SerializeField] private TextMeshProUGUI text_AttackPower;
    [SerializeField] private TextMeshProUGUI text_Cost;
    [SerializeField] private TextMeshProUGUI text_Endurance;
    [SerializeField] private TextMeshProUGUI text_InventorySize;
    [SerializeField] private TextMeshProUGUI text_Speed;

    [SerializeField] private TextMeshProUGUI text_CardCount_EnableSelect;

    [SerializeField] private TextMeshProUGUI text_Name_Passive0;
    [SerializeField] private TextMeshProUGUI text_Name_Passive1;
    [SerializeField] private TextMeshProUGUI text_Name_Passive2;
    [SerializeField] private TextMeshProUGUI text_Name_Passive3;
    [Space(10f)]
    // ##

    [SerializeField] private GameObject go_SubWindow_Passive;
    [SerializeField] private TextMeshProUGUI text_Name_SubWindow_Passive;
    [SerializeField] private TextMeshProUGUI text_Description_SubWindow_Passive;

    [SerializeField] private GameObject[] gos_SelectedMark_Passive;

    private CharacterData characterData;
    private CardData cardData;

    [SerializeField] private RectTransform rect_Image_Line;
    private float[] positions_LineImage;

    private int selectedPartyIndex_ForDrag;
    [SerializeField] private GameObject line_Forward;
    [SerializeField] private GameObject line_Backward;
    [SerializeField] private Image image_Face_Party_ForDrag;
    private bool isForward_ForDrag;

    [SerializeField] private RectTransform rect_Cards_Basic;
    [SerializeField] private RectTransform rect_Cards_Standard;
    [SerializeField] private GameObject go_Text_Title_Width_Cards_Basic;
    [SerializeField] private GameObject go_Text_Title_Height_Cards_Basic;
    [SerializeField] private GameObject go_Text_Title_Width_Cards_Standard;
    [SerializeField] private GameObject go_Text_Title_Height_Cards_Standard;

    private bool isCardsBasic;

    [SerializeField] private GameObject go_ScrollView_Basic;
    [SerializeField] private GameObject go_ScrollView_Standard;

    [SerializeField] private UI_Card_CharacterScene[] cards_Basic;
    [SerializeField] private UI_Card_CharacterScene[] cards_Standard;

    [SerializeField] private GameObject[] gos_Deck;
    private List<int> list_CardCode_Deck;

    [SerializeField] private TextMeshProUGUI text_Count_Card;
    private int count_Basic_Deck;

    private int count_Character_Party;

    [SerializeField] private GameObject go_SubWindow_Passive_Card;
    [SerializeField] private TextMeshProUGUI text_Name_SubWindow_Passive_Card;
    [SerializeField] private TextMeshProUGUI text_Description_SubWindow_Passive_Card;

    [SerializeField] private GameObject go_Window_SlotName;
    [SerializeField] private Text text_SlotName;

    private void Awake()
    {
        characterData = new CharacterData();

        cardData = new CardData();

        characters_Party = new Character_Party[4];

        positions_LineImage = new float[5];

        list_CardCode_Deck = new List<int>(); // 일단 통상 10장, 스탠다드 9장 기준
    }

    void Start()
    {
        Init_Main();
    }

    void Update()
    {
        
    }

    private void Init_Main() // 캐릭터 세팅 씬의 초기화를 담당하는 함수
    {
        for (int i = 0; i < 4; i++)
        {
            characters_Party[i] = new Character_Party();
            characters_Party[i].characterCode = -1;
            characters_Party[i].passive = -1;

            ui_Party[i].characterCode = -1;
        }

        characters_Party[0].characterCode = 1;

        ui_Party[0].characterCode = 1;

        positions_LineImage[0] = 400f;
        positions_LineImage[1] = 200f;
        positions_LineImage[2] = 0f;
        positions_LineImage[3] = -200f;
        positions_LineImage[4] = -400f;

        isCardsBasic = true;
    }
        
    private void Init_Card() // 카드 선택창의 초기 셋팅을 하는 함수
    {
        for (int i = 0; i < 4; i++)
            ui_Party_Card[i].gameObject.SetActive(false);

        for (int i = 0; i < 4; i++) // 카드 선택창의 좌측 파티의 모습을 이전 캐릭터 선택창에서 선택한 것을 기반으로 초기화시키는 반복문
        {
            if (ui_Party[i].characterCode != -1)
            {
                ui_Party_Card[i].gameObject.SetActive(true);

                ui_Party_Card[i].characterCode = ui_Party[i].characterCode;
                ui_Party_Card[i].image_Face.sprite = ui_Party[i].image_Face.sprite;
                ui_Party_Card[i].text_Name = ui_Party[i].text_Name;

                if (ui_Party_Card[i].characterCode == 1)
                    ui_Party_Card[i].text_Count_Card.text = ui_Party_Card[i].count_SelectedCard + " / 5";
                else
                    ui_Party_Card[i].text_Count_Card.text = ui_Party_Card[i].count_SelectedCard + " / 1";
            }
        }

        Init_Cards();
    }

    private void Init_Cards()
    {
        for (int i = 0; i < 4; i++) // 통상 카드의 정보를 할당하는 반복문
        {
            cards_Basic[i].ownerCode = 0;

            cards_Basic[i].image_BackGround.sprite = so_CardList.cardDetails[cards_Basic[i].cardCode].sprite_Card;
            cards_Basic[i].text_Cost.text = "" + so_CardList.cardDetails[cards_Basic[i].cardCode].cost;
            cards_Basic[i].text_Number.text = "" + so_CardList.cardDetails[cards_Basic[i].cardCode].code;
            cards_Basic[i].text_Name.text = "" + so_CardList.cardDetails[cards_Basic[i].cardCode].name;
            cards_Basic[i].text_Owner.text = "" + so_CardList.cardDetails[cards_Basic[i].cardCode].ownerCharacter;
            cards_Basic[i].text_Thema.text = "" + so_CardList.cardDetails[cards_Basic[i].cardCode].thema;
            cards_Basic[i].text_Description.text = "" + so_CardList.cardDetails[cards_Basic[i].cardCode].description_KR;

            cards_Basic[i].count = 5;
        }

        // 스탠다드 카드의 정보를 할당하는 코드
        int cursor = 0; // 스탠다드 카드 목록을 선택한 파티에 맞게 나열하기 위해 사용되는 변수

        for (int i = 0; i < characters_Party.Length; i++)
        {
            if (characters_Party[i].characterCode == 1) // Q
            {
                cards_Standard[cursor].cardCode = 5; cards_Standard[cursor].ownerCode = 1; cursor++;
                cards_Standard[cursor].cardCode = 6; cards_Standard[cursor].ownerCode = 1; cursor++;
                cards_Standard[cursor].cardCode = 7; cards_Standard[cursor].ownerCode = 1; cursor++;
            }
            else if (characters_Party[i].characterCode == 2) // 스파키
            {
                cards_Standard[cursor].cardCode = 10; cards_Standard[cursor].ownerCode = 2; cursor++;
                cards_Standard[cursor].cardCode = 11; cards_Standard[cursor].ownerCode = 2; cursor++;
            }
            else if (characters_Party[i].characterCode == 3) // 반가라
            {
                cards_Standard[cursor].cardCode = 8; cards_Standard[cursor].ownerCode = 3; cursor++;
                cards_Standard[cursor].cardCode = 9; cards_Standard[cursor].ownerCode = 3; cursor++;
            }
            else if (characters_Party[i].characterCode == 4) // 포르테
            {
                cards_Standard[cursor].cardCode = 12; cards_Standard[cursor].ownerCode = 4; cursor++;
                cards_Standard[cursor].cardCode = 13; cards_Standard[cursor].ownerCode = 4; cursor++;
            }
            else if (characters_Party[i].characterCode == 5) // 령
            {
                cards_Standard[cursor].cardCode = 27; cards_Standard[cursor].ownerCode = 5; cursor++;
                cards_Standard[cursor].cardCode = 28; cards_Standard[cursor].ownerCode = 5; cursor++;
            }
            else if (characters_Party[i].characterCode == 6) // 흉귀
            {
                cards_Standard[cursor].cardCode = 32; cards_Standard[cursor].ownerCode = 6; cursor++;
                cards_Standard[cursor].cardCode = 33; cards_Standard[cursor].ownerCode = 6; cursor++;
            }
        }

        for (int i = 0; i < cards_Standard.Length; i++)
            cards_Standard[i].gameObject.SetActive(false);

        for (int i = 0; i < cursor; i++)
        {
            cards_Standard[i].gameObject.SetActive(true);

            cards_Standard[i].image_BackGround.sprite = so_CardList.cardDetails[cards_Standard[i].cardCode].sprite_Card;
            cards_Standard[i].text_Cost.text = "" + so_CardList.cardDetails[cards_Standard[i].cardCode].cost;
            cards_Standard[i].text_Number.text = "" + so_CardList.cardDetails[cards_Standard[i].cardCode].code;
            cards_Standard[i].text_Name.text = "" + so_CardList.cardDetails[cards_Standard[i].cardCode].name;
            cards_Standard[i].text_Owner.text = "" + so_CardList.cardDetails[cards_Standard[i].cardCode].ownerCharacter;
            cards_Standard[i].text_Thema.text = "" + so_CardList.cardDetails[cards_Standard[i].cardCode].thema;
            cards_Standard[i].text_Description.text = "" + so_CardList.cardDetails[cards_Standard[i].cardCode].description_KR;

            if (cards_Standard[i].cardCode >= 5 && cards_Standard[i].cardCode <= 7)
                cards_Standard[i].count = 5;
            else
                cards_Standard[i].count = 1;
        }
    }

    public void Click_Revivor()
    {
        go_Window_SelectRevivor.SetActive(false);
        go_Window_SelectContractor.SetActive(true);
    }

    public void Click_Party(int partyCode) // 왼쪽 파티의 파티원 항목을 누르면 실행되는 함수
    {
        ui_Party[selectedPartyCode].go_Frame.SetActive(false);

        selectedPartyCode = partyCode;
        ui_Party[partyCode].go_Frame.SetActive(true);

        int selectedCharacterCode = ui_Party[partyCode].characterCode; // 선택된 파티원 항목에 들어 있는 캐릭터 코드를 구하는 코드

        if (selectedCharacterCode != -1) // 선택된 파티원 항목에 캐릭터가 존재한다면
        {
            for (int i = 0; i < 4; i++)
            {
                if (characters_Party[i].characterCode == selectedCharacterCode)
                    selectedCharacterIndex = i; // 해당 파티원의 정보를 담고 있는 배열 characters_Party에서의 인덱스를 구함
            }
        }
        else
            selectedCharacterIndex = partyCode;

        if (selectedCharacterCode == 1) // 부활자인 경우
        {
            go_Characters.SetActive(false);

            Select_Character(1);
        }
        else // 부활자가 아닌 경우
        {
            go_Characters.SetActive(true);

            if (selectedCharacterCode == -1)
                go_SelectedCharacter.SetActive(false);
            else
                Select_Character(selectedCharacterCode);
        }
    }

    public void Click_Select_Character(int characterCode) // 오른쪽 캐릭터 나열의 캐릭터를 누르면 실행되는 함수
    {
        for (int i = 0; i < 4; i++) // 현재 파티중
        {
            if (ui_Party[i].characterCode == characterCode) // 이미 존재하는 파티원과 동일한 캐릭터라면 리턴
                return;
        }

        characters_Party[selectedCharacterIndex].passive = -1;
        characters_Party[selectedCharacterIndex].characterCode = characterCode;

        Select_Character(characterCode);

        ui_Party[selectedPartyCode].characterCode = characterCode;
        ui_Party[selectedPartyCode].image_Face.sprite = so_CharacterList.characterDetails[characterCode].sprite_Face;
        ui_Party[selectedPartyCode].text_Name.text = so_CharacterList.characterDetails[characterCode].name;
    }

    public void Click_Line(bool isForward) // 전방, 후방 버튼 클릭 시 실행되는 함수
    {
        characters_Party[selectedCharacterIndex].isForward = isForward; // 현재 선택된 캐릭터의 전방, 후방을 변경하는 코드

        Arrange_Party();

        for (int i = 0; i < 4; i++) // Arrange_Party()가 끝난 뒤 기존에 선택되었던 캐릭터가 있었던 위치에 남아있는 프레임을 제거하고
            ui_Party[i].go_Frame.SetActive(false);

        for (int i = 0; i < 4; i++) // 새로 위치한 곳에 프레임을 활성화
        {
            if (ui_Party[i].characterCode == selectedCharacterCode)
            {
                ui_Party[i].go_Frame.SetActive(true);
                Click_Party(i);
            }
        }
    }

    public void Click_Passive(int code) // 패시브 선택 시 실행되는 함수
    {
        characters_Party[selectedCharacterIndex].passive = code;

        for (int i = 0; i < 4; i++)
            gos_SelectedMark_Passive[i].SetActive(false);

        gos_SelectedMark_Passive[code].SetActive(true);
    }

    public void Click_Complete_CharacterSetting() // 캐릭터 설정 창의 설정 완료 버튼 클릭 시 실행되는 함수
    {
        for (int i = 0; i < 4; i++)
        {
            if (characters_Party[i].characterCode != -1) // 파티원이 존재하는데
            {
                if (characters_Party[i].passive == -1) // 패시브가 선택되지 않았다면 return
                    return;
            }
        }

        bool isAllBackward = true;

        for (int i = 0; i < 4; i++)
        {
            if (characters_Party[i].characterCode != -1) // 파티원이 존재하는데
            {
                if (characters_Party[i].isForward) // 포지션이 전방인 파티원이 한명이라도 있다면 문제 없지만
                    isAllBackward = false;
            }
        }

        if (isAllBackward) // 모두 후방이라면 리턴
            return;

        characterData.size = 0;

        for (int i = 0; i < 4; i++)
        {
            if (characters_Party[i].characterCode == -1)
                break;

            int[] passive = new int[4];

            characterData.characterDatas[i].name = so_CharacterList.characterDetails[characters_Party[i].characterCode].name;
            characterData.characterDatas[i].code = characters_Party[i].characterCode;
            characterData.characterDatas[i].cost = so_CharacterList.characterDetails[characters_Party[i].characterCode].cost;
            characterData.characterDatas[i].atk = so_CharacterList.characterDetails[characters_Party[i].characterCode].attackPower;
            characterData.characterDatas[i].maxHp = so_CharacterList.characterDetails[characters_Party[i].characterCode].hp;
            characterData.characterDatas[i].curHp = so_CharacterList.characterDetails[characters_Party[i].characterCode].hp;
            characterData.characterDatas[i].endurance = so_CharacterList.characterDetails[characters_Party[i].characterCode].endurance;
            characterData.characterDatas[i].speed = so_CharacterList.characterDetails[characters_Party[i].characterCode].speed;
            characterData.characterDatas[i].passive = passive;
            characterData.characterDatas[i].passive[characters_Party[i].passive]++;
            if (characters_Party[i].isForward)
                characterData.characterDatas[i].curFormation = 0;
            else
                characterData.characterDatas[i].curFormation = 1;
            characterData.characterDatas[i].curEquip = -1;

            if (characterData.characterDatas[i].curFormation == 0)
                characterData.line++;
            characterData.size++;

            string characterDataForJson = JsonConvert.SerializeObject(characterData);
            string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CharacterDatas[GameManager.Instance.selectedSlot_Main]);
            File.WriteAllText(path, characterDataForJson);
        }

        go_Window_SelectContractor.SetActive(false);
        go_Window_SelectCard.SetActive(true);

        Init_Card();
    }

    public void Click_Tab_Cards() // 카드 설정 창의 통상 / 스탠다드 탭 클릭시 실행되는 함수
    {
        if (!isCardsBasic)
        {
            isCardsBasic = true;

            rect_Cards_Basic.anchoredPosition = new Vector2(340f, rect_Cards_Basic.anchoredPosition.y);
            rect_Cards_Standard.anchoredPosition = new Vector2(1510f, rect_Cards_Standard.anchoredPosition.y);

            go_Text_Title_Width_Cards_Basic.SetActive(true);
            go_Text_Title_Height_Cards_Basic.SetActive(false);

            go_Text_Title_Width_Cards_Standard.SetActive(false);
            go_Text_Title_Height_Cards_Standard.SetActive(true);

            go_ScrollView_Basic.SetActive(true);
            go_ScrollView_Standard.SetActive(false);
        }
        else
        {
            isCardsBasic = false;

            rect_Cards_Standard.anchoredPosition = new Vector2(420f, rect_Cards_Standard.anchoredPosition.y);

            go_Text_Title_Width_Cards_Standard.SetActive(true);
            go_Text_Title_Height_Cards_Standard.SetActive(false);

            go_Text_Title_Width_Cards_Basic.SetActive(false);
            go_Text_Title_Height_Cards_Basic.SetActive(true);

            go_ScrollView_Basic.SetActive(false);
            go_ScrollView_Standard.SetActive(true);
        }
    }

    public void Click_Card_Basic(int positionCode) // 통상 카드 클릭시 실행되는 함수
    {
        // 이미 통상 카드가 10장 이상 덱에 존재한다면 리턴
        if (count_Basic_Deck >= 10)
            return;

        if (cards_Basic[positionCode].count > 0)
        {
            bool isSuccess_Search = false;

            for (int i = 0; i < list_CardCode_Deck.Count; i++)
            {
                if (list_CardCode_Deck[i] == cards_Basic[positionCode].cardCode)
                {
                    list_CardCode_Deck.Insert(i, cards_Basic[positionCode].cardCode);
                    isSuccess_Search = true;

                    break;
                }
            }

            if (!isSuccess_Search)
                list_CardCode_Deck.Add(cards_Basic[positionCode].cardCode);

            cards_Basic[positionCode].count--;

            if (cards_Basic[positionCode].count == 0)
                cards_Basic[positionCode].image_BackGround.color = new Color(1f, 1f, 1f, 100 / 255f);

            count_Basic_Deck++;

            Reset_Deck();
        }
    }

    public void Click_Card_Standard(int positionCode) // 스탠다드 카드 클릭시 실행되는 함수
    {
        // 이미 스탠다드 카드가 해당 캐릭터의 최대 스탠다드 카드 소지 수 이상 덱에 존재한다면 리턴
        if (cards_Standard[positionCode].ownerCode == 1) // Q(Revivor)
        {
            for (int i = 0; i < 4; i++)
            {
                if (ui_Party_Card[i].characterCode == 1)
                {
                    if (ui_Party_Card[i].count_SelectedCard >= 5)
                        return;

                    break;
                }
            }
        }
        else if (cards_Standard[positionCode].ownerCode > 1) // Contractor
        {
            for (int i = 0; i < 4; i++)
            {
                if (ui_Party_Card[i].characterCode == cards_Standard[positionCode].ownerCode)
                {
                    if (ui_Party_Card[i].count_SelectedCard >= 1)
                        return;

                    break;
                }
            }
        }

        if (cards_Standard[positionCode].count > 0)
        {
            bool isSuccess_Search = false;

            for (int i = 0; i < list_CardCode_Deck.Count; i++)
            {
                if (list_CardCode_Deck[i] == cards_Standard[positionCode].cardCode)
                {
                    list_CardCode_Deck.Insert(i, cards_Standard[positionCode].cardCode);
                    isSuccess_Search = true;

                    break;
                }
            }

            if (!isSuccess_Search)
                list_CardCode_Deck.Add(cards_Standard[positionCode].cardCode);

            cards_Standard[positionCode].count--;

            if (cards_Standard[positionCode].count == 0)
                cards_Standard[positionCode].image_BackGround.color = new Color(1f, 1f, 1f, 100 / 255f);

            for (int i = 0; i < 4; i++)
            {
                if (ui_Party_Card[i].characterCode == cards_Standard[positionCode].ownerCode)
                {
                    ui_Party_Card[i].count_SelectedCard++;

                    if (ui_Party_Card[i].characterCode == 1)
                        ui_Party_Card[i].text_Count_Card.text = ui_Party_Card[i].count_SelectedCard + " / 5";
                    else if (ui_Party_Card[i].characterCode > 1)
                        ui_Party_Card[i].text_Count_Card.text = ui_Party_Card[i].count_SelectedCard + " / 1";

                    break;
                }
            }
            
            Reset_Deck();
        }
    }

    public void Click_Remove_Deck(int positionCode) // 덱에 있는 카드 클릭시 실행되는 함수
    {
        if (list_CardCode_Deck[positionCode] <= 4) // Basic Card
        {
            for (int i = 0; i < cards_Basic.Length; i++)
            {
                if (cards_Basic[i].cardCode == list_CardCode_Deck[positionCode])
                {
                    cards_Basic[i].count++;
                    count_Basic_Deck--;

                    if (cards_Basic[i].count == 1)
                        cards_Basic[i].image_BackGround.color = new Color(1f, 1f, 1f, 1f);

                    break;
                }
            }
        }
        else if (list_CardCode_Deck[positionCode] > 4) // Standard Card
        {
            for (int i = 0; i < cards_Standard.Length; i++)
            {
                if (cards_Standard[i].cardCode == list_CardCode_Deck[positionCode])
                {
                    cards_Standard[i].count++;

                    if ((int)so_CardList.cardDetails[list_CardCode_Deck[positionCode]].ownerCharacter == 1)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (ui_Party_Card[j].characterCode == 1)
                            {
                                ui_Party_Card[j].count_SelectedCard--;
                                ui_Party_Card[j].text_Count_Card.text = ui_Party_Card[j].count_SelectedCard + " / 5";

                                break;
                            }
                        }
                    }
                    else if ((int)so_CardList.cardDetails[list_CardCode_Deck[positionCode]].ownerCharacter > 1)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (ui_Party_Card[j].characterCode == (int)so_CardList.cardDetails[list_CardCode_Deck[positionCode]].ownerCharacter)
                            {
                                ui_Party_Card[j].count_SelectedCard--;
                                ui_Party_Card[j].text_Count_Card.text = ui_Party_Card[j].count_SelectedCard + " / 1";

                                break;
                            }
                        }
                    }

                    if (cards_Standard[i].count == 1)
                        cards_Standard[i].image_BackGround.color = new Color(1f, 1f, 1f, 1f);

                    break;
                }
            }
        }

        list_CardCode_Deck.RemoveAt(positionCode);
        
        Reset_Deck();
    }

    public void Click_Complete_CardSetting() // 카드 설정 창의 설정 완료 버튼 클릭 시 실행되는 함수
    {
        //if (list_CardCode_Deck.Count < 20) // 선택된 카드 수가 20장 미만일시 리턴
        //    return;

        for (int i = 0; i < list_CardCode_Deck.Count; i++) // 덱에 넣은 카드들을 순회하며
        {
            if (list_CardCode_Deck[i] <= 4) // 통상 카드일시 카드 데이터에 추가
            {
                cardData.cardCode.Add(list_CardCode_Deck[i]);
                cardData.cardCost.Add(so_CardList.cardDetails[list_CardCode_Deck[i]].cost);
                cardData.cardGetOrder.Add(cardData.count);
                cardData.count++;
            }
        }

        for (int i = 0; i < list_CardCode_Deck.Count; i++) // 다시 덱에 넣은 카드들을 순회하며
        {
            if (list_CardCode_Deck[i] > 4) // 스탠다드 카드일시 카드 데이터에 추가
            {
                cardData.cardCode.Add(list_CardCode_Deck[i]);
                cardData.cardCost.Add(so_CardList.cardDetails[list_CardCode_Deck[i]].cost);
                cardData.cardGetOrder.Add(cardData.count);
                cardData.count++;
            }
        }

        // 위 작업에 따라 카드 데이터의 앞쪽에는 통상카드, 뒤쪽에는 스탠다드 카드가 위치하게 됨

        string cardDataForJson = JsonConvert.SerializeObject(cardData);
        string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CardDatas[GameManager.Instance.selectedSlot_Main]);
        File.WriteAllText(path, cardDataForJson);

        go_Window_SlotName.SetActive(true);
    }

    public void Click_Button_OK_SlotName()
    {
        if (text_SlotName.Equals(""))
            return;

        GameManager.Instance.slot_Names[GameManager.Instance.selectedSlot_Main] = text_SlotName.text;
        GameManager.Instance.Save();
        Debug.Log("슬롯의 이름이 " + text_SlotName.text + "(으)로 설정되었습니다.");

        StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene3_Lobby"));
    }

    private void Select_Character(int characterCode) // 선택한 캐릭터의 세부정보를 나타내는 함수
    {
        selectedCharacterCode = characterCode;

        go_SelectedCharacter.SetActive(true);

        text_Name_SelectedCharacter.text = so_CharacterList.characterDetails[characterCode].name;
        text_Aka_SelectedCharacter.text = so_CharacterList.characterDetails[characterCode].aka;

        text_Hp.text = so_CharacterList.characterDetails[characterCode].hp.ToString();
        text_AttackPower.text = so_CharacterList.characterDetails[characterCode].attackPower.ToString();
        text_Cost.text = so_CharacterList.characterDetails[characterCode].cost.ToString();
        text_Endurance.text = so_CharacterList.characterDetails[characterCode].endurance.ToString();
        text_InventorySize.text = so_CharacterList.characterDetails[characterCode].inventorySize.ToString();
        text_Speed.text = so_CharacterList.characterDetails[characterCode].speed.ToString();

        if (so_CharacterList.characterDetails[characterCode].isReviver)
            text_CardCount_EnableSelect.text = "5";
        else
            text_CardCount_EnableSelect.text = "1";

        text_Name_Passive0.text = so_CharacterList.characterDetails[characterCode].passive0_Name.ToString();
        text_Name_Passive1.text = so_CharacterList.characterDetails[characterCode].passive1_Name.ToString();
        text_Name_Passive2.text = so_CharacterList.characterDetails[characterCode].passive2_Name.ToString();
        text_Name_Passive3.text = so_CharacterList.characterDetails[characterCode].passive3_Name.ToString();

        for (int i = 0; i < 4; i++)
            gos_SelectedMark_Passive[i].SetActive(false);

        if (characters_Party[selectedCharacterIndex].passive != -1)
            gos_SelectedMark_Passive[characters_Party[selectedCharacterIndex].passive].SetActive(true);
    }

    private void Arrange_Party() // 현재 파티원들의 전방, 후방에 맞게 보여지는 UI상 파티 순서를 변경하고 전방, 후방을 나누는 라인을 긋는 함수
    {
        int ui_Party_Index = 0;

        for (int i = 0; i < 4; i++) // 현재 파티원들을 탐색하며 전방인 파티원을 UI상 파티 순서의 앞쪽에 놓는 반복문
        {
            if (characters_Party[i].characterCode != -1)
            {
                if (characters_Party[i].isForward)
                {
                    ui_Party[ui_Party_Index].characterCode = characters_Party[i].characterCode;
                    ui_Party[ui_Party_Index].text_Name.text = so_CharacterList.characterDetails[characters_Party[i].characterCode].name;
                    ui_Party[ui_Party_Index].image_Face.sprite = so_CharacterList.characterDetails[characters_Party[i].characterCode].sprite_Face;

                    ui_Party_Index++;
                }
            }
        }

        for (int i = 0; i < 4; i++) // 현재 파티원들을 탐색하며 후방인 파티원을 UI상 파티 순서의 뒤쪽에 놓는 반복문
        {
            if (characters_Party[i].characterCode != -1)
            {
                if (!characters_Party[i].isForward)
                {
                    ui_Party[ui_Party_Index].characterCode = characters_Party[i].characterCode;
                    ui_Party[ui_Party_Index].text_Name.text = so_CharacterList.characterDetails[characters_Party[i].characterCode].name;
                    ui_Party[ui_Party_Index].image_Face.sprite = so_CharacterList.characterDetails[characters_Party[i].characterCode].sprite_Face;

                    ui_Party_Index++;
                }
            }
        }

        int line = 0;

        for (int i = 0; i < 4; i++) // 현재 파티원들을 탐색하며 라인의 위치를 결정하는 반복문
        {
            if (characters_Party[i].isForward)
                line++;
        }

        rect_Image_Line.anchoredPosition = new Vector2(rect_Image_Line.anchoredPosition.x, positions_LineImage[line]); // 결정된 라인의 위치에 라인을 긋는 코드
    }

    private void Reset_Deck()
    {
        for (int i = 0; i < gos_Deck.Length; i++)
        {
            gos_Deck[i].SetActive(false);
            gos_Deck[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

        for (int i = 0; i < list_CardCode_Deck.Count; i++)
        {
            gos_Deck[i].SetActive(true);
            gos_Deck[i].GetComponentInChildren<TextMeshProUGUI>().text = so_CardList.cardDetails[list_CardCode_Deck[i]].name;
        }

        text_Count_Card.text = "" + list_CardCode_Deck.Count;
    }

    // ******* Method For Event Trigger *******

    public void PointerEnter_Passive(int code) // 패시브 이름 위에 포인터를 진입시킬 때 실행되는 함수
    {
        go_SubWindow_Passive.SetActive(true);

        if (code == 0)
        {
            text_Name_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive0_Name.ToString();
            text_Description_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive0_Description.ToString();
        }
        else if (code == 1)
        {
            text_Name_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive1_Name.ToString();
            text_Description_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive1_Description.ToString();
        }
        else if (code == 2)
        {
            text_Name_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive2_Name.ToString();
            text_Description_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive2_Description.ToString();
        }
        else if (code == 3)
        {
            text_Name_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive3_Name.ToString();
            text_Description_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive3_Description.ToString();
        }
    }

    public void PointerExit_Passive() // 패시브 이름 위의 포인터를 퇴출시킬 때 실행되는 함수
    {
        go_SubWindow_Passive.SetActive(false);
    }

    public void PointerEnter_Passive_Card(int positionCode)
    {
        go_SubWindow_Passive_Card.SetActive(true);

        int passiveCode = 0;

        for (int i = 0; i < 4; i++)
        {
            if (ui_Party[positionCode].characterCode == characters_Party[i].characterCode)
                passiveCode = characters_Party[i].passive + 1;
        }
        
        text_Name_SubWindow_Passive_Card.text = DataHeap.Instance.passiveName[(ui_Party[positionCode].characterCode - 1) * 4 + passiveCode];
        text_Description_SubWindow_Passive_Card.text = DataHeap.Instance.passiveDescription[(ui_Party[positionCode].characterCode - 1) * 4 + passiveCode];
    }

    public void PointerExit_Passive_Card() // 패시브 이름 위의 포인터를 퇴출시킬 때 실행되는 함수
    {
        go_SubWindow_Passive_Card.SetActive(false);
    }

    public void BeginDrag_Party(int index) // 파티원을 드래그 시작할 때 실행되는 함수
    {
        selectedPartyIndex_ForDrag = index;

        if (ui_Party[index].characterCode != -1) // 드래그한 파티원 항목이 비어 있지 않으면 아래 코드 실행
        {
            image_Face_Party_ForDrag.sprite = so_CharacterList.characterDetails[ui_Party[index].characterCode].sprite_Face;

            image_Face_Party_ForDrag.gameObject.SetActive(true);
        }
    }

    public void Drag_Party() // 파티원을 드래그 중일 때 실행되는 함수
    {
        if (ui_Party[selectedPartyIndex_ForDrag].characterCode != -1) // 드래그한 파티원 항목이 비어 있지 않으면 아래 코드 실행
        {
            Vector3 mouseCursor = Input.mousePosition - new Vector3(20, 580);

            image_Face_Party_ForDrag.gameObject.GetComponent<RectTransform>().localPosition = mouseCursor;

            if (mouseCursor.y > 0)
            {
                line_Forward.SetActive(true);
                line_Backward.SetActive(false);

                isForward_ForDrag = true;
            }
            else
            {
                line_Forward.SetActive(false);
                line_Backward.SetActive(true);

                isForward_ForDrag = false;
            }
        }
    }

    public void EndDrag_Party() // 파티원을 드래그 종료할 때 실행되는 함수
    {
        if (ui_Party[selectedPartyIndex_ForDrag].characterCode != -1) // 드래그한 파티원 항목이 비어 있지 않으면 아래 코드 실행
        {
            int selectedCharacterCode = ui_Party[selectedPartyIndex_ForDrag].characterCode;
            int index = -1;

            for (int i = 0; i < 4; i++)
            {
                if (characters_Party[i].characterCode == selectedCharacterCode)
                    index = i;
            }

            if (isForward_ForDrag) // 드래그 종료한 위치가 전방인데
            {
                if (!characters_Party[index].isForward) // 드래그한 파티원의 위치가 후방이라면 전방으로 교체
                {
                    characters_Party[index].isForward = true;
                    Arrange_Party();
                }
            }
            else // 드래그 종료한 위치가 후방인데
            {
                if (characters_Party[index].isForward) // 드래그한 파티원의 위치가 전방이라면 후방으로 교체
                {
                    characters_Party[index].isForward = false;
                    Arrange_Party();
                }
            }

            line_Forward.SetActive(false);
            line_Backward.SetActive(false);

            image_Face_Party_ForDrag.gameObject.SetActive(false);

            go_SelectedCharacter.SetActive(false);
            go_Characters.SetActive(false);
        }
    }
}

public class Character_Party
{
    public int characterCode;
    public bool isForward;
    public int passive;
}

public class CharacterData // json으로 저장 될 캐릭터 정보
{
    public struct curCharacterData
    {
        public string name;
        public int code;
        public int cost;
        public int atk;
        public int maxHp;
        public int curHp;
        public int endurance;
        public float speed;
        public int[] passive;
        public int curFormation; // 0:전방, 1:후방
        public int curEquip;
        public int trainStack;
        public curCharacterData(string name, int code, int cost, int atk, int endurance, float speed, int maxHp, int curHp, int[] passive, int curFormation, int curEquip)
        {
            this.name = name;
            this.code = code;
            this.cost = cost;
            this.atk = atk;
            this.maxHp = maxHp;
            this.curHp = curHp;
            this.endurance = endurance;
            this.speed = speed;
            this.passive = passive;
            this.curFormation = curFormation;
            this.curEquip = curEquip;
            trainStack = 0;
        }
    }

    public curCharacterData[] characterDatas = new curCharacterData[4];
    public int line; // 전, 후방을 나누는 기준. 0:전부 후방, 1:전방 하나, 2:전방 둘, 3:전방 셋, 4:전부 전방
    public int size; // 파티의 캐릭터 수
}

public class CardData
{
    public List<int> cardCode = new List<int>(); // 소유한 카드들의 코드 리스트
    public List<int> cardCost = new List<int>(); // 소유한 카드들의 코스트 리스트
    public List<int> cardGetOrder = new List<int>(); // 소유한 카드들의 획득 순서 리스트
    public int count; // 소유한 카드의 총 갯수
}
