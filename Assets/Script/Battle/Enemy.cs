using System.Collections;
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
    public string Name;
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
            if (hit.collider != null&&!isDie)
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
            if (noDie)
            {
                Hp += dmg;
                power = true;
            }
            else
            {
                isDie = true;
                Hp = 0;
                Color color = new Color(0.3f, 0.3f, 0.3f);
                GetComponent<Image>().color = color;
               
            }
        }
    }
    public void GetArmor(int arm)
    {
     
        nextTurnArmor += arm;
        Board.text += "Armor + " + arm + "\n";
    }
}
