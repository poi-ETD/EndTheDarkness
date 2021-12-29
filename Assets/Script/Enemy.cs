using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int Hp;
    public int maxHp;
    public int Armor;
    public int Atk;
    public TurnManager TM;
    public BattleManager BM;
    [SerializeField] Text HpT;
    [SerializeField] Text Board;
    [SerializeField] Text ArmorT;
    private void Awake()
    {
        Hp = maxHp;
    }
    private void Update()
    {
        HpT.text = Hp + "/" + maxHp;
        ArmorT.text = "Armor:" + Armor;
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (BM.EnemySelectMode)
                    {
                        BM.EnemySelect(hit.collider.gameObject);
                    }
                    else if (!BM.EnemySelectMode)
                    {
                        if (BM.enemy == hit.collider.GetComponent<Enemy>())
                        {

                            BM.EnemySelectMode = true;
                            BM.enemy = null;
                        }
                        else
                        {
                            BM.EnemySelect(hit.collider.gameObject);
                        }
                    }
                }
            }


        }

    }


    public void EnemyStartTurn()
    {
        Board.text = "";
    }
    public void EnemyEndTurn()
    {
        TM.PlayerTurnStart();
    }
    public void onHit(int dmg)
    {
        if (Armor > 0)
        {
            Armor -= dmg;
            if (Armor < 0)
            {
                Hp += Armor;
                Armor = 0;
            }
        }
        else
        {
            Hp -= dmg;
        }
        if (Hp <= 0)
        {
            Debug.Log("폴리가 쓰러졌습니다.");
        }
    }
    public void GetArmor(int arm)
    {
        Armor += arm;
        Board.text += "Armor + " + arm + "\n";
    }
}
