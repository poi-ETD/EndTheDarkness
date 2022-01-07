using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porte : MonoBehaviour
{
    [SerializeField] Character myCharacter;
    public bool[] passive;
    TurnManager TM;
    BattleManager BM;
    bool Passive1;
    CardManager CM;
    int specialDrow;
    bool Passive2;
    private void Awake()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
        myCharacter.Name = "포르테";
        // Update is called once per frame
    }
    public void passive1()
    {
        if (CM.FiledCardCount > 3)
        {

            BM.TurnCardCount+=2;
            BM.log.logContent.text += "\n창조의 잠재력!다음 턴 드로우를 2장 더 합니다.";
        }
    }
    public void passive2()
    {
        if (CM.specialDrow > specialDrow)
        {
            specialDrow++;
            if (!Passive2)
            {
                Passive2 = true;
                BM.cost++;
                myCharacter.Act++;
                BM.log.logContent.text += "\n미라클 드로우!포르테의 행동력,코스트가 증가합니다";
            }
        }
    }
    void Update()
    {
        if (!myCharacter.isDie)
        {
            passive2();
            if (myCharacter.isSet)
            {

                myCharacter.isSet = false;
            }
            if (myCharacter.isTurnEnd)
            {
                passive1();
                myCharacter.isTurnEnd = false;
            }
            if (myCharacter.isTurnStart)
            {
              Passive2 =  false;
                myCharacter.isTurnStart = false;
            }
        }
    }
}