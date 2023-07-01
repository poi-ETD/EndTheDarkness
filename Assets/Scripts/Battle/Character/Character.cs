using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Character : MonoBehaviour
{
    public int characterNo; 
    public int maxHp;
    public int Hp;
    public int atk;
    public int armor;
    public int turnAtk;
    public int def;
    public int turnDef;


    public float speed;
    public float curSpeed;
    public float curTurnActTime;


    public TurnManager TM;
    public BattleManager BM;
    [SerializeField] private TextMeshProUGUI text_Name; // YH
    public TextMeshProUGUI hpT; // YH : 배틀 씬 내의 좌측 캐릭터 정보 인스턴스 내의 HP 텍스트
    public TextMeshProUGUI atkT; // YH : 배틀 씬 내의 좌측 캐릭터 정보 인스턴스 내의 ATK 텍스트
    public TextMeshProUGUI defT; // YH : 배틀 씬 내의 좌측 캐릭터 정보 인스턴스 내의 DEF 텍스트
    public TextMeshProUGUI spdT; // YH : 배틀 씬 내의 좌측 캐릭터 정보 인스턴스 내의 SPD 텍스트

    public TextMeshProUGUI text_Armor; // YH : 배틀 씬 내의 좌측 캐릭터 정보 인스턴스 내의 방어도 텍스트
    public TextMeshProUGUI text_Ghost; // YH : 배틀 씬 내의 좌측 캐릭터 정보 인스턴스 내의 망자부활 텍스트
    public TextMeshProUGUI text_Blood; // YH : 배틀 씬 내의 좌측 캐릭터 정보 인스턴스 내의 출혈 텍스트
    public TextMeshProUGUI text_Weak; // YH : 배틀 씬 내의 좌측 캐릭터 정보 인스턴스 내의 약화 텍스트

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
    public int[] status=new int[20];
    public int curNo;
    public CharacterPassive myPassive;

    public int stringHp;
    public int stringArmor;
    //0 -> 중독

    public int beforeArmor;

    public int lobbyNum;

    [SerializeField] SO_CharacterList so_CharacterList;
    
    public void SetCharacterInPrefebs()
    {
        Debug.Log(characterNo);
        text_Name.text = Name;
        myImage.sprite = so_CharacterList.characterDetails[characterNo].sprite_Face;
    }
    public void StatusChange(int type,int mount)
    {
        status[type] += mount;
		if(type == (int)Status.blood)
		{
			text_Blood.text=status[type].ToString();
		}
		else if(type==(int)Status.weak)
		{
			text_Weak.text=status[type].ToString();
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
        armor += a;
        if (armor < 0) armor = 0;
        if (bless[2])
        {
            armor = 0;
        }
        stringArmor = armor;
		text_Armor.text = armor.ToString();
        myPassive.GetArmor(a);
    }
    public void ArmorValueGoZero()
    {
        armor = 0;
        stringArmor = armor;
       // armorT.text = "" + armor;
    }

    public void onClickEvent()
    {
        if (isDie) return;

        if (BM.characterSelectMode)
            BM.CharacterSelect(gameObject);

    }

    private void Start()
    {
        
    }
    
    public void AfterInstantiate()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myPassive = GetComponent<CharacterPassive>();

        if (BM.ChD.characterDatas[lobbyNum].curEquip != -1)
        {
            equipment myEquip = BM.GD.EquipmentList[BM.ChD.characterDatas[lobbyNum].curEquip];
            if (myEquip.special == 0)
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
                            speed += myEquip.improveMount[i] * 0.1f;
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
                    case 4:
                        speed -= myEquip.degradeMount * 0.1f;
                        break;
                }

            }
            else //전용 장비
            {
                if (myEquip.special == characterNo)
                    myPassive.haveMyEquip = true;
            }
        }

        spdT.text = "" + speed;
        defT.text = "" + def;

        if (Hp <= 0) die();
        if (Hp > maxHp) Hp = maxHp;

        stringHp = Hp;
        hpT.text = "<color=#a39fff><b>" + Hp + "</color></b><size=15>/" + maxHp + "</size>";
    }
    public void OnSpeedText(float amount)
    {

            SpeedTextChange();
            GameObject dmgText = Instantiate(BM.DmgPrefebs, gameObject.transform);
            dmgText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            dmgText.GetComponent<DMGtext>().GetType(1, amount);
      
    }
    public void SetTurnAtk()
    {
        turnAtk = atk;
        if (bless[6])
        {
            turnAtk = 1;
        }
        atkT.text = turnAtk + "";
    }
    public void TurnAtkUp(int i)
    {
        
        turnAtk += i;
        int newAtk = i;
        if (characterNo == 6 && passive[0] > 0)
        {
            for (int j = 0; j < passive[0]; j++)
            {                
                turnAtk += newAtk;
                newAtk += newAtk;
            }
        }
        if (bless[6])
        {
            turnAtk = 1;
        }
        atkT.text = turnAtk + "";
    }


    public void AtkUp(int i)
    {
        
        atk += i;
        turnAtk += i;
        int newAtk = i;
        if (characterNo == 6 && passive[0] > 0)
        {
            for (int j = 0; j < passive[0]; j++)
            {
               
                atk += newAtk;
                turnAtk += newAtk;
                newAtk += newAtk;
            }
        }
        if (bless[6])
        {
            atk = 1;
            turnAtk = 1;
        }
        atkT.text = turnAtk + "";
    }

    public void onHit(int dmg,Enemy E)
    {
		if (dmg <= 0) return;

		BM.log.logContent.text+="\n"+Name+"이(가) "+E.Name+"에게 "+dmg+"의 피해를 입었다.";
        for (int i = 0; i < BM.ChD.size; i++)
        {
            if (i == curNo) myPassive.MyHit(E,dmg);
            else { BM.characters[i].myPassive.TeamHit(curNo);}
        }
        if (armor > 0)
        {
            int startArmor = armor;
            armor -= dmg;
            if (armor < 0)
            {
                Hp += armor;
                armor = 0;
            }
            if (startArmor > dmg)
            {
                myPassive.MyArmorHit((armor) / 2, E);
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
    public void onHit(int dmg)
    {
		//최종 데미지가 0이하라면 x
        if (dmg <= 0) return;
        for (int i = 0; i < BM.ChD.size; i++)
        {
            if (i == curNo) myPassive.MyHit(null, dmg);
            else { BM.characters[i].myPassive.TeamHit(curNo); }
        }
        if (armor > 0)
        {
            int startArmor = armor;
            armor -= dmg;
            if (armor < 0)
            {
                Hp += armor;
                armor = 0;
            }
            if (startArmor > dmg)
            {
                myPassive.MyArmorHit((armor) / 2, null);
            }
            else
            {
                myPassive.MyArmorHit((dmg) / 2, null);
            }
        }
        else
        {
            Hp -= dmg;

        }

    }
    public void SpeedTextChange()
    {
        spdT.text = "" + speed;
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
       // armorT.text = stringArmor + "";
        if (stringHp <= 0)
        {
            stringHp = 0;
            if (!isDie)
            {
                stringHp = 0;
                die();
            }
        }
        if (Hp < 0) Hp = 0;
        hpT.text = "<color=#a39fff><b>" + stringHp + "</color></b><size=15>/" + maxHp + "</size>";           
    }
    public void MaxHpChange(int amount)
    {
        maxHp += amount;
        if (Hp > maxHp) Hp = maxHp;
        hpT.text = "<color=#a39fff><b>" + Hp + "</color></b><size=15>/" + maxHp + "</size>";
    }
    public void Recover(int amount)
    {
        Hp += amount;
        if (Hp > maxHp) Hp = maxHp;
        hpT.text = "<color=#a39fff><b>" + Hp + "</color></b><size=15>/" + maxHp + "</size>";
    }
    void die()
    {
        isDie = true;
        Hp = 0;
        
        hpT.text = "<color=#a39fff><b>" + Hp + "</color></b><size=15>/" + maxHp + "</size>";
        Color color = new Color(0.3f, 0.3f, 0.3f);
        myImage.color = color;
       
        armor = 0;
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
    public void act()
    {

    }

	public void onEnterEvnent()
	{
		//if (isDie || !BM.EnemySelectMode) return; // 적이 죽거나 적 선택이 필요한 카드로 인한 적 선택 모드가 아닐시 반응하지 않게 하는 코드
		if (isDie) return;//YC->적 선택 모드가 아닐 시에도 마우스를 가져다 대면 정보를 표시해야 함
		BM.selectedCharacter = this;
	}
	public void onExitEvent()
	{
		BM.selectedCharacter = null;
	}
}
