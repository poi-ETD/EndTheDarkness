using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Q : MonoBehaviour
{
    [SerializeField] Text ghostT;
    public int Ghost;
    [SerializeField] Character myCharacter;
    public int passive;
    TurnManager TM;
    BattleManager BM;
    bool passive1;
    CardManager CM;
    int specialDrow;
    int turnStartGhost;
    private void Awake()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
        // Update is called once per frame
    }

    // Update is called once per frame
    void Update()
    {
        ghostT.text = "망자:" + Ghost;
        if (passive==2)
        {
            if (myCharacter.isTurnStart)
            {
                turnStartGhost = Ghost;
                myCharacter.isTurnStart = false;
                passive1 = false;
            }
            if ((Ghost - turnStartGhost) % 3 == 0 && Ghost - turnStartGhost != 0&&!passive1)
            {
                myCharacter.Act++;
                passive1 = true;
            }
            else if((Ghost - turnStartGhost) % 3 != 0)
            {
                passive1 = false;
            }
        }
        if (passive == 3)
        {
            if (specialDrow != CM.specialDrow)
            {
                int gap = CM.specialDrow - specialDrow;
                specialDrow++;
       
                GameObject specialCard = CM.field[CM.field.Count - gap];
                if (specialCard.GetComponent<BlackWhite>() == null)
                {
                    specialCard.AddComponent<BlackWhite>();
                    specialCard.GetComponent<BlackWhite>().birth();
                }
                else
                {
                    specialCard.GetComponent<BlackWhite>().PlusStack();
                }
            }
        }
    }
}
