using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Dagger002 : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    public BattleManager BM;
    public List<Character> HaveArmor = new List<Character>();
    public List<Character> ForwardHaveArmor = new List<Character>();
    bool pattern2done;
    
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
        myEnemy.Name = "단검";
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
            if (pattern2done)
            {
                BM.EnemyGetAromor(5, myEnemy, myEnemy);
                pattern2done = false;
            }
            else
            {
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    BM.EnemyAttack(3, myEnemy, BM.SelectCharacterInEnemyTurn(0, 1));
                    BM.EnemyAttack(3, myEnemy, BM.SelectCharacterInEnemyTurn(0, 0));
                }
                else
                {
                    if (myEnemy.CanShadow())
                    {
                        BM.EnemyStateChange(myEnemy, 0);
                    }
                    else
                    {
                        BM.EnemyAttack(3, myEnemy, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyAttack(3, myEnemy, BM.SelectCharacterInEnemyTurn(0, 0));
                        BM.EnemyAttack(3, myEnemy, BM.SelectCharacterInEnemyTurn(0, 0));
                    }
                    pattern2done = true;
                }
            }
        }
        myEnemy.BM.AM.EnemyAct();



    }
}
