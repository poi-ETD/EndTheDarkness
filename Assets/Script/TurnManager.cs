using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurnManager : MonoBehaviour
{
    public bool PlayerTurn;
    [SerializeField] Enemy enemy;
    public int t;
    public GameObject EndButton;
    public BattleManager BM;
    public Text turnText;
    public int turnCard;
    public CardManager CM;
    public int leftCost;
    private void Awake()
    {
     
        t = 1;
        turnText.text = "현재 턴 : " + t;
        PlayerTurn = false;
        enemy.EnemyStartTurn();
        EndButton.SetActive(false);
    }
 
    public void PlayerTurnEnd()
    {
        leftCost = BM.cost;
        for (int i = 0; i < 4; i++)
        {
            if (!BM.characters[i].isDie&&BM.characters[i].card8)
            {
                BM.characters[i].card8 = false;
                BM.characters[i].Armor += leftCost * BM.characters[i].card8point;
            }
        }
        turnCard = 0;
        PlayerTurn = false;
        enemy.EnemyStartTurn();
        EndButton.SetActive(false);
        BM.CharacterSelectMode = false;
        BM.EnemySelectMode = false;
        enemy.Armor += enemy.nextTurnArmor;
        enemy.nextTurnArmor = 0;
        for(int i = 0; i < 4; i++)
        {if (!BM.characters[i].isDie)
            {
               for(int j=0;j< BM.characters[i].DMGboards.Count; j++)
                { int count = -1;
                    while (BM.characters[i].DMGboards[j].count != count)
                    {
                        BM.characters[i].onDamage(BM.characters[i].DMGboards[j].dmg, BM.characters[i].DMGboards[j].name);
                        count++;
                    }
                }
                BM.characters[i].DMGboards.Clear();

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
                BM.characters[i].NextTurnMinusAct = 0;
                BM.characters[i].turnAtk = BM.characters[i].Atk;
                BM.characters[i].Armor += BM.characters[i].nextarmor;
                BM.characters[i].nextarmor = 0;      
            }
        }
  
        BM.TurnStart();
    }

}
