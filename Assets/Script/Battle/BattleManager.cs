using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
public class BattleManager : MonoBehaviour
{
    public Character[] characters;
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
    float nowZ;
    public int StartCost;
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
    public BattleData bd;
    public CharacterData CD;
    [SerializeField] GameObject victory;
    [SerializeField] GameObject defeated;
    public bool SelectMode;
    [SerializeField] Character[] passiveCharacters;
    bool porte3mode;
    [SerializeField] GameObject condition;
    public Text conditionText;
    public bool GraveReviveMode;
    [SerializeField] Text graveClose;
    [SerializeField] Text StackT;
    [SerializeField] GameObject StackPopUp;
    public void StackPopUpOn()
    {
        if (StackPopUp.activeSelf)
        {
            StackPopUp.SetActive(false);
        }
        else
        { StackT.text = "";
            
            StackT.text += "이번 턴 사용한 카드 수:" + CM.TM.turnCard + "\n";
            for(int i = 0; i < 4; i++)
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
        SceneManager.LoadScene("Main");
    }
    private void Awake()
    {
        string path = Path.Combine(Application.dataPath, "battleData.json");
        string battleData = File.ReadAllText(path);
        bd = JsonUtility.FromJson<BattleData>(battleData);
        path = Path.Combine(Application.dataPath, "CharacterData.json");
        string characterData= File.ReadAllText(path);
        CD = JsonUtility.FromJson<CharacterData>(characterData);
        for(int i = 0; i < CD.passive.Length; i++)
        {
            if (CD.passive[i])
            {
                
                passiveCharacters[i / 3].passive[i % 3] = true;
            }
        }
        if (bd.battleNo == 0)
        {
            GameObject EnemySummon = Instantiate(Enemys[bd.battleNo], new Vector2(0, 6.5f), transform.rotation, GameObject.Find("CharacterCanvas").transform);
        }
        else if (bd.battleNo == 1)
        {
            GameObject EnemySummon = Instantiate(Enemys[bd.battleNo], new Vector2(3, 6f), transform.rotation, GameObject.Find("CharacterCanvas").transform);
        }
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        TurnCardCount = CardCount;
        nowZ = 1;
        for (int i = 0; i < 4; i++)
            startCost+= characters[i].cost;
        for(int i = 0; i < line; i++)
        {
            forward.Add(characters[i]);
        }
        for(int i = line; i < 4; i++)
        {
            back.Add(characters[i]);
        }
    }
    GameObject c20;
    private void Update()
    {if(pcard!=null)
        Debug.Log(pcard);
            if (Input.GetKey("escape"))
                Application.Quit();            
        costT.text = "cost:" + cost;
        if (card == null||porte3mode)
            useButton.SetActive(false);
        else useButton.SetActive(true);
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
                porte3mode = false;
                card = null;
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
            for (int i = 0; i < 4; i++)
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
            tra2 = c.GetComponent<Transform>();
            tra2.localScale = new Vector2(1.5f, 1.5f);
            card = c;
            c.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -230, 0);
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

    public void TurnStart()
    {
        pcharacter = null;
        pcard = null;
        penemy = null;
        log.logContent.text += "\n" + CM.TM.t + "턴 시작!";
        Setting();
        for (int i = 0; i < TurnCardCount; i++)
            CM.CardToField();
        for (int i = 0; i < 4; i++)
        {
         
            characters[i].isTurnStart = true;
        }

    }
    public void Setting()
    {
        for (int i = 0; i < 4; i++)
        {
            characters[i].isSet = true;
        }
    }
    public void TurnEnd()
    {
        TurnCardCount = CardCount;
        Setting();
        for (int i = 0; i < 4; i++)
        {
            characters[i].isTurnEnd = true;
        }
    }
    public void useCard()
    {
        if (card != null)
        {
            
            card.GetComponent<Card>().useCard();
           
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
        for (int i = 0; i < drow; i++)
            CM.SpecialCardToField();
    }
    public void ghostRevive(int ghostCount)
    {
        log.logContent.text += "\n망자부활 : " + ghostCount + "!";
        Q q=null;
        for(int i = 0; i < 4; i++)
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
        for (int i = 0; i < 4; i++)
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
        int g = CM.field.Count;
        for(int i = CM.field.Count - 1; i >= 0; i--)
        {if(card!=CM.field[i])
            CM.FieldToDeck(CM.field[i]);
        }
        specialDrow(g);
    }
    public void GraveOn()
    {
        otherCanvasOn = true;
        if (GraveReviveMode)
            graveClose.text = "부활";
        CM.GraveOn();
        graveView.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
    }
    public void GraveOff()
    {
        if (GraveReviveMode)
        {
            graveClose.text = "닫기";
            GraveReviveMode = false;
            card7mode = false;
            CM.Revive();
        }
        else
        {
            CM.GraveOff();
            otherCanvasOn = false;          
            cancleCard();
            CancleCharacter();
            CancleEnemy();
           
            graveView.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 1000, 0);
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
        for(int i = 0; i < 4; i++)
        {
            if (!characters[i].isDie&&characters[i].characterNo == c)
            {
                log.logContent.text += "\n" + characters[i].Name + "의 공격력이 1증가합니다.";
                characters[i].Act++;
            }
        }
    }
   public bool card20done=false;
    public void card20Active()
    {
        card20done = true;
        GameObject curCard = card;
        Card p = pcard.GetComponent<Card>();
        cost += p.cardcost;
        pcard.SetActive(true);
        p.isGrave = false;
        p.isUsed = false;
        p.useCard();
       
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
}
