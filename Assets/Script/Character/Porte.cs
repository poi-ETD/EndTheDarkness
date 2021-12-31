using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porte : MonoBehaviour
{
    [SerializeField] Character myCharacter;
    public int passive;
    TurnManager TM;
    BattleManager BM;
    bool passive1;
    CardManager CM;
    int specialDrow;
    bool passive2;
    private void Awake()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
        // Update is called once per frame
    }
    void Update()
    {
       
        if (passive==1)
        {
            if (myCharacter.isTurnStart)
            {
                myCharacter.isTurnStart = false;
                if (passive1)
                {
                    passive1 = false;
                    BM.CardCount--; 
                }
            }
            if (myCharacter.isTurnEnd)
            {
                if (CM.FiledCardCount > 0)
                {
                    myCharacter.isTurnEnd = false;
                    BM.CardCount++;                              
                    passive1 = true;
                }
            }
        }
        if (passive == 2)
        {
            if (myCharacter.isTurnStart)
            {
                specialDrow = CM.specialDrow;
                passive2 = false;
                myCharacter.isTurnStart = false;
            }
            if (specialDrow != CM.specialDrow&&!passive2)
            {
                passive2 = true;         
                BM.cost += 1;
                myCharacter.Act++;
            }
        }
    }
}
