using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reply : MonoBehaviour
{

    [SerializeField] Character myCharacter;
    public bool[] passive;
    GameObject[] enemys;
    Enemy[] enemyScript;
    [SerializeField] int[] EnemyStack;
    TurnManager TM;
    BattleManager BM;
    bool Passive1;
    bool Passive3;
    int AttackCount;
    private void Start()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemyScript = new Enemy[enemys.Length];
        EnemyStack = new int[enemys.Length];
        for (int i = 0; i < enemys.Length; i++)
        {
            enemyScript[i] = enemys[i].GetComponent<Enemy>();
            
        }
        myCharacter.Name = "스파키";
    }
    // Update is called once per frame
    void passive3()
    {
        if (myCharacter.Act == 0&&!Passive3)
        {              
                if (TM.CM.field.Count > 0)
            {
                for (int j = 0; j < myCharacter.passive[2]; j++)
                {
                    int rand = Random.Range(0, TM.CM.field.Count);
                    Passive3 = true;
                    TM.CM.field[rand].GetComponent<Card>().cardcost = 0;
                    TM.CM.field[rand].GetComponent<Card>().costT.text = "" + 0;
                  BM.log.logContent.text += "\n부서진 족쇄!" + TM.CM.field[rand].GetComponent<Card>().Name.text + "의 코스트가 0이 됩니다.";
                }
            }
        }
        else if(myCharacter.Act>0&&Passive3)
        {
            Passive3 = false;
        }
    }
    void passive1()
    {
        if (TM.turnCard % 3 == 0 && TM.turnCard > 0 && !Passive1)
        {
            Passive1 = true;
            for (int j = 0; j < myCharacter.passive[0]; j++)
            {
                myCharacter.ActUp(1);
                BM.log.logContent.text += "\n지치지 않는 폭주!스파키의 현재 행동력이 추가됩니다.";
            }
        }
        else if (TM.turnCard % 3 != 0 || TM.turnCard == 0)
        {
            Passive1 = false;
        }
    }
    void passive2()
    {
        for (int i = 0; i < enemys.Length; i++)
        {
            if (EnemyStack[i] != enemyScript[i].dmgStack)
            {
             
                if (!enemyScript[i].isDie)
                {
                    EnemyStack[i]++;
                    for (int j = 0; j < myCharacter.passive[1]; j++)
                    {
                        myCharacter.AttackCount++;
                        enemyScript[i].onHit(myCharacter.turnAtk);                  
                        EnemyStack[i]++;
                        BM.log.logContent.text += "\n독단적인 팀플레이!" + enemyScript[i].Name + "에게 " + myCharacter.turnAtk + "의 데미지가 주어집니다.";
                    } 
                }
            }
        }
    }
    bool p4;
    public void passive4()
    {
        if (AttackCount != myCharacter.AttackCount)
        {
            AttackCount++;
            if (AttackCount != 0 && AttackCount % 20 == 0&&!p4) {
                p4 = true;
                for (int j = 0; j < myCharacter.passive[3]; j++)
                {
                    myCharacter.AtkUp(2); myCharacter.Atk += 2;
                } 
            }
        }
    }
    void Update()
    {       
        if (!myCharacter.isDie)
        {
            if (myCharacter.passive[3]>0)
            {
                passive4();
            }
         
            if (myCharacter.passive[1]>0)
            {
                passive2();
            }
            if(myCharacter.passive[2]>0)
            passive3();
            if (myCharacter.passive[0]>0)
            {
                passive1();
            }
            if (myCharacter.isSet)
            {
                myCharacter.isSet = false;
            }
            if (myCharacter.isTurnEnd)
            {
                myCharacter.isTurnEnd = false;
            }
            if (myCharacter.isTurnStart)
            {
                for(int i = 0; i < enemyScript.Length; i++)
                {
                    EnemyStack[i] =
                       enemyScript[i].dmgStack;
                }
                myCharacter.isTurnStart = false;
            }
        }
    }
}
