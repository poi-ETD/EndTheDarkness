using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Q : MonoBehaviour
{
    [SerializeField] Text ghostT;
    public int Ghost;
    [SerializeField] Character myCharacter;
    TurnManager TM;
    BattleManager BM;
    bool isKing;
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
    void passive1()
    {
        if (Ghost > 50 && !isKing)
        {
            BM.log.logContent.text += "\nQ가 백옥의 왕 Q로 변신합니다.";
            myCharacter.Hp = 100;
            myCharacter.maxHp = 100;
            isKing = true;
            myCharacter.Atk+=2;
            BM.startCost++;
        }
    }
    void kingpassive2()
    {
        myCharacter.Act++;
        CM.CardToField();
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
            if (myCharacter.passive[2]&&!isKing) passive3();
            if (myCharacter.passive[1]&&!isKing) passive2();
           // ghostT.text = "망자:" + Ghost;
            if (myCharacter.isSet)
            {

                myCharacter.isSet = false;
            }
            if (myCharacter.isTurnEnd)
            {
                if (myCharacter.passive[0]) passive1();
                myCharacter.isTurnEnd = false;
            }
            if (myCharacter.isTurnStart)
            {
                if (isKing)
                {
                    kingpassive2();
                }
                turnStartGhost = Ghost;
                GhostPlus = 0;
                myCharacter.isTurnStart = false;
            }
        }
    }
}
