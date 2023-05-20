using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject enemyPrefab; // YH

    [SerializeField] private GameObject[] EnemyList;
    public List<Character> characters = new List<Character>(); // 현재 게임에 있는 캐릭터들의 목록,순서또한 동일
    public bool characterSelectMode; // 캐릭터를 고를 수 있는 상태
    public bool enemySelectMode; // 적을 고를 수 있는 상태
    public Character actCharacter; // 현재 행동하는 캐릭터
    public Enemy selectedEnemy; // 현재 지정된 적
    public Character selectedCharacter; // 현재 선택된 캐릭터
    public GameObject selectedCard; // 현재 지정된 카드

    public GameObject previousSelectedCard;//바로 이전에 사용한 카드
    public Enemy previousEnemy;
    public Character previousCharacter;


    public TextMeshProUGUI costT;
    public int leftCost;//남은 코스트
    public int nextTurnStartCost;//다음 턴 시작 시 코스트를 올려주는 변수

    public int CardCount;//턴이 시작 될 때마다 뽑는 카드 수
    public int TurnCardCount;//다음 턴 시작 시 뽑게 될 카드 수

    [SerializeField] CardManager CM;
    public ActManager AM;

    public GameObject[] Enemys;//적들

    [SerializeField] GameObject warnObj;
    public Text warnT;
    public List<Character> forward = new List<Character>();//전방에 있는 캐릭터들 목록
    public List<Character> back = new List<Character>();//후방에 있는 캐릭터들 목록
    public int line;//전방,후방을 나누는 선
    public int teamDieCount;//죽은 아군의 수

    [SerializeField] GameObject completeButton;

    public bool otherCanvasOn;//다른 캔버스(무덤이나 로그,덱 등)이 켜져 있을 때
    public Log log;

    [SerializeField] GameObject window_Grave; // YH
    public int reviveCount;
    public bool card7mode;
    bool card12On;

    public GameData GD;
    public CharacterData ChD;

    [SerializeField] GameObject victory_window; //승리 시 나오는 팝업 창
    [SerializeField] GameObject defeated_window; //패배 시 나오는 팝업 창

    public bool cardSelectMode; //스타키티시모등 카드 선택 해야 할 때
    public bool porte3mode;//스타키티시모가 켜져 있을 때

    [SerializeField] GameObject condition;

    public Text conditionText;

    public bool GraveReviveMode;

    [SerializeField] Text StackT;
    [SerializeField] GameObject StackPopUp;

    [HideInInspector] public bool isGraveWindowOn; // YH
    public bool card20Activing;
    [SerializeField] GameObject go_GraveView_Button_Revive; // YH
    public bool CancleReviveMode;

    [SerializeField] GameObject window_Deck; // YH
    [HideInInspector] public bool isDeckWindowOn; // YH
    [SerializeField] GameObject go_DeckView_Button_OK; // YH
    [SerializeField] GameObject go_DeckView_Button_Cancel; // YH

    public GameObject DeckView;
    public GameObject SelectedCard;
    [SerializeField] GameObject LineObject;

    [SerializeField] GameObject FormationCollapsePopup;
    [SerializeField] Text FormationCollapseText;
    [SerializeField] GameObject[] FormationCollapseButton;
    [SerializeField] Text[] FormationCollapseButtonText;
    bool MoveToForward; //진형 붕괴 시 후방에서 전방으로 가는 상황이면 true,아니면 false

    public int porte3count;//포르테 패시브의 중첩 갯수

    public RectTransform rect_HandCanvas;
    HandManager HM;
    [SerializeField] private GameObject go_Menus;

    List<Character> characterOriginal = new List<Character>();

    [SerializeField] TextMeshProUGUI rewardIgnum;
    [SerializeField] TextMeshProUGUI rewardTribute;
    [SerializeField] GameObject RewardCanvas;
    public bool isVictoryPopupOn;//승리 팝업이 켜져있을 때
    [SerializeField] GameObject rewardCardPrefebs;
    public int selectedRewardCount;
    [SerializeField] GameObject noSelectCard;
    int rewardListlength = 3;
    List<int> RandomCardList = new List<int>();

    List<int> RancomSelectCard = new List<int>();
    [SerializeField] TextMeshProUGUI[] RewardEquipmentString;
    //승리 시 보상 선택하는 창에 들어갈 변수들

    [SerializeField] GameObject go_Warning_LessSelectedCard_DeckSelectMode; // 덱 선택 모드에서 확인 버튼을 클릭 했을 시 선택된 카드가 부족하면 실행되는 경고창 오브젝트

    public bool ReviveMode;
    public int SelectDeckCount;//덱에서 선택 할 카드 수
    public bool DeckSelectMode;
    public bool DeckSelect;
    public bool DeckSelectCancle;
    //덱에서 카드 고를 때 필요한 변수들

    public GameObject usedInCard20;//스케치 반복용

    public bool otherCorIsRun; //코루틴이 동작 중일 때

    public bool turnStartIsRun;//턴 시작 코루틴이 동작 중일 때

    //YH
    [HideInInspector] public bool isPointerinHand = false;
    [HideInInspector] public bool isSelectedCardinHand = false;
    private EnemyInfo ei;

    public GameObject DmgPrefebs;//데미지 프리펩

    public Card curSelectedCardInRevive;

    private void Start()
    {
        ei = GameObject.Find("SelectEnemyInformation").GetComponent<EnemyInfo>();
        string path = Path.Combine(Application.persistentDataPath, "GameData.json");
        string gameData = File.ReadAllText(path);
        GD = JsonConvert.DeserializeObject<GameData>(gameData);
        path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CharacterDatas[GameManager.Instance.nowPlayingSlot]);
        string characterData = File.ReadAllText(path);
        ChD = JsonConvert.DeserializeObject<CharacterData>(characterData);
        line = ChD.line;

        SetCharacterOnBattle();
        SetEnemyOnBattle();
        GameObject EnemySummon = Instantiate(EnemyList[GD.victory], new Vector2(-2, -2), transform.rotation, GameObject.Find("CharacterCanvas").transform);
        
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        TurnCardCount = CardCount;

        LineObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-820, 360 - 150 * line);
        HM = GameObject.Find("HandManager").GetComponent<HandManager>();
        if (GD.blessbool[20])
        {
            SetBless20();
        }
    }

    public void SetCharacterOnBattle()
    {
        for (int i = 0; i < ChD.size; i++)
        {        
            GameObject CharacterC = Instantiate(characterPrefab, new Vector2(-880 / 45f, (330 - 150 * characters.Count) / 45f), transform.rotation, GameObject.Find("CharacterCanvas").transform);
            Character CharacterComponenet = CharacterC.GetComponent<Character>();
            characterOriginal.Add(CharacterComponenet);
            CharacterComponenet.atk = ChD.characterDatas[i].atk;
            CharacterComponenet.Hp = ChD.characterDatas[i].curHp;
            CharacterComponenet.maxHp = ChD.characterDatas[i].maxHp;
            CharacterComponenet.def = ChD.characterDatas[i].endurance;
            CharacterComponenet.cost = ChD.characterDatas[i].cost;
            CharacterComponenet.passive = ChD.characterDatas[i].passive;
            CharacterComponenet.Name = ChD.characterDatas[i].name;
            CharacterComponenet.speed = ChD.characterDatas[i].speed;
            CharacterComponenet.characterNo = ChD.characterDatas[i].code;
            //캐릭터들의 기본 스탯을 데이터와 같게 설정
            CharacterComponenet.lobbyNum = i; //로비에 있는 순서대로 불러오기 떄문에 미리 저장
            CharacterComponenet.SetCharacterInPrefebs();

            if (ChD.characterDatas[i].curFormation == 0) //전방
            {

                forward.Add(CharacterComponenet);
            }
            else //후방
            {

                back.Add(CharacterComponenet);
            } //전방과 후방 목록에 각각 캐릭터를 넣음
            CharacterComponenet.AfterInstantiate();
        }

        for (int i = 0; i < line; i++)
        {
            forward[i].transform.position = new Vector2(-800 / 45f, (300 - 150 * characters.Count) / 45f);
            characters.Add(forward[i]);
        }//위치가 전방인 캐릭터들을 목록에 넣음
        for (int i = line; i < ChD.size; i++)
        {
            back[i - line].transform.position = new Vector2(-800 / 45f, (270 - 150 * characters.Count) / 45f);
            characters.Add(back[i - line]);
        }//위치가 후방인 캐릭터들을 목록에 넣음
        for (int i = 1; i < ChD.size; i++)
        {
            characters[i].curNo = i;
        }
    }

    public void SetEnemyOnBattle() // YH
    {
        int enemyCount = 0;

        switch (GD.BattleNo)
        {
            case 0: enemyCount = 2; break;
            case 1: enemyCount = 2; break;
            case 2: enemyCount = 2; break;
            case 3: enemyCount = 1; break;
            case 4: enemyCount = 4; break;
            case 5: enemyCount = 1; break;
            case 6: enemyCount = 2; break;
            case 7: enemyCount = 2; break;
            case 8: enemyCount = 1; break;
            default: break;
        }

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject EnemyC =
                Instantiate(enemyPrefab, new Vector2(840 / 45f, (330 - 150 * i) / 45f), transform.rotation, GameObject.Find("CharacterCanvas").transform);
            UI_Enemy_Battle enemyComponenet = EnemyC.GetComponent<UI_Enemy_Battle>();

            //enemyComponenet.image_Face = ;
            //enemyComponenet.text_Name = ;
            //enemyComponenet.text_Hp = ;
            //enemyComponenet.text_Attack = ;
            //enemyComponenet.text_Speed = ;
        }
    }

    public void SetBless20() //bless20이 켜져있다면 적용 될 함수
    {
        for (int i = 0; i < forward.Count; i++)
        {
            forward[i].AtkUp(-1);
        }
        for (int i = 0; i < back.Count; i++)
        {
            back[i].AtkUp(1);
        }
    }
    public void ResetBless20() //원래 공격력으로 돌려놓는 함수
    {
        for (int i = 0; i < forward.Count; i++)
        {
            forward[i].AtkUp(1);
        }
        for (int i = 0; i < back.Count; i++)
        {
            back[i].AtkUp(-1);
        }
    }
    public void costUp(int amount) //현재 코스트를 올려주는 함수
    {
        leftCost += amount;
        costT.text = "" + leftCost;
    }
    public void FormationCollapse()
    {
        if (forward.Count < back.Count)
        {
            RandomBackGoForward();
        }
        else if (forward.Count > back.Count)
        {
            RandomForwardGoBack();
        }
        else
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) RandomBackGoForward();
            else RandomForwardGoBack();
        }

        line = ChD.line;
        for (int i = 0; i < line; i++)
        {
            forward[i].transform.position = new Vector2(-600 / 45f, (300 - 150 * i) / 45f);
        }
        for (int i = 0; i < ChD.size - line; i++)
        {
            back[i].transform.position = new Vector2(-600 / 45f, (270 - 150 * (i + line)) / 45f);
        }
        for (int i = 1; i < ChD.size; i++)
        {
            characters[i].curNo = i;
        }
        LineObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-820, 360 - 150 * line);
    }
    public void RandomForwardGoBack()
    {
        ChD.line--;
        List<int> forwardInt = new List<int>();
        for (int i = 0; i < ChD.size; i++)
        {
            if (ChD.characterDatas[i].curFormation == 0)
            {
                forwardInt.Add(i);
            }
        }
        int rand = Random.Range(0, forwardInt.Count);
        ChD.characterDatas[forwardInt[rand]].curFormation = 1;
        int key = -1;
        for (int i = 0; i < forward.Count; i++)
        {
            if (forward[i].characterNo == ChD.characterDatas[forwardInt[rand]].code)
            {
                key = i;
            }
        }
        Debug.Log(rand);
        Debug.Log(forwardInt.Count);
        Debug.Log(ChD.characterDatas[forwardInt[rand]].code);
        if (key != -1)
        {
            back.Add(forward[key]);
            forward.RemoveAt(key);
        }
    }
    public void RandomBackGoForward()
    {
        ChD.line++;
        List<int> backInt = new List<int>();
        for (int i = 0; i < ChD.size; i++)
        {
            if (ChD.characterDatas[i].curFormation == 1)
            {
                backInt.Add(i);
            }
        }
        int rand = Random.Range(0, backInt.Count);
        ChD.characterDatas[backInt[rand]].curFormation = 0;
        int key = -1;
        for (int i = 0; i < back.Count; i++)
        {
            if (back[i].characterNo == ChD.characterDatas[backInt[rand]].code)
            {
                key = i;
            }
        }
        if (key != -1)
        {
            forward.Add(back[key]);
            back.RemoveAt(key);
        }
    }

    public void Click_StackPopUpOn() //현재 스택을 보여주는 함수 (구현 더 해야 함)
    {
        if (StackPopUp.activeSelf)
        {
            otherCanvasOn = false;
            StackPopUp.SetActive(false);
        }
        else
        {
            StackT.text = "";
            otherCanvasOn = true;
            StackT.text += "이번 턴 사용한 카드 수:" + CM.TM.turnCard + "\n";
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].characterNo == 1)
                {
                    StackT.text += "망자:" + characters[i].GetComponent<CharacterPassive>().ghost + "\n";
                    break;
                }
            }

            StackPopUp.SetActive(true);
        }
    }
    public void Click_toMain() //메인화면으로 가기 위함
    {
        StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene1_Main"));
        //SceneManager.LoadScene("Scene1_Main"); //지금은 메인화면으로 갈 때 데이터를 저장하게 되는데, 승리시에만 하도록 추후에 변경해야함
    }

    public void Click_Menu()
    {
        if (go_Menus.activeSelf)
            go_Menus.SetActive(false);
        else
            go_Menus.SetActive(true);
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameObject.Find("ESC(Clone)"))//이 창은 하나만 띄우자
        {
            Instantiate(GameManager.Instance.EscViewPrefeb, GameObject.Find("NonGameCanvas").transform);
        }
    }

    public void Porte3On() //스타키티시모 on
    {
        porte3mode=true;
        condition.SetActive(true);
        conditionText.text = "스타키티시모";
        cardSelectMode = true;
    }
    public void Click_Complete()//카드를 고르는 기능용
    {
        condition.SetActive(false);
        cardSelectMode = false;
        completeButton.SetActive(false);
        if (porte3mode) //현재 상태가 스타키티시모 라면
        {

            if (selectedCard != null)
            {
               
            }
            else //아무 카드도 선택이 되어있지 않을 때
            {
                condition.SetActive(true);
                cardSelectMode = true;
                completeButton.SetActive(true);
                warnT.text = "스타카티시모:카드 선택을 해주세요.";
                WarnOn();
            }
        }
    }
    public void CancleCharacter() //발동->캐릭터를 두번 눌럿을때 or 눌러진 상태에서 다른 캐릭터를 눌렀을때
    {
        if (!otherCanvasOn)
        {

            if (actCharacter != null)//이미 눌러진 캐릭터가 있다면
                actCharacter.SelectBox.SetActive(false); //선택이 되었다는 표시를 없앤다.

            actCharacter = null;
        }
    }
    public void ShowCharacterHaveTurn(GameObject characterHaveTurn)
    {
        actCharacter = characterHaveTurn.GetComponent<Character>();

        actCharacter.myPassive.ActStart();
        AM.isStartAct = true;
        AM.Act();

        if (actCharacter != null)
            actCharacter.SelectBox.SetActive(true);

    }
    public void useCost(int amount, GameObject card) //코스트 사용 함수
    {
        if (actCharacter.characterNo == 5 && actCharacter.passive[3] > 0)
        {
            leftCost -= 1;
            card.GetComponent<Card>().GetRemove();
        }
        else
            leftCost -= amount;
        costT.text = "" + leftCost;
    }
    public void SpeedChange(Character character, float amount)
    {
        character.speed += amount;
        character.curSpeed += amount;
        if(character.speed < 2)
        {
            character.speed=2;
        }
        if(character.curSpeed < 2)
        {
            character.curSpeed= 2;
        }
        character.OnSpeedText(amount);

        AM.SpeedChangeByEffect(0, character.curNo);
    }
    public void TurnSpeedChange(Character character, float amount)
    {
        character.curSpeed += amount;
        character.OnSpeedText(amount);

        AM.SpeedChangeByEffect(0, character.curNo);
    }
    public void EnemySelect(GameObject SelectedEnemyInSelectMode) //공격 카드가 발동되었을 시 적을 선택
    {
        if (CM.TM.turn == 1)
        {
            if (GD.blessbool[4] || GD.blessbool[12])
                return;
        }
        if (!otherCanvasOn)
        {
            if (leftCost < selectedCard.GetComponent<Card>().cardcost)
            {
                HandManager.Instance.CancelToUse();
                costOver();
                return;
            }
            selectedEnemy = SelectedEnemyInSelectMode.GetComponent<Enemy>();
            if (!selectedEnemy.Shadow)//은신일 때는 카드 지정 x
                selectedCard.GetComponent<Card>().EnemySelectCard();
        }
    }
    public void CharacterSelect(GameObject SelectedCharacterInSelectMode) //아군 선택 카드가 발동되었을 시 아군을 선택
    {
        if (!otherCanvasOn)
        {
            selectedCharacter = SelectedCharacterInSelectMode.GetComponent<Character>();
            selectedCard.GetComponent<Card>().CharacterSelectCard();
        }
    }

    public void CancleEnemy() //현재 지정된 적을 풀어야 할 때 사용 (공격 카드를 발동 한 후 등)
    {
        if (!otherCanvasOn)
        {
            selectedEnemy = null;
        }
    }
    public void SetCard(GameObject c) //카드를 눌렀을 때
    {
        if (cardSelectMode)
        {
            cancleCard();
            //HandManager.Instance.go_UseButton.SetActive(true);
            selectedCard = c;

            Debug.Log("Set Card a" + selectedCard.GetComponent<Card>().selectType.ToString());
            return;
        }

        if (!otherCanvasOn)
        {
            cancleCard();

            //HandManager.Instance.go_UseButton.SetActive(true);

            selectedCard = c;

            Debug.Log("Set Card b" + selectedCard.GetComponent<Card>().selectType.ToString());
        }
    }
    public void cancleCard() //카드를 두번 누르거나, 다른 카드를 눌렀을때 이미 눌러진 카드에게 적용되는 함수
    {
        if (!otherCanvasOn)
        {
            enemySelectMode = false;

            selectedCard = null;

            //HandManager.Instance.go_UseButton.SetActive(false);

            if (!isPointerinHand)
                HandManager.Instance.SelectCardToOriginPosition();
        }
    }

    public void Click_useCard() // 카드 드래그 상태에서 아무곳이나(또는 적,아군) 클릭시 사용되는 함수
    {
        if (porte3mode) //일반 카드 사용보다 먼저 처리
        {
            selectedCard.transform.localScale = new Vector3(1, 1, 1);
            CM.FieldToDeck(selectedCard);//선택한 카드를 덱에 보내고
            CM.CardToField();//랜덤한 카드를 필드 가장 오른쪽으로 보냄
            CM.field[CM.field.Count - 1].GetComponent<Card>().cardcost -= 2;//필드의 가장 오른쪽 카드의 코스트를 2감소시킴
            if (CM.field[CM.field.Count - 1].GetComponent<Card>().cardcost < 0)
                CM.field[CM.field.Count - 1].GetComponent<Card>().cardcost = 0;
            CM.field[CM.field.Count - 1].GetComponent<Card>().costT.text = CM.field[CM.field.Count - 1].GetComponent<Card>().cardcost + "";
            log.logContent.text += "\n스타카티시모!" + CM.field[CM.field.Count - 1].GetComponent<Card>().Name.text + "의 코스트가 감소하였습니다.";
           
            porte3count--;
            HandManager.Instance.Porte3DO();
            if (porte3count == 0) //포르테3의 중첩이 다 끝나면
            {
                condition.SetActive(false);
                porte3mode = false;
            }
            else
            {
                Porte3On();
            }
        }
        else
        {
            //HandManager.Instance.go_SelectedCardTooltip.SetActive(false);
            if (selectedCard.GetComponent<Card>().selectType != 1)
            {
                if (selectedCard != null)
                {
                    selectedCard.GetComponent<Card>().useCard();
                    //HandManager.Instance.CancelToUse(); 카드 사용 시 발동되게 옮깁니다.
                }
            }
            else if (selectedCard.GetComponent<Card>().selectType == 1)
            {

                if (actCharacter.passive[0] > 0 && actCharacter.characterNo == 6)
                {
                    GameObject randomEnemy = Enemys[Random.Range(0, Enemys.Length)];
                    while (randomEnemy.GetComponent<Enemy>().isDie)
                    {
                        randomEnemy = Enemys[Random.Range(0, Enemys.Length)];
                    }
                    EnemySelect(randomEnemy);
                }
                else if (ei.SelectedEnemy != null)
                {
                    EnemySelect(ei.SelectedEnemy.gameObject);


                }
                enemySelectMode = false;
            }
            else if (selectedCard.GetComponent<Card>().selectType == 5)
            {
                /* if (usedInCard20 != null)
                 {
                     Destroy(selectedCard);
                     selectedCard = usedInCard20;
                     otherCanvasOn = false;
                 }*/
                // CharacterSelectMode = false;
            }
        }
    }

    public void WarnOn()//여러가지 경고 처리
    {
        warnObj.SetActive(true);

        Invoke("TargetOff", 1f);
    }
    public void overAct()
    {
        warnT.text = "캐릭터의 행동력이 없습니다.";
        WarnOn();
    }
    public void TargetOn()
    {
        cancleCard();
        CancleEnemy();
        warnObj.SetActive(true);
        warnT.text = "타겟 설정을 다시 해주세요";
        Invoke("TargetOff", 1f);
    }
    public void allClear()//지정된 카드,적,캐릭터 모두 null로 만듬(턴 종료시 발동)
    {
        cancleCard();
        CancleCharacter();
        CancleEnemy();
    }
    public void TargetOff()
    {
        warnObj.SetActive(false);
    }
    public void costOver()
    {
        HandManager.Instance.SelectCardToOriginPosition();
        HandManager.Instance.isSelectedCard = false;
        cancleCard();
        warnObj.SetActive(true);
        warnT.text = "코스트가 부족합니다";
        Invoke("costOverOff", 1f);
    }
    void costOverOff()
    {
        warnObj.SetActive(false);
    }
    public void OnDmgOneTarget(int dmg, Enemy enemy, Character character, int time) //카드를 사용해 데미지를 입힐 경우
    {
        enemySelectMode = false;
        //log.logContent.text += "\n" + enemy.Name + "에게 " + (dmg + character.turnAtk) + "의 데미지!("+time+")";
        for (int k = 0; k < time; k++)
        {
            Debug.Log(character + "" + enemy);
            enemy.OnHitCal(dmg + character.turnAtk, character.curNo, false);
        }
    }

    public void OnAttack(int dmg, Enemy enemy, Character character, int time) //패시브나 리플렉트로 인하여 데미지 입힐 경우
    {
        for (int k = 0; k < time; k++)
        {
            enemy.OnHitCal(dmg, character.curNo, false);
        }
    }
    public void OnRandomAttack(int dmg, Character character, int time)
    {
        for (int k = 0; k < time; k++)
        {
            Enemy enemy = Enemys[Random.Range(0, Enemys.Length)].GetComponent<Enemy>();
            enemy.OnHitCal(dmg + +character.turnAtk, character.curNo, false);
        }
    }
    public void AllAttack(int dmg, Character character, int time) //흑백등을 이용해 데미지를 전체에게 입힐 경우
    {
        log.logContent.text += "\n적 전체에게 " + dmg + character.turnAtk + "의 데미지!(" + time + ")";
        for (int k = 0; k < time; k++)
        {
            for (int i = 0; i < Enemys.Length; i++)
            {
                Enemys[i].GetComponent<Enemy>().OnHitCal(dmg + character.turnAtk, character.curNo, false);

            }
        }

    }
    public void AllAttackOnPercent(int percent, Character character) //모든적에게 체력 퍼센트 데미지 부여
    {

        for (int i = 0; i < Enemys.Length; i++)
        {
            Enemy e = Enemys[i].GetComponent<Enemy>();
            e.OnHitCal(Mathf.FloorToInt(percent * e.Hp * 0.01f), character.curNo, false);
        }


    }


    public void getArmor(int armor, Character character) //방어도 획득
    {
        log.logContent.text += "\n" + character.Name + "이(가) " + armor + "의 방어도 획득!";
        character.getArmor(armor);
    }
    List<GameObject> specialDrowList = new List<GameObject>();
    public void specialDrow(int drow) //카드를 통한 드로우
    {
        log.logContent.text += "\n 카드를 통해 드로우 " + drow + "장!";
        int curDrow = 0;
        while (curDrow != drow)
        {
            curDrow++;
            if (CM.Deck.Count > 0)
            {

                int rand = Random.Range(0, CM.Deck.Count);
                CM.field.Add(CM.Deck[rand]);
                specialDrowList.Add(CM.Deck[rand]);
                CM.Deck.RemoveAt(rand);

            }
        }
        for (int i = 0; i < ChD.size; i++)
        {
            characters[i].myPassive.SpecialDrow(drow);
        }
        StartCoroutine("specialDrowCor");
    }
    IEnumerator specialDrowCor()
    {
        while (specialDrowList.Count != 0)
        {

            CM.SpecialCardToField(specialDrowList[0]);
            specialDrowList.RemoveAt(0);

            yield return new WaitForSeconds(0.3f);
        }
        if (card12On)
        {
            AM.Act();
            card12On = false;
        }
        //AM.MyAct();
        // CM.Rebatch();           
    }
    public void ghostRevive(int ghostCount) //망자부활 + ghostCount
    {
        log.logContent.text += "\n망자부활 : " + ghostCount + "!";
        CharacterPassive QpassiveScript = null;
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].characterNo == 1)
            {
                QpassiveScript = characters[i].GetComponent<CharacterPassive>();
            }

        }
        if (QpassiveScript != null)
        {

            QpassiveScript.GhostRevive(ghostCount);
        }
    }
    public void CopyCard(int CopyCount) //덱에 카드 복사
    {
        log.logContent.text += "\n덱에" + selectedCard.GetComponent<Card>().Name.text + "(을)를 복사!";
        for (int i = 0; i < CopyCount; i++)
        {
            GameObject newCard = Instantiate(selectedCard, new Vector3(0, 0, 0), transform.rotation, GameObject.Find("CardCanvas").transform);
            newCard.GetComponent<Card>().isGrave = false;
            newCard.GetComponent<Card>().isDeck = false;
            newCard.GetComponent<Card>().isSelected = false;
            newCard.GetComponent<Transform>().localScale = new Vector2(1, 1);
            newCard.SetActive(false);
            CM.Deck.Add(newCard);
        }
    }
    public void NextTurnArmor(int armor, Character character) //다음 턴 방어도 획득
    {
        log.logContent.text += "\n" + actCharacter.Name + "이(가) 다음턴에 " + armor + "의 방어도 획득!";
        character.nextarmor += armor;
    }
    public void card8(int point) //car8개별 효과 함수
    {
        log.logContent.text += "\n" + actCharacter.Name + "이(가) 이번 턴 종료시 남은 cost*10의 방어도를 얻습니다.";
        actCharacter.card8 = true;
        actCharacter.card8point = point;
    }
    public void teamTurnAtkUp(int atk) //해당 턴 동안 모든 아군 공격력 증가
    {
        log.logContent.text += "\n이번 턴 동안 팀 모두의 공격력이 " + atk + "만큼 증가합니다.";
        for (int i = 0; i < characters.Count; i++)
        {
            if (!characters[i].isDie)
            {
                characters[i].TurnAtkUp(atk);
            }
        }
    }
    public void TurnAtkUp(Character character, int atk) //해당 턴 동안 공격력 증가
    {
        log.logContent.text += "\n이번 턴 동안 " + character.Name + "의 공격력이 " + atk + "만큼 증가합니다.";
        character.TurnAtkUp(atk);
    }
    public void AtkUp(Character character, int atk) //해당 전투동안 공격력 증가
    {
        log.logContent.text += "\n" + character.Name + "의 공격력이 " + atk + "만큼 증가합니다.";
        character.AtkUp(atk);
    }


    public void card12()
    {
        card12On = true;
        StartCoroutine("card12C");
    }

    IEnumerator card12C()
    {
        CM.UseCard(selectedCard);
        int g = CM.field.Count;
        for (int i = CM.field.Count - 1; i >= 0; i--)
        {
            if (CM.field[i] != null)
            {
                if (selectedCard != CM.field[i])
                    CM.FieldToDeck(CM.field[i]);
            }
            yield return new WaitForSeconds(0.25f);
        }
        specialDrow(g);

    }

    public void Click_GraveOn() //무덤 열기. 특정 경우에는 클릭 안 해도 자동으로 열림
    {
        if (HandManager.Instance.isEnableOtherButton)
        {

            otherCanvasOn = true;
            isGraveWindowOn = true;

            go_GraveView_Button_Revive.SetActive(false);

            CM.GraveOn();
            window_Grave.SetActive(true);
        }
    }
    public void GraveOn_ByCardEffect()
    {
        otherCanvasOn = true;
        isGraveWindowOn = true;

        go_GraveView_Button_Revive.SetActive(true);
        curSelectedCardInRevive = selectedCard.GetComponent<Card>();
        HandManager.Instance.CancelToUse();
        CM.GraveOn();
        window_Grave.SetActive(true);
    }
    public void Click_GraveOff() //무덤에서 그냥 종료 버튼
    {
        otherCanvasOn = false;
        isGraveWindowOn = false;

        if (GraveReviveMode) //만약에 무덤에서 카드를 고르는 카드 사용 중일 때는 그 카드의 발동을 취소하는 함수 호출
            Grave_ReviveCancel();
        else
        {
            CM.GraveOff();
            cancleCard();
            //CancleCharacter();
            CancleEnemy();
            window_Grave.SetActive(false);
        }
    }

    public void Click_Grave_Revive() //무덤에서 부활버튼 누를 때
    {


        GraveReviveMode = false;

        reviveCount = 0;
        CM.Revive();
        curSelectedCardInRevive.SelectRevive();
    }

    public void Grave_ReviveCancel()
    {
        GraveReviveMode = false;
        for (int i = 0; i < CM.ReviveCard.Count; i++)
        {
            CM.ReviveCard[i].GetComponent<Transform>().localScale = new Vector2(1f, 1f);
        }
        CM.ReviveCard.Clear();

        go_GraveView_Button_Revive.SetActive(false);
        reviveCount = 0;

        CM.GraveOff();
        window_Grave.SetActive(false);
    }

    public void Click_DeckOn(bool isCardSkill)
    {
        if (HandManager.Instance.isEnableOtherButton||isCardSkill)
        {
            otherCanvasOn = true;
            isDeckWindowOn = true;

            if (DeckSelectMode)
            {
                go_DeckView_Button_OK.SetActive(true);
                go_DeckView_Button_Cancel.SetActive(true);
            }
            else
            {
                go_DeckView_Button_OK.SetActive(false);
                go_DeckView_Button_Cancel.SetActive(false);
            }

            CM.DeckOn();
            window_Deck.SetActive(true);
        }
    }

    public void Click_DeckOff()
    {
        SelectDeckCount = 0;
        otherCanvasOn = false;
        CM.DeckOff();
        window_Deck.SetActive(false);
    }

    public void ReviveToField(int r)
    {
        GraveReviveMode = true;
        GraveOn_ByCardEffect();
        reviveCount += r;
    }
    public void RandomReviveToField(int n) //랜덤으로 무덤에서 필드로 카드 가져오기
    {
        if (n > CM.Grave.Count)
            n = CM.Grave.Count;
        for (int i = 0; i < n; i++)
        {
            int rand = Random.Range(0, CM.Grave.Count);
            CM.GraveToField(CM.Grave[rand]);


        }
    }
    public void ActUpCharacter(int c) //특정 캐릭터(no가 c인)의 행동력을 증가시킴
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (!characters[i].isDie && characters[i].characterNo == c)
            {
                log.logContent.text += "\n" + characters[i].Name + "의 행동력이 1증가합니다.";

            }
        }
    }

    public bool card20done = false;
    GameObject copyCard;

    public void UsePreviousCard()
    {
        usedInCard20 = selectedCard;
        // Debug.Log(usedInCard20.GetComponent<Card>().cardNo);
        copyCard = Instantiate(previousSelectedCard, CM.HandCanvas.transform);
        copyCard.SetActive(false);
        copyCard.GetComponent<Card>().iscard20Mode = true;
        copyCard.GetComponent<Card>().BM = this;
        copyCard.GetComponent<Card>().CM = CM;
        copyCard.GetComponent<Card>().useCardInCard20();
    }

    public void reflectUp(int amount) //반사데미지를 올려줌
    {
        actCharacter.reflect += amount;
    }

    public void Victory() //승리시 발동하는 함수
    {
        otherCanvasOn = true;
        victory_window.SetActive(true);
        isVictoryPopupOn = true;
        otherCanvasOn = true;
        int ignum = Random.Range(0, 3) * 50 + 150 + GD.victory * 20;
        int tribute = Random.Range(0, 2) * 100 + 200 + 50 * GD.victory;
        if (GD.blessbool[15]) ignum *= 3;
        rewardIgnum.text = ignum + "이그넘 획득";
        rewardTribute.text = tribute + "공물 획득";
        GD.Ignum += ignum;
        GD.BattleNo++;
        //정해진 공식에 따라 이그넘을 획득 후
        if (GD.blessbool[16]) rewardListlength = 4; //축복 16번이 true라면 4개를 보여줘야함
        if (!GD.blessbool[9]) noSelectCard.SetActive(false); //축복 9번이 없다면 아무것도 선택 안하는 버튼을 없앰
        for (int i = 0; i < CardInfo.Instance.cd.Length; i++)
        {
            for (int j = 0; j < characters.Count; j++)
            {
                if (CardInfo.Instance.cd[i].Deck == characters[j].characterNo && CardInfo.Instance.cd[i].type != 2)
                {
                    RandomCardList.Add(i);//획득 할 수 있는 모든 카드를 리스트에 넣은 후
                }
            }
        }
        int rand = Random.Range(0, RandomCardList.Count);
        for (int i = 1; i <= rewardListlength; i++)
        {
            int temp = RandomCardList[rand];
            RandomCardList[rand] = RandomCardList[RandomCardList.Count - i];
            RandomCardList[RandomCardList.Count - i] = temp;
            rand = Random.Range(0, RandomCardList.Count - i);
        }
        for (int i = 1; i <= rewardListlength; i++)
        {
            Debug.Log(RandomCardList[RandomCardList.Count - i]);
            GameObject newCard = Instantiate(rewardCardPrefebs, RewardCanvas.transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(RandomCardList[RandomCardList.Count - i], 0);
            RancomSelectCard.Add(RandomCardList[RandomCardList.Count - i]);
        }
        //랜덤함수를 반복 해서 맨 앞 n장의 카드들을 중복되지 않는 카드들로
        equipment e = EquipmentManager.Instance.makeEquipment(); //새로운 장비를 생성함.
        List<string> sList = EquipmentManager.Instance.equipmentStrings(e);
        RewardEquipmentString[0].text = sList[0];
        RewardEquipmentString[1].text = sList[1] + '\n' + sList[2] + '\n' + sList[3];
        GD.EquipmentList.Add(e);
    }
    public void Click_SelectReward()//원하는 카드를 선택해서 카드 데이터에 저장하는 함수
    {
        bool isSelect = false;
        for (int i = 0; i < RancomSelectCard.Count; i++)
        {

            if (RewardCanvas.transform.GetChild(i).GetComponent<NoBattleCard>().select)
            {
                isSelect = true;
            }
        }
        if (!isSelect) return;
        CardData CardD;
        string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CardDatas[GameManager.Instance.nowPlayingSlot]);
        string cardData = File.ReadAllText(path);
        CardD = JsonConvert.DeserializeObject<CardData>(cardData);
        for (int i = 0; i < RancomSelectCard.Count; i++)
        {

            if (RewardCanvas.transform.GetChild(i).GetComponent<NoBattleCard>().select)
            {
                CardD.cardCode.Add(RancomSelectCard[i]);
                CardD.cardCost.Add(CardInfo.Instance.cd[RancomSelectCard[i]].Cost);
                CardD.cardGetOrder.Add(CardD.count);
                CardD.count++;
            }
        }
        cardData = JsonConvert.SerializeObject(CardD);
        path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CardDatas[GameManager.Instance.nowPlayingSlot]);
        File.WriteAllText(path, cardData);
        Click_NoSelectAndMain();
    }
    public void Click_NoSelectAndMain()//전투 정보를 모두 저장한 후 로비로 가는 함수
    {
        if (GD.bless3count > 0) GD.bless3count--;
        if (GD.bless3count == 0) GD.blessbool[3] = false; //블레스3이 켜져있다면 1개씩 감소시킴.
        for (int i = 0; i < characters.Count; i++)
        {
            bool isForward = false;
            ChD.characterDatas[i].curHp = characterOriginal[i].Hp;
            for (int j = 0; j < forward.Count; j++)
            {
                if (forward[j] == characterOriginal[i])
                {
                    ChD.characterDatas[i].curFormation = 0;
                    isForward = true;
                    break;
                }
            }
            if (!isForward) ChD.characterDatas[i].curFormation = 1; //진형붕괴를 통해 위치가 바꼈다면 저장
        }

        string path4 = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CharacterDatas[GameManager.Instance.nowPlayingSlot]);
        string CharacterData = JsonConvert.SerializeObject(ChD);
        File.WriteAllText(path4, CharacterData);
        GD.isAct = false;
        GD.isNight = false;
        GD.isActInDay = false;
        GD.Day+=2;
        GD.victory++;

        //로비에 가게되면 새로운 날짜기 때문에 행동력이 주어지고 낮으로 바뀌게된다.
        string path3 = Path.Combine(Application.persistentDataPath, "GameData.json");
        string GameData = JsonConvert.SerializeObject(GD);
        File.WriteAllText(path3, GameData);
        Time.timeScale = 1;
        StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene3_Lobby"));
        //SceneManager.LoadScene("Scene2_Lobby");
    }

    public void Defetead() //패배했을 시 패배 창 띄우기
    {
        defeated_window.SetActive(true);
    }

    public void goEnemySelectMode() //공격 카드 선택 시 어떤 적을 공격할지 고르는 모드
    {
        if(!porte3mode)
        enemySelectMode = true; //이 상태로 들어가면 Enemy를 클릭시 선택이 된다.
    }

    public void goCharacterSelectMode() //아군 선택 카드 선택 시 어떤 아군을 선택할지 고르는 모드
    {
        characterSelectMode = true; //이 상태로 들어가면 character를 클릭시 선택이 된다.
    }
    //type==0 랜덤 대상 type==1 방어도 높은 적 우선 type==2 체력 높은 적 우선 



    public Character SelectCharacterInEnemyTurn(int type, int pos) //pos ->0->전방 1->후방 2->전체 중
    {//특정 위치에 특정 조건을 가지고 있는 캐릭터 한명을 랜덤으로 가져오는 함수
        List<Character> selectedCharacters = new List<Character>();
        if (pos == 0)
        {
            bool alive = false;
            for (int i = 0; i < forward.Count; i++)
            {
                if (!forward[i].isDie)
                    alive = true;
            }
            if (!alive)
            {
                return SelectCharacterInEnemyTurn(type, 2);
            }
        }
        else if (pos == 1)
        {
            bool alive = false;
            for (int i = 0; i < back.Count; i++)
            {
                if (!back[i].isDie)
                    alive = true;
            }
            if (!alive)
            {
                return SelectCharacterInEnemyTurn(type, 2);
            }
        }
        if (pos == 0)
        {
            for (int i = 0; i < forward.Count; i++)
            {
                if (!forward[i].isDie)
                    selectedCharacters.Add(forward[i]);
            }
        }
        else if (pos == 1)
        {
            for (int i = 0; i < back.Count; i++)
            {
                if (!back[i].isDie)
                    selectedCharacters.Add(back[i]);
            }
        }
        else if (pos == 2)
        {
            for (int i = 0; i < forward.Count; i++)
            {
                if (!forward[i].isDie)
                    selectedCharacters.Add(forward[i]);
            }
            for (int i = 0; i < back.Count; i++)
            {
                if (!back[i].isDie)
                    selectedCharacters.Add(back[i]);
            }
        }
        if (type == 0)
        {
            return selectedCharacters[Random.Range(0, selectedCharacters.Count)];
        }
        else if (type == 1)
        {
            List<Character> MaxAromorList = new List<Character>();
            int maxArmor = 0;
            for (int i = 0; i < selectedCharacters.Count; i++)
            {
                if (selectedCharacters[i].armor > maxArmor)
                {
                    maxArmor = selectedCharacters[i].armor;
                    MaxAromorList.Clear();
                    MaxAromorList.Add(selectedCharacters[i]);
                }
                else if (selectedCharacters[i].armor == maxArmor)
                {
                    MaxAromorList.Add(selectedCharacters[i]);
                }
            }
            return MaxAromorList[Random.Range(0, MaxAromorList.Count)];
        }
        else if (type == 2)
        {
            List<Character> MaxHPList = new List<Character>();
            int maxHP = 0;
            for (int i = 0; i < selectedCharacters.Count; i++)
            {
                if (selectedCharacters[i].Hp > maxHP)
                {
                    maxHP = selectedCharacters[i].Hp;
                    MaxHPList.Clear();
                    MaxHPList.Add(selectedCharacters[i]);
                }
                else if (selectedCharacters[i].armor == maxHP)
                {
                    MaxHPList.Add(selectedCharacters[i]);
                }
            }
            return MaxHPList[Random.Range(0, MaxHPList.Count)];
        }

        return null;



    }

    public List<Character> SelectCharacterListInEnemyTurn(int pos) //특정 위치에 있는 살아있는 캐릭터 모두를 불러오는 함수
    {
        List<Character> selectedCharacters = new List<Character>();
        if (pos == 0)
        {
            for (int i = 0; i < forward.Count; i++)
            {
                if (!forward[i].isDie)
                    selectedCharacters.Add(forward[i]);
            }
        }
        else if (pos == 1)
        {
            for (int i = 0; i < back.Count; i++)
            {
                if (!back[i].isDie)
                    selectedCharacters.Add(back[i]);
            }
        }
        else if (pos == 2)
        {
            for (int i = 0; i < forward.Count; i++)
            {
                if (!forward[i].isDie)
                    selectedCharacters.Add(forward[i]);
            }
            for (int i = 0; i < back.Count; i++)
            {
                if (!back[i].isDie)
                    selectedCharacters.Add(back[i]);
            }
        }
        return selectedCharacters;
    }
    public void EnemyAttack(int dmg, Enemy enemy, Character target)
    {
        AM.MakeEnemyAct(0, dmg+enemy.atk, target, enemy, null);
    }
    public void EnemyIncreaseSpeed(int amount, Enemy enemy, Character target)
    {
        AM.SpdIncreaseByEnemy(1, amount, target, enemy, null);
    }

    public void EnemyGetAromor(int mount, Enemy myEnemy, Enemy target)
    {
        AM.MakeEnemyAct(2, mount, null, myEnemy, target);
    }

    public void EnemyGetHp(int mount, Enemy myEnemy, Enemy target)
    {
        AM.MakeEnemyAct(3, mount, null, myEnemy, target);
    }


    public void EnemyStateChange(Enemy myEnemy, int mount) //0->은신 1->무적 2->불사
    {

        AM.MakeEnemyAct(4, mount, null, myEnemy, myEnemy);
    }
    public void EnemyActStatusChange(Enemy myEnemy, int mount, int statusType, Character target)
    {
        AM.MakeEnemyAct(statusType, mount, target, myEnemy, null);
    }

    public void EnemyFormationCollapse(Enemy myEnemy) //적이 선 행동으로 진형붕괴를 선택했을 때
    {
        AM.SpdIncreaseByEnemy(6, 0, null, myEnemy, null);
    }

    public void EnemyAtkUp(Enemy targetEnemy, int mount, Enemy actEnemy)
    {
        AM.MakeEnemyAct(7, mount, null, actEnemy, targetEnemy);
    }
    public void EnemySpeedDown(Enemy targetEnemy, int mount, Enemy actEnemy)
    {
        AM.MakeEnemyAct(8, mount, null, actEnemy, targetEnemy);
    }
    public void card22()  //결의
    {
        List<int> minArmorList = new List<int>();
        int minArmor = 0;
        for (int i = 0; i < characters.Count; i++)
        {
            if (!characters[i].isDie)
            {
                characters[i].getArmor(4 + characters[i].turnDef);
                if (characters[i].armor < minArmor)
                {
                    minArmor = characters[i].armor;
                    minArmorList.Clear();
                    minArmorList.Add(i);
                }
                else if (characters[i].armor == minArmor)
                {
                    minArmorList.Add(i);
                }
            }
        }
        getArmor(10, characters[minArmorList[Random.Range(0, minArmorList.Count)]]);
    }

    public void card24() //사이코키네시스
    {
        StartCoroutine("card24cor");
    }

    IEnumerator card24cor() //덱에있는 모든 패를 무덤으로 보내는 과정
    {
        while (CM.field.Count > 1)
        {
            CM.FieldToGrave(CM.field[0]);
            HM.InitCard();
            yield return new WaitForSeconds(0.2f);
        }

    }
    public void card35(GameObject MyCard)
    {
        StartCoroutine(card35cor(MyCard));
    }
    IEnumerator card35cor(GameObject MyCard) //덱에있는 모든 패를 무덤으로 보내는 과정
    {
        otherCorIsRun = true;
        int myFieldCount = CM.field.Count - 1;
        for (int i = CM.field.Count - 1; i >= 0; i--)
        {

            if (CM.field[i] != MyCard)
            {
                CM.field[i].GetComponent<Card>().RemoveThisCardInField();
            }
            yield return new WaitForSeconds(0.1f);
        }
        otherCorIsRun = false;
        OnRandomAttack(10, actCharacter, myFieldCount);
        if (!MyCard.GetComponent<Card>().iscard20Mode)
        {
            useCost(MyCard.GetComponent<Card>().cardcost, gameObject);

        }
        MyCard.GetComponent<Card>().UseCardResult();
        AM.Act();

    }
    public void SelectDeckCard(int count)
    {
        SelectDeckCount = count;
        DeckSelectMode = true;
        Click_DeckOn(true);
    }

    public void Click_Cancle_DeckWindow() //덱에서 선택을 취소하는 함수
    {
        DeckSelectCancle = true;
        DeckSelectMode = false;

        Click_DeckOff();
    }

    public void Click_OK_DeckWindow() //덱에서 패로 가져올 카드 선택
    {
        if (SelectDeckCount == CM.SelectedCard.Count)
        {
            selectedCard.GetComponent<Card>().SelectDeck();
            SelectedCard = CM.SelectedCard[0];
            CM.DeckToField();
            DeckSelectMode = false;
            DeckSelect = true;
            Click_DeckOff();
        }
        else
        {
            go_Warning_LessSelectedCard_DeckSelectMode.SetActive(true);
            Invoke("go_Warning_LessSelectedCard_DeckSelectMode_Off", 1f);
        }
    }

    void go_Warning_LessSelectedCard_DeckSelectMode_Off() //선택된 카드가 적을 때
    {
        go_Warning_LessSelectedCard_DeckSelectMode.SetActive(false);
    }
}