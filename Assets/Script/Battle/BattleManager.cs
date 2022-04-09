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

    [SerializeField] TextMeshProUGUI costT;
    public int startCost;
    public int CardCount;

    [SerializeField] CardManager CM;
    [SerializeField]ActManager AM;

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
    int curCharacterNumber;
    public bool otherCanvasOn;
    public Log log;
    [SerializeField] GameObject graveView;
    public int ReviveCount;
    public bool card7mode;
    public GameObject[] Enemys;
    public GameData gd;
    public CharacterData CD;
    [SerializeField] GameObject victory;
    [SerializeField] GameObject defeated;
    public bool SelectMode;
    public  bool porte3mode;
    [SerializeField] GameObject condition;
    public Text conditionText;
    public bool GraveReviveMode;
    [SerializeField] Text graveClose;
    [SerializeField] Text StackT;
    [SerializeField] GameObject StackPopUp;
    public TextMeshProUGUI CardUseText;
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
  
    public int porte3count;
    public RectTransform rect_HandCanvas;
    HandManager HM;

    List<Character> characterOriginal = new List<Character>();
    [SerializeField] TextMeshProUGUI rewardIgnum;
    [SerializeField] GameObject RewardCanvas;
    public bool isV;
    [SerializeField] GameObject RewardCardPrefebs;
    [SerializeField] private GameObject go_Menus;
    public int SelectedRewardCount;
    [SerializeField] GameObject noSelect;
    [SerializeField] GameObject Sless;
    int toGraveCount = 0;

    public int SelectDeckCount;
    public bool DeckSelectMode;
    public bool DeckSelect;
    public bool DeckSelectCancle;

    public GameObject c20;

    public bool card22on;
    public Character card22c;

    int listlength = 3;
    List<int> RandomCardList = new List<int>();
    CardData2 data2 = new CardData2();
    List<int> RancomSelectCard = new List<int>();
    public TextMeshProUGUI curMessage;

    public bool otherCor;
    public bool sparkyPassive2;
    public bool turnStarting;

    public void costUp(int i)
    {
        cost += i;
        costT.text = "" + cost;
    }
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
            forward[i].transform.position = new Vector2(-880 / 45f, (300-150*characters.Count) / 45f);
            characters.Add(forward[i]);
        }
        for (int i = line; i < CD.size; i++)
        {
            back[i - line].transform.position = new Vector2(-880 / 45f, (270 - 150 * characters.Count) / 45f);
            characters.Add(back[i - line]);
        }
        for(int i = 1; i < CD.size; i++)
        {
            characters[i].curNo = i;
        }
        otherCanvasOn = false;
        FormationCollapsePopup.SetActive(false);
        LineObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-820, 360 - 150 * line);
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
                    StackT.text += "망자:" + characters[i].GetComponent<CharacterPassive>().ghost + "\n";
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
       string characterData= File.ReadAllText(path);
        CD = JsonConvert.DeserializeObject<CharacterData>(characterData);
        line = CD.line;
        for(int i = 0; i < CD.size; i++)
        {
            GameObject CharacterC=null;
          
            if (CD.characterDatas[i].curFormation == 0) //전방
            {
                
               CharacterC = Instantiate(CharacterPrefebs[CD.characterDatas[i].No], new Vector2(-880 / 45f, 375 / 45f), transform.rotation, GameObject.Find("CharacterCanvas").transform);           
                forward.Add(CharacterC.GetComponent<Character>());
            }
            else //후방
            {
               CharacterC = Instantiate(CharacterPrefebs[CD.characterDatas[i].No], new Vector2(-880 / 45f, (330 - 150 * characters.Count) / 45f), transform.rotation, GameObject.Find("CharacterCanvas").transform);
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
            forward[i].transform.position = new Vector2(-880 / 45f, (300 - 150 * characters.Count) / 45f);
            characters.Add(forward[i]);     
        }
        for (int i = line; i <CD.size; i++)
        {
            back[i -line].transform.position = new Vector2(-880 / 45f, (270 - 150 * characters.Count) / 45f);
            characters.Add(back[i-line]);
        }
        for (int i = 1; i < CD.size; i++)
        {
            characters[i].curNo = i;
        }
        if (gd.BattleNo == 3||gd.BattleNo==6) //폴리만 예외
        { 
            GameObject EnemySummon = Instantiate(Enemys[gd.BattleNo], new Vector2(0, 6), transform.rotation, GameObject.Find("CharacterCanvas").transform);
        }
        else
        {
            GameObject EnemySummon = Instantiate(Enemys[gd.victory], new Vector2(-2, -2), transform.rotation, GameObject.Find("CharacterCanvas").transform);
        }
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < Enemys.Length; i++) {
            Enemys[i].GetComponent<Enemy>().myNo = i;
        }
        TurnCardCount = CardCount;
      
        LineObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-820, 360-150*line);
        HM = GameObject.Find("HandManager").GetComponent<HandManager>();
    }

 
    private void Update()
    {
        if (Input.GetKey("escape"))
                Application.Quit();                     
    }
    public void porte3()
    {
      
        porte3mode = true;
    }
    public void Porte3On()
    {
        condition.SetActive(true);
        conditionText.text = "스타키티시모";
        completeButton.SetActive(true);
        SelectMode = true;
    }
    public void Complete()
    {
        condition.SetActive(false);
        SelectMode = false;
        completeButton.SetActive(false);      
        if (porte3mode)
        {
            Debug.Log(porte3count);
            if (card != null)
            {
                card.transform.localScale = new Vector3(1, 1, 1);
                CM.FieldToDeck(card);
                CM.CardToField();
                CM.field[CM.field.Count - 1].GetComponent<Card>().cardcost -= 2;
                    if (CM.field[CM.field.Count - 1].GetComponent<Card>().cardcost < 0)
                    CM.field[CM.field.Count - 1].GetComponent<Card>().cardcost = 0;
                   CM.field[CM.field.Count - 1].GetComponent<Card>().costT.text = CM.field[CM.field.Count - 1].GetComponent<Card>().cardcost + "";
              
                log.logContent.text += "\n스타카티시모!"+CM.field[CM.field.Count - 1].GetComponent<Card>().Name.text + "의 코스트가 감소하였습니다.";
                porte3count--;
                card = null;
                if (porte3count == 0)
                {
                    porte3mode = false;                
                }
                else
                {
                    Porte3On();
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
           
            if(character!=null)
            character.SelectBox.SetActive(false);
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
          
          

            character = c.GetComponent<Character>();
            if (character != null)
                character.SelectBox.SetActive(true);
        }
    }
    public void useCost(int c)
    {
        cost -= c;
        costT.text = "" + cost;
    }
    public void EnemySelect(GameObject e)
    {
        if (!otherCanvasOn)
        {
            enemy = e.GetComponent<Enemy>();
            card.GetComponent<Card>().EnemySelectCard();              
        }
    }
    public void CancleEnemy()
    {
        if (!otherCanvasOn)
        {
        
            enemy = null;
        }
    }
    public void SetCard(GameObject c)
    {
        if (SelectMode)
        {
            cancleCard();
            card = c;
            return;
        }
        
        if (!otherCanvasOn)
        {
            cancleCard();
            /*tra2 = c.GetComponent<Transform>();
            tra2.localScale = new Vector2(1.5f, 1.5f);       
            c.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -230, 0);*/
            useButton.SetActive(true);
                 card = c;
        }
    }
    public void cancleCard()
    {
        if (!otherCanvasOn)
        {
            EnemySelectMode = false;
            card = null;
           
            useButton.SetActive(false);
            CM.Rebatch();
        }
    }


    public void useCard()
    {
        CancleButton.SetActive(false);
        if (!EnemySelectMode)
        {
            if (card != null)
            {
                card.GetComponent<Card>().useCard();
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
    public void OnDmgOneTarget(int dmg,Enemy E,int n)
    {
       
        CardUseText.text = "사용";
        EnemySelectMode = false;
        log.logContent.text += "\n"+E.Name+"에게 "+(dmg + character.turnAtk)+"의 데미지!";
        OnAttack(dmg, E,character,n);
    }
    public void OnAttack(int dmg,Enemy enemy,Character c,int n)
    {     
        StartCoroutine(PlayerAttack(dmg, enemy,c,n));
    }

    IEnumerator PlayerAttack(int dmg, Enemy enemy, Character c,int n)
    {
        for (int k = 0; k < n; k++)
        {
            enemy.Hit(); 
            float t = enemy.onHit(dmg + c.turnAtk, c.curNo);
            yield return new WaitForSeconds(t+0.5f);

            enemy.HitEnd();
            yield return new WaitForSeconds(0.2f);
        }
      
    }

    public void AllAttack(int dmg,Character c,int n)
    {

        StartCoroutine(AllAttackCo(dmg,c,n));

    }

    IEnumerator AllAttackCo(int dmg, Character c,int n)
    {
        for (int k = 0; k < n; k++)
        {
        
            for (int i = 0; i < Enemys.Length; i++)
            {
                if (Enemys[i].GetComponent<Enemy>().isDie) yield return null;
                else
                {
                    Enemys[i].GetComponent<Enemy>().Hit();

                    float t = Enemys[i].GetComponent<Enemy>().onHit(dmg + c.turnAtk, c.curNo);

                    yield return new WaitForSeconds(t + 0.5f);

                    Enemys[i].GetComponent<Enemy>().HitEnd();
                    
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void getArmor(int armor) //방어도 획득
    {
        log.logContent.text += "\n" + character.Name + "이(가) " + armor + "의 방어도 획득!";
        character.getArmor(armor);
    }
    public void specialDrow(int drow) //카드를 통한 드로우
    {
        log.logContent.text += "\n 카드를 통해 드로우 " + drow + "장!";     
        StartCoroutine("specialDrowC", drow);
    }
    IEnumerator specialDrowC(int drow) 
    {

        float t = 0;
        for (int i = 0; i < drow; i++)
        {
           
            CM.SpecialCardToField();


            yield return new WaitForSeconds(0.25f);
        }
        for (int i = 0; i < CD.size; i++)
        {
            t += characters[i].myPassive.SpecialDrow(drow);
            yield return new WaitForSeconds(t);
        }
    }
    public void ghostRevive(int ghostCount) //망자부활 + ghostCount
    {
        log.logContent.text += "\n망자부활 : " + ghostCount + "!";
        CharacterPassive q=null;
        for(int i = 0; i < characters.Count; i++)
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
    } //한번 손 봐야 함
    public void CopyCard(int CopyCount) //덱에 카드 복사
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
        log.logContent.text += "\n이번 턴 동안 "+character.Name +"의 공격력이 " + atk + "만큼 증가합니다.";
        character.AtkUp(atk);
    }
    public void AtkUp(int atk) //해당 전투동안 공격력 증가
    {
        log.logContent.text += "\n"+character.Name + "의 공격력이 " + atk + "만큼 증가합니다."; 
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
            yield return new WaitForSeconds(0.3f);
        }
        specialDrow(g);
      
    }
    public void GraveOn() //무덤 열기
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
        card.GetComponent<Card>().CancleRevive();
        for(int i = 0; i < CM.ReviveCard.Count; i++)
        {
            CM.ReviveCard[i].GetComponent<Transform>().localScale = new Vector2(1f, 1f);
        }
        CM.ReviveCard.Clear();
        graveClose.text = "닫기";
        
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
            card.GetComponent<Card>().SelectRevive();
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
    public void RandomReviveToField(int n) //랜덤으로 무덤에서 필드로 카드 가져오기
    {if (n > CM.Grave.Count)
            n = CM.Grave.Count;
        for(int i = 0; i < n; i++)
        {
            int rand = Random.Range(0, CM.Grave.Count);           
            CM.GraveToField(CM.Grave[rand]);

         
        }
    }
    public void ActUpCharacter(int c) //특정 캐릭터의 행동력을 증가시킴
    {
        for(int i = 0; i < characters.Count; i++)
        {
            if (!characters[i].isDie&&characters[i].characterNo == c)
            {
                log.logContent.text += "\n" + characters[i].Name + "의 행동력이 1증가합니다.";
                characters[i].ActUp(1);
            }
        }
    }

    public bool card20done=false;
    GameObject copyCard;
    public void card20Active()
    {
        c20 = card;
        otherCanvasOn = true;
        copyCard = Instantiate(pcard, CM.HandCanvas.transform);
        copyCard.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -230, 0);
        copyCard.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        copyCard.SetActive(true);
        CancleButton.SetActive(true);
        copyCard.GetComponent<Card>().iscard20Mode = true;
        copyCard.GetComponent<Card>().cardcost = 0;
        card = copyCard;
        useButton.SetActive(true);
    }
    public void cancleButtonUse()
    {
            Destroy(copyCard);
            otherCanvasOn = false;             
            CancleButton.SetActive(false);
            card = c20;
            c20 = null;
        
    }
    public void reflectUp(int r)
    {
        character.reflect+=r;
    }
 
    public void Victory()
    {
        otherCanvasOn = true;
        victory.SetActive(true);
        isV = true;
        int ignum = Random.Range(15, 26) * 10 + gd.victory * 20;
        if (gd.blessbool[15]) ignum *= 3;
        rewardIgnum.text = ignum + "이그넘 획득";
        gd.Ignum += ignum;    
        if (gd.blessbool[16]) listlength = 4;
        if (!gd.blessbool[9]) noSelect.SetActive(false);
        for (int i=0;i<data2.cd.Length; i++)
        {
           for(int j = 0; j < characters.Count; j++)
            {
                if (data2.cd[i].Deck == characters[j].characterNo && data2.cd[i].type != 2)
                {
                    RandomCardList.Add(i);
                }
            }
        }
        int rand = Random.Range(0, RandomCardList.Count);
        
        for(int i = 1; i <= listlength; i++)
        {
            int temp = RandomCardList[rand];
            RandomCardList[rand] = RandomCardList[RandomCardList.Count  - i];
            RandomCardList[RandomCardList.Count - i] = temp;
            rand = Random.Range(0, RandomCardList.Count-i);
        }
        for (int i = 1; i <= listlength; i++)
        {
            Debug.Log(RandomCardList[RandomCardList.Count - i]);
            GameObject newCard = Instantiate(RewardCardPrefebs, RewardCanvas.transform);
            newCard.GetComponent<NoBattleCard>().setCardInfoInLobby(RandomCardList[RandomCardList.Count-i], 0);
            RancomSelectCard.Add(RandomCardList[RandomCardList.Count - i]);
        }
    }
    public void SelectReward()
    {
        CardData CardD;
        string path = Path.Combine(Application.persistentDataPath, "CardData.json");         
            string cardData = File.ReadAllText(path);
            CardD = JsonConvert.DeserializeObject<CardData>(cardData);
        
        for (int i = 0; i < RancomSelectCard.Count; i++)
        {
            Debug.Log(RancomSelectCard[i]);
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
        NoSelectAndMain();
    }
    public void NoSelectAndMain()
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
        string path3 = Path.Combine(Application.persistentDataPath, "GameData.json");
        string GameData = JsonConvert.SerializeObject(gd);
        File.WriteAllText(path3, GameData);
        Time.timeScale = 1;
        SceneManager.LoadScene("Lobby");
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

    public struct EnemyAct
    {
        public int type; /*
                    0->데미지 1->행감 2->방어도 3->체력 회복 4->상태이상 5->은신/무적
        그 외 는 해당 스크립트 가 아닌 각각 개별 스크립트에서 즉발로 처리 ex)공격력 증가
        행감,(은신 / 무적 ) 등은 선행동, 그 외에는 후 행동
        */
        public int mount; //데미지 , 방어도, 회복 량,상태이상 종류*10+상태이상 양(여기는 변경가능성 多,0>은신 1>무적 2>불사)
        public Character target; 
        public Enemy myEnemy;
        public Enemy targetEnemy;

        public EnemyAct(int type, int mount, Character target, Enemy myEnemy,Enemy targetEnemy) 
        {
            this.type = type;
            this.mount = mount;
            this.target = target;
            this.myEnemy = myEnemy;
            this.targetEnemy = targetEnemy;
           
        }
    }
    public List<EnemyAct> earlyActList = new List<EnemyAct>();
    public List<EnemyAct> lateActList = new List<EnemyAct>();

    //type==0 랜덤 대상 type==1 방어도 높은 적 우선 type==2 체력 높은 적 우선 type==3 방어도 있는 적 우선
    //type==4 모든 대상
    //ActM =>true 일 시 행동력 감소 포함
    public void HitFront(int dmg,int type,Enemy enemy,bool ActM)
    {
        bool Alive = false;
      
        for(int i = 0; i < forward.Count; i++)
        {
            if (!characters[i].isDie) Alive = true;
        }
        if (!Alive) {
          
            HitAll(dmg, type, enemy, ActM);
        }
        else
        {
            int rand2 = 0;
            if (type == 0)
            {
                rand2 = Random.Range(0, line);
                while (characters[rand2].isDie) rand2 = Random.Range(0, line);                                
                EnemyAct enemyAct = new EnemyAct(0, dmg, characters[rand2], enemy,null);
                lateActList.Add(enemyAct);
                if (ActM)
                {
                    EnemyAct enemyAct2 = new EnemyAct(1,1, characters[rand2], enemy, null);
                    earlyActList.Add(enemyAct2);
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

                EnemyAct enemyAct = new EnemyAct(0, dmg, MaxArmor[rand2], enemy, null);
                lateActList.Add(enemyAct);
                if (ActM)
                {
                    EnemyAct enemyAct2 = new EnemyAct(1, 1, MaxArmor[rand2], enemy, null);
                    earlyActList.Add(enemyAct2);
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
             

                EnemyAct enemyAct = new EnemyAct(0, dmg, MaxHp[rand2] , enemy, null);
                lateActList.Add(enemyAct);
                if (ActM)
                {
                    EnemyAct enemyAct2 = new EnemyAct(1, 1, MaxHp[rand2], enemy, null);
                    earlyActList.Add(enemyAct2);
                }
            }
            if (type == 3)
            {
                List<Character> HaveArmor = new List<Character>();
                for(int i = 0; i < forward.Count; i++)
                {
                    if (forward[i].Armor > 0) HaveArmor.Add(forward[i]);
                }
                if (HaveArmor.Count == 0) HitFront(dmg, 0, enemy,ActM);
                else
                {
                    rand2 = Random.Range(0, HaveArmor.Count);
                   

                    EnemyAct enemyAct = new EnemyAct(0, dmg,  HaveArmor[rand2], enemy, null);
                    lateActList.Add(enemyAct);
                    if (ActM)
                {
                        EnemyAct enemyAct2 = new EnemyAct(1, 1, HaveArmor[rand2], enemy, null);
                        earlyActList.Add(enemyAct2);
                    }
                }
            }
            if (type == 4)
            {
                for(int i = 0; i < line; i++)
                {

                    EnemyAct enemyAct = new EnemyAct(0, dmg, characters[i], enemy, null);
                    lateActList.Add(enemyAct);
                    if (ActM)
                    {
                        EnemyAct enemyAct2 = new EnemyAct(1, 1, characters[i], enemy, null);
                        earlyActList.Add(enemyAct2);
                    }
                }
            }
        }
    }
    public void HitAll(int dmg,int type,Enemy enemy,bool ActM)
    {
        int rand2 = 0;
        if (type == 0)
        {
            rand2 = Random.Range(0, characters.Count);
            while (characters[rand2].isDie) rand2 = Random.Range(0, characters.Count);

            EnemyAct enemyAct = new EnemyAct(0, dmg, characters[rand2], enemy, null);
            lateActList.Add(enemyAct);
            if (ActM)
            {
                EnemyAct enemyAct2 = new EnemyAct(1, 1, characters[rand2], enemy, null);
                earlyActList.Add(enemyAct2);
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
          

            EnemyAct enemyAct = new EnemyAct(0, dmg, MaxArmor[rand2] , enemy, null);
            lateActList.Add(enemyAct);
            if (ActM)
            {
                EnemyAct enemyAct2 = new EnemyAct(1, 1,MaxArmor[rand2], enemy, null);
                earlyActList.Add(enemyAct2);
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
        

            EnemyAct enemyAct = new EnemyAct(0, dmg,    MaxHp[rand2], enemy, null);
            lateActList.Add(enemyAct);
            if (ActM)
            {
                EnemyAct enemyAct2 = new EnemyAct(1, 1, MaxHp[rand2], enemy, null);
                earlyActList.Add(enemyAct2);
            }
        }
        if (type == 3)
        {
            List<Character> HaveArmor = new List<Character>();
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].Armor > 0) HaveArmor.Add(characters[i]);
            }
            if (HaveArmor.Count == 0) HitAll(dmg, 0, enemy,ActM);
            else
            {
                rand2 = Random.Range(0, HaveArmor.Count);
           

                EnemyAct enemyAct = new EnemyAct(0, dmg,   HaveArmor[rand2], enemy, null);
                lateActList.Add(enemyAct);
                if (ActM)
                {
                    EnemyAct enemyAct2 = new EnemyAct(1, 1,HaveArmor[rand2], enemy, null);
                    earlyActList.Add(enemyAct2);
                }
            }
        }
        if (type == 4)
        {
            for (int i = 0; i < characters.Count; i++)
            {
              

                EnemyAct enemyAct = new EnemyAct(0, dmg,  characters[i], enemy, null);
                lateActList.Add(enemyAct);
                if (ActM)
                {
                    EnemyAct enemyAct2 = new EnemyAct(1, 1, characters[i], enemy, null);
                    earlyActList.Add(enemyAct2);
                }
            }
        }
    }
    public void HitBack(int dmg,int type,Enemy enemy,bool ActM)
    {
        bool Alive = false;

        for (int i = 0; i < back.Count; i++)
        {
            if (!characters[i].isDie) Alive = true;
        }
        if (!Alive)
        {
            HitAll(dmg, type,enemy, ActM);
        }
        else
        {
            int rand2 = 0;
            if (type == 0)
            {
                rand2 = Random.Range(line, characters.Count);
                while (characters[rand2].isDie) rand2 = Random.Range(line, characters.Count);

                EnemyAct enemyAct = new EnemyAct(0, dmg, characters[rand2], enemy, null);
                lateActList.Add(enemyAct);
               
                if (ActM)
                {
                    EnemyAct enemyAct2 = new EnemyAct(1, 1, characters[rand2], enemy, null);
                    earlyActList.Add(enemyAct2);
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
             

                EnemyAct enemyAct = new EnemyAct(0, dmg,  MaxArmor[rand2], enemy, null);
                lateActList.Add(enemyAct);
                if (ActM)
                {
                    EnemyAct enemyAct2 = new EnemyAct(1, 1, MaxArmor[rand2], enemy, null);
                    earlyActList.Add(enemyAct2);
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
             

                EnemyAct enemyAct = new EnemyAct(0, dmg, MaxHp[rand2], enemy, null);
                lateActList.Add(enemyAct);
                if (ActM)
                {
                    EnemyAct enemyAct2 = new EnemyAct(1, 1, MaxHp[rand2], enemy, null);
                    earlyActList.Add(enemyAct2);
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
                    

                    EnemyAct enemyAct = new EnemyAct(0, dmg, HaveArmor[rand2], enemy, null);
                    lateActList.Add(enemyAct);
                    if (ActM)
                    {
                        EnemyAct enemyAct2 = new EnemyAct(1, 1, HaveArmor[rand2], enemy, null);
                        earlyActList.Add(enemyAct2);
                    }
                }
            }
            if (type == characters.Count)
            {
                for (int i = line; i < characters.Count; i++)
                {

                    EnemyAct enemyAct = new EnemyAct(0, dmg, characters[i], enemy, null);
                    lateActList.Add(enemyAct);
                    if (ActM)
                    {
                        EnemyAct enemyAct2 = new EnemyAct(1, 1, characters[i], enemy, null);
                        earlyActList.Add(enemyAct2);
                    }
                }
            }
        }
    }
    
    public void EnemyGetAromor(int mount,Enemy myEnemy,Enemy target)
    {
        EnemyAct enemyAct = new EnemyAct(2, mount, null, myEnemy, target);
        lateActList.Add(enemyAct);
    }

    public void EnemyGetHp(int mount, Enemy myEnemy, Enemy target)
    {
        EnemyAct enemyAct = new EnemyAct(3, mount, null, myEnemy, target);
        lateActList.Add(enemyAct);
    }


    public void EnemyStateChange(Enemy myEnemy,int mount) //0->은신 1->무적 2->불사
    {if (mount == 0) myEnemy.goingShadow = true;
        EnemyAct enemyAct = new EnemyAct(4, mount, null, myEnemy, null);
        earlyActList.Add(enemyAct);
    }

    public void EnemyFormationCollapse(Enemy myEnemy)
    {
        EnemyAct enemyAct = new EnemyAct(6, 0, null, myEnemy, null);
        earlyActList.Add(enemyAct);
    }
    public void card22()
    {
        card22c = character;
        card22on = true;
        for(int i = 0; i < characters.Count; i++)
        {
            if (!characters[i].isDie)
            {
                characters[i].getArmor(6);
            }
        }
    }
    public void card23()
    {
        for(int i = 0; i < characters.Count; i++)
        {
            characters[i].onMinusAct(characters[i].Act);
        }

        AllAttack(1, character,3);
     
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
   
    public void DeckComplete()
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
    void SlessOff()
    {
        Sless.SetActive(false);
    }
    
    
}
