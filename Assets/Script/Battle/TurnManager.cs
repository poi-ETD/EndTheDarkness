using System.Collections;   
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TurnManager : MonoBehaviour
{
    public bool PlayerTurn;//플레이어의 턴인지 확인용
    [SerializeField] GameObject[] enemys;
    [SerializeField] Enemy[] enemy;
    public int t;
    public GameObject EndButton;
    public BattleManager BM;
    ActManager AM;
    public TextMeshProUGUI turnText;
    public int turnCard;//이번턴에 사용한 카드의 수
    public CardManager CM;
    public int leftCost;
    [SerializeField] GameObject pleaseSelect;
    public Image turnEndImage;
    public int turnAtk;
    public void turnCardPlus() //카드를 사용 시 발동
    {
        turnCard++;

        if (turnCard == 4) //현재 턴에 카드를 4번 사용했다면, 모든 스트레이트 펀치의 코스트를 감소시켜야함.
        {
            for(int i = 0; i < CM.Grave.Count; i++)
            {
                if (CM.Grave[i].GetComponent<Card>().cardNo == 11)
                {
                    CM.Grave[i].GetComponent<Card>().decreaseCost(3);
                }
            }
            for (int i = 0; i < CM.field.Count; i++)
            {
                if (CM.field[i].GetComponent<Card>().cardNo == 11)
                {
                    CM.field[i].GetComponent<Card>().decreaseCost(3);
                }
            }
            for (int i = 0; i < CM.Deck.Count; i++)
            {
                if (CM.Deck[i].GetComponent<Card>().cardNo == 11)
                {
                    CM.Deck[i].GetComponent<Card>().decreaseCost(3);
                }
            }
            BM.log.logContent.text += "\n모든 스트레이트 펀치의 코스트가 3 감소합니다.";
        }
    }
    private void Awake()
    {   
        t = 1;
        turnText.text = "" + t;
        PlayerTurn = false;       
        EndButton.SetActive(false);
        turnEndImage.color = new Color(0.3f, 0.3f, 0.3f);
        AM = GameObject.Find("ActManager").GetComponent<ActManager>();
    }
    private void Start()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemy = new Enemy[enemys.Length];
      
        for (int i = 0; i < enemys.Length; i++)
        {
            enemy[i] = enemys[i].GetComponent<Enemy>();
            enemy[i].EnemyStartTurn();
        }
    }
    public void PlayerTurnEnd()
    {
        for (int i = 0; i < BM.Enemys.Length; i++)
        {
            if (!BM.Enemys[i].GetComponent<Enemy>().isDie)
            {
                BM.Enemys[i].GetComponent<Enemy>().EnemyStartTurn();//내 턴 종료시 상대의 턴 한정 은신이나 무적,불사를 해제한다.
            }
        }
        
           
          
            turnCard = 0;

            PlayerTurn = false;

            EndButton.SetActive(false);
            BM.CharacterSelectMode = false;
            BM.EnemySelectMode = false;



            for (int i = 0; i < BM.Enemys.Length; i++)
            {
                BM.Enemys[i].GetComponent<Enemy>().Board.text = "";

            }

            BM.TurnCardCount = BM.CardCount; //뽑아야 할 카드의 수를 디폴트로 변경
            BM.allClear();
            StartCoroutine("TurnEnd");

      
    }

    IEnumerator TurnEnd()
    {
        BM.otherCanvasOn = true;
        for (int i = 0; i < BM.CD.size; i++)
        {
            BM.characters[i].myPassive.TurnEndTimeCount();//턴 종료시 발동할 패시브를 위해

            yield return null;

        }

        AM.Act();//패시브 발동

        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }



        CM.FieldOff();//코루틴이 끝나면 필드에 있는 카드들을 모두 덱으로 넣음

        BM.characters[BM.CD.size - 1].transform.localScale = new Vector3(1, 1, 1);
        BM.otherCanvasOn = false;
        t++; //턴을 1 올림
        turnText.text = "" + t;

    }




    public void PlayerTurnEndButton()
    {
        if (BM.otherCor || BM.turnStarting) return; // 내 턴이 아니므로 눌러도 반응 x
        turnEndImage.color = new Color(0.3f, 0.3f, 0.3f);
        HandManager.Instance.go_UseButton.SetActive(false); //YH
        HandManager.Instance.go_SelectedCardTooltip.SetActive(false); //YH
        HandManager.Instance.SelectCardToOriginPosition(); //YH
        GameObject.Find("ActManager").GetComponent<ActManager>().LateAct();
    }

   
    void PSoff()
    {
        pleaseSelect.SetActive(false);
    }
    public void PlayerTurnStart()
    {
        BM.turnStarting = true;
        GameObject.Find("HandManager").GetComponent<HandManager>().isInited = false;
     
        BM.log.logContent.text += "\n" + t + "턴 시작!";

        BM.cost = BM.startCost+BM.nextTurnStartCost;//턴 시작 시 전 턴에 코스트 증가하는 효과가 있었다면 적용.

        BM.useCost(0);//코스트 글자 변경을 위함

        BM.nextTurnStartCost = 0;
        PlayerTurn =true;

        BM.CharacterSelectMode = true;

        EndButton.SetActive(true);

        for(int i = 0; i < BM.characters.Count; i++)
        {
            if (!BM.characters[i].isDie)
            {
                BM.characters[i].Act = 1;
                if (BM.gd.blessbool[4] && t == 1)  BM.characters[i].Act = 0; 
                if (BM.gd.blessbool[12] && t == 1)BM.characters[i].Act = 0; 
               
                if (BM.gd.blessbool[4]&&t!=1) BM.characters[i].Act++;
                BM.characters[i].onMinusAct(BM.characters[i].NextTurnMinusAct);
                BM.characters[i].turnAtk = BM.characters[i].Atk;
                BM.characters[i].AtkUp(turnAtk);
                BM.characters[i].turnDef = BM.characters[i].def;
                BM.characters[i].getArmor(BM.characters[i].nextarmor);
                if (BM.characters[i].Armor < 0) BM.characters[i].Armor = 0;
                BM.characters[i].nextarmor = 0;
              //턴 시작 시 전 턴에 올라간 값들을 수정
            }
        }

        turnAtk = 0;      

       

        CM.TurnStartCardSet();
       
        
    }
    public void TurnStartPassive()//턴 종료와 똑같이 구성
    {     
        for (int i = 0; i < BM.CD.size; i++)
        {
            BM.characters[i].myPassive.TurnStart();
        }
        AM.Act();
      
    }

}
