﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public int Hp;
    public int maxHp;
    public int Armor;
    public int Atk;
    public TurnManager TM;
    public BattleManager BM;
    [SerializeField] TextMeshProUGUI HpT;
   public TextMeshProUGUI Board;
    [SerializeField] TextMeshProUGUI ArmorT;
    public int hitStack;
    public int dmgStack;
    public int nextTurnArmor;
    public bool isDie;
    public bool noDie;
    public bool power;
    public bool immortal;
    public string Name;
    int RecoverHp;
    public bool Shadow;
    int EndTurnDmg;
    public bool CanShadow()
    {
        bool can = false;
        for(int i = 0; i < BM.Enemys.Length; i++)
        {
            if (BM.Enemys[i] == gameObject)
                continue;
            if (!BM.Enemys[i].GetComponent<Enemy>().isDie && !BM.Enemys[i].GetComponent<Enemy>().Shadow)
            {
                can = true;
            }
        }
        return can;
    }
    private void Awake()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
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
            if (hit.collider != null&&!isDie&&!BM.SelectMode&&BM.EnemySelectMode&&!Shadow)
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
        Shadow = false;
        immortal = false;
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
        if (!power)
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
                if (immortal)
                {
                    Hp = 1;
                }
                else
                {
                    isDie = true;
                    bool V = true;
                    GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
                    for (int i = 0; i < e.Length; i++)
                    {
                        if (!e[i].GetComponent<Enemy>().isDie)
                        {
                            V = false;
                        }
                    }
                    if (V) BM.Victory();
                    Hp = 0;
                    Color color = new Color(0.3f, 0.3f, 0.3f);
                    GetComponent<Image>().color = color;

                }
            }
        }
    }
    public void GetArmor(int arm,string enemyname)
    {
        
        nextTurnArmor += arm;
        string newstring = "<sprite name="+enemyname+"><sprite name=armor>" + arm + "\n";
        Board.text += newstring;
    }
    List<int> HpI = new List<int>();
    List<string> HpS = new List<string>();
    public void GetHp(int amount,string enemyname)
    {
        
        HpS.Add(enemyname);
        HpI.Add(amount);
        string newstring = "<sprite name=" + enemyname + "><sprite name=recover>" + amount + "\n";
        RecoverHp = amount;
        Board.text += newstring;
    }
    public void GetDynamicHp(int amount,string enemyname)
    {
        
        string newstring = Board.text;
        bool isThere = false;
        for(int i = 0; i < HpS.Count; i++)
        {
            if (HpS[i] == enemyname)
            {
                isThere = true;
                int curR = HpI[i];
                if (amount > 0)
                {
                    newstring = newstring.Replace("<sprite name=" + enemyname + "><sprite name=recover>" + curR + "\n"
                    , "<sprite name=" + enemyname + "><sprite name=recover>" + amount + "\n");
                    HpS.RemoveAt(i);
                    HpI.RemoveAt(i);
                    HpS.Add(enemyname);
                    HpI.Add(amount);
                }
                else
                {
                    newstring = newstring.Replace("<sprite name=" + enemyname + "><sprite name=recover>" + curR + "\n"
                  , "");
                    HpS.RemoveAt(i);
                    HpI.RemoveAt(i);
                }
                RecoverHp -= curR;
                RecoverHp += amount;            
                break;
            }
        }
        if (!isThere && amount !=0)
        {
            RecoverHp += amount;
            HpS.Add(enemyname);
            HpI.Add(amount);
            newstring = "<sprite name=" + enemyname + "><sprite name=recover>" + amount + "\n";
        }
        Board.text = newstring;
    }
    public void onShadow()
    {
        Shadow = true;
        Board.text += "은신\n";
    }
    public void onEnemyHit(int dmg, string Name)
    {
        EndTurnDmg += dmg;
        
        string newstring = "<sprite name=" + Name+ "><sprite name=dmg>" + dmg + "\n";
        Board.text += newstring;
    }
    public void HpUp()
    {
        HpI.Clear();
        HpS.Clear();
        Hp -= EndTurnDmg;
        EndTurnDmg = 0;
        Hp += RecoverHp;
        RecoverHp = 0;
        if (Hp >= maxHp)
            Hp = maxHp;
    }
}

