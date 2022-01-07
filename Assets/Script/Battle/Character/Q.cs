using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Q : MonoBehaviour
{
    [SerializeField] Text ghostT;
    public int Ghost;
    [SerializeField] Character myCharacter;
    public bool[] passive;
    TurnManager TM;
    BattleManager BM;
    bool passive1;
    CardManager CM;
    int specialDrow;
    int turnStartGhost;
    int GhostPlus;
    private void Awake()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
        myCharacter.Name = "큐";
        // Update is called once per frame
    }
    void passive2()
    {
        if (turnStartGhost < Ghost)
        {

            GhostPlus++;
            turnStartGhost++;
            if (GhostPlus % 4 == 0 && GhostPlus > 0) {
                BM.log.logContent.text += "\n군단!큐의 행동력이 증가합니다.";
              myCharacter.Act++; }
        }
    }
    void passive3()
    {
        if (specialDrow < CM.specialDrow)
        {
            int gap;
            gap = CM.specialDrow - specialDrow;
            GameObject newCard = CM.field[CM.field.Count - gap];
            if (newCard.GetComponent<BlackWhite>() == null)
            {
                newCard.AddComponent<BlackWhite>();
                newCard.GetComponent<BlackWhite>().birth();
            }
            else
            {
                newCard.GetComponent<BlackWhite>().PlusStack();
            }
            BM.log.logContent.text += "\n" + newCard.GetComponent<Card>().Name.text;
            BM.log.logContent.text += "에 흑백 효과가 추가됩니다";
          specialDrow++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!myCharacter.isDie)
        {
            if (passive[3]) passive3();
            if (passive[2]) passive2();
           // ghostT.text = "망자:" + Ghost;
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
                turnStartGhost = Ghost;
                GhostPlus = 0;
                myCharacter.isTurnStart = false;
            }
        }
    }
}
