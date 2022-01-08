using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
public class BattleManager : MonoBehaviour
{
    public Character[] characters;
    public bool CharacterSelectMode;
    public bool EnemySelectMode;
    public Character character;
    public Enemy enemy;
    public int cost;
    public GameObject card;
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
    [SerializeField] GameObject targetI;
    [SerializeField] GameObject costI;
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
    [SerializeField] GameObject[] Enemys;
    public BattleData bd;
    private void Awake()
    {
        string path = Path.Combine(Application.dataPath, "battleData.json");
        string battleData = File.ReadAllText(path);
        bd = JsonUtility.FromJson<BattleData>(battleData);
        GameObject EnemySummon = Instantiate(Enemys[bd.battleNo], new Vector2(0, 6.5f), transform.rotation, GameObject.Find("CharacterCanvas").transform);      
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
    private void Update()
    {
       
            if (Input.GetKey("escape"))
                Application.Quit();
            
        costT.text = "cost:" + cost;
        if (card == null)
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
  public void Complete()
    {
        completeButton.SetActive(false);
        if (card12On)
        {
            specialDrow(toGraveCount-1);
            toGraveCount = 0;
            card12On = false;
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

    public void TargetOn()
    {
        allClear();   
        targetI.SetActive(true);
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
        targetI.SetActive(false);
    }
    public void costOver()
    {
        allClear();
        costI.SetActive(true);
        Invoke("costOverOff", 1f);
    }
    void costOverOff()
    {
        costI.SetActive(false);
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
            GameObject newCard = Instantiate(card, new Vector3(0, 0, 0), transform.rotation);
            newCard.GetComponent<Card>().use = false;
            newCard.GetComponent<Card>().isUsed = false;
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
        log.logContent.text += "\n리셋!";
        card12On = true;
        completeButton.SetActive(true);
        
    }
    public void GraveOn()
    {
        otherCanvasOn = true;
        CM.GraveOn();
        graveView.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
    }
    public void GraveOff()
    {
        card7mode = false;
        CM.GraveOff();
        ReviveCount = 0;
        otherCanvasOn = false;
        cancleCard();
        CancleCharacter();
        CancleEnemy();
        graveView.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 1000, 0);
    }
    public void ReviveToField(int r)
    {
        
        GraveOn(); 
        ReviveCount += r;
    }
 
}
