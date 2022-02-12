using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurnManager : MonoBehaviour
{
    public bool PlayerTurn;
    [SerializeField] GameObject[] enemys;
    [SerializeField] Enemy[] enemy;
    public int t;
    public GameObject EndButton;
    public BattleManager BM;
    public Text turnText;
    public int turnCard;
    public CardManager CM;
    public int leftCost;
    [SerializeField] GameObject pleaseSelect;
    private void Awake()
    {
     
        t = 1;
        turnText.text = "현재 턴 : " + t;
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
            for (int i = 0; i < 4; i++)
            {
                BM.characters[i].board.text = "";
                if (!BM.characters[i].isDie && BM.characters[i].card8)
                {

                    BM.characters[i].card8 = false;
                    BM.characters[i].Armor += leftCost * BM.characters[i].card8point;
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
                    enemy[i].Armor += enemy[i].nextTurnArmor;
                    enemy[i].nextTurnArmor = 0;

                }
            }
            EndButton.SetActive(false);
            BM.CharacterSelectMode = false;
            BM.EnemySelectMode = false;
            for (int i = 0; i < 4; i++)
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
                    BM.characters[i].DMGboards.Clear();
                    BM.characters[i].BoardClear();
                }
            }
            t++;
            BM.TurnEnd();
            BM.cancleCard();
            BM.CancleCharacter();
            BM.CancleEnemy();
            CM.FieldOff();
            turnText.text = "현재 턴 : " + t;
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
        BM.cost = BM.startCost+BM.nextTurnStartCost;
        BM.nextTurnStartCost = 0;
        PlayerTurn =true;
        BM.CharacterSelectMode = true;
        EndButton.SetActive(true);
        for(int i = 0; i < 4; i++)
        {
            if (!BM.characters[i].isDie)
            {
                BM.characters[i].Act = 1 - BM.characters[i].NextTurnMinusAct;
                if (BM.characters[i].Act < 0) BM.characters[i].Act = 0;
                BM.characters[i].NextTurnMinusAct = 0;
                BM.characters[i].turnAtk = BM.characters[i].Atk+turnAtk;             
                BM.characters[i].Armor += BM.characters[i].nextarmor;
                if (BM.characters[i].Armor < 0) BM.characters[i].Armor = 0;
                BM.characters[i].nextarmor = 0;
              
            }
        }
        turnAtk = 0;
        CM.TurnStartCardSet();
        BM.TurnStart();
    }

}
