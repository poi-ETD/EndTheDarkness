using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActManager : MonoBehaviour
{
    BattleManager BM;
    TurnManager TM;
    void Start()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
    }
   
   
    int earlyCount;
    public int sum;
    public void EarlyAct()
    {
        earlyCount++;
       
        if (earlyCount == BM.Enemys.Length)
        {
           
            TM.PlayerTurnStart();

            earlyCount = 0;
        }
    }
    public void EarlyActCorStart()
    {
      
        StartCoroutine("EarlyActCor");
    }
    IEnumerator EarlyActCor()
    {
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        while (BM.earlyActList.Count != 0)
        {
           
            bool notPop = false;
            if (BM.earlyActList[0].type == 1)
            {               
                BM.earlyActList[0].myEnemy.transform.DOMove(BM.earlyActList[0].myEnemy.transform.position+new Vector3(0, -0.5f,0), 0.3f);
            
                yield return new WaitForSeconds(0.5f);
                BM.earlyActList[0].myEnemy.transform.DOMove(BM.earlyActList[0].myEnemy.transform.position + new Vector3(0, 0.5f, 0), 0.2f);
                BM.earlyActList[0].target.myPassive.ActMinus(BM.earlyActList[0].mount,BM.earlyActList[0].myEnemy);
                yield return new WaitForSeconds(0.3f);
                
            }
            if (BM.earlyActList[0].type == 4)
            { //0은신 1무적 2불사
                if (BM.earlyActList[0].mount == 0) {
                    BM.earlyActList[0].myEnemy.onShadow();
                    for (int i = 0; i < 10; i++)
                    {
                        BM.earlyActList[0].myEnemy.myImage.color -= new Color(0, 0, 0, 0.07f);
                       yield return new WaitForSeconds(0.05f);
                    }
                }
                

            }
            if (BM.earlyActList[0].type == 6)
            {
                if (BM.earlyActList.Count == 1)
                {
                    BM.FormationCollapse(BM.earlyActList[0].myEnemy.Name);
                }
                else
                {
                    BM.earlyActList.Reverse();
                    notPop = true;
                }
            }
            if(!notPop)
            BM.earlyActList.RemoveAt(0);
           
        }       
        BM.turnStarting = false;
      
        TM.turnEndImage.color = new Color(1, 1, 1);
        if (BM.porte3mode)
            BM.Porte3On();
        MyAct();
        yield return null;
    }
    public void LateAct()
    {
        StartCoroutine(LateActCor());
    }
    IEnumerator LateActCor()
    {      while (BM.otherCor)
            {
                yield return new WaitForSeconds(0.1f);
            }
        while (BM.lateActList.Count != 0)
        {
           
            if (BM.lateActList[0].myEnemy.isDie)
            {
                BM.lateActList.RemoveAt(0);
                yield return null;
            }
            else
            {
                if (BM.lateActList[0].type == 0)
                {                 
                    BM.lateActList[0].myEnemy.transform.DOMove(BM.lateActList[0].myEnemy.transform.position + new Vector3(0, -0.5f, 0), 0.3f);                   
                    yield return new WaitForSeconds(0.5f);
                    BM.lateActList[0].myEnemy.transform.DOMove(BM.lateActList[0].myEnemy.transform.position + new Vector3(0, 0.5f, 0), 0.2f);
                    BM.lateActList[0].target.onHit(BM.lateActList[0].mount, BM.lateActList[0].myEnemy);
                    yield return new WaitForSeconds(0.3f);             
                }
                else if (BM.lateActList[0].type == 2)
                {
                    BM.lateActList[0].myEnemy.transform.DOMove(BM.lateActList[0].myEnemy.transform.position + new Vector3(0, -0.5f, 0), 0.3f);
                    yield return new WaitForSeconds(0.5f);
                    BM.lateActList[0].myEnemy.transform.DOMove(BM.lateActList[0].myEnemy.transform.position + new Vector3(0, 0.5f, 0), 0.2f);
                    BM.lateActList[0].targetEnemy.GetArmorStat(BM.lateActList[0].mount);
                    yield return new WaitForSeconds(0.3f);

                }

                else if (BM.lateActList[0].type == 3)
                {
                    BM.lateActList[0].myEnemy.transform.DOMove(BM.lateActList[0].myEnemy.transform.position + new Vector3(0, -0.5f, 0), 0.3f);
                    yield return new WaitForSeconds(0.5f);
                    BM.lateActList[0].myEnemy.transform.DOMove(BM.lateActList[0].myEnemy.transform.position + new Vector3(0, 0.5f, 0), 0.2f);
                    BM.lateActList[0].targetEnemy.GetHp(BM.lateActList[0].mount);

                    yield return new WaitForSeconds(0.3f);

                }
                if(BM.lateActList.Count>0)
                BM.lateActList.RemoveAt(0);
            }
          
            for (int i = 0; i < BM.CD.size; i++) BM.characters[i].onDamage();
        }              
        TM.PlayerTurnEnd();
    }


    public struct ActStruct
    {
        public int no;
        //1~4 -> 큐 패시브 5~8 스파키  101~120->축복 1~20번 201~220 아이템 100->행동력감소

        public int mount;//mount가 필요한 패시브만 적용
        public Enemy E;
        public Character myC;
        public Character targetC;
        public int count;

        public ActStruct(int no, int mount, Enemy e, Character myC, Character targetC, int count)
        {
            this.no = no;
            this.mount = mount;
            E = e;
            this.myC = myC;
            this.targetC = targetC;
            this.count = count;
        }
    };

    List<ActStruct> myActList = new List<ActStruct>();

    public void MakeAct(int no, int mount, Enemy e, Character myC, Character targetC, int count) 
    {
       sum+=count;
       ActStruct newActStruct = new ActStruct(no, mount, e, myC, targetC, count);
       myActList.Add(newActStruct);
    }
    public void MyAct()
    {
        StartCoroutine(MyActCo());
    }
    string SetPassiveName(int i) //패시브 번호에 따라 로그 출력을 위한 함수
    {
        Debug.Log(i);
        string s = "";
        CharacterData2 chD=new CharacterData2();
        int k = i % 4-1;
        int m = i / 4 + 1;
        if (k == -1) { k = 3; m--; }
        s = chD.cd[m].passive[k];
        return s;
    }
    IEnumerator MyActCo()
    {
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.01f);
        }
        BM.otherCor = true;  
        
        yield return new WaitForSeconds(0.2f);

      
        while (myActList.Count != 0)
        {
          
            int c = 0;
            string passiveName="";
            passiveName = SetPassiveName(myActList[0].no);
            BM.log.logContent.text += "\n" + myActList[0].myC.Name + "이(가) " + passiveName + " 발동(" + myActList[0].count + ")";

            while (myActList[0].count != c)
            {           
                c++;
                myActList[0].myC.SelectBox.SetActive(true);
                if (myActList[0].no == 1)
                {
                    myActList[0].myC.myPassive.Q1();
                }
                if (myActList[0].no == 2)
                {                  
                    myActList[0].myC.myPassive.Q2(myActList[0].mount);
                }
                if (myActList[0].no == 3)
                {
                    myActList[0].myC.myPassive.Q3(myActList[0].mount);
                }
                if (myActList[0].no == 4)
                {
                    myActList[0].myC.myPassive.Q4();
                }
                if (myActList[0].no == 5)
                {
                    myActList[0].myC.myPassive.Sparky1();
                }
                if (myActList[0].no == 6)
                {
                  
                 myActList[0].myC.myPassive.Sparky2(myActList[0].mount,myActList[0].E);
                }
                if (myActList[0].no == 7)
                {
                    myActList[0].myC.myPassive.Sparky3();
                }
                if (myActList[0].no == 8)
                {
                    myActList[0].myC.myPassive.Sparky4();
                }
                if (myActList[0].no == 9)
                {
                    myActList[0].myC.myPassive.Vangara1();
                }
                if (myActList[0].no == 10)
                {
                    myActList[0].myC.myPassive.Vangara2(myActList[0].mount);
                }

                if (myActList[0].no == 11)
                {
                    myActList[0].myC.myPassive.Vangara3(myActList[0].mount,myActList[0].E);
                }
                if (myActList[0].no == 12)
                {
                    myActList[0].myC.myPassive.Vangara4();
                }
                if (myActList[0].no == 13)
                {
                    myActList[0].myC.myPassive.Porte1();
                    
                }
                if (myActList[0].no == 14)
                {
                    myActList[0].myC.myPassive.Porte2();
                }
                if (myActList[0].no == 15)
                {
                   // myActList[0].myC.myPassive.Porte3();
                }
                if (myActList[0].no == 16)
                {                    
                    myActList[0].myC.myPassive.Porte4();
                }               
                
                yield return new WaitForSeconds(2f/sum);
              
                myActList[0].myC.SelectBox.SetActive(false);
                yield return new WaitForSeconds(0.5f/sum);
            }
          
            c = 0;
            myActList.RemoveAt(0);
          
        }
        if (BM.turnStarting)
        {
            EarlyActCorStart();
        }
        sum = 1;
        BM.turnStarting = false;
        BM.otherCor = false;
        yield return null;
     
    }
}
