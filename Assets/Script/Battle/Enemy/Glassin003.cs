using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Glassin003 : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;
    public BattleManager BM;
    [SerializeField] Enemy[] daggers;

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
        if (myEnemy.Hp <= 0)
        {
            Escape(true);
        }
    }
    void Escape(bool isDie)
    {
        if (isDie)
        {
            daggers[0].Atk = 0;
            daggers[1].Atk = 0;
        }
        daggers[0].GetComponent<Dagger003>().pattern++;
        daggers[1].GetComponent<Dagger003>().pattern++;
        gameObject.tag = "Untagged";
        BM.Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < BM.Enemys.Length; i++)
        {
            BM.Enemys[i].GetComponent<Enemy>().myNo = i;
        }
        Destroy(gameObject);

    }
    void StartPattern()
    {
        if (BM.teamDieCount < BM.characters.Count)
        {

            if (!myEnemy.isDie)
            {
                curTurn++;
                BM.HitFront(0, 4, myEnemy, 50);
                BM.EnemyAtkUp(daggers[0], 1, myEnemy);
                BM.EnemyAtkUp(daggers[1], 1, myEnemy);
                if (curTurn == 4) Escape(false);
            }
            myEnemy.BM.AM.EnemyAct();

            // myEnemy.EnemyEndTurn();


        }
    }
}