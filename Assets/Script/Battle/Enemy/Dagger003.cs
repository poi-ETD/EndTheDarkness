using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Dagger003 : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    public BattleManager BM;
    [SerializeField] int plusname;
    public int pattern;
    bool[] done = new bool[3];
    
    // YH
    public Image image_character;
    public Sprite sprite_idle;
    public Sprite sprite_highlight;

    [SerializeField] TextMeshProUGUI NameT;

    private void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy = GetComponent<Enemy>();
        myEnemy.Name = "단검" + plusname;
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
    void StartPattern()
    {
        if (BM.teamDieCount < BM.characters.Count)
        {
            if (!myEnemy.isDie)
            {
                int rand = Random.Range(0, pattern);
                if (rand == 0)
                {
                    BM.HitFront(1+myEnemy.Atk, 1, myEnemy, 0);
                    BM.HitFront(1 + myEnemy.Atk, 1, myEnemy, 0);
                    BM.HitFront(1 + myEnemy.Atk, 1, myEnemy, 0);
                }
                else if (rand == 1)
                {
                    BM.HitBack(1 + myEnemy.Atk, 0, myEnemy, 0);
                    BM.HitBack(1 + myEnemy.Atk, 0, myEnemy, 0);
                }
                else
                {
                    BM.HitFront(1 + myEnemy.Atk, 0, myEnemy, 0);
                    BM.HitFront(1 + myEnemy.Atk, 0, myEnemy, 0);
                    BM.HitFront(1 + myEnemy.Atk, 0, myEnemy, 0);
                    if (myEnemy.CanShadow())
                    {
                        BM.EnemyStateChange(myEnemy, 0);
                    }
                    else
                    {
                        BM.HitAll(2 + myEnemy.Atk, 4, myEnemy, 0);
                    }
                }
            }
        }
        myEnemy.BM.AM.EnemyAct();

        // myEnemy.EnemyEndTurn();


    }
}

