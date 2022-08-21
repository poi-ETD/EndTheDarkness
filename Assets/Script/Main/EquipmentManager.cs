using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private static EquipmentManager instance;

    public static EquipmentManager Instance
    {
        get
        {
            if (null == instance)
            {
              
                instance = new EquipmentManager();
            }
            return instance;
        }
    }
    public Sprite[] equipSpr = { Resources.Load<Sprite>("temporal/axe_001"),
        Resources.Load<Sprite>("temporal/axe_002"),
Resources.Load<Sprite>("temporal/axe_003"),
Resources.Load<Sprite>("temporal/axe_004"),
Resources.Load<Sprite>("temporal/axe_005"),
Resources.Load<Sprite>("temporal/axe_006"),
Resources.Load<Sprite>("temporal/axe_007"),
Resources.Load<Sprite>("temporal/axe_008"),
Resources.Load<Sprite>("temporal/axe_009"),
Resources.Load<Sprite>("temporal/axe_010")
        };
    
    string[] prefix = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
    string[] equipName = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
    public equipment makeEquipment()
    {
   
        int degradeMount = 0;
        int improveMount = 0;
        int rand1 = Random.Range(0, 5);
        int rand2 = Random.Range(0, 4);

        while (rand1 == rand2) rand2 = Random.Range(0, 4);

        switch (rand1)
        {
            case (int)Enums.EquipmentStat.atk:
                improveMount = Random.Range(1, 3);
                break;
            case (int)Enums.EquipmentStat.def:
                improveMount = Random.Range(1, 4);
                break;
            case (int)Enums.EquipmentStat.maxHp:
                improveMount = Random.Range(1, 5) * 5;
                break;
            case (int)Enums.EquipmentStat.cost:
                improveMount = Random.Range(1, 3);
                break;
            case (int)Enums.EquipmentStat.act:
                improveMount = 1;
                break;
        }
        switch (rand2)
        {
            case (int)Enums.EquipmentStat.atk:
                degradeMount = Random.Range(1, 3);
                break;
            case (int)Enums.EquipmentStat.def:
                degradeMount = Random.Range(1, 4);
                break;
            case (int)Enums.EquipmentStat.maxHp:
                degradeMount = Random.Range(2, 5) * 5;
                break;
            case (int)Enums.EquipmentStat.cost:
                degradeMount = 1;
                break;
            
        }
        int t = Random.Range(1, 101);
        if (t <= 45)
        {
            t = 0;
        }
        else if (t <= 90) t = 1;
        else t = 2;
        int randPrefix = Random.Range(0, 10);
        int randEquip = Random.Range(0, 10);
        List<int> l1 = new List<int>();
        l1.Add(rand1);
        List<int> l2 = new List<int>();
        l2.Add(improveMount);
        equipment e = new equipment(t,0,"", prefix[randPrefix]+" "+equipName[randEquip],l1,l2, rand2, degradeMount,randEquip);
        return e;
    }
    public equipment makeSpecialEquipment(int characterNo)
    {       
        
        string Name = CharacterInfo.Instance.cd[characterNo].specialEquipName;
        string content = CharacterInfo.Instance.cd[characterNo].specialEquipContent;
        int t=0;
        if (characterNo == 1||characterNo==2||characterNo==4||characterNo==5)
        {
            t = 2;
        }
        else if (characterNo == 3)
        {
            t = 0;
        }
        equipment e = new equipment(t,characterNo,Name,content,null,null,0,0,0);
        return e;
    }
    public List<string> equipmentStrings(equipment e)
    {
        if (e.special == 0)
        {
            List<string> names = new List<string>();
            string equipName = e.equipName;
            names.Add(equipName);
            string s = "착용 조건: ";
            switch (e.type)
            {
                case 0:
                    s += "전방";
                    break;
                case 1:
                    s += "후방";
                    break;
                case 2:
                    s += "전체";
                    break;
            }
            names.Add(s);
            s = "증가 스탯:";
            for (int i = 0; i < e.improveStat.Count; i++)
            {
                if (i > 0) s += "\n";
                switch (e.improveStat[i])
                {
                    case (int)Enums.EquipmentStat.atk:
                        s += " 공격력(" + e.improveMount[i] + ")";
                        break;
                    case (int)Enums.EquipmentStat.def:
                        s += " 지구력(" + e.improveMount[i] + ")";
                        break;
                    case (int)Enums.EquipmentStat.maxHp:
                        s += " 최대 HP(" + e.improveMount[i] + ")";
                        break;
                    case (int)Enums.EquipmentStat.cost:
                        s += " 부여 코스트(" + e.improveMount[i] + ")";
                        break;
                    case (int)Enums.EquipmentStat.act:
                        s += " 행동력(" + e.improveMount[i] + ")";
                        break;
                }
            }
            names.Add(s);
            s = "감소 스탯:";
            switch (e.degradeStat)
            {
                case (int)Enums.EquipmentStat.atk:
                    s += " 공격력(" + e.degradeMount + ")";
                    break;
                case (int)Enums.EquipmentStat.def:
                    s += " 지구력(" + e.degradeMount + ")";
                    break;
                case (int)Enums.EquipmentStat.maxHp:
                    s += " 최대 HP(" + e.degradeMount + ")";
                    break;
                case (int)Enums.EquipmentStat.cost:
                    s += " 부여 코스트(" + e.degradeMount + ")";
                    break;

            }
            names.Add(s);
            return names;
        }
        else
        {
            List<string> names = new List<string>();
            names.Add(e.equipName);
            names.Add(e.equipContent);
            return names;
        }
     
    }
    public equipment ResetEquipment(equipment e)
    {
        int degradeMount = 0;
        int improveMount = 0;
        int rand1 = Random.Range(0, 5);
        int rand2 = Random.Range(0, 4);
        while (rand1 == rand2) rand2 = Random.Range(0, 4);
        switch (rand1)
        {
            case (int)Enums.EquipmentStat.atk:
                improveMount = Random.Range(1, 3);
                break;
            case (int)Enums.EquipmentStat.def:
                improveMount = Random.Range(1, 4);
                break;
            case (int)Enums.EquipmentStat.maxHp:
                improveMount = Random.Range(1, 5) * 5;
                break;
            case (int)Enums.EquipmentStat.cost:
                improveMount = Random.Range(1, 3);
                break;
            case (int)Enums.EquipmentStat.act:
                improveMount = 1;
                break;
        }
        switch (rand2)
        {
            case (int)Enums.EquipmentStat.atk:
                degradeMount = Random.Range(1, 3);
                break;
            case (int)Enums.EquipmentStat.def:
                degradeMount = Random.Range(1, 4);
                break;
            case (int)Enums.EquipmentStat.maxHp:
                degradeMount = Random.Range(2, 5) * 5;
                break;
            case (int)Enums.EquipmentStat.cost:
                degradeMount = 1;
                break;

        }

        int t = Random.Range(1, 101);
        
        if (t <= 45)
        {
            t = 0;
        }
        else if (t <= 90) t = 1;
        else t = 2;
        List<int> l1 = new List<int>();
        l1.Add(rand1);
        List<int> l2 = new List<int>();
        l2.Add(improveMount);
        equipment newE = new equipment(t,0,"", e.equipName, l1, l2, rand2, degradeMount, e.equipNum);
        return newE;
    }
    public equipment AddEquipments(equipment e1,equipment e2)
    {
        int randPrefix = Random.Range(0, 10);
        int randEquip = Random.Range(0, 10);
        int rand = Random.Range(0, 2);//0일시 e1이 상승 옵, e2가 하락 옵, 1일 시 반대
        
        
        List<int> l1 = e1.improveStat; 
        List<int> l2 = e1.improveMount; 
        int rand2 = e2.degradeStat; 
        int degradeMount = e2.degradeMount;
        int t = e1.type;
        if (Random.Range(0, 2) == 0) t = e2.type;
        if (Random.Range(0, 100) <3)
        {
            l1.AddRange(e2.improveStat);
            l2.AddRange(e2.improveMount);
        }   

        
        equipment e = new equipment(t,0,"", prefix[randPrefix] + " " + equipName[randEquip], l1, l2, rand2, degradeMount, randEquip);
        return e;
    }
}
