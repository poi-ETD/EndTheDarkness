using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private void Awake()
    {
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
        if(tra1!=null)
        tra1.localScale = new Vector2(1, 1);
        character = null;
    }
    public void CharacterSelect(GameObject c)
    {
        CancleCharacter();
        tra1 = c.GetComponent<Transform>();
        tra1.localScale = new Vector2(1.2f, 1.2f);
        character = c.GetComponent<Character>();
       
    }
    public void EnemySelect(GameObject e)
    {
        CancleEnemy();       
        enemy = e.GetComponent<Enemy>();
        tra3 = e.GetComponent<Transform>();
        tra3.localScale = new Vector2(1.2f, 1.2f);
    }
    public void CancleEnemy()
    {
        if (tra3 != null)
            tra3.localScale = new Vector2(1, 1);
        enemy= null;
    }
    public void SetCard(GameObject c)
    {
        cancleCard();
        tra2 = c.GetComponent<Transform>();
        tra2.localScale = new Vector2(1.2f, 1.2f);
        card = c;
        nowZ = tra2.transform.position.z;
        tra2.transform.position = new Vector3(tra2.position.x, tra2.position.y, -6
          );
    }
    public void cancleCard()
    {
        EnemySelectMode = false;
        card = null;
        if (tra2 != null)
        {
            tra2.localScale = new Vector2(1, 1);
            tra2.transform.position = new Vector3(tra2.position.x, tra2.position.y, nowZ
                );
        }
    }

    public void TurnStart()
    {
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
        enemy.onHit(dmg + character.turnAtk);
    }
    public void getArmor(int armor)
    {
        character.Armor += armor;   
    }
    public void specialDrow(int drow)
    {
        
        for (int i = 0; i < drow; i++)
            CM.SpecialCardToField();
    }
    public void ghostRevive(int ghostCount)
    {
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
        for(int i = 0; i < CopyCount; i++)
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
        character.nextarmor += armor;
    }
    public void card8(int point)
    {
        character.card8 = true;
        character.card8point = point;
    }
    public void teamTurnAtkUp(int atk)
    {
        for(int i = 0; i < 4; i++)
        {
            if (!characters[i].isDie)
            {
                characters[i].turnAtk += atk;
            }
        }
    }
    public void AtkUp(int atk)
    {
        character.Atk += atk;
        character.turnAtk += atk;
    }
    int toGraveCount = 0;
    public void card12()
    {
        
        card12On = true;
        completeButton.SetActive(true);
        
    }
}
