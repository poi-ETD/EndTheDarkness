using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Character : MonoBehaviour
{
    public int characterNo; 
    public int maxHp;
    public int Hp;
    public int Atk;
    public int Armor;
    public int Act;
    public int turnAtk;
    public int endur;
    public int turnEndur;
    public TurnManager TM;
    public BattleManager BM;
    public TextMeshProUGUI hpT;
    public TextMeshProUGUI atkT;
    public TextMeshProUGUI armorT;
    public TextMeshProUGUI actT;
    public TextMeshProUGUI endurT;
    public TextMeshProUGUI board;

    public int[] passive;


    public string enemyName;
    public int AttackCount;
    public bool[] bless = new bool[20];
    public GameObject SelectBox;
    public Image myImage;
  
 
    public int cost;

    public int hitStack;
    public int dmgStack;
    public int NextTurnMinusAct;
    public bool isDie;
    public int nextarmor;
    public bool card8;
    public int card8point;


    public string Name;
    public int reflect;
    public int[] Status=new int[20];
    public int[] nextStatus = new int[20];
    public int curNo;
    public CharacterPassive myPassive;
    //0 -> 중독

    public int beforeArmor;
    public void useAct(int i)
    {
        Act -= i;
        if (Act < 0) Act = 0;
        actT.text = "" + Act;
    }
 
    public void Acting()
    {
        if (Status[0] > 0)
        {
            //onDamage(Status[0],null);
        }
    }
    public void getArmor(int a)
    {
       
        if (a > 0)
        {
            GameObject Dmg = Instantiate(BM.DmgPrefebs, transform);
            Dmg.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            Dmg.GetComponent<DMGtext>().GetType(2, a);
        }
        Armor += a;
        if (Armor < 0) Armor = 0;
        if (bless[2])
        {
            Armor = 0;
        }
        armorT.text = "" + Armor;
    }
    public void StatusAbnom(int status,int count)
    {
        nextStatus[status] += count;
    }
    // Start is called before the first frame update

    public void onClickEvent()
    {
      
        if (!isDie && !BM.SelectMode && !BM.EnemySelectMode)
        {
            if (BM.character != this)
            {
                BM.CharacterSelect(gameObject);
            }
            else
                BM.CancleCharacter();
        }
    }
    private void Start()
    {
        Act = 1;
        actT.text = "" + Act;
        //endurT.text = "" + endur;

        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myPassive = GetComponent<CharacterPassive>();
        myImage = transform.GetChild(7).GetComponent<Image>();
   
        if (Hp <= 0) die();
        if (Hp > maxHp) Hp = maxHp;
        hpT.text = "<color=purple><b>" + Hp + "</color></b><size=15>/" + maxHp + "</size>";
    }
    
    public void BoardClear()
    {
        board.text = "";     
        string newstring = "";
        if (Status[0] != 0)
            {
                newstring = "<sprite name=poison>" + Status[0] + "\n";
            }
        board.text += newstring;        
    }

  
    public void onMinusAct(int i)
    {
        Act -= i;
        if (Act < 0) Act = 0;
        actT.text = "" + Act;
        NextTurnMinusAct = 0;
    }
    public void AtkUp(int i)
    {
        turnAtk += i;
        if (bless[6])
        {
            turnAtk = 1;
        }
        atkT.text = turnAtk + "";
    }


    public void RealAtkUp(int i)
    {
        Atk += i;
        turnAtk += i;
        if (bless[6])
        {
            Atk = 1;
            turnAtk = 1;
        }
        atkT.text = turnAtk + "";
    }
    public void ActUp(int i)
    {      
        Act += i;
        actT.text = "" + Act;
    }

    public void onHit(int dmg,Enemy E)
    {
        if (dmg == 0) return;
        BM.log.logContent.text+="\n"+Name+"이(가) "+E.Name+"에게 "+dmg+"의 피해를 입었다.";
        for (int i = 0; i < BM.CD.size; i++)
        {
            if (i == curNo) myPassive.MyHit(E,dmg);
            else { BM.characters[i].myPassive.TeamHit(curNo);}
        }
        if (Armor > 0)
        {
            int startArmor = Armor;
            Armor -= dmg;
            if (Armor < 0)
            {
                Hp += Armor;
                Armor = 0;
            }
            if (startArmor > dmg)
            {
                myPassive.MyArmorHit((Armor) / 2, E);
            }
            else
            {
                myPassive.MyArmorHit((dmg) / 2, E);
            }          
        }
        else
        {
            Hp -= dmg;
        }
    }
    public void onDamage()
    {                                
        if (Hp <= 0)
        {
            if (!isDie)
            {
                Hp = 0;
                die();
            }
        }
        if (Hp < 0) Hp = 0;
        armorT.text = Armor + "";

        hpT.text = "<color=purple><b>" + Hp + "</color></b><size=15>/" + maxHp + "</size>";   

    }
    void die()
    {
        isDie = true;
        Hp = 0;
        hpT.text = "<color=purple><b>" + Hp + "</color></b><size=15>/" + maxHp + "</size>";
        Color color = new Color(0.3f, 0.3f, 0.3f);
        myImage.color = color;
        Act = 0;
        board.text = "";
        Armor = 0;
        BM.diecount++;
        if (BM.diecount == BM.characters.Count)
        {   Time.timeScale = 0;
            BM.Defetead();
        }
      /*  for(int i = 0; i < BM.forward.Count; i++)
        {
            if (BM.forward[i] == gameObject.GetComponent<Character>())
            {
                
                BM.forward.RemoveAt(i);
            }
        }*/
    }
}
