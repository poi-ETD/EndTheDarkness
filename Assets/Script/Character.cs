using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public int maxHp;
    public int Hp;
    public int Atk;
    public int Armor;
    public int Act;
    public TurnManager TM;
    public BattleManager BM;
    public Text hpT;
    public Text atkT;
    public Text armorT;
    public Text actT;
    public Text board;
    public int DMG;
    public int cost;
    public bool isSet;
    public bool isTurnStart;
    public bool isTurnEnd;
    // Start is called before the first frame update
    private void Update()
    {
        hpT.text = Hp + "/" + maxHp;
        atkT.text = "Atk : "+Atk;
        armorT.text = "Armor : "+Armor;
        actT.text = "Act : "+Act;
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject&&Act>0)
                {               
                    if (BM.CharacterSelectMode)
                    {
                        BM.CharacterSelect(hit.collider.gameObject);
                    }
                   else if (!BM.CharacterSelectMode)
                    {
                        if (BM.character == hit.collider.GetComponent<Character>())
                        {
                          
                            BM.CharacterCancle();
                         
                        }
                        else
                        {
                            BM.CharacterCancle();
                            BM.CharacterSelect(hit.collider.gameObject);
                        }
                    }
                }
            }
            
           
        }

    }
    private void Awake()
    {
        Act = 1;
        Hp = maxHp;
    }
    public void onHit(int dmg)
    {
        DMG += dmg;
        board.text += "\ndmg:" + dmg;
    }
    public void onDamage(int dmg)
    {
        BM.Setting();
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
            Debug.Log("캐릭터가 쓰러졌습니다.");
        }
    }
}
