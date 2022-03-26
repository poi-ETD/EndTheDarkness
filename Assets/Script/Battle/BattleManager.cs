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
    [SerializeField] GameObject[] CharacterPrefebs;
    public List<Character> characters=new List<Character>();
    public bool CharacterSelectMode;
    public bool EnemySelectMode;
    public Character character;
    public Enemy enemy;
    public int cost;
    public GameObject card;
    public Enemy penemy;
    public GameObject pcard;
    public Character pcharacter;
    Transform tra1;
    Transform tra2;
    Transform tra3;
    [SerializeField] Text costT;
    public int startCost;
    public int CardCount;
    [SerializeField] CardManager CM;
    public int TurnCardCount;
    [SerializeField] GameObject Warn;
    public Text warntext;
    [SerializeField] GameObject useButton;
    public List<Character> forward = new List<Character>();
    public List<Character> back = new List<Character>();
    public int line;
    public int diecount;
    public int nextTurnStartCost;
    [SerializeField] GameObject completeButton;
    bool card12On;
    [SerializeField] GameObject[] RedLine;
    int curCharacterNumber;
    public bool otherCanvasOn;
    public Log log;
    [SerializeField] GameObject graveView;
    public  int ReviveCount;
    public bool card7mode;
    public GameObject[] Enemys;
    public GameData gd;
    public CharacterData CD;
    [SerializeField] GameObject victory;
    [SerializeField] GameObject defeated;
    public bool SelectMode;
    bool porte3mode;
    [SerializeField] GameObject condition;
    public Text conditionText;
    public bool GraveReviveMode;
    [SerializeField] Text graveClose;
    [SerializeField] Text StackT;
    [SerializeField] GameObject StackPopUp;
    public Text CardUseText;
    [SerializeField] GameObject CancleButton;
    public bool card20Activing;
    [SerializeField] GameObject ReviveCancle;
    public bool CancleReviveMode;
    public bool ReviveMode;
    public GameObject DeckView;
    public GameObject SelectedCard;
    [SerializeField] GameObject LineObject;
    [SerializeField] GameObject FormationCollapsePopup;
    [SerializeField] Text FormationCollapseText;
    [SerializeField] GameObject[] FormationCollapseButton;
    [SerializeField] Text[] FormationCollapseButtonText;
    bool MoveToForward;
    public bool[] BlessBM = new bool[20];
    public int porte3count;
    HandManager HM;
    List<Character> characterOriginal = new List<Character>();

    public void FormationCollapse(string ename)
    {
        otherCanvasOn = true;
        if (forward.Count < back.Count)
        {
            MoveToForward = true;
            line++;
        }    
        else if (forward.Count ==back.Count)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) { rand = -1;MoveToForward = false; }
            if (rand == 1) MoveToForward = true;
            line += rand;
        }
        else if (forward.Count>back.Count)
        {
            MoveToForward = false;
            line--;
        }
        for(int i = 0; i < 3; i++)
        {
            FormationCollapseButton[i].SetActive(false);
        }
        FormationCollapsePopup.SetActive(true);
        if (MoveToForward)
        {   FormationCollapseText.text = ename + "이(가) 진형붕괴를 시전했습니다." + "\n누구를 전방으로 보내겠습니까?"; 
            for(int i = 0; i < back.Count; i++)
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
                FormationCollapseButtonText[i].text =forward[i].Name;             
            }
        }
    }
    public void SelectFormationCollapse(int c)
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
        characters.Clear();
        for (int i = 0; i < line; i++)
        {
            forward[i].transform.position = new Vector2(-800 / 45f, (400 - 250 * characters.Count) / 45f);
            characters.Add(forward[i]);
        }
        for (int i = line; i < CD.size; i++)
        {
            back[i - line].transform.position = new Vector2(-800 / 45f, (400 - 250 * characters.Count) / 45f);
            characters.Add(back[i - line]);
        }
        otherCanvasOn = false;
        FormationCollapsePopup.SetActive(false);
        LineObject.transform.position = new Vector2(-16.66f, (11.66f - 5.55f * line));
   
   
    }
    public void StackPopUpOn()
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
            for(int i = 0; i < characters.Count; i++)
            {
                if (characters[i].characterNo == 1)
                {
                    StackT.text += "망자:" + characters[i].GetComponent<Q>().Ghost + "\n";
                    break;                  
                }
            }
           
            StackPopUp.SetActive(true);
        }
    }
    public void toMain()
    {
       
            for(int i = 0; i < characters.Count; i++)
            {
                bool isForward = false;
                CD.characterDatas[i].curHp = characterOriginal[i].Hp;
                for(int j = 0; j < forward.Count; j++)
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
       
        SceneManager.LoadScene("Main");
    }
    private void Awake()
    {
        startCost = 0;
        string path = Path.Combine(Application.persistentDataPath, "GameData.json");
        string gameData = File.ReadAllText(path);
        gd = JsonConvert.DeserializeObject<GameData>(gameData);
        path = Path.Combine(Application.persistentDataPath, "CharacterData.json");
       string characterData= File.ReadAllText(path);
        CD = JsonConvert.DeserializeObject<CharacterData>(characterData);
        line = CD.line;
        for(int i = 0; i < CD.size; i++)
        {
            GameObject CharacterC=null;
          
            if (CD.characterDatas[i].curFormation == 0)
            {
               CharacterC = Instantiate(CharacterPrefebs[CD.characterDatas[i].No], new Vector2(-800 / 45f, (400 - 250 * characters.Count) / 45f), transform.rotation, GameObject.Find("CharacterCanvas").transform);           
                forward.Add(CharacterC.GetComponent<Character>());
            }
            else
            {
               CharacterC = Instantiate(CharacterPrefebs[CD.characterDatas[i].No], new Vector2(-800 / 45f, (400 - 250 * characters.Count) / 45f), transform.rotation, GameObject.Find("CharacterCanvas").transform);
                back.Add(CharacterC.GetComponent<Character>());
            }
            characterOriginal.Add(CharacterC.GetComponent<Character>());
            CharacterC.GetComponent<Character>().Atk = CD.characterDatas[i].Atk;
            CharacterC.GetComponent<Character>().Hp = CD.characterDatas[i].curHp;
            startCost += CD.characterDatas[i].Cost;
            CharacterC.GetComponent<Character>().passive = CD.characterDatas[i].passive;
            CharacterC.GetComponent<Character>().Name = CD.characterDatas[i].Name;
        }
            
     for(int i = 0; i < line; i++)
        {
            forward[i].transform.position = new Vector2(-800 / 45f, (400 - 250 * characters.Count) / 45f);
            characters.Add(forward[i]);
        }
        for (int i = line; i <CD.size; i++)
        {
            back[i -line].transform.position = new Vector2(-800 / 45f, (400 - 250 * characters.Count) / 45f);
            characters.Add(back[i-line]);
        }
        if (gd.BattleNo == 3||gd.BattleNo==6) //폴리만 예외
        { 
            GameObject EnemySummon = Instantiate(Enemys[gd.BattleNo], new Vector2(0, 6), transform.rotation, GameObject.Find("CharacterCanvas").transform);
        }
        else
        {
            GameObject EnemySummon = Instantiate(Enemys[gd.BattleNo], new Vector2(-2, -2), transform.rotation, GameObject.Find("CharacterCanvas").transform);
        }
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        TurnCardCount = CardCount;
      
        LineObject.transform.position = new Vector2(-16.66f, (11.66f - 5.55f * line));
        HM = GameObject.Find("HandManager").GetComponent<HandManager>();
    }

    GameObject c20;
    private void Update()
    {
      
        if (Input.GetKey("escape"))
                Application.Quit();            
        costT.text = "cost:" + cost;
        if (card == null||porte3mode)
            useButton.SetActive(false);
        else useButton.SetActive(true);
        if (card20done) useButton.SetActive(true);
        if (card12On)
        {
            if (card != null)
            {
                toGraveCount++;
                CM.UseCard(card);
                CM.TM.turnCard--;
                card = null;
            }
        }  
    }
    public void porte3()
    {
        condition.SetActive(true);
        conditionText.text = "스타키티시모";
        completeButton.SetActive(true);
        SelectMode = true;
        porte3mode = true;
    }
    public void Complete()
    {
        condition.SetActive(false);
        SelectMode = false;
        completeButton.SetActive(false);
        if (card12On)
        {
            specialDrow(toGraveCount-1);
            toGraveCount = 0;
            card12On = false;
        }
        if (porte3mode)
        {
            if (card != null)
            {
                card.transform.localScale = new Vector3(1, 1, 1);
                CM.FieldToDeck(card);
                CM.CardToField();
                CM.field[CM.field.Count - 1].GetComponent<Card>().cardcost -= 2;
                if (CM.field[CM.field.Count - 1].GetComponent<Card>().cardcost < 0)
                    CM.field[CM.field.Count - 1].GetComponent<Card>().cardcost = 0;
                log.logContent.text += "\n스타카티시모!"+CM.field[CM.field.Count - 1].GetComponent<Card>().Name.text + "의 코스트가 감소하였습니다.";
                porte3count--;
                card = null;
                if (porte3count == 0)
                {
                    porte3mode = false;
                
                }
                else
                {
                    porte3();
                }
            }
            else
            {
                condition.SetActive(true);
                SelectMode = true;
                completeButton.SetActive(true);
                warntext.text = "스타카티시모:카드 선택을 해주세요.";
                      WarnOn();
            }
        }
    }
    public void CancleCharacter()
    {
        if (!otherCanvasOn)
        {
            if (tra1 != null)
                tra1.localScale = new Vector2(1, 1);
            RedLine[curCharacterNumber].SetActive(false);
            character = null;
        }
    }
    public void CharacterSelect(GameObject c)
    {
        if (!otherCanvasOn)
        {
            CancleCharacter();
            for (int i = 0; i < characters.Count; i++)
            {
                if (c.GetComponent<Character>() == characters[i])
                    curCharacterNumber = i;
            }
            RedLine[curCharacterNumber].SetActive(true);
            tra1 = c.GetComponent<Transform>();
            tra1.localScale = new Vector2(1.2f, 1.2f);

            character = c.GetComponent<Character>();
        }
    }
    public void EnemySelect(GameObject e)
    {
        if (!otherCanvasOn)
        {
            CancleEnemy();
            enemy = e.GetComponent<Enemy>();
            tra3 = e.GetComponent<Transform>();
            tra3.localScale = new Vector2(1.2f, 1.2f);
        }
    }
    public void CancleEnemy()
    {
        if (!otherCanvasOn)
        {
        
            if (tra3 != null)
                tra3.localScale = new Vector2(1, 1);
            enemy = null;
        }
    }
    public void SetCard(GameObject c)
    {
        if (!otherCanvasOn)
        {
            cancleCard();
            /*tra2 = c.GetComponent<Transform>();
            tra2.localScale = new Vector2(1.5f, 1.5f);
       
            c.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -230, 0);*/
                 card = c;
        }
    }
    public void cancleCard()
    {
        if (!otherCanvasOn)
        {
            EnemySelectMode = false;
            card = null;
            if (tra2 != null)
            {
                tra2.localScale = new Vector2(1, 1);

            }
            CM.Rebatch();
        }
    }
    IEnumerator turnStartDrow()
    {
      
        for (int i = 0; i < TurnCardCount; i++)
        {  
            CM.CardToField();
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(1.2f);
        HM.InitCard();
    }
    public void TurnStart()
    {
        HM.isInited = false;
        pcharacter = null;
        pcard = null;
        penemy = null;
        log.logContent.text += "\n" + CM.TM.t + "턴 시작!";
        Setting();
        StartCoroutine("turnStartDrow");
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].isTurnStart = true;
        }
        if (card22on)
        {
            int ArmorSum = 0;
            for (int i = 0; i < characters.Count; i++)
            {
                if (!characters[i].isDie)
                {
                    ArmorSum += characters[i].Armor;
                    characters[i].Armor = 0;
                }
            }
            card22c.Armor += ArmorSum;
            card22on = false;
        }
    }
    public void Setting()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].isSet = true;
        }
    }
    public void TurnEnd()
    {
        for (int i = 0; i < Enemys.Length; i++)
        {
            Enemys[i].GetComponent<Enemy>().Board.text = "";
        }
        TurnCardCount = CardCount;
        Setting();
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].isTurnEnd = true;
        }
    }
    public void useCard()
    {
      if (!EnemySelectMode)
        {
            if (card20done)
            {        
                cost += pcard.GetComponent<Card>().cardcost;
                pcard.GetComponent<Card>().isGrave = false;
                pcard.GetComponent<Card>().isUsed = false;
                if (pcard.GetComponent<Card>().Name.text == "리셋")
                {
                    card = pcard;
                }
                pcard.GetComponent<Card>().useCard();
                card20Activing = false;              
                pcard = card;
                
                CancleButton.SetActive(false);
            }
            else if (card != null)
            {
                
                card.GetComponent<Card>().useCard();
            }
            
        }
        else
        {
            if (card20done)
            {
                warntext.text = "스케치 반복 사용 중입니다.";
                WarnOn();
            }
            else
            {
                CardUseText.text = "카드 사용";
                EnemySelectMode = false;
            }
        }

    }
    public void WarnOn()
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
    public void allClear()
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
    public void OnDmgOneTarget(int dmg)
    {
        character.AttackCount++;
        CardUseText.text = "카드사용";
        EnemySelectMode = false;
        log.logContent.text += "\n"+enemy.Name+"에게 "+(dmg + character.turnAtk)+"의 데미지!";
        enemy.onHit(dmg + character.turnAtk);
    }
    public void getArmor(int armor)
    {
        log.logContent.text += "\n" + character.Name + "이(가) " + armor + "의 방어도 획득!"; 
        character.Armor += armor;   
    }
    public void specialDrow(int drow)
    {
        log.logContent.text += "\n 카드를 통해 드로우 " + drow + "장!";
        StartCoroutine("specialDrowC", drow);
    }
    IEnumerator specialDrowC(int drow)
    {
        for (int i = 0; i < drow; i++)
        {
            CM.SpecialCardToField();
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void ghostRevive(int ghostCount)
    {
        log.logContent.text += "\n망자부활 : " + ghostCount + "!";
        Q q=null;
        for(int i = 0; i < characters.Count; i++)
        {
            if (characters[i].characterNo == 1)
            {
                q = characters[i].GetComponent<Q>();
            }
            
        }
        if (q != null)
        {
            q.Ghost+=ghostCount;
        }
    }
    public void CopyCard(int CopyCount)
    {
        log.logContent.text += "\n덱에"+card.GetComponent<Card>().Name.text+"(을)를 복사!";
        for (int i = 0; i < CopyCount; i++)
        {
            GameObject newCard = Instantiate(card, new Vector3(0, 0, 0), transform.rotation,GameObject.Find("CardCanvas").transform);
            newCard.GetComponent<Card>().use = false;
            newCard.GetComponent<Card>().isUsed = false;
            newCard.GetComponent<Card>().isGrave = false;
            newCard.GetComponent<Transform>().localScale = new Vector2(1, 1);
            newCard.SetActive(false);
            CM.Deck.Add(newCard);
        }
    }
    public void NextTurnArmor(int armor)
    {
        log.logContent.text += "\n" + character.Name + "이(가) 다음턴에 " + armor + "의 방어도 획득!";
        character.nextarmor += armor;
    }
    public void card8(int point)
    {
        log.logContent.text += "\n" + character.Name + "이(가) 이번 턴 종료시 남은 cost*10의 방어도를 얻습니다.";
        character.card8 = true;
        character.card8point = point;
    }
    public void teamTurnAtkUp(int atk)
    {
        log.logContent.text += "\n이번 턴 동안 팀 모두의 공격력이 " + atk + "만큼 증가합니다.";
        for (int i = 0; i < characters.Count; i++)
        {
            if (!characters[i].isDie)
            {
                characters[i].turnAtk += atk;
            }
        }
    }
    public void TurnAtkUp(int atk)
    {
        log.logContent.text += "\n이번 턴 동안 "+character.Name +"의 공격력이 " + atk + "만큼 증가합니다.";
        character.turnAtk += atk;
    }
    public void AtkUp(int atk)
    {
        log.logContent.text += "\n"+character.Name + "의 공격력이 " + atk + "만큼 증가합니다."; 
        character.Atk += atk;
        character.turnAtk += atk;
    }
    int toGraveCount = 0;
    public void card12()
    {
        condition.SetActive(true);
        conditionText.text = "리셋";
        SelectMode = true;
        log.logContent.text += "\n리셋!";
        card12On = true;
        completeButton.SetActive(true);
        
    }
    public void card12remake()
    {
        
        StartCoroutine("card12C");
        
    }
    IEnumerator card12C()
    {
        GameObject pc = card;
        CM.UseCard(card);
        pc.GetComponent<Card>().use = false;
        int g = CM.field.Count;
        for (int i = CM.field.Count - 1; i >= 0; i--)
        {
            if (CM.field[i] != null)
            {
                if (card != CM.field[i])
                    CM.FieldToDeck(CM.field[i]);
            }
            yield return new WaitForSeconds(0.3f);
        }
        specialDrow(g);
        pcard = pc;
    }
    public void GraveOn()
    {
        otherCanvasOn = true;
        if (GraveReviveMode)
        {   graveClose.text = "부활";
            ReviveCancle.SetActive(true);
            
        }
        CM.GraveOn();
        graveView.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
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
    public void ReviveCancleUse()
    {
        CancleReviveMode = true;
        for(int i = 0; i < CM.ReviveCard.Count; i++)
        {
            CM.ReviveCard[i].GetComponent<Transform>().localScale = new Vector2(1.3f, 1.3f);
        }
        CM.ReviveCard.Clear();
        graveClose.text = "닫기";
        GraveReviveMode = false;
        ReviveCancle.SetActive(false);
        ReviveCount = 0;
        card7mode = false;
        CM.GraveOff();
        otherCanvasOn = false;
        graveView.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 2000, 0);
    }
    public void GraveOff()
    {
        if (GraveReviveMode)
        {
            ReviveMode = true;
            graveClose.text = "닫기";
            GraveReviveMode = false;
            ReviveCancle.SetActive(false);
            ReviveCount = 0;
            CM.Revive();
           
        }
        else
        {
            CM.GraveOff();
            otherCanvasOn = false;          
            cancleCard();
            CancleCharacter();
            CancleEnemy();           
            graveView.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,2000, 0);
        }
    }
    public void ReviveToField(int r)
    {
        GraveReviveMode = true;
        GraveOn(); 
        ReviveCount += r;
    }
    public void RandomReviveToField(int n)
    {if (n > CM.Grave.Count)
            n = CM.Grave.Count;
        for(int i = 0; i < n; i++)
        {
            int rand = Random.Range(0, CM.Grave.Count);           
            CM.GraveToField(CM.Grave[rand]);

         
        }
    }
    public void ActUpCharacter(int c)
    {
        for(int i = 0; i < characters.Count; i++)
        {
            if (!characters[i].isDie&&characters[i].characterNo == c)
            {
                log.logContent.text += "\n" + characters[i].Name + "의 행동력이 1증가합니다.";
                characters[i].Act++;
            }
        }
    }
   public bool card20done=false;
    public void card20Active()
    {
     
        card20done = true;
        pcard.SetActive(true);
        pcard.transform.parent = CM.CardCanvas.transform;
        c20 = card;
        pcard.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        pcard.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        pcard.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -230, 0);
        pcard.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        CancleButton.SetActive(true);
    }
    public void cancleButtonUse()
    {
       
        if (card20done)
        {
            pcard.SetActive(false);
            card20done = false;
            CM.ToGrave(pcard);
            cost += c20.GetComponent<Card>().cardcost;      
            CancleButton.SetActive(false);
            card20Activing = false;
            CM.GraveToField(c20);
        }
    }
    public void reflectUp(int r)
    {
        character.reflect+=r;
    }
 public void Victory()
    {
        victory.SetActive(true);
    }
    public void Defetead()
    {
        defeated.SetActive(true);
    }
    public void goEnemySelectMode()
    {
        EnemySelectMode = true;
        CardUseText.text = "취소";
    }
    //type==0 랜덤 대상 type==1 방어도 높은 적 우선 type==2 체력 높은 적 우선 type==3 방어도 있는 적 우선
    //type==4 모든 대상
    //ActM =>true 일 시 행동력 감소 포함
    public void HitFront(int dmg,int type,string Ename,bool ActM)
    {
        bool Alive = false;
      
        for(int i = 0; i < forward.Count; i++)
        {
            if (!characters[i].isDie) Alive = true;
        }
        if (!Alive) {
          
            HitAll(dmg, type, Ename, ActM);
        }
        else
        {
            int rand2 = 0;
            if (type == 0)
            {
                rand2 = Random.Range(0, line);
                while (characters[rand2].isDie) rand2 = Random.Range(0, line);
                characters[rand2].onHit(dmg, Ename);
                if (ActM)
                {
                    characters[rand2].NextTurnMinusAct++;    
                }
            }
            if (type == 1)
            {
                List<Character> MaxArmor = new List<Character>();
                int maxArmor = 0;
                for(int i = 0; i < forward.Count; i++)
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
                while(MaxArmor[rand2].isDie) rand2 = Random.Range(0, MaxArmor.Count);
                MaxArmor[rand2].onHit(dmg, Ename);
                if (ActM)
                {
                    MaxArmor[rand2].NextTurnMinusAct++;
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
                MaxHp[rand2].onHit(dmg, Ename);
                if (ActM)
                {
                    MaxHp[rand2].NextTurnMinusAct++;
                }
            }
            if (type == 3)
            {
                List<Character> HaveArmor = new List<Character>();
                for(int i = 0; i < forward.Count; i++)
                {
                    if (forward[i].Armor > 0) HaveArmor.Add(forward[i]);
                }
                if (HaveArmor.Count == 0) HitFront(dmg, 0, Ename,ActM);
                else
                {
                    rand2 = Random.Range(0, HaveArmor.Count);
                    HaveArmor[rand2].onHit(dmg, Ename);
                    if (ActM)
                {
                        HaveArmor[rand2].NextTurnMinusAct++;   
                }
                }
            }
            if (type == 4)
            {
                for(int i = 0; i < line; i++)
                {
                    characters[i].onHit(dmg, Ename);
                    if (ActM)
                    {
                        characters[i].NextTurnMinusAct++;
                    }
                }
            }
        }
    }
    public void HitAll(int dmg,int type,string Ename,bool ActM)
    {
        int rand2 = 0;
        if (type == 0)
        {
            rand2 = Random.Range(0, characters.Count);
            while (characters[rand2].isDie) rand2 = Random.Range(0, characters.Count);
            characters[rand2].onHit(dmg, Ename);
            if (ActM)
            {
                characters[rand2].NextTurnMinusAct++;
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
            MaxArmor[rand2].onHit(dmg, Ename);
            if (ActM)
            {
                MaxArmor[rand2].NextTurnMinusAct++;
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
            MaxHp[rand2].onHit(dmg, Ename);
            if (ActM)
            {
                MaxHp[rand2].NextTurnMinusAct++;
            }
        }
        if (type == 3)
        {
            List<Character> HaveArmor = new List<Character>();
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].Armor > 0) HaveArmor.Add(characters[i]);
            }
            if (HaveArmor.Count == 0) HitAll(dmg, 0, Ename,ActM);
            else
            {
                rand2 = Random.Range(0, HaveArmor.Count);
                HaveArmor[rand2].onHit(dmg, Ename);
                if (ActM)
                {
                    HaveArmor[rand2].NextTurnMinusAct++;
                }
            }
        }
        if (type == 4)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                characters[i].onHit(dmg, Ename);
                if (ActM)
                {
                    characters[i].NextTurnMinusAct++;
                }
            }
        }
    }
    public void HitBack(int dmg,int type,string Ename,bool ActM)
    {
        bool Alive = false;

        for (int i = 0; i < back.Count; i++)
        {
            if (!characters[i].isDie) Alive = true;
        }
        if (!Alive)
        {
            HitAll(dmg, type, Ename, ActM);
        }
        else
        {
            int rand2 = 0;
            if (type == 0)
            {
                rand2 = Random.Range(line, characters.Count);
                while (characters[rand2].isDie) rand2 = Random.Range(line, characters.Count);
                characters[rand2].onHit(dmg, Ename);
                if (ActM)
                {
                    characters[rand2].NextTurnMinusAct++;
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
                MaxArmor[rand2].onHit(dmg, Ename);
                if (ActM)
                {
                    MaxArmor[rand2].NextTurnMinusAct++;
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
                MaxHp[rand2].onHit(dmg, Ename);
                if (ActM)
                {
                    MaxHp[rand2].NextTurnMinusAct++;
                }
            }
            if (type == 3)
            {
                List<Character> HaveArmor = new List<Character>();
                for (int i = line; i < characters.Count; i++)
                {
                    if (characters[i].Armor > 0) HaveArmor.Add(characters[i]);
                }
                if (HaveArmor.Count == 0) HitBack(dmg, 0, Ename, ActM);
                else
                {
                    rand2 = Random.Range(0, HaveArmor.Count);
                    HaveArmor[rand2].onHit(dmg, Ename);
                    if (ActM)
                    {
                        HaveArmor[rand2].NextTurnMinusAct++;
                    }
                }
            }
            if (type == characters.Count)
            {
                for (int i = line; i < characters.Count; i++)
                {
                    characters[i].onHit(dmg, Ename);
                    if (ActM)
                    {
                        characters[i].NextTurnMinusAct++;
                    }
                }
            }
        }
    }
    bool card22on;
    Character card22c;
    public void card22()
    {
        card22c = character;
        card22on = true;
        for(int i = 0; i < characters.Count; i++)
        {
            if (!characters[i].isDie)
            {
                characters[i].Armor += 7;
            }
        }
    }
    public void card23()
    {
        for(int i = 0; i < characters.Count; i++)
        {
            characters[i].Act = 0;
        }
        for(int i = 0; i < Enemys.Length; i++)
        {
            if (!Enemys[i].GetComponent<Enemy>().isDie)
            {
                character.AttackCount+=3;
                Enemys[i].GetComponent<Enemy>().onHit(1 + character.turnAtk);
                Enemys[i].GetComponent<Enemy>().onHit(1 + character.turnAtk);
                Enemys[i].GetComponent<Enemy>().onHit(1 + character.turnAtk);
            }
        }
        CM.TM.turnAtk += 2;
    }
    public void card24()
    {
        StartCoroutine("card24c");
    }
    IEnumerator card24c()
    {
        while (CM.field.Count > 1)
        {

            CM.FieldToGrave(CM.field[0]);
            HM.InitCard();
            yield return new WaitForSeconds(0.2f);
        }

    }

    public int SelectDeckCount;
    public void SelectDeckCard(int count)
    {
        DeckOn();  
        SelectDeckCount = count;
        DeckSelectMode = true;
    }
    public void DeckCancle()
    {
        DeckOff(); 
        DeckSelectCancle = true;
        DeckSelectMode = false;
    }
    public bool DeckSelectMode;
    public bool DeckSelect;
    public bool DeckSelectCancle;
    public void DeckComplete()
    {
        if (SelectDeckCount == CM.SelectedCard.Count)
        {
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
    void SlessOff()
    {
        Sless.SetActive(false);
    }
    [SerializeField] GameObject Sless;
    
}
