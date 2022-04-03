using System.Collections;   
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TurnManager : MonoBehaviour
{
    public bool PlayerTurn;
    [SerializeField] GameObject[] enemys;
    [SerializeField] Enemy[] enemy;
    public int t;
    public GameObject EndButton;
    public BattleManager BM;
    public TextMeshProUGUI turnText;
    public int turnCard;
    public CardManager CM;
    public int leftCost;
    [SerializeField] GameObject pleaseSelect;
    public void turnCardPlus()
    {
        turnCard++;
        if (turnCard == 4)
        {
            for(int i = 0; i < CM.Grave.Count; i++)
            {
                if (CM.Grave[i].GetComponent<Card>().cardNo == 11)
                {
                    CM.Grave[i].GetComponent<Card>().decreaseCost(2);
                }
            }
            for (int i = 0; i < CM.field.Count; i++)
            {
                if (CM.field[i].GetComponent<Card>().cardNo == 11)
                {
                    CM.field[i].GetComponent<Card>().decreaseCost(2);
                }
            }
            for (int i = 0; i < CM.Deck.Count; i++)
            {
                if (CM.Deck[i].GetComponent<Card>().cardNo == 11)
                {
                    CM.Deck[i].GetComponent<Card>().decreaseCost(2);
                }
            }
            BM.log.logContent.text += "\n모든 스트레이트 펀치의 코스트가 2 감소합니다.";
        }
    }
    private void Awake()
    {
     
        t = 1;
        turnText.text = "" + t;
        PlayerTurn = false;       
        EndButton.SetActive(false);
    }
    private void Start()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemy = new Enemy[enemys.Length];
        for (int i = 0; i < enemys.Length; i++)
        {
            enemy[i] = enemys[i].GetComponent<Enemy>();
            enemy[i].EnemyStartTurn();
        }
    }

    public void PlayerTurnEnd()
    {
        if (!BM.SelectMode&&!BM.EnemySelectMode)
        {
            leftCost = BM.cost;
            for (int i = 0; i < BM.characters.Count; i++)
            {
                BM.characters[i].board.text = "";
                if (!BM.characters[i].isDie && BM.characters[i].card8)
                {
                    BM.characters[i].card8 = false;
                    BM.characters[i].getArmor(leftCost * BM.characters[i].card8point);
                }
            }
            turnCard = 0;
            PlayerTurn = false;
            for (int i = 0; i < enemy.Length; i++)
            {
                if (!enemy[i].isDie)
                {
                    enemy[i].EnemyStartTurn();
                    enemy[i].HpUp();
                    enemy[i].GetArmorStat(enemy[i].nextTurnArmor);
                    enemy[i].nextTurnArmor = 0;
                }
            }
            EndButton.SetActive(false);
            BM.CharacterSelectMode = false;
            BM.EnemySelectMode = false;
            for (int i = 0; i < BM.characters.Count; i++)
            {
                if (!BM.characters[i].isDie)
                {
                    for (int j = 0; j < BM.characters[i].DMGboards.Count; j++)
                    {
                        int count = -1;
                        while (BM.characters[i].DMGboards[j].count != count)
                        {
                            BM.characters[i].onDamage(BM.characters[i].DMGboards[j].dmg, BM.characters[i].DMGboards[j].name);
                            count++;
                        }
                    }
                    for(int j = 0; j < 20; j++) //상태이상 처리
                    {
                        BM.characters[i].Status[j] += BM.characters[i].nextStatus[j];
                        BM.characters[i].nextStatus[j] = 0;
                    }
                    BM.characters[i].DMGboards.Clear();
                    BM.characters[i].BoardClear();
                }
            }
            t++;
            
            for (int i = 0; i < BM.Enemys.Length; i++)
            {
               BM.Enemys[i].GetComponent<Enemy>().Board.text = "";
              
            }
            BM.TurnCardCount = BM.CardCount;
            BM.allClear();
            CM.FieldOff();
            turnText.text = "" + t;
        }
        else
        {
            pleaseSelect.SetActive(true);
            Invoke("PSoff", 1f);
        }
    }
    public int turnAtk;
    void PSoff()
    {
        pleaseSelect.SetActive(false);
    }
    public void PlayerTurnStart()
    {
        GameObject.Find("HandManager").GetComponent<HandManager>().isInited = false;
        BM.log.logContent.text += "\n" + t + "턴 시작!";
        BM.cost = BM.startCost+BM.nextTurnStartCost;
        BM.useCost(0);
        BM.nextTurnStartCost = 0;
        PlayerTurn =true;
        BM.CharacterSelectMode = true;
        EndButton.SetActive(true);
        for(int i = 0; i < BM.characters.Count; i++)
        {
            if (!BM.characters[i].isDie)
            {
             
               
                if (BM.BlessBM[4] && t == 1) BM.characters[i].Act = 0;
                if (BM.BlessBM[12] && t == 1) BM.characters[i].Act = 0;
                BM.characters[i].Act = 1;
                if (BM.BlessBM[4]) BM.characters[i].Act++;
                BM.characters[i].onMinusAct(BM.characters[i].NextTurnMinusAct);
                BM.characters[i].turnAtk = BM.characters[i].Atk;
                BM.characters[i].AtkUp(turnAtk);          
                BM.characters[i].getArmor(BM.characters[i].nextarmor);
                if (BM.characters[i].Armor < 0) BM.characters[i].Armor = 0;
                BM.characters[i].nextarmor = 0;
              
            }
        }
        turnAtk = 0;      
        if (BM.card22on)
        {
            int ArmorSum = 0;
            for (int i = 0; i < BM.characters.Count; i++)
            {
                if (!BM.characters[i].isDie)
                {
                    ArmorSum += BM.characters[i].Armor;
                    BM.characters[i].getArmor(-1 * BM.characters[i].Armor);
                }
            }
            BM.card22c.getArmor(ArmorSum);
            BM.card22on = false;
        }
        CM.TurnStartCardSet();
    }
  
}
