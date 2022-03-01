using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vangara : MonoBehaviour
{
    [SerializeField] Character myCharacter;
    public bool[] passive;
    [SerializeField] int[] TeamStack=new int[3];
    GameObject[] enemys;
    Enemy[] enemyScript;
    [SerializeField] int[] EnemyStack;
    BattleManager BM;
    Character[] TeamCharacter = new Character[3];
    int armorcount = 0;
    private void Start()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        int count = 0;
        for(int i = 0; i < BM.characters.Count; i++)
        {
            if (BM.characters[i] != myCharacter)
            {
                TeamCharacter[count] = BM.characters[i];                
                count++;
            }
        }
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemyScript = new Enemy[enemys.Length];
        EnemyStack = new int[enemys.Length];
        for (int i = 0; i < enemys.Length; i++)
        {
            enemyScript[i] = enemys[i].GetComponent<Enemy>();
           
        }
        myCharacter.Name = "반가라";
     
    }
    void passive1()
    {
        for (int i = 0; i < 3; i++)
        {
            if (TeamStack[i] != TeamCharacter[i].dmgStack)
            {
                while (TeamStack[i] != TeamCharacter[i].dmgStack)
                {
                    for (int j = 0; j < myCharacter.passive[0]; j++)
                    {
                        TeamStack[i]++;
                        myCharacter.Armor++;
                        BM.log.logContent.text += "\n굳건한 위치!반가라의 방어력이 증가합니다.";
                    }
                }
            }
        }
        for(int i = 0; i < enemys.Length; i++)
        {
            if (EnemyStack[i] != enemyScript[i].dmgStack)
            {
                while (EnemyStack[i] != enemyScript[i].dmgStack)
                {
                    for (int j = 0; j < myCharacter.passive[0]; j++)
                    {
                        EnemyStack[i]++;
                        myCharacter.Armor++;
                        BM.log.logContent.text += "\n굳건한 위치!반가라의 방어력이 증가합니다.";
                    }
                }
            }
        }
    }
    void passive2()
    {
        if (myCharacter.Armor > 0)
        {
            for (int j = 0; j < myCharacter.passive[1]; j++)
            {
                BM.cost++;
                BM.log.logContent.text += "\n선봉의 호령!코스트가 1 증가합니다.";
            }
        }
        else
        {
            for (int j = 0; j < myCharacter.passive[1]; j++)
            {
                myCharacter.Armor += 3;
                BM.log.logContent.text += "\n선봉의 호령!반가라의 방어도가 3증가합니다.";
            }
        }
    }
    void passive3()
    {
       
        if (armorcount !=myCharacter.armorBreak.Count)
        {      
            for(int i = 0; i < enemyScript.Length; i++)
            {
                if (enemyScript[i].Name == myCharacter.armorBreak[armorcount].name)
                {
                    if (myCharacter.armorBreak[armorcount].dmg > 0)
                    {
                        for (int j = 0; j < myCharacter.passive[2]; j++)
                        {
                            enemyScript[i].onHit(myCharacter.armorBreak[armorcount].dmg);
                            BM.log.logContent.text += "\n무장!" + enemyScript[i].Name + "에게 추가 데미지!";
                        }
                    }
                }
            }

            armorcount++;
        }
    }
    void passive4()
    {
        if (BM.forward.Count == 1 && BM.forward[0] == GetComponent<Character>())
        {
            for (int j = 0; j < myCharacter.passive[3]; j++)
            {
                myCharacter.Armor += 7;
                myCharacter.Act++;
            }
        }
        if (BM.back.Count == 1 && BM.back[0] == GetComponent < Character> ()){

            for (int j = 0; j < myCharacter.passive[3]; j++)
            {
                myCharacter.Armor += 7;
                myCharacter.Act++;
            }
        }
    }
    private void Update()
    {
        if (myCharacter.passive[2]>0)
        {
            passive3();
        }
        if (!myCharacter.isDie) { 
        if (myCharacter.isSet)
        {
            if (myCharacter.passive[0]>0)
            {
                passive1();
            }
             
            myCharacter.isSet = false;
        }
        if (myCharacter.isTurnEnd)
            {
               
                myCharacter.isTurnEnd = false;
        }
        if (myCharacter.isTurnStart)
        {
                if (myCharacter.passive[3]>0) passive4();
            if (myCharacter.passive[1]>0)
                passive2();
            myCharacter.isTurnStart = false;
        }

    }
}}
