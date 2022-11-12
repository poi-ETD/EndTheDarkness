﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public int Hp;
    public int maxHp;
    [HideInInspector] public int Armor;
    [HideInInspector] public int Atk;
    [HideInInspector]public TurnManager TM;
    [HideInInspector] public BattleManager BM;
    public TextMeshProUGUI Board;
    [HideInInspector] public int hitStack;
    [HideInInspector] public int dmgStack;
    [HideInInspector] public int nextTurnArmor;
    [HideInInspector] public bool isDie;
    [HideInInspector] public bool noDie;
    [HideInInspector] public bool power;
    [HideInInspector] public bool immortal;
    public string Name;
    int RecoverHp;
    [HideInInspector] public bool Shadow;
    int EndTurnDmg;
    EnemyInfo ei;
    public Slider hpSlider;
    public Image myImage;
    ActManager AM;

    public bool dieNotEnd;//해당 적이 죽어도 게임이 끝나지 않을 경우

    [HideInInspector] public int[] status = new int[10];

    public float speed;

    [HideInInspector] public float curTurnSpeed;

    public Sprite face;

    [HideInInspector] public bool isAct;
    public virtual void EnemySelectPattern()
    {
        Debug.Log("C");
    }
    public virtual void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        Hp = maxHp;
        ei = GameObject.Find("SelectEnemyInformation").GetComponent<EnemyInfo>();
        AM = GameObject.Find("ActManager").GetComponent<ActManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ei.setNull();
        }
    }

    public void StatusChange(int kind, int amount)
    {
        status[kind] += amount;
    }

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
        
    public void onClickEvent()
    {
        if (isDie) return;
        if (BM.cardSelectMode) return;
        if (Shadow) return;

        if (BM.EnemySelectMode)
        {
            Debug.Log("OnSelect");
            BM.EnemySelect(gameObject);
        }
    }
    public void onEnterEvnent()
    {
        //if (isDie || !BM.EnemySelectMode) return; // 적이 죽거나 적 선택이 필요한 카드로 인한 적 선택 모드가 아닐시 반응하지 않게 하는 코드
        if (isDie) return;//YC->적 선택 모드가 아닐 시에도 마우스를 가져다 대면 정보를 표시해야 함
        ei.setThis(this);
    }
    public void onExitEvent()
    {
        if (isDie)
        {
            ei.g.SetActive(false);
            return;
        }
        //if (!BM.EnemySelectMode) return; // 적 선택이 필요한 카드로 인한 적 선택 모드가 아닐시 반응하지 않게 하는 코드
        //위와 동일
        ei.setNull();
    }
    public virtual void EnemyStartTurn()
    {
        if (isDie) return;
        if (Shadow&&!isDie)
        {
            Shadow = false;
            myImage.color = new Color(1, 1, 1, 1);
        }
        power = false;
        immortal = false;
    }
    public void EnemyEndTurn()
    {             
        //AM.EarlyAct();
    }
  
   
   
    public void OnHitCal(int dmg, int no, bool reply)
    {
        if (isDie) return;
        AM.MakeAct(2, 0, dmg, null, this, null, null, 1);
        BM.characters[no].myPassive.MyAttack();
        for (int i = 0; i < BM.ChD.size; i++)
        {
            if (!reply)
            {
                BM.characters[i].myPassive.EnemyHit(this);
            }
            else
            {
                BM.characters[i].myPassive.EnemyHitBySparky(this);
            }
        }

    }
    public virtual void onHit(int dmg)
    {
        if (isDie) return;
        if (status[(int)Status.weak] > 0)
        {
            status[(int)Status.weak]--;
            dmg *= 2;
        }
        GameObject Dmg = Instantiate(BM.DmgPrefebs, transform);
        Dmg.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Dmg.GetComponent<DMGtext>().GetType(0, dmg);      
        if (!power)
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
                if (!power)
                {
                 
                    Hp -= dmg;
                    if (Hp > maxHp)
                        Hp = maxHp;
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
                        if (!e[i].GetComponent<Enemy>().isDie&&!e[i].GetComponent<Enemy>().dieNotEnd)
                        {
                            V = false;
                        }
                    }
                    die();
                    if (V&&!BM.isVictoryPopupOn) BM.Victory();
                    Hp = 0;
                    Color color = new Color(0.3f, 0.3f, 0.3f);
                    myImage.color = color;
                    hpSlider.transform.Find("Fill Area").gameObject.SetActive(false);
                }
            }
        }
        hpSlider.value= Hp / (float)maxHp;
       
    }
    public void GetArmorStat(int arm)
    {
        GameObject Dmg = Instantiate(BM.DmgPrefebs, transform);
        Dmg.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Dmg.GetComponent<DMGtext>().GetType(2,arm);
        Armor += arm;
    }


    public void GetArmor(int arm,string enemyname)
    {
        
        nextTurnArmor += arm;
        string newstring = "<sprite name="+enemyname+"><sprite name=armor>" + arm + "\n";
        Board.text += newstring;
    }
    List<int> HpI = new List<int>();
    List<string> HpS = new List<string>();


    public void GetHp(int amount)
    {
        Hp += amount;
        if (Hp > maxHp) Hp = maxHp;
        hpSlider.value = Hp / (float)maxHp;

    }
    public void GetAtk(int amount)
    {
        GameObject Dmg = Instantiate(BM.DmgPrefebs, transform);
        Dmg.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Dmg.GetComponent<DMGtext>().GetType(4, amount);
        Atk += amount;
    }
  public void GetSpeed(int amount)
    {
        GameObject Dmg = Instantiate(BM.DmgPrefebs, transform);
        Dmg.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Dmg.GetComponent<DMGtext>().GetType(1, amount/100);
        speed -= amount / 100;
    }
    public void onShadow()
    {
        
        Shadow = true;      
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
        hpSlider.value = Hp / (float)maxHp;
    }
    public virtual void die()
    {
        Hp = 0;
        
    }
}

