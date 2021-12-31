using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public Character[] characters;
    public bool CharacterSelectMode;
    public bool EnemySelectMode;
    public Character character;
    public Enemy enemy;
    public int cost;
    public GameObject card;
    Transform tra1;
    Transform tra2;
    [SerializeField] Text costT;
    public int startCost;
    public int CardCount;
    [SerializeField] CardManager CM;
    float nowZ;
    public int StartCost;
    private void Awake()
    {
        nowZ = 1;
        for (int i = 0; i < 4; i++)
            startCost+= characters[i].cost;
    }
    private void Update()
    {

        costT.text = "cost:" + cost;
        
    }
    public void CharacterCancle()
    {
        if(tra1!=null)
        tra1.localScale = new Vector2(1, 1);
        CharacterSelectMode = true;
        character = null;
        cancleCard();
    }
    public void CharacterSelect(GameObject c)
    {
        tra1 = c.GetComponent<Transform>();
        tra1.localScale = new Vector2(1.2f, 1.2f);
        CharacterSelectMode = false;
        character = c.GetComponent<Character>();
       
    }
    public void EnemySelect(GameObject e)
    {
        EnemySelectMode = false;
        enemy = e.GetComponent<Enemy>();
    }
    public void SetCard(GameObject c)
    {
        tra2 = c.GetComponent<Transform>();
        tra2.localScale = new Vector2(1.2f, 1.2f);
        card = c;
        nowZ = tra2.transform.position.z;
        tra2.transform.position = new Vector3(tra2.position.x, tra2.position.y, -6
          );
    }
    public void cancleCard()
    {
        EnemySelectMode = false;
        card = null;
        if (tra2 != null)
        {
            tra2.localScale = new Vector2(1, 1);
            tra2.transform.position = new Vector3(tra2.position.x, tra2.position.y, nowZ
                );
        }
    }

    public void TurnStart()
    {
        Setting();
        for (int i = 0; i < CardCount; i++)
            CM.CardToField();
        for (int i = 0; i < 4; i++)
        {
            characters[i].isTurnStart = true;
        }

    }
    public void Setting()
    {
        for (int i = 0; i < 4; i++)
        {
            characters[i].isSet = true;
        }
    }
    public void TurnEnd()
    {
        Setting();
        for (int i = 0; i < 4; i++)
        {
            characters[i].isTurnEnd = true;
        }
    }
}
