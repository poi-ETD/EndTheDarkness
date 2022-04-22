using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Dagger001 : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    public BattleManager BM;
    public List<Character> HaveArmor = new List<Character>();
    public List<Character> ForwardHaveArmor = new List<Character>();
    [SerializeField] Text t;
    [SerializeField] int plusname;
    [SerializeField] Dagger001 anotherDagger;
    int pattern;
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
        myEnemy.Name = "단검" + plusname;
        NameT.text = myEnemy.Name;
        StartPattern();
    }
    private void Update()
    {
        if (curTurn != TM.t)
        {
            StartPattern();
        }
    }
    void StartPattern()
    {
        if (BM.diecount < BM.characters.Count)
        {
            if (!myEnemy.isDie)
            {
              
               if ((curTurn + 1) % 5 != 0)
                  {
                      pattern = Random.Range(0, 3);
                      if (plusname == 2)
                      {
                          while (pattern == anotherDagger.pattern)
                          {
                              pattern = Random.Range(0, 3);
                          }
                      }

                      if (pattern == 0)
                      {
                          BM.HitFront(3, 0, myEnemy, false);
                          BM.HitFront(3, 0, myEnemy, false);
                      }
                      if (pattern == 1)
                      {
                          BM.EnemyGetAromor(3, myEnemy, myEnemy);
                          BM.HitFront(3, 0, myEnemy, false);
                      }
                      if (pattern == 2)
                      {
                          
                          BM.HitBack(1, 0, myEnemy, true);
                      }
                  }
                  else
                  {
                      BM.HitFront(3, 0, myEnemy, false);
                      BM.HitFront(3, 0, myEnemy, false);
                      BM.HitFront(3, 0, myEnemy, false);
                      BM.HitBack(1, 0, myEnemy, false);
                  }
              }
           }

               
                myEnemy.EnemyEndTurn();

                curTurn++;
            }
        }
    
