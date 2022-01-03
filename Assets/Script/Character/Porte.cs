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
        // Update is called once per frame
    }
    public void passive1()
    {
        if (CM.FiledCardCount > 0)
        {

            BM.TurnCardCount++;
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