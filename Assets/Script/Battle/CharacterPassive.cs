using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterPassive : MonoBehaviour
{
    BattleManager BM;
    CardManager CM;

    int myNo;
    int[] myPassvie = new int[4];
    Character myCharacter;
    public int ghost=49;//큐 전용 변수
    bool isKing;//큐 전용 변수
    [SerializeField] TextMeshProUGUI ghostText;//큐 전용 변수
    [SerializeField] Sprite upQ;//큐 전용 변수
    int turnGhost;//큐 전용 변수

    

    bool isAct1;//포르테 전용 변수

    int ReplyCardDrow;//리플리 전용 변수
    bool ReplyAttackDone;//리플리 전용 변수
    bool ReplyIsUp;//리플리 전용 변수
    int ReplyAttackCount;

    private void Start()
    {
        myNo = GetComponent<Character>().characterNo;
        myPassvie = GetComponent<Character>().passive;
        myCharacter = GetComponent<Character>();
        BM = myCharacter.BM;
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
    }
    public float MyHit(Enemy e)
    {

        float timeCount = 0;
        bool Reply2 = false;
        for (int i = 0; i < BM.characters.Count; i++)
        {
            if (BM.characters[i].characterNo == 2 && BM.characters[i].passive[1] > 0)
            {
                Reply2 = true;
            }
        }
        if (myCharacter.reflect>0)
        {
            if (Reply2)
            {
                timeCount += (1 + EnemyHitTimeCal2());
            }
            else
            {
                timeCount += (1 + EnemyHitTimeCal());
            }
        }
        StartCoroutine(MyHitCor(e));
        return timeCount;
    }
    IEnumerator MyHitCor(Enemy e)
    {
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BM.otherCor = true;

        if (myCharacter.reflect > 0)
        {
         
                transform.localScale *= 1.2f;
                if (!e.isDie)
                {
                    BM.OnAttack(myCharacter.reflect, e, myCharacter, 1);
                    BM.log.logContent.text += "" + e.Name + "에게 반사데미지" + myCharacter.reflect  + "이 주어집니다.";
                    BM.curMessage.text += "" + e.Name + "에게 반사데미지" + myCharacter.reflect  + "이 주어집니다.";
                }

                yield return new WaitForSeconds(0.7f);
                transform.localScale /= 1.2f;
                BM.curMessage.text = "";
                yield return new WaitForSeconds(0.3f);
        }



        
        BM.otherCor = false;
        yield return null;
    }




    public int MyAttack()
    {
       
        int timeCount = 0;
        if (myNo == 2 && myPassvie[3] > 0 && !ReplyIsUp)
        {
          
            ReplyAttackCount++;
            if (ReplyAttackCount >20)
            {              
                timeCount += myPassvie[3];
            }
        }
        StartCoroutine("MyAttackCor");
        return timeCount;
    }


    IEnumerator MyAttackCor()
    {
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BM.otherCor = true;
        if (myNo == 2 && myPassvie[3] > 0 && !ReplyIsUp&&ReplyAttackCount>20)
        {
            
            int c = 0;
            while (c < myPassvie[3])
            {
                transform.localScale *= 1.2f;
                myCharacter.RealAtkUp(2);
              
                BM.log.logContent.text += "\n몰아치기! 스파키의 공격력이 이번 전투동안 2 증가합니다.";
              
                BM.curMessage.text = "몰아치기! 스파키의 공격력이 이번 전투동안 2 증가합니다.";
                
                c++;
                yield return new WaitForSeconds(0.7f);
                transform.localScale /= 1.2f;
                BM.curMessage.text = "";
                yield return new WaitForSeconds(0.3f);
                ReplyIsUp = true;
            }
            
        }

      
        BM.otherCor = false;
        yield return null;
    }


    public int SpecialDrow(int drow)
    {
      
        int timeCount = 0;
        if (myNo == 1 && myPassvie[2] > 0 && !isKing)
        {
            timeCount += 1 * myPassvie[2]       ;          
        }
        if (myNo == 4 && myPassvie[1] > 0&&!isAct1)
        {
            timeCount += 1 * myPassvie[1];
                      
        }
        StartCoroutine(SpecialDrowCor(drow));
        return timeCount;
    }
    IEnumerator SpecialDrowCor(int drow)
    {
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BM.otherCor = true;
        if (myNo == 1 && myPassvie[2] > 0 && !isKing)
        {
          
           
            int d = 0;
            while (d != drow)
            { int c = 0;
                Debug.Log("흑백");
                while (c < myPassvie[2])
                {
                    transform.localScale *= 1.2f;
                    GameObject newCard = CM.field[CM.field.Count - 1-d];
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
                    BM.curMessage.text += "\n" + newCard.GetComponent<Card>().Name.text;
                    BM.curMessage.text += "에 흑백 효과가 추가됩니다";
                    c++;
                    yield return new WaitForSeconds(0.8f);
                    transform.localScale /= 1.2f;
                    BM.curMessage.text = "";
                    yield return new WaitForSeconds(0.2f);

                }
                d++;
            }
        }

        if (myNo == 4 && myPassvie[1] > 0&&!isAct1)
        {
            int c = 0;
            while (c < myPassvie[0])
            {
                transform.localScale *= 1.2f;
                BM.costUp(1);
                myCharacter.ActUp(1);
                BM.log.logContent.text += "\n미라클 드로우!포르테의 행동력,코스트가 증가합니다";
                BM.curMessage.text = "미라클 드로우!포르테의 행동력,코스트가 증가합니다";
                c++;
                yield return new WaitForSeconds(0.7f);
                transform.localScale /= 1.2f;
                BM.curMessage.text = "";
                yield return new WaitForSeconds(0.3f);
            }


            isAct1 = true;
        }
        BM.otherCor = false;
        yield return null;
    }
    public int TurnStart()
    {
        int timeCount = 0;
        if (myNo == 1 && myPassvie[1] > 0)
        {
            turnGhost = 0;
        }
        if (myNo == 3 && myPassvie[1] > 0)
        {
            timeCount += 1 * myPassvie[1];
            //선봉의 호령
        }
        if (myNo == 3 && myPassvie[3] > 0)
        {
            if(BM.back.Count == 1 && BM.back[0] == myCharacter)
            timeCount += 1 * myPassvie[3];
            if(BM.forward.Count == 1 && BM.forward[0] == myCharacter)
             timeCount += 1 * myPassvie[3];
            //독불장군
        }
        if (myNo == 4 && myPassvie[1] > 0)
        {
            isAct1 = false;
        }
        if (myNo == 4 && myPassvie[2] > 0)
        {
            BM.porte3count = myCharacter.passive[2];
            BM.porte3();
        }
        if (myNo == 4 && myPassvie[3] > 0)
        {
            timeCount += 1 * myPassvie[3];
        }
        if (myNo == 2 && myPassvie[0] > 0) ReplyCardDrow = 0;
        StartCoroutine("TurnStartCor"); 
        return timeCount;
    }

    IEnumerator TurnStartCor()
    {
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BM.otherCor = true;
        if (myNo == 3 && myPassvie[1] > 0)
        {
            int c = 0;
            while (c < myPassvie[1])
            {            
                transform.localScale *= 1.2f;
                
                if (myCharacter.Armor > 0)
                {                  
                        BM.costUp(1);
                        BM.log.logContent.text += "\n선봉의 호령!코스트가 1 증가합니다.";
                        BM.curMessage.text += "선봉의 호령!코스트가 1 증가합니다.";                  
                }
                else
                {                 
                        myCharacter.getArmor(3);
                        BM.log.logContent.text += "\n선봉의 호령!반가라의 방어도가 3증가합니다.";
                        BM.curMessage.text += "선봉의 호령!반가라의 방어도가 3증가합니다.";                   
                }
                c++;
                yield return new WaitForSeconds(0.7f);
                transform.localScale /= 1.2f;
                BM.curMessage.text = "";
                yield return new WaitForSeconds(0.3f);
            }
        }
        if (myNo == 3 && myPassvie[3] > 0)
        {
            int c = 0;
            while (c < myPassvie[3])
            {

                c++;
                if (BM.forward.Count == 1 && BM.forward[0] == myCharacter)
                {
                    transform.localScale *= 1.2f;
                    BM.log.logContent.text += "\n독불장군! 자신에게 방어도 7,행동력 1이 추가됩니다.";
                    BM.curMessage.text += "독불장군! 자신에게 방어도 7,행동력 1이 추가됩니다.";
                    myCharacter.getArmor(7);
                    myCharacter.ActUp(1);
                    yield return new WaitForSeconds(0.7f);
                    transform.localScale /= 1.2f;
                    BM.curMessage.text = "";
                    yield return new WaitForSeconds(0.3f);

                }
                else if (BM.back.Count == 1 && BM.back[0] == myCharacter)
                {
                    transform.localScale *= 1.2f;
                    BM.log.logContent.text += "\n독불장군! 자신에게 방어도 7,행동력 1이 추가됩니다.";
                    BM.curMessage.text += "독불장군! 자신에게 방어도 7,행동력 1이 추가됩니다.";
                    myCharacter.getArmor(7);
                    myCharacter.ActUp(1);
                    yield return new WaitForSeconds(0.7f);
                    transform.localScale /= 1.2f;
                    BM.curMessage.text = "";
                    yield return new WaitForSeconds(0.3f);
                }
                else yield return null;
             
            
            }
        }
        if (myNo == 4 && myPassvie[3] > 0)
        {
            int c = 0;
            while (c < myPassvie[3])
            {
                transform.localScale *= 1.2f;
                
                    if (CM.Deck.Count > CM.Grave.Count)
                    {
                    BM.log.logContent.text += "\n평균율! 덱에서 무덤으로 카드 1장이 이동합니다.";
                    BM.curMessage.text += "평균율! 덱에서 무덤으로 카드 1장이 이동합니다.";
                    CM.DeckToGrave(CM.Deck[Random.Range(0, CM.Deck.Count)]);
                    }
                    else if (CM.Deck.Count < CM.Grave.Count)
                {
                    BM.log.logContent.text += "\n평균율! 무덤에서 덱으로 카드 1장이 이동합니다.";
                    BM.curMessage.text += "평균율! 무덤에서 덱으로 카드 1장이 이동합니다.";
                    CM.GraveToDeck(CM.Grave[Random.Range(0, CM.Grave.Count)]);
                    }
                
                c++;
                yield return new WaitForSeconds(0.7f);
                transform.localScale /= 1.2f;
                BM.curMessage.text = "";
                yield return new WaitForSeconds(0.3f);
            }
        }
        BM.otherCor = false;
        yield return null;

    }

    public int TurnEndTimeCount()
    {
        int timeCount = 0;
       
        if (myNo== 1 && myPassvie[0] > 0&&!isKing&&ghost>50)
        {
            timeCount += 2*myPassvie[0];
            //군단           
        }
        if (myNo == 4 && myPassvie[0] > 0&& CM.field.Count > 3)
        {
            timeCount += 1 * myPassvie[0];
        }
        StartCoroutine("TurnEndCor"); 
        return timeCount;
    }
  

    IEnumerator TurnEndCor()
    {
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BM.otherCor = true;
        if (myNo == 1 && myPassvie[0] > 0 && !isKing && ghost > 50)
        {
            int c = 0;
            while (c < myPassvie[0])
            {               
                transform.GetChild(7).GetComponent<Image>().sprite = upQ;
                transform.localScale *= 1.2f;
                BM.log.logContent.text += "\nQ가 백옥의 왕 Q로 변신합니다.";
                BM.curMessage.text += "Q가 백옥의 왕 Q로 변신합니다.";
                myCharacter.maxHp = 100;//진화 후 체력은?
                myCharacter.hpT.text = myCharacter.Hp + "/" + myCharacter.maxHp;
                for (int j = 0; j < myCharacter.passive[0]; j++)
                {
                    CM.PlusCard(13);
                    CM.PlusCard(13);
                    CM.PlusCard(14);
                    CM.PlusCard(14);
                }
                isKing = true;
                myCharacter.Atk += 2;
                myCharacter.AtkUp(2);
                BM.startCost++;
                c++;
                yield return new WaitForSeconds(1.5f);
                transform.localScale /= 1.2f;
                BM.curMessage.text = "";
                yield return new WaitForSeconds(0.5f);
            }
        }
        if (myNo == 4 && myPassvie[0] > 0 && CM.field.Count > 3)
        {
            int c = 0;
            while (c < myPassvie[0])
            {
                transform.localScale *= 1.2f;
              
               
                BM.TurnCardCount += 2;
                BM.log.logContent.text += "\n창조의 잠재력!다음 턴 드로우를 2장 더 합니다.";
                BM.curMessage.text= "창조의 잠재력!다음 턴 드로우를 2장 더 합니다.";
               c++;
                yield return new WaitForSeconds(0.7f);
                transform.localScale /= 1.2f;
                BM.curMessage.text = "";
                yield return new WaitForSeconds(0.3f);
            }
        }
        BM.otherCor = false;
        yield return null;
        
    }
    
    
    public float EnemyHit(Enemy e)
    {
     
        float timeCount = 0;
        if (myNo == 2 && myPassvie[1] > 0)
        { 
            if (!ReplyAttackDone)
            {
                timeCount = myPassvie[1]*(1+EnemyHitTimeCal());
                ReplyAttackDone = true;
            }         
        }
        if (myNo == 3 && myPassvie[0] > 0)
        {
            timeCount = myPassvie[0];
        }  
        if (timeCount > 0)
        {
            StartCoroutine("EnemyHitCor", e);
            return timeCount + 0.5f;
        }
        else return 0;
    }
    public float EnemyHitTimeCal()
    {
        float timeCount=0;
        for(int i = 0; i < BM.characters.Count; i++)
        {
            if (BM.characters[i].characterNo == 3 && BM.characters[i].passive[0] > 0)
            {
                timeCount += BM.characters[i].passive[0] + 0.5f;
            }
        }
        return timeCount;
    }

    public float EnemyHitTimeCal2()
    {
        float timeCount = 0;
        for (int i = 0; i < BM.characters.Count; i++)
        {
            if (BM.characters[i].characterNo == 3 && BM.characters[i].passive[0] > 0)
            {
                timeCount += (BM.characters[i].passive[0]*2)+0.5f;
            }
            if (BM.characters[i].characterNo == 2 && BM.characters[i].passive[1] > 0)
            {
                timeCount += BM.characters[i].passive[0] + 0.5f;
            }
        }
        return timeCount;
    }

    IEnumerator EnemyHitCor(Enemy e)
    {
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BM.otherCor = true;
        yield return new WaitForSeconds(0.5f);
        if (myNo == 2 && myPassvie[1] > 0 && ReplyAttackDone)
        {
            int c = 0;
            while (c < myPassvie[1])
            {
              
                
                transform.localScale *= 1.2f;
                if (!e.isDie)
                {
                    
                    BM.OnAttack(0, e, myCharacter,1);
                    BM.log.logContent.text += "\n독단적인 팀플레이!" + e.Name + "에게 " + myCharacter.turnAtk + "의 데미지가 주어집니다.";
                    BM.curMessage.text = "독단적인 팀플레이!" + e.Name + "에게 " + myCharacter.turnAtk + "의 데미지가 주어집니다.";
                }
                c++;
                yield return new WaitForSeconds(0.7f);
                transform.localScale /= 1.2f;
                BM.curMessage.text = "";
                yield return new WaitForSeconds(0.3f);
            }
            ReplyAttackDone = false;
        }
        if (myNo == 3 && myPassvie[0] > 0)
        {
            int c = 0;
            while (c < myPassvie[0])
            {


                    transform.localScale *= 1.2f;

                    myCharacter.getArmor(1);
                    BM.log.logContent.text += "\n굳건한 위치! 자신에게 방어도가 1 추가됩니다.";
                    BM.curMessage.text += "\n굳건한 위치! 자신에게 방어도가 1 추가됩니다.";
               
                c++;
                yield return new WaitForSeconds(0.7f);
                transform.localScale /= 1.2f;
                BM.curMessage.text = "";
                yield return new WaitForSeconds(0.3f);
            }
        }

        BM.otherCor = false;
        yield return null;
    }

    public int TeamHit(int no)
    {
        int timeCount = 0;
        if (myNo == 3 && myPassvie[0] > 0)
        {
            
                timeCount += myPassvie[0];
            
        }
        StartCoroutine("TeamHitCor");
        return timeCount;
    }
    IEnumerator TeamHitCor()
    {
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BM.otherCor = true;
        if (myNo == 3 && myPassvie[0] > 0)
        {
            int c = 0;
            while (c < myPassvie[0])
            {

                transform.localScale *= 1.2f;

                myCharacter.getArmor(1);
                BM.log.logContent.text += "\n굳건한 위치! 자신에게 방어도가 1 추가됩니다.";
                BM.curMessage.text += "굳건한 위치! 자신에게 방어도가 1 추가됩니다.";
                c++;
                yield return new WaitForSeconds(0.7f);
                transform.localScale /= 1.2f;
                BM.curMessage.text = "";
                yield return new WaitForSeconds(0.3f);
            }
        }
        BM.otherCor = false;
        yield return null;
    }


    public float MyArmorHit(int armor,Enemy e)
    {

        float timeCount = 0;
        bool Reply2 = false;
        for (int i = 0; i < BM.characters.Count; i++)
        {          
            if (BM.characters[i].characterNo == 2 && BM.characters[i].passive[1] > 0)
            {
                Reply2 = true;
            }
        }
        if (myNo == 3 && myPassvie[2] > 0)
        {
            if (Reply2)
            {
                timeCount += (myPassvie[2] + EnemyHitTimeCal2());
            }
            else
            {
                timeCount += (myPassvie[2] + EnemyHitTimeCal());
            }
        }
        StartCoroutine(MyArmorHitCor(armor,e));
        return timeCount;
    }
    IEnumerator MyArmorHitCor(int armor,Enemy e)
    {
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BM.otherCor = true;
        
        if (myNo == 3 && myPassvie[2] > 0 && !isAct1)
        {
            int c = 0;
            while (c < myPassvie[0])
            {
               
                c++;
                transform.localScale *= 1.2f;
                if (!e.isDie)
                {
                    BM.OnAttack(armor, e, myCharacter, 1);
                    BM.log.logContent.text += "\n무장!" + e.Name + "에게 " + armor + "의 데미지가 주어집니다.";
                    BM.curMessage.text += "무장!" + e.Name + "에게 " + armor + "의 데미지가 주어집니다.";
                }
              
                yield return new WaitForSeconds(0.7f);
                transform.localScale /= 1.2f;
                BM.curMessage.text = "";
                yield return new WaitForSeconds(0.3f);
            }


           
        }
        BM.otherCor = false;
        yield return null;
    }


    public int CardUse()
    {
        int timeCount = 0;
        if (myNo == 2 && myPassvie[0] > 0)
        {
            ReplyCardDrow++;
            if (ReplyCardDrow % 3 == 0)
            {
                timeCount += myPassvie[1];
            }
        }
        StartCoroutine("CardUseCor");
        return timeCount;
    }
    IEnumerator CardUseCor()
    {
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BM.otherCor = true;
        if (myNo == 2 && myPassvie[0] > 0 && ReplyCardDrow%3==0)
        {
            int c = 0;
            while (c < myPassvie[2])
            {
                    
                    transform.localScale *= 1.2f;
                    myCharacter.ActUp(1);
                    BM.log.logContent.text += "\n지치지 않는 폭주! 리플리의 행동력이 증가합니다.";
                    BM.curMessage.text += "지치지 않는 폭주! 리플리의 행동력이 증가합니다.";              
                c++;
                yield return new WaitForSeconds(0.7f);
                transform.localScale /= 1.2f;
                BM.curMessage.text = "";
                yield return new WaitForSeconds(0.3f);
            }
        }
        BM.otherCor = false;
        yield return null;
    }

    public int myAct()
    {

        int timeCount = 0;
        if (myNo == 2 && myPassvie[2]>0&&myCharacter.Act == 0)
        {
            timeCount += 1 * myPassvie[2];
        }
        StartCoroutine("myActCor");
        return timeCount;
    }

    IEnumerator myActCor()
    {
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BM.otherCor = true;
        if (myNo == 2 && myPassvie[2] > 0 && myCharacter.Act==0)
        {
            int c = 0;
            while (c < myPassvie[2])
            {


                transform.localScale *= 1.2f;
                if (CM.field.Count > 0)
                {
                    
                        int rand = Random.Range(0, CM.field.Count);                      
                        CM.field[rand].GetComponent<Card>().cardcost = 0;
                        CM.field[rand].GetComponent<Card>().costT.text = "" + 0;
                        BM.log.logContent.text += "\n부서진 족쇄!" + CM.field[rand].GetComponent<Card>().Name.text + "의 코스트가 0이 됩니다.";
                        BM.curMessage.text += "부서진 족쇄!" + CM.field[rand].GetComponent<Card>().Name.text + "의 코스트가 0이 됩니다.";
                }
                c++;
                yield return new WaitForSeconds(0.7f);
                transform.localScale /= 1.2f;
                BM.curMessage.text = "";
                yield return new WaitForSeconds(0.3f);
            }
        }
        BM.otherCor = false;
        yield return null;
    }

    public int ActMinus(int m)
    {   myCharacter.onMinusAct(m);
        int timeCount = 0;
        if (myNo == 1 && myPassvie[3] > 0 && CM.Grave.Count > 0)
        {
            if (myPassvie[3] > CM.Grave.Count)
                timeCount += CM.Grave.Count;
            else timeCount += myPassvie[3];
        }
        StartCoroutine("ActMinusCor");
        return timeCount;
    }

    IEnumerator ActMinusCor()
    {
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BM.otherCor = true;
        if (myNo == 1 && myPassvie[3] > 0 && CM.Grave.Count > 0)
        {
            int c = 0;
            while (c < myPassvie[3])
            {


                transform.localScale *= 1.2f;
                if (CM.Grave.Count > 0)
                {                                      
                            int rand = Random.Range(0, CM.Grave.Count);

                            GameObject newCard = CM.Grave[rand];
                            if (newCard.GetComponent<BlackWhite>() == null)
                            {
                                newCard.AddComponent<BlackWhite>();
                                newCard.GetComponent<BlackWhite>().birth();
                            }
                            else
                            {
                                newCard.GetComponent<BlackWhite>().PlusStack();
                            }
                            CM.GraveToField(newCard);
                        
                    
                    BM.log.logContent.text += "\n절망!" + newCard.GetComponent<Card>().Name.text + "를 무덤에서 가져옵니다.";
                    BM.curMessage.text += "절망!" + newCard.GetComponent<Card>().Name.text + "를 무덤에서 가져옵니다.";
                }
                c++;
                yield return new WaitForSeconds(0.7f);
                transform.localScale /= 1.2f;
                BM.curMessage.text = "";
                yield return new WaitForSeconds(0.3f);
            }
        }
        BM.otherCor = false;
        yield return null;
    }

    public void GhostRevive(int ghostplus)
    {
        ghost += ghostplus;        
        ghostText.text = ghost+"";
        int x = turnGhost + ghostplus;
        int q1Count = 0;
        if (myPassvie[1] > 0&&!isKing)
        {
            while (turnGhost != x)
            {
                turnGhost++;
                if (turnGhost % 4 == 0)
                {
                    q1Count++;
                }
            }
           BM.otherCanvasOn = true;
            StartCoroutine("Q1", q1Count*myPassvie[1]);

        }
    }


 IEnumerator Q1(int i)
    {
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BM.otherCor = true;
        int c = 0;
        while (c < i)
        {
            c++;
            BM.curMessage.text = "Q의 군단효과로 인해 행동력을 얻습니다.";
            myCharacter.transform.localScale *= 1.2f;
            myCharacter.ActUp(1);
           yield return new WaitForSeconds(0.8f);
            BM.curMessage.text = "";
            myCharacter.transform.localScale /= 1.2f;
            yield return new WaitForSeconds(0.2f);
        }
        BM.otherCanvasOn = false;
        BM.otherCor = false;
    }
   
}
