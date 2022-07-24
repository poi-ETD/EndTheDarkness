using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Ranger002 : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    public BattleManager BM;
    public List<Character> HaveArmor = new List<Character>();
    public List<Character> ForwardHaveArmor = new List<Character>();
    int[] pattern=new int[2];
    bool[] done = new bool[3];
    int myturn;

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
        myEnemy.Name = "레인져";
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
                if (pattern[0] > 0 && pattern[1] > 0)
                {
                    pattern[0] = 0;
                    pattern[1] = 0;
                    BM.HitFront(15, 1, myEnemy, 0);
                }
                else
                {
                    int rand = Random.Range(0, 2);
                    pattern[rand]++;
                    if (rand == 0)
                    {
                        BM.HitFront(3, 0, myEnemy, 0);
                        BM.HitBack(1, 2, myEnemy, 0);

                    }
                    else
                    {
                        BM.HitFront(5, 0, myEnemy, 0);
                        BM.EnemyGetAromor(3, myEnemy, myEnemy);
                    }
                }

            }
        }
        myEnemy.BM.AM.EnemyAct();

        // myEnemy.EnemyEndTurn();


    }
}

