using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porte : MonoBehaviour
{
    [SerializeField] Character myCharacter;
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
        if (CM.FiledCardCount > 3)
        {
            for (int i = 0; i < myCharacter.passive[0]; i++)
            {
                BM.TurnCardCount += 2;
                BM.log.logContent.text += "\n창조의 잠재력!다음 턴 드로우를 2장 더 합니다.";
            }
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
                for (int i = 0; i < myCharacter.passive[1];i++) {
                    BM.costUp(1);
                    
                    myCharacter.ActUp(1);
                    BM.log.logContent.text += "\n미라클 드로우!포르테의 행동력,코스트가 증가합니다"; }
            }
        }
    }
    public void passive3()
    {
        BM.porte3count = myCharacter.passive[2];
        BM.porte3();
    }
    public void passive4()
    {
        for (int j = 0; j < myCharacter.passive[3]; j++)
        {
            if (CM.Deck.Count > CM.Grave.Count)
            {

                CM.DeckToGrave(CM.Deck[Random.Range(0, CM.Deck.Count)]);
            }
            else if (CM.Deck.Count < CM.Grave.Count)
            {
                CM.GraveToDeck(CM.Grave[Random.Range(0, CM.Grave.Count)]);
            }
        }
    }
    void Update()
    {
        if (!myCharacter.isDie)
        {
            if(myCharacter.passive[1]>0)
            passive2();
            if (myCharacter.isSet)
            {

                myCharacter.isSet = false;
            }
            if (myCharacter.isTurnEnd)
            {
                if (myCharacter.passive[0]>0)
                    passive1();
                myCharacter.isTurnEnd = false;
            }
            if (myCharacter.isTurnStart)
            {
                if (myCharacter.passive[2]>0)
                {
                   
                    passive3();
                }
                if (myCharacter.passive[3]>0)
                {
                    passive4();
                }
                Passive2 =  false;
                myCharacter.isTurnStart = false;
            }
        }
    }
}