using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranger : MonoBehaviour
{
    public TurnManager TM;
    public int curTurn;
    public Enemy myEnemy;  
    public BattleManager BM;
    public List<Character> HaveArmor = new List<Character>();
    public List<Character> ForwardHaveArmor = new List<Character>();
    [SerializeField] Text t;
    [SerializeField] int plusname;
    int onecounter;
    bool[] done=new bool[3];
    private void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        myEnemy.Name = "레인저"+plusname;
        StartPattern();
    }
    void Update()
    {
        if (myEnemy.isDie)
        {
            myEnemy.Hp = 0;
         
        }
        if (curTurn != TM.turn)
        {
            StartPattern();
        }
    }
    void StartPattern()
    {
      
        myEnemy.Board.text = "";
        if (BM.teamDieCount < BM.characters.Count)
        {
            if (!myEnemy.isDie)
            {              
                if (onecounter == 4)
                {
                    done[0] = false;
                    done[1] = false;
                    done[2] = false;
                    onecounter = 0;
                   
                        for (int i = 0; i < 2; i++)
                        {
                        BM.HitFront(3, 0, myEnemy, false);
                        }
                  
                }
                else { 
                for (int i = 0; i < 3; i++)
                {
                    if (!done[i])
                    {
                        break;
                    }
                    if (i == 2)
                    {
                        done[0] = false;
                        done[2] = false;
                        done[1] = false;
                    }
                }//done 3개 다 true라면 초기화
                int rand = Random.Range(0, 3);
                while (done[rand])
                {
                    rand = Random.Range(0, 3);
                }
                    done[rand] = true;
                    if (rand == 2)
                    {

                        myEnemy.GetArmor(3, myEnemy.Name);
                    }
                    else
                    {

                        BM.HitFront(3, 0, myEnemy, false);

                    }
                 
                    
                    onecounter++;
                }
            
            }
          
            curTurn++;
        }
    }
}