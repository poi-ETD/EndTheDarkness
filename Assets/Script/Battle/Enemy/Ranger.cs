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
        if (curTurn != TM.t)
        {
            StartPattern();
        }
    }
    void StartPattern()
    {
      
        myEnemy.Board.text = "";
        if (BM.diecount < 4)
        {
            if (!myEnemy.isDie)
            {              
                if (onecounter == 4)
                {
                    done[0] = false;
                    done[1] = false;
                    done[2] = false;
                    onecounter = 0;
                    if (BM.forward.Count > 0)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            int rand2 = Random.Range(0, BM.forward.Count);
                            while (BM.characters[rand2].isDie)
                                rand2 = Random.Range(0, BM.forward.Count);
                            BM.characters[rand2].onHit(3, myEnemy.Name);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            int rand2 = Random.Range(0, 4);
                            while (BM.characters[rand2].isDie)
                                rand2 = Random.Range(0, 4);
                            BM.characters[rand2].onHit(3, myEnemy.Name);
                        }

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
                    if (BM.forward.Count > 0)
                    {
                        int rand2 = Random.Range(0, BM.forward.Count);
                        while (BM.characters[rand2].isDie)
                        rand2 = Random.Range(0, BM.forward.Count);
                        BM.characters[rand2].onHit(3, myEnemy.Name);

                    }
                    else
                    {
                        int rand2 = Random.Range(0, 4);
                        while (BM.characters[rand2].isDie)
                            rand2 = Random.Range(0, 4);
                        BM.characters[rand2].onHit(3, myEnemy.Name);


                    }
                    onecounter++;
                }
            }
            }
          
            curTurn++;
        }
    }
}