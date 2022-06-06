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
    public int turnAct;
    public int turnAtk;
    public int def;
    public int turnDef;
    public int Act=1;

    public float speed;
    public int curSpeed;
    public bool isAct;

    public TurnManager TM;
    public BattleManager BM;
    public TextMeshProUGUI hpT;
    public TextMeshProUGUI atkT;
    public TextMeshProUGUI armorT;
    public TextMeshProUGUI actT;
    public TextMeshProUGUI defT;

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

    public int stringHp;
    public int stringArmor;
    //0 -> 중독

    public int beforeArmor;

    public int lobbyNum;

    
    public void useAct(int i)
    {
       // turnAct -= i;
        if (turnAct < 0) turnAct = 0;
        actT.text = "" + turnAct;
    }
 
    public void Acting()
    {
        if (Status[0] > 0)
        {
            //onDamage(Status[0],null);
        }
    }
    public void DefUp(int i)
    {
     
        def += i;
        turnDef += i;
        defT.text = turnDef + "";
    }
    public void TurnDefUp(int i)
    {
        turnDef += i;
        defT.text = turnDef + "";
    }
    public void getArmor(int a)
    {
           
        if (a != 0)
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
        stringArmor = Armor;
        armorT.text = "" + Armor;
        
    }
    public void StatusAbnom(int status,int count)
    {
        nextStatus[status] += count;
    }
    // Start is called before the first frame update

    public void onClickEvent()
    {
        return;
      
        if (!isDie && !BM.cardSelectMode && !BM.EnemySelectMode)
        {
            if (BM.selectedCharacter != this)
            {
                BM.CharacterSelect(gameObject);
            }
            else
                BM.CancleCharacter();
        }
    }
    private void Start()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        if (BM.ChD.characterDatas[lobbyNum].curEquip != -1)
        {
            equipment myEquip = BM.GD.EquipmentList[BM.ChD.characterDatas[lobbyNum].curEquip];
            bool cantEquip = false;
            if (myEquip.type == 1 && BM.ChD.characterDatas[lobbyNum].curFormation == 0)
            {
                cantEquip = true;
            }
            if (myEquip.type == 0 && BM.ChD.characterDatas[lobbyNum].curFormation == 1)
            {
                cantEquip = true;
            }

            if (!cantEquip)
            {
                for (int i = 0; i < myEquip.improveMount.Count; i++)
                {

                    switch (myEquip.improveStat[i])
                    {
                        case 0:
                            AtkUp(myEquip.improveMount[i]);
                            break;
                        case 1:
                            DefUp(myEquip.improveMount[i]);
                            break;
                        case 2:
                            maxHp += myEquip.improveMount[i];
                            break;
                        case 3:
                            cost += myEquip.improveMount[i];
                            break;
                        case 4:
                            Act++;
                            break;
                    }
                }
                switch (myEquip.degradeStat)
                {
                    case 0:
                        AtkUp(-myEquip.degradeMount);
                        break;
                    case 1:
                        DefUp(-myEquip.degradeMount);
                        break;
                    case 2:
                        maxHp -= myEquip.degradeMount;
                        break;
                    case 3:
                        cost -= myEquip.degradeMount;
                        break;

                }
            }
        }
        turnAct = Act;
        actT.text = "" + turnAct;
        defT.text = "" + def;
        //endurT.text = "" + endur;

      
        myPassive = GetComponent<CharacterPassive>();
        myImage = transform.GetChild(8).GetComponent<Image>();
      
        if (Hp <= 0) die();
        if (Hp > maxHp) Hp = maxHp;

        stringHp = Hp;
        hpT.text = "<color=#a39fff><b>" + Hp + "</color></b><size=15>/" + maxHp + "</size>";
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
        turnAct -= i;
        if (i > 0)
        {
            GameObject dmgText = Instantiate(BM.DmgPrefebs, gameObject.transform);
            dmgText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            dmgText.GetComponent<DMGtext>().GetType(1, i);
        }
        if (turnAct < 0) turnAct = 0;
        actT.text = "" + turnAct;
        NextTurnMinusAct = 0;
    }
    public void TurnAtkUp(int i)
    {
        
        turnAtk += i;
        if (bless[6])
        {
            turnAtk = 1;
        }
        atkT.text = turnAtk + "";
    }


    public void AtkUp(int i)
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
        turnAct += i;
        actT.text = "" + turnAct;
    }

    public void onHit(int dmg,Enemy E)
    {
        if (dmg == 0) return;
        BM.log.logContent.text+="\n"+Name+"이(가) "+E.Name+"에게 "+dmg+"의 피해를 입었다.";
        for (int i = 0; i < BM.ChD.size; i++)
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
    public void onDamage(int dmg)
    {
        GameObject dmgText = Instantiate(BM.DmgPrefebs, gameObject.transform);
        dmgText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        dmgText.GetComponent<DMGtext>().GetType(0, dmg);
      
        if (stringArmor > 0)
        {
            stringArmor -= dmg;
            if (stringArmor < 0)
            {
                stringHp += stringArmor;
                stringArmor = 0;
                
            }
        }
        else
        {
            stringHp -= dmg;
        }
        armorT.text = stringArmor + "";
        if (stringHp <= 0)
        {
            if (!isDie)
            {
                stringHp = 0;
                die();
            }
        }
        if (Hp < 0) Hp = 0;
        hpT.text = "<color=#a39fff><b>" + stringHp + "</color></b><size=15>/" + maxHp + "</size>";   

    }
    void die()
    {
        isDie = true;
        Hp = 0;
        
        hpT.text = "<color=#a39fff><b>" + Hp + "</color></b><size=15>/" + maxHp + "</size>";
        Color color = new Color(0.3f, 0.3f, 0.3f);
        myImage.color = color;
        turnAct = 0;
        board.text = "";
        Armor = 0;
        BM.teamDieCount++;
        if (BM.teamDieCount == BM.characters.Count)
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
