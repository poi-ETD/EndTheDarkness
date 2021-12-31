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
        turnCard = 0;
        PlayerTurn = false;
        enemy.EnemyStartTurn();
        EndButton.SetActive(false);
        BM.CharacterSelectMode = false;
        BM.EnemySelectMode = false; 
        for(int i = 0; i < 4; i++)
        {
            BM.characters[i].onDamage(BM.characters[i].DMG);
            BM.characters[i].DMG = 0;
            BM.characters[i].board.text = "";           
        }
        t++;
        BM.TurnEnd();
        BM.cancleCard();
        BM.CharacterCancle();
        BM.enemy = null;
        CM.FieldOff();
        turnText.text = "현재 턴 : " + t;
    }   
    public void PlayerTurnStart()
    {
        BM.cost = BM.startCost;
        PlayerTurn =true;
        BM.CharacterSelectMode = true;
        EndButton.SetActive(true);
        for(int i = 0; i < 4; i++)
        {
            BM.characters[i].Act = 1;
        }      
        BM.TurnStart();
    }

}
