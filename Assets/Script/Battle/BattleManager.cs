﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
public class BattleManager : MonoBehaviour
{
    [SerializeField] GameObject[] CharacterPrefebs; 
    public List<Character> characters = new List<Character>(); //현재 게임에 있는 캐릭터들의 목록,순서또한 동일
    public bool CharacterSelectMode;//캐릭터를 고를 수 있는 상태
    public bool EnemySelectMode;//적을 고를 수 있는 상태
    public Character character;//현재 지정된 캐릭터
    public Enemy enemy;//현재 지정된 적

    public GameObject card;//현재 지정된 카드
    public GameObject pcard;//바로 이전에 사용한 카드

    [SerializeField] TextMeshProUGUI costT;
    public int startCost;//턴이 시작 될 때마다 채워지는 코스트
    public int cost;//남은 코스트
    public int nextTurnStartCost;//다음 턴 시작 시 코스트를 올려주는 변수

    public int CardCount;//턴이 시작 될 때마다 뽑는 카드 수
    public int TurnCardCount;//현재 턴에 사용 한 카드 수

    [SerializeField] CardManager CM;
    public ActManager AM;


    public GameObject[] Enemys;//적들

    [SerializeField] GameObject Warn;
    public Text warntext;
    public List<Character> forward = new List<Character>();//전방에 있는 캐릭터들 목록
    public List<Character> back = new List<Character>();//후방에 있는 캐릭터들 목록
    public int line;//전방,후방을 나누는 선
    public int diecount;//죽은 아군의 수

    [SerializeField] GameObject completeButton;

    public bool otherCanvasOn;//다른 캔버스(무덤이나 로그,덱 등)이 켜져 있을 때
    public Log log;
    [SerializeField] GameObject graveView;//무덤
    public int ReviveCount;//무덤에서 부활시킬 카드의 수


    public GameData gd;
    public CharacterData CD;
    [SerializeField] GameObject victory; //승리 시 나오는 팝업 창
    [SerializeField] GameObject defeated; //패배 시 나오는 팝업 창

    public bool SelectMode; //스타키티시모등 카드 선택 해야 할 때
    public bool porte3mode;//스타키티시모가 켜져 있을 때

    [SerializeField] GameObject condition;
    public Text conditionText;

    public bool GraveReviveMode;//무덤에서 부활시키는 카드를 선택한 상태

    [SerializeField] Text graveClose;
    [SerializeField] Text StackT;
    [SerializeField] GameObject StackPopUp;
    public TextMeshProUGUI CardUseText;
    [SerializeField] GameObject CancleButton;
    public bool card20Activing;//스케치 반복이 발동중이면
    [SerializeField] GameObject ReviveCancle; 
    public bool ReviveMode; //무덤에서 부활시키는 카드를 켰을 때
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
    [SerializeField] GameObject RewardCanvas;
    public bool isV;//승리 팝업이 켜져있을 때
    [SerializeField] GameObject RewardCardPrefebs;
    public int SelectedRewardCount;
    [SerializeField] GameObject noSelect;
    int listlength = 3;
    List<int> RandomCardList = new List<int>();
    CardData2 data2 = new CardData2();
    List<int> RancomSelectCard = new List<int>();
    //승리 시 보상 선택하는 창에 들어갈 변수들

    [SerializeField] GameObject Sless;//덱에서 카드 선택할 때 아무것도 선택 안 되어 있으 면
    
   

    public int SelectDeckCount;//덱에서 선택 할 카드 수
    public bool DeckSelectMode;
    public bool DeckSelect;
    public bool DeckSelectCancle;
    //덱에서 카드 고를 때 필요한 변수들

    public GameObject c20;//스케치 반복용



    public bool otherCor; //코루틴이 동작 중일 때
 
    public bool turnStarting;//턴 시작 코루틴이 동작 중일 때

    public int turnEndu;//턴 종료 코루틴이 동작중일 때


    //YH
    [HideInInspector] public bool isPointerinHand = false;
    [HideInInspector] public bool isSelectedCardinHand = false;

    public GameObject DmgPrefebs;//데미지 프리펩

    public void SetBless20() //bless20이 켜져있다면 적용 될 함수
    {
        for(int i = 0; i < forward.Count; i++)
        {
            forward[i].RealAtkUp(-1);
        }
        for(int i = 0; i < back.Count; i++)
        {
            back[i].RealAtkUp(1);
        }
    }
    public void ResetBless20() //원래 공격력으로 돌려놓는 함수
    {
        for (int i = 0; i < forward.Count; i++)
        {
            forward[i].RealAtkUp(1);
        }
        for (int i = 0; i < back.Count; i++)
        {
            back[i].RealAtkUp(-1);
        }
    }
    public void costUp(int i) //현재 코스트를 올려주는 함수
    {
        cost += i;
        costT.text = "" + cost;
    }
    public void FormationCollapse(string ename) //진형붕괴 함수
    {
        if (gd.blessbool[20])
        {
            ResetBless20();
        }
        otherCanvasOn = true;
        if (forward.Count < back.Count) //더 많은 쪽에서 적은쪽으로 옮긴다.
        {
            MoveToForward = true;
            line++;
        }
        else if (forward.Count == back.Count) //전방과 후방의 수가 같으면 랜덤으로 결정
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) { rand = -1; MoveToForward = false; }
            if (rand == 1) MoveToForward = true;
            line += rand;
        }
        else if (forward.Count > back.Count)
        {
            MoveToForward = false;
            line--;
        }
        //line변수의 값 조정으로 전방과 후방의 수 조정
        for (int i = 0; i < 3; i++)
        {
            FormationCollapseButton[i].SetActive(false);
        }
        FormationCollapsePopup.SetActive(true);
        if (MoveToForward)
        {
            FormationCollapseText.text = ename + "이(가) 진형붕괴를 시전했습니다." + "\n누구를 전방으로 보내겠습니까?";
            for (int i = 0; i < back.Count; i++)
            {
                FormationCollapseButton[i].SetActive(true);
                FormationCollapseButtonText[i].text = back[i].Name;
            }
        }
        else
        {
            FormationCollapseText.text = ename + "이(가) 진형붕괴를 시전했습니다." + "\n누구를 후방으로 보내겠습니까?";
            for (int i = 0; i < forward.Count; i++)
            {
                FormationCollapseButton[i].SetActive(true);
                FormationCollapseButtonText[i].text = forward[i].Name;
            }
        }
    }
    public void Click_SelectFormationCollapse(int c)//진형붕괴 팝업창에 뜬 버튼을 클릭했을 시
    {
        CD.line = line;
        if (MoveToForward)
        {

            forward.Add(back[c]);
            back.RemoveAt(c);
        }
        else
        {

            back.Add(forward[c]);
            forward.RemoveAt(c);
        }
        //선택한 캐릭터를 옮긴다.
        characters.Clear();
        for (int i = 0; i < line; i++)
        {
            forward[i].transform.position = new Vector2(-880 / 45f, (300 - 150 * characters.Count) / 45f);
            characters.Add(forward[i]);
        }
        for (int i = line; i < CD.size; i++)
        {
            back[i - line].transform.position = new Vector2(-880 / 45f, (270 - 150 * characters.Count) / 45f);
            characters.Add(back[i - line]);
        }
        //캐릭터들의 위치 재설정
        for (int i = 1; i < CD.size; i++)
        {
            characters[i].curNo = i;
        }
        //캐릭터들의 현재 위치 정보 재설정
        otherCanvasOn = false; //진형붕괴 팝업이 꺼졌기 때문
        otherCor = false; //턴 시작 코루틴을 진행시킴
        FormationCollapsePopup.SetActive(false);
        if (gd.blessbool[20])
        {
            SetBless20();
        }
        LineObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-820, 360 - 150 * line);
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

        for (int i = 0; i < characters.Count; i++)
        {
            bool isForward = false;
            CD.characterDatas[i].curHp = characterOriginal[i].Hp;
            for (int j = 0; j < forward.Count; j++)
            {
                if (forward[j] == characterOriginal[i])
                {
                    CD.characterDatas[i].curFormation = 0;
                    isForward = true;
                    break;
                }
            }
            if (!isForward) CD.characterDatas[i].curFormation = 1;
        }

        string path4 = Path.Combine(Application.persistentDataPath, "CharacterData.json");
        string CharacterData = JsonConvert.SerializeObject(CD);
        File.WriteAllText(path4, CharacterData);
        Time.timeScale = 1;

        SceneManager.LoadScene("Main"); //지금은 메인화면으로 갈 때 데이터를 저장하게 되는데, 승리시에만 하도록 추후에 변경해야함
    }

    public void Click_Menu()
    {
        if (go_Menus.activeSelf)
            go_Menus.SetActive(false);
        else
            go_Menus.SetActive(true);
    }

    private void Awake()
    {
        startCost = 0;
        string path = Path.Combine(Application.persistentDataPath, "GameData.json");
        string gameData = File.ReadAllText(path);
        gd = JsonConvert.DeserializeObject<GameData>(gameData);
        path = Path.Combine(Application.persistentDataPath, "CharacterData.json");
        string characterData = File.ReadAllText(path);
        CD = JsonConvert.DeserializeObject<CharacterData>(characterData);
        line = CD.line;
        for (int i = 0; i < CD.size; i++)
        {
            GameObject CharacterC = null;

            if (CD.characterDatas[i].curFormation == 0) //전방
            {

                CharacterC = Instantiate(CharacterPrefebs[CD.characterDatas[i].No], new Vector2(-880 / 45f, 375 / 45f), transform.rotation, GameObject.Find("CharacterCanvas").transform);
                forward.Add(CharacterC.GetComponent<Character>());
            }
            else //후방
            {
                CharacterC = Instantiate(CharacterPrefebs[CD.characterDatas[i].No], new Vector2(-880 / 45f, (330 - 150 * characters.Count) / 45f), transform.rotation, GameObject.Find("CharacterCanvas").transform);
                back.Add(CharacterC.GetComponent<Character>());
            } //전방과 후방 목록에 각각 캐릭터를 넣음
            characterOriginal.Add(CharacterC.GetComponent<Character>());
            CharacterC.GetComponent<Character>().Atk = CD.characterDatas[i].Atk;
            CharacterC.GetComponent<Character>().Hp = CD.characterDatas[i].curHp;
            CharacterC.GetComponent<Character>().def = CD.characterDatas[i].def;
            startCost += CD.characterDatas[i].Cost;
            CharacterC.GetComponent<Character>().passive = CD.characterDatas[i].passive;
            CharacterC.GetComponent<Character>().Name = CD.characterDatas[i].Name;
            //캐릭터들의 기본 스탯을 데이터와 같게 설정
        }

        for (int i = 0; i < line; i++)
        {
            forward[i].transform.position = new Vector2(-880 / 45f, (300 - 150 * characters.Count) / 45f);
            characters.Add(forward[i]);
        }//위치가 전방인 캐릭터들을 목록에 넣음
        for (int i = line; i < CD.size; i++)
        {
            back[i - line].transform.position = new Vector2(-880 / 45f, (270 - 150 * characters.Count) / 45f);
            characters.Add(back[i - line]);
        }//위치가 후방인 캐릭터들을 목록에 넣음
        for (int i = 1; i < CD.size; i++)
        {
            characters[i].curNo = i;
        }
        if (gd.BattleNo == 3 || gd.BattleNo == 6) //폴리만 예외,추후에 폴리 다시 작업할 때 수정예정
        {
            GameObject EnemySummon = Instantiate(Enemys[gd.BattleNo], new Vector2(0, 6), transform.rotation, GameObject.Find("CharacterCanvas").transform);
        }
        else
        {
            GameObject EnemySummon = Instantiate(Enemys[gd.victory], new Vector2(-2, -2), transform.rotation, GameObject.Find("CharacterCanvas").transform);
        }
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < Enemys.Length; i++)
        {
            Enemys[i].GetComponent<Enemy>().myNo = i;
        }
        TurnCardCount = CardCount;

        LineObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-820, 360 - 150 * line);
        HM = GameObject.Find("HandManager").GetComponent<HandManager>();
        if (gd.blessbool[20])
        {
            SetBless20();
        }
    }


    private void Update()
    {
        if (Input.GetKey("escape")) //esc누르면 게임 나가게
            Application.Quit();
    }

    public void Porte3On() //스타키티시모 on
    {
        condition.SetActive(true);
        conditionText.text = "스타키티시모";
        completeButton.SetActive(true);
        SelectMode = true;
    }
    public void Click_Complete()//카드를 고르는 기능용
    {
        condition.SetActive(false);
        SelectMode = false;
        completeButton.SetActive(false);
        if (porte3mode) //현재 상태가 스타키티시모 라면
        {
           
            if (card != null)
            {
                card.transform.localScale = new Vector3(1, 1, 1);
                CM.FieldToDeck(card);//선택한 카드를 덱에 보내고
                CM.CardToField();//랜덤한 카드를 필드 가장 오른쪽으로 보냄
                CM.field[CM.field.Count - 1].GetComponent<Card>().cardcost -= 2;//필드의 가장 오른쪽 카드의 코스트를 2감소시킴
                if (CM.field[CM.field.Count - 1].GetComponent<Card>().cardcost < 0)
                    CM.field[CM.field.Count - 1].GetComponent<Card>().cardcost = 0; 
                CM.field[CM.field.Count - 1].GetComponent<Card>().costT.text = CM.field[CM.field.Count - 1].GetComponent<Card>().cardcost + "";
                log.logContent.text += "\n스타카티시모!" + CM.field[CM.field.Count - 1].GetComponent<Card>().Name.text + "의 코스트가 감소하였습니다.";
                porte3count--;
                card = null;
                if (porte3count == 0) //포르테3의 중첩이 다 끝나면
                {
                    porte3mode = false;
                }
                else
                {
                    Porte3On();
                }
            }
            else //아무 카드도 선택이 되어있지 않을 때
            {
                condition.SetActive(true);
                SelectMode = true;
                completeButton.SetActive(true);
                warntext.text = "스타카티시모:카드 선택을 해주세요.";
                WarnOn();
            }
        }
    }
    public void CancleCharacter() //발동->캐릭터를 두번 눌럿을때 or 눌러진 상태에서 다른 캐릭터를 눌렀을때
    {

        if (!otherCanvasOn)
        {

            if (character != null)//이미 눌러진 캐릭터가 있다면
                character.SelectBox.SetActive(false); //선택이 되었다는 표시를 없앤다.

            character = null;
        }
    }
    public void CharacterSelect(GameObject c)//캐릭터 클릭이 가능 할 때 누르면 발동
    {
     
        if (!otherCanvasOn)
        {
         
            CancleCharacter();//이미 눌러진 캐릭터를 취소

            character = c.GetComponent<Character>();
            if (character != null)
                character.SelectBox.SetActive(true);
        }
    }
    public void useCost(int c) //코스트 사용 함수
    {
        cost -= c;
        costT.text = "" + cost;
    }
    public void EnemySelect(GameObject e) //공격 카드가 발동되었을 시 적을 선택
    {
        if (!otherCanvasOn)
        {
            enemy = e.GetComponent<Enemy>();
            card.GetComponent<Card>().EnemySelectCard();
        }
    }
    public void CancleEnemy() //현재 지정된 적을 풀어야 할 때 사용 (공격 카드를 발동 한 후 등)
    {
        if (!otherCanvasOn)
        {
            enemy = null;
        }
    }
    public void SetCard(GameObject c) //카드를 눌렀을 때
    {
        if (SelectMode)
        {           
            cancleCard();
            HandManager.Instance.go_UseButton.SetActive(true);
            card = c;
            return;
        }

        if (!otherCanvasOn)
        {
            cancleCard();
          
            HandManager.Instance.go_UseButton.SetActive(true);
            
            card = c;
        }
    }
    public void cancleCard() //카드를 두번 누르거나, 다른 카드를 눌렀을때 이미 눌러진 카드에게 적용되는 함수
    {
        if (!otherCanvasOn)
        {
            EnemySelectMode = false;

            card = null;

            HandManager.Instance.go_UseButton.SetActive(false);

            if (!isPointerinHand)
                HandManager.Instance.SelectCardToOriginPosition();
        }
    }


    public void Click_useCard() //카드 사용 버튼을 누를 때
    {
        CancleButton.SetActive(false);
        HandManager.Instance.go_SelectedCardTooltip.SetActive(false);

        if (!EnemySelectMode) 
        {
            if (card != null)
            {
                card.GetComponent<Card>().useCard(); //선택된 카드를 사용한다.
            }
        }
        else
        {
            if (c20 != null)
            {
                Destroy(card);
                card = c20;
                otherCanvasOn = false;
            }
            CardUseText.text = "사용";
            EnemySelectMode = false;
        }

    }


    public void WarnOn()//여러가지 경고 처리
    {
        Warn.SetActive(true);

        Invoke("TargetOff", 1f);
    }
    public void overAct()
    {
        warntext.text = "캐릭터의 행동력이 없습니다.";
        WarnOn();
    }
    public void TargetOn()
    {
        allClear();
        Warn.SetActive(true);
        warntext.text = "타겟 설정을 다시 해주세요";
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
        Warn.SetActive(false);
    }
    public void costOver()
    {
        allClear();
        Warn.SetActive(true);
        warntext.text = "코스트가 부족합니다";
        Invoke("costOverOff", 1f);
    }
    void costOverOff()
    {
        Warn.SetActive(false);
    }
    public void OnDmgOneTarget(int dmg, Enemy E, int n) //카드를 사용해 데미지를 입힐 경우
    {
        CardUseText.text = "사용";
        EnemySelectMode = false;
        log.logContent.text += "\n" + E.Name + "에게 " + (dmg + character.turnAtk) + "의 데미지!("+n+")";
        for (int k = 0; k < n; k++)
        {
            enemy.OnHitCal(dmg + character.turnAtk, character.curNo, false);
        }
        
    }

    public void OnAttack(int dmg,Enemy E,Character c,int n) //패시브나 리플렉트로 인하여 데미지 입힐 경우
    {
        for (int k = 0; k < n; k++)
        {
           E.OnHitCal(dmg, c.curNo, false);
        }     
    }

    public void AllAttack(int dmg, Character c, int n) //흑백등을 이용해 데미지를 전체에게 입힐 경우
    {
        log.logContent.text += "\n적 전체에게 " + dmg + c.turnAtk + "의 데미지!(" + n + ")";
        for (int k = 0; k < n; k++)
        {
            for (int i = 0; i < Enemys.Length; i++)
            {
                Enemys[i].GetComponent<Enemy>().OnHitCal(dmg + c.turnAtk, c.curNo, false);

            }        
        }
        
    }

   

    public void getArmor(int armor) //방어도 획득
    {
        log.logContent.text += "\n" + character.Name + "이(가) " + armor + "의 방어도 획득!";
        character.getArmor(armor);
    }
    List<GameObject> specialDrowList = new List<GameObject>();
    public void specialDrow(int drow) //카드를 통한 드로우
    {
        log.logContent.text += "\n 카드를 통해 드로우 " + drow + "장!";

      
        int c = 0;
        while (c != drow)
        {
            c++;
            if (CM.Deck.Count > 0)
            {
             
               int rand = Random.Range(0,CM.Deck.Count);
                CM.field.Add(CM.Deck[rand]);
                specialDrowList.Add(CM.Deck[rand]);
                CM.Deck.RemoveAt(rand);
               
            }
        }
        for (int i = 0; i < CD.size; i++)
        {
            characters[i].myPassive.SpecialDrow(drow);
        }
        StartCoroutine("specialDrowC");
    }
    IEnumerator specialDrowC() 
    {
        while (specialDrowList.Count != 0)
        {

            CM.SpecialCardToField(specialDrowList[0]);
            specialDrowList.RemoveAt(0);

            yield return new WaitForSeconds(0.3f);
        }
        //AM.MyAct();
        // CM.Rebatch();           
    }
    public void ghostRevive(int ghostCount) //망자부활 + ghostCount
    {
        log.logContent.text += "\n망자부활 : " + ghostCount + "!";
        CharacterPassive q = null;
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].characterNo == 1)
            {
                q = characters[i].GetComponent<CharacterPassive>();
            }

        }
        if (q != null)
        {
            q.GhostRevive(ghostCount);
        }
    } 
    public void CopyCard(int CopyCount) //덱에 카드 복사
    {
        log.logContent.text += "\n덱에" + card.GetComponent<Card>().Name.text + "(을)를 복사!";
        for (int i = 0; i < CopyCount; i++)
        {
            GameObject newCard = Instantiate(card, new Vector3(0, 0, 0), transform.rotation, GameObject.Find("CardCanvas").transform);
            newCard.GetComponent<Card>().use = false;
            newCard.GetComponent<Card>().isUsed = false;
            newCard.GetComponent<Card>().isGrave = false;
            newCard.GetComponent<Transform>().localScale = new Vector2(1, 1);
            newCard.SetActive(false);
            CM.Deck.Add(newCard);
        }
    }
    public void NextTurnArmor(int armor) //다음 턴 방어도 획득
    {
        log.logContent.text += "\n" + character.Name + "이(가) 다음턴에 " + armor + "의 방어도 획득!";
        character.nextarmor += armor;
    }
    public void card8(int point) //car8개별 효과 함수
    {
        log.logContent.text += "\n" + character.Name + "이(가) 이번 턴 종료시 남은 cost*10의 방어도를 얻습니다.";
        character.card8 = true;
        character.card8point = point;
    }
    public void teamTurnAtkUp(int atk) //해당 턴 동안 모든 아군 공격력 증가
    {
        log.logContent.text += "\n이번 턴 동안 팀 모두의 공격력이 " + atk + "만큼 증가합니다.";
        for (int i = 0; i < characters.Count; i++)
        {
            if (!characters[i].isDie)
            {
                characters[i].AtkUp(atk);
            }
        }
    }
    public void TurnAtkUp(int atk) //해당 턴 동안 공격력 증가
    {
        log.logContent.text += "\n이번 턴 동안 " + character.Name + "의 공격력이 " + atk + "만큼 증가합니다.";
        character.AtkUp(atk);
    }
    public void AtkUp(int atk) //해당 전투동안 공격력 증가
    {
        log.logContent.text += "\n" + character.Name + "의 공격력이 " + atk + "만큼 증가합니다.";
        character.Atk += atk;
        character.turnAtk += atk;
    }

    public void card12()
    {
        condition.SetActive(true);
        conditionText.text = "리셋";
        SelectMode = true;
        log.logContent.text += "\n리셋!";
        completeButton.SetActive(true);

    }
    public void card12remake()
    {

        StartCoroutine("card12C");

    }
    IEnumerator card12C()
    {

        CM.UseCard(card);
        int g = CM.field.Count;
        for (int i = CM.field.Count - 1; i >= 0; i--)
        {
            if (CM.field[i] != null)
            {
                if (card != CM.field[i])
                    CM.FieldToDeck(CM.field[i]);
            }
            yield return new WaitForSeconds(0.25f);
        }
        specialDrow(g);

    }
    public void Click_GraveOn() //무덤 열기 //특정 경우에는 클릭 안 해도 자동으로 열림
    {
        otherCanvasOn = true;
        if (GraveReviveMode)
        {
            graveClose.transform.parent.gameObject.SetActive(true);
            graveClose.text = "부활";
            ReviveCancle.SetActive(true);

        }
        else
        {
            graveClose.transform.parent.gameObject.SetActive(false);
        }
        CM.GraveOn();
        graveView.SetActive(true);
    }
    public void Click_ReviveGrave()
    {
        log.logContent.text += "\n" + character.Name + "이(가) " + card.GetComponent<Card>().Name.text + " 발동!";
        ReviveMode = true;
       
        GraveReviveMode = false;
       
        ReviveCount = 0;
        CM.Revive();
        card.GetComponent<Card>().SelectRevive();
    }
    public void DeckOn()
    {
        otherCanvasOn = true;
        CM.DeckOn();
        DeckView.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
    }
    public void DeckOff()
    {
        SelectDeckCount = 0;
        otherCanvasOn = false;
        CM.DeckOff();
        DeckView.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 2000, 0);
    }
    public void Click_ReviveCancleUse() //무덤에서 선택버튼 누를 때
    {
        card.GetComponent<Card>().CancleRevive();
        for (int i = 0; i < CM.ReviveCard.Count; i++)
        {
            CM.ReviveCard[i].GetComponent<Transform>().localScale = new Vector2(1f, 1f);
        }
        CM.ReviveCard.Clear();
        graveClose.text = "닫기";

        ReviveCancle.SetActive(false);
        ReviveCount = 0;
       
        CM.GraveOff();
        otherCanvasOn = false;
        graveView.SetActive(false);
    }
    public void Click_GraveOff() //무덤에서 그냥 종료 버튼
    {
        if (GraveReviveMode) //만약에 무덤에서 카드를 고르는 카드 사용 중일 때는 그 카드의 발동을 취소하는 함수 호출
        {
            Click_ReviveCancleUse();
        }
        else
        {
            CM.GraveOff();
            otherCanvasOn = false;
            cancleCard();
            CancleCharacter();
            CancleEnemy();
            graveView.SetActive(false);
        }
    }
    public void ReviveToField(int r)
    {
        GraveReviveMode = true;
        Click_GraveOn();
        ReviveCount += r;
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
                characters[i].ActUp(1);
            }
        }
    }

    public bool card20done = false;
    GameObject copyCard;
    public void card20Active() //스케치발동이 발동 되었다면
    {
        c20 = card; //스케치발동을 c20이라는 오브젝트에 저장후,
        otherCanvasOn = true;
        copyCard = Instantiate(pcard, CM.HandCanvas.transform);
        copyCard.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -230, 0);
        copyCard.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        copyCard.SetActive(true);
        CancleButton.SetActive(true);
        copyCard.GetComponent<Card>().iscard20Mode = true;
        copyCard.GetComponent<Card>().cardcost = 0;
        card = copyCard;//현재 지정된 카드를 이전에 사용한 카드의 복사본을 변경
        HandManager.Instance.go_UseButton.SetActive(true);
    }
    public void Click_cancleButtonUse()
    {
        Destroy(copyCard);
        otherCanvasOn = false;
        CancleButton.SetActive(false);
        card = c20;
        c20 = null;

    }
    public void reflectUp(int r) //반사데미지를 올려줌
    {
        character.reflect += r;
    }

    public void Victory() //승리시 발동하는 함수
    {
        otherCanvasOn = true;
        victory.SetActive(true);
        isV = true;
        int ignum = Random.Range(15, 26) * 10 + gd.victory * 20;
        if (gd.blessbool[15]) ignum *= 3;
        rewardIgnum.text = ignum + "이그넘 획득";
        gd.Ignum += ignum;
        //정해진 공식에 따라 이그넘을 획득 후
        if (gd.blessbool[16]) listlength = 4; //축복 16번이 true라면 4개를 보여줘야함
        if (!gd.blessbool[9]) noSelect.SetActive(false); //축복 9번이 없다면 아무것도 선택 안하는 버튼을 없앰
        for (int i = 0; i < data2.cd.Length; i++)
        {
            for (int j = 0; j < characters.Count; j++)
            {
                if (data2.cd[i].Deck == characters[j].characterNo && data2.cd[i].type != 2)
                {
                    RandomCardList.Add(i);//획득 할 수 있는 모든 카드를 리스트에 넣은 후
                }
            }
        }
        int rand = Random.Range(0, RandomCardList.Count);
        for (int i = 1; i <= listlength; i++)
        {
            int temp = RandomCardList[rand];
            RandomCardList[rand] = RandomCardList[RandomCardList.Count - i];
            RandomCardList[RandomCardList.Count - i] = temp;
            rand = Random.Range(0, RandomCardList.Count - i);
        }
        for (int i = 1; i <= listlength; i++)
        {
            Debug.Log(RandomCardList[RandomCardList.Count - i]);
            GameObject newCard = Instantiate(RewardCardPrefebs, RewardCanvas.transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(RandomCardList[RandomCardList.Count - i], 0);
            RancomSelectCard.Add(RandomCardList[RandomCardList.Count - i]);
        }
        //랜덤함수를 반복 해서 맨 앞
    }
    public void Click_SelectReward()//원하는 카드를 선택해서 카드 데이터에 저장하는 함수
    {
        CardData CardD;
        string path = Path.Combine(Application.persistentDataPath, "CardData.json");
        string cardData = File.ReadAllText(path);
        CardD = JsonConvert.DeserializeObject<CardData>(cardData);

        for (int i = 0; i < RancomSelectCard.Count; i++)
        {
           
            if (RewardCanvas.transform.GetChild(i).GetComponent<NoBattleCard>().select)
            {
                CardD.cardNo.Add(RancomSelectCard[i]);
                CardD.cardCost.Add(data2.cd[RancomSelectCard[i]].Cost);
                CardD.cardGet.Add(CardD.get);
                CardD.get++;
            }
        }
        cardData = JsonConvert.SerializeObject(CardD);
        path = Path.Combine(Application.persistentDataPath, "CardData.json");
        File.WriteAllText(path, cardData);
        Click_NoSelectAndMain();
    }
    public void Click_NoSelectAndMain()//전투 정보를 모두 저장한 후 로비로 가는 함수
    {
        for (int i = 0; i < characters.Count; i++)
        {
            bool isForward = false;
            CD.characterDatas[i].curHp = characterOriginal[i].Hp;
            for (int j = 0; j < forward.Count; j++)
            {
                if (forward[j] == characterOriginal[i])
                {
                    CD.characterDatas[i].curFormation = 0;
                    isForward = true;
                    break;
                }
            }
            if (!isForward) CD.characterDatas[i].curFormation = 1;
        }

        string path4 = Path.Combine(Application.persistentDataPath, "CharacterData.json");
        string CharacterData = JsonConvert.SerializeObject(CD);
        File.WriteAllText(path4, CharacterData);
        gd.isAct = false;
        gd.Day++;
        gd.isNight = false;
        gd.victory++;
        gd.isActInDay = false;
        //로비에 가게되면 새로운 날짜기 때문에 행동력이 주어지고 낮으로 바뀌게된다.
        string path3 = Path.Combine(Application.persistentDataPath, "GameData.json");
        string GameData = JsonConvert.SerializeObject(gd);
        File.WriteAllText(path3, GameData);
        Time.timeScale = 1;
        SceneManager.LoadScene("Lobby");
    }
    public void Defetead() //패배했을 시 패배 창 띄우기
    {
        defeated.SetActive(true);
    }
    public void goEnemySelectMode() //공격 카드 선택 시 어떤 적을 공격할지 고르는 모드
    {
        EnemySelectMode = true; //이 상태로 들어가면 Enemy를 클릭시 선택이 된다.
        CardUseText.text = "취소";
    }

 
   

    //type==0 랜덤 대상 type==1 방어도 높은 적 우선 type==2 체력 높은 적 우선 type==3 방어도 있는 적 우선
    //type==4 모든 대상
    //ActM =>true 일 시 행동력 감소 포함
    public void HitFront(int dmg, int type, Enemy enemy, bool ActM) //타켓을 전방으로
    {
        bool Alive = false;

        for (int i = 0; i < forward.Count; i++)
        {
            if (!characters[i].isDie) Alive = true;
        }
        if (!Alive)
        {

            HitAll(dmg, type, enemy, ActM);//전방에 아무도 없다면 타겟을 모두로
        }
        else
        {
            int rand2 = 0;
            if (type == 0)
            {
                rand2 = Random.Range(0, line);
                while (characters[rand2].isDie) rand2 = Random.Range(0, line);
               AM.MakeLateAct(0, dmg, characters[rand2], enemy, null);
             
                if (ActM)
                {
                    AM.MakeEarlyAct(1, 1, characters[rand2], enemy, null);                   
                }
            }
            if (type == 1)
            {
                List<Character> MaxArmor = new List<Character>();
                int maxArmor = 0;
                for (int i = 0; i < forward.Count; i++)
                {
                    if (characters[i].Armor == maxArmor)
                    {
                        MaxArmor.Add(forward[i]);
                    }
                    else if (characters[i].Armor > maxArmor)
                    {
                        maxArmor = characters[i].Armor;
                        MaxArmor.Clear();
                        MaxArmor.Add(forward[i]);
                    }
                }
                rand2 = Random.Range(0, MaxArmor.Count);
                while (MaxArmor[rand2].isDie) rand2 = Random.Range(0, MaxArmor.Count);

               AM.MakeLateAct(0, dmg, MaxArmor[rand2], enemy, null);
              
                if (ActM)
                {
                  AM.MakeEarlyAct(1, 1, MaxArmor[rand2], enemy, null);
                   
                }
            }
            if (type == 2)
            {
                List<Character> MaxHp = new List<Character>();
                int maxHp = 0;
                for (int i = 0; i < forward.Count; i++)
                {
                    if (characters[i].Hp == maxHp)
                    {
                        MaxHp.Add(forward[i]);
                    }
                    else if (characters[i].Hp > maxHp)
                    {
                        maxHp = characters[i].Hp;
                        MaxHp.Clear();
                        MaxHp.Add(forward[i]);
                    }
                }
                rand2 = Random.Range(0, MaxHp.Count);
                while (MaxHp[rand2].isDie) rand2 = Random.Range(0, MaxHp.Count);


              AM.MakeLateAct(0, dmg, MaxHp[rand2], enemy, null);
             
                if (ActM)
                {
                    AM.MakeEarlyAct(1, 1, MaxHp[rand2], enemy, null);
                   
                }
            }
            if (type == 3)
            {
                List<Character> HaveArmor = new List<Character>();
                for (int i = 0; i < forward.Count; i++)
                {
                    if (forward[i].Armor > 0) HaveArmor.Add(forward[i]);
                }
                if (HaveArmor.Count == 0) HitFront(dmg, 0, enemy, ActM);
                else
                {
                    rand2 = Random.Range(0, HaveArmor.Count);


                   AM.MakeLateAct(0, dmg, HaveArmor[rand2], enemy, null);
                    
                    if (ActM)
                    {
                        AM.MakeEarlyAct(1, 1, HaveArmor[rand2], enemy, null);
                   
                    }
                }
            }
            if (type == 4)
            {
                for (int i = 0; i < line; i++)
                {

                    AM.MakeLateAct(0, dmg, characters[i], enemy, null);

                    if (ActM)
                    {
                        AM.MakeEarlyAct(1, 1, characters[i], enemy, null);
                    }
                }
            }
            if (type == 5)
            {
                for (int i = 0; i < line; i++)
                {
                    AM.MakeEarlyAct(1, dmg, characters[i], enemy, null);
                 
                }
            }
        }
    }
    public void HitAll(int dmg, int type, Enemy enemy, bool ActM) //타겟을 모두로
    {
      
        int rand2 = 0;
        if (type == 0)
        {
            rand2 = Random.Range(0, characters.Count);
            while (characters[rand2].isDie) rand2 = Random.Range(0, characters.Count);

            AM.MakeLateAct(0, dmg, characters[rand2], enemy, null);

            if (ActM)
            {
                AM.MakeEarlyAct(1, 1, characters[rand2], enemy, null);
            }
        }
        if (type == 1)
        {
            List<Character> MaxArmor = new List<Character>();
            int maxArmor = 0;
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].Armor == maxArmor)
                {
                    MaxArmor.Add(characters[i]);
                }
                else if (characters[i].Armor > maxArmor)
                {
                    maxArmor = characters[i].Armor;
                    MaxArmor.Clear();
                    MaxArmor.Add(characters[i]);
                }
            }
            rand2 = Random.Range(0, MaxArmor.Count);
            while (MaxArmor[rand2].isDie) rand2 = Random.Range(0, MaxArmor.Count);


            AM.MakeLateAct(0, dmg, MaxArmor[rand2], enemy, null);

            if (ActM)
            {
                AM.MakeEarlyAct(1, 1, MaxArmor[rand2], enemy, null);

            }
        }
        if (type == 2)
        {
            List<Character> MaxHp = new List<Character>();
            int maxHp = 0;
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].Hp == maxHp)
                {
                    MaxHp.Add(characters[i]);
                }
                else if (characters[i].Hp > maxHp)
                {
                    maxHp = characters[i].Hp;
                    MaxHp.Clear();
                    MaxHp.Add(characters[i]);
                }
            }
            rand2 = Random.Range(0, MaxHp.Count);
            while (MaxHp[rand2].isDie) rand2 = Random.Range(0, MaxHp.Count);



            AM.MakeLateAct(0, dmg, MaxHp[rand2], enemy, null);

            if (ActM)
            {
                AM.MakeEarlyAct(1, 1, MaxHp[rand2], enemy, null);

            }
        }
        if (type == 3)
        {
            List<Character> HaveArmor = new List<Character>();
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].Armor > 0) HaveArmor.Add(characters[i]);
            }
            if (HaveArmor.Count == 0) HitAll(dmg, 0, enemy, ActM);
            else
            {
                rand2 = Random.Range(0, HaveArmor.Count);


                AM.MakeLateAct(0, dmg, HaveArmor[rand2], enemy, null);

                if (ActM)
                {
                    AM.MakeEarlyAct(1, 1, HaveArmor[rand2], enemy, null);

                }
            }
        }
        if (type == 4)
        {
            for (int i = 0; i < characters.Count; i++)
            {


                AM.MakeLateAct(0, dmg, characters[i], enemy, null);

                if (ActM)
                {
                    AM.MakeEarlyAct(1, 1, characters[i], enemy, null);
                }
            }
        }
        if (type == 5)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                AM.MakeEarlyAct(1, dmg, characters[i], enemy, null);
            }
        }
    }
    public void HitBack(int dmg, int type, Enemy enemy, bool ActM) //타겟을 후방으로
    {
        bool Alive = false;

        for (int i = 0; i < back.Count; i++)
        {
            if (!characters[i].isDie) Alive = true;
        }
        if (!Alive)
        {
            HitAll(dmg, type, enemy, ActM);//후방에 아무도 없다면 타겟을 전체로
        }
        else
        {
            int rand2 = 0;
            if (type == 0)
            {
                rand2 = Random.Range(line, characters.Count);
                while (characters[rand2].isDie) rand2 = Random.Range(line, characters.Count);

                AM.MakeLateAct(0, dmg, characters[rand2], enemy, null);

                if (ActM)
                {
                    AM.MakeEarlyAct(1, 1, characters[rand2], enemy, null);
                }
            }
            if (type == 1)
            {
                List<Character> MaxArmor = new List<Character>();
                int maxArmor = 0;
                for (int i = line; i < characters.Count; i++)
                {
                    if (characters[i].Armor == maxArmor)
                    {
                        MaxArmor.Add(characters[i]);
                    }
                    else if (characters[i].Armor > maxArmor)
                    {
                        maxArmor = characters[i].Armor;
                        MaxArmor.Clear();
                        MaxArmor.Add(characters[i]);
                    }
                }
                rand2 = Random.Range(0, MaxArmor.Count);
                while (MaxArmor[rand2].isDie) rand2 = Random.Range(0, MaxArmor.Count);


                AM.MakeLateAct(0, dmg, MaxArmor[rand2], enemy, null);

                if (ActM)
                {
                    AM.MakeEarlyAct(1, 1, MaxArmor[rand2], enemy, null);

                }
            }
            if (type == 2)
            {
                List<Character> MaxHp = new List<Character>();
                int maxHp = 0;
                for (int i = line; i < characters.Count; i++)
                {
                    if (characters[i].Hp == maxHp)
                    {
                        MaxHp.Add(characters[i]);
                    }
                    else if (characters[i].Hp > maxHp)
                    {
                        maxHp = characters[i].Hp;
                        MaxHp.Clear();
                        MaxHp.Add(characters[i]);
                    }
                }
                rand2 = Random.Range(0, MaxHp.Count);
                while (MaxHp[rand2].isDie) rand2 = Random.Range(0, MaxHp.Count);



                AM.MakeLateAct(0, dmg, MaxHp[rand2], enemy, null);

                if (ActM)
                {
                    AM.MakeEarlyAct(1, 1, MaxHp[rand2], enemy, null);

                }
            }
            if (type == 3)
            {
                List<Character> HaveArmor = new List<Character>();
                for (int i = line; i < characters.Count; i++)
                {
                    if (characters[i].Armor > 0) HaveArmor.Add(characters[i]);
                }
                if (HaveArmor.Count == 0) HitBack(dmg, 0, enemy, ActM);
                else
                {
                    rand2 = Random.Range(0, HaveArmor.Count);


                    AM.MakeLateAct(0, dmg, HaveArmor[rand2], enemy, null);

                    if (ActM)
                    {
                        AM.MakeEarlyAct(1, 1, HaveArmor[rand2], enemy, null);

                    }
                }
            }
            if (type == 4)
            {
                for (int i = line; i < characters.Count; i++)
                {

                  AM.MakeLateAct(0, dmg, characters[i], enemy, null);
                 
                    if (ActM)
                    {
                        AM.MakeEarlyAct(1, 1, characters[i], enemy, null);
                    }
                }
            }
            if (type == 5)
            {
                for (int i = line; i < characters.Count; i++)
                {
                    AM.MakeEarlyAct(1, dmg, characters[i], enemy, null);
                }
            }
        }
    }

    public void EnemyGetAromor(int mount, Enemy myEnemy, Enemy target)
    {
       AM.MakeLateAct(2, mount, null, myEnemy, target);
     
    }

    public void EnemyGetHp(int mount, Enemy myEnemy, Enemy target)
    {
        AM.MakeLateAct(3, mount, null, myEnemy, target);
     
    }


    public void EnemyStateChange(Enemy myEnemy, int mount) //0->은신 1->무적 2->불사
    {
        if (mount == 0) myEnemy.goingShadow = true;
        AM.MakeEarlyAct(4, mount, null, myEnemy, null);
    }

    public void EnemyFormationCollapse(Enemy myEnemy) //적이 선 행동으로 진형붕괴를 선택했을 때
    {
       AM.MakeEarlyAct(6, 0, null, myEnemy, null);
    }
    public void card22()  //결의
    {
      
        for (int i = 0; i < characters.Count; i++)
        {
            if (!characters[i].isDie)
            {       
                characters[i].getArmor(4+characters[i].turnDef); 
            }
        }
    }
    public void card23() //매치포인트
    {
        
        for (int i = 0; i < characters.Count; i++)
        {
            if(characters[i].characterNo!=2)
            characters[i].onMinusAct(characters[i].Act);
            else
            {
                characters[i].AtkUp(3);
            }
        }

      

       
    }
    public void card24() //사이코키네시스
    {
        StartCoroutine("card24c");
    }
    IEnumerator card24c() //덱에있는 모든 패를 무덤으로 보내는 과정
    {
        while (CM.field.Count > 1)
        {
            CM.FieldToGrave(CM.field[0]);
            HM.InitCard();
            yield return new WaitForSeconds(0.2f);
        }

    }

    public void SelectDeckCard(int count)
    {
        DeckOn();
        SelectDeckCount = count;
        DeckSelectMode = true;
    }
    public void Click_DeckCancle() //덱에서 선택을 취소하는 함수
    {
        DeckOff();
        DeckSelectCancle = true;
        DeckSelectMode = false;
    }

    public void Click_DeckComplete() //덱에서 패로 가져올 카드 선택
    {
        if (SelectDeckCount == CM.SelectedCard.Count)
        {
            card.GetComponent<Card>().SelectDeck();
            SelectedCard = CM.SelectedCard[0];
            CM.DeckToField();
            DeckSelectMode = false;
            DeckSelect = true;
            DeckOff();
        }
        else
        {
            Sless.SetActive(true);
            Invoke("SlessOff", 1f);
        }
    }
    void SlessOff() //선택된 카드가 적을 때
    {
        Sless.SetActive(false);
    }
}