﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public int Hp;
    public int maxHp;
    public int Armor;
    public int Atk;
    public TurnManager TM;
    public BattleManager BM;
    [SerializeField] TextMeshProUGUI HpT;
    [SerializeField] TextMeshProUGUI Board;
    [SerializeField] TextMeshProUGUI ArmorT;
    public int hitStack;
    public int dmgStack;
    public int nextTurnArmor;
    public bool isDie;
    public bool noDie;
    public bool power;
    public string Name;
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
                    if (BM.enemy != hit.collider.GetComponent<Enemy>())
                        BM.EnemySelect(hit.collider.gameObject);
                    else
                        BM.CancleEnemy();                 
                }
            }


        }

    }


    public void EnemyStartTurn()
    {
        power = false;
        Board.text = "";
    }
    public void EnemyEndTurn()
    {
      
        Invoke("TurnStart", 0.5f);
    }
    void TurnStart()
    {
        TM.PlayerTurnStart();
    }
    public void onHit(int dmg)
    {
     
        BM.Setting();
       dmgStack++;
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
            if (!power)
            {
                hitStack++;
                Hp -= dmg;
            }
        }
        if (Hp <= 0)
        {
            isDie = true;
            if (noDie)
            {
                Hp += dmg;
                power = true;
            }
        }
    }
    public void GetArmor(int arm)
    {
        nextTurnArmor += arm;
        Board.text += "Armor + " + arm + "\n";
    }
}
