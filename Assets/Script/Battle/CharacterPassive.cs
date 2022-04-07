using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterPassive : MonoBehaviour
{
    int myNo;
    int[] myPassvie = new int[4];
    Character myCharacter;
    public int ghost=49;//큐 전용 변수
    bool isKing;//큐 전용 변수
    [SerializeField] TextMeshProUGUI ghostText;//큐 전용 변수
    [SerializeField] Sprite upQ;//큐 전용 변수
    int turnGhost;//큐 전용 변수
    BattleManager BM;
    CardManager CM;

    
    
    private void Start()
    {
        myNo = GetComponent<Character>().characterNo;
        myPassvie = GetComponent<Character>().passive;
        myCharacter = GetComponent<Character>();
        BM = myCharacter.BM;
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
    }
    public void MyHit()
    {

    }
    public void MyAttack()
    {

    }
    public void SpecialDrow(int c)
    {

    }
    public void TurnStart()
    {
        if (myNo == 1 && myPassvie[1] > 0)
        {
            turnGhost = 0;
        }
    }
    public int TurnEndTimeCount()
    {
        int timeCount = 0;
        BM.log.logContent.text = "";
        if (myNo== 1 && myPassvie[0] > 0&&!isKing&&ghost>50)
        {
            timeCount += 2*myPassvie[0];
            StartCoroutine("TurnEndCor");
           
        }
        return timeCount;
    }

    IEnumerator TurnEndCor()
    {
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
                BM.curMessage.text = "";
                yield return new WaitForSeconds(0.5f);
            }
        }
        yield return null;
        
    }
    
    
    public void EnemyHit(int no)
    {

    }
    public void TeamHit(int no)
    {

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
    }
}
