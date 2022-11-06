using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Glassin003 : Enemy
{
  
    public int curTurn;
    public Enemy myEnemy;
    private int myTurn;
    private bool[] myAct = new bool[2];
    // YH
    public Image image_character;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;

    [SerializeField] TextMeshProUGUI NameT;

    public override void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy = GetComponent<Enemy>();
        myEnemy.Name = "글래신";
        NameT.text = myEnemy.Name;

    }
    
    private void Update()
    {
        if (myEnemy.isAct)
        {
            StartPattern();
            myEnemy.isAct = false;
        }
    }
    private void Escape() {
        BM.GD.isTriggerOn = true;
        BM.Victory();
    }
    public override void EnemyStartTurn()
    {
        base.EnemyStartTurn();
        myTurn = 0;
        curTurn++;
        if (curTurn == 5)
        {
            Escape();
        }
    }
    public override void onHit(int dmg)
    {

        base.onHit(dmg);
        
    }
    void StartPattern()
    {
        if (BM.teamDieCount < BM.characters.Count)
        {

            if (!myEnemy.isDie)
            {
                if (myTurn == 0)
                {
                    BM.EnemyStateChange(myEnemy, 0);
                }
                else
                {
                    int rand = Random.Range(0, 2);
                    while (myAct[rand])
                    {
                        rand = Random.Range(0, 2);
                    }
                    myAct[rand] = true;
                    if (rand == 0)
                    {
                        BM.EnemyGetHp(7, myEnemy, myEnemy);
                    }
                    else
                    {
                        BM.EnemyGetAromor(5, myEnemy, myEnemy);
                      
                    }
                    List<Character> allCharacters = BM.SelectCharacterListInEnemyTurn(2);
                    for (int i = 0; i < allCharacters.Count; i++)
                    {
                        BM.EnemyIncreaseSpeed(50, myEnemy, allCharacters[i]);
                    }
                    if (myAct[0] && myAct[1])
                    {
                        myAct[0] = false;
                        myAct[1] = false;
                    }
                }
                
            }
            myEnemy.BM.AM.EnemyAct();
        }
    }
}