using System.Collections;   
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TurnManager : MonoBehaviour
{
    public bool PlayerTurn;
    [SerializeField] GameObject[] enemys;
    [SerializeField] Enemy[] enemy;
    public int t;
    public GameObject EndButton;
    public BattleManager BM;
    ActManager AM;
    public TextMeshProUGUI turnText;
    public int turnCard;
    public CardManager CM;
    public int leftCost;
    [SerializeField] GameObject pleaseSelect;
    public Image turnEndImage;
    public int turnAtk;
    public void turnCardPlus()
    {
        turnCard++;
        if (turnCard == 4)
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
        for(int i = 0; i < BM.Enemys.Length; i++)
        {
            if (!BM.Enemys[i].GetComponent<Enemy>().isDie)
            {
                BM.Enemys[i].GetComponent<Enemy>().EnemyStartTurn();
            }
        }
        
            leftCost = BM.cost;
            for (int i = 0; i < BM.characters.Count; i++)
            {
                BM.characters[i].board.text = "";
                if (!BM.characters[i].isDie && BM.characters[i].card8)
                {
                    BM.characters[i].card8 = false;
                    BM.characters[i].getArmor(leftCost * BM.characters[i].card8point);
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

            BM.TurnCardCount = BM.CardCount;
            BM.allClear();
            StartCoroutine("TurnEnd");

      
    }

    IEnumerator TurnEnd()
    {
        BM.otherCanvasOn = true;
        for (int i = 0; i < BM.CD.size; i++)
        {

            BM.characters[i].myPassive.TurnEndTimeCount();
            yield return null;

        }

        AM.Act();

        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }



        CM.FieldOff();

        BM.characters[BM.CD.size - 1].transform.localScale = new Vector3(1, 1, 1);
        BM.otherCanvasOn = false;
        t++;
        turnText.text = "" + t;
        BM.curMessage.text = "";

    }



    public void Click_PlayerTurnEndButton()
    {   if (BM.otherCor || BM.turnStarting||AM.isEarlyActing) return; //위의 3 경우에는 내 턴이 아니므로 눌러도 반응 x
        turnEndImage.color = new Color(0.3f, 0.3f, 0.3f);
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

        BM.cost = BM.startCost+BM.nextTurnStartCost;

        BM.useCost(0);
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
                BM.characters[i].turnEndur = BM.characters[i].endur;
                BM.characters[i].getArmor(BM.characters[i].nextarmor);
                if (BM.characters[i].Armor < 0) BM.characters[i].Armor = 0;
                BM.characters[i].nextarmor = 0;
              
            }
        }

        turnAtk = 0;      

        if (BM.card22on)
        {
            int ArmorSum = 0;
            for (int i = 0; i < BM.characters.Count; i++)
            {
                if (!BM.characters[i].isDie)
                {
                    ArmorSum += BM.characters[i].Armor;
                    BM.characters[i].getArmor(-1 * BM.characters[i].Armor);
                }
            }
            BM.card22c.getArmor(ArmorSum);
            BM.card22on = false;
        }

        CM.TurnStartCardSet();
       
        
    }
    public void TurnStartPassive()
    {     
        for (int i = 0; i < BM.CD.size; i++)
        {
            BM.characters[i].myPassive.TurnStart();
        }
        AM.Act();
      
    }

}
