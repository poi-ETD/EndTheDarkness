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
   
   
    public int earlyCount;//적이 행동을 정한 만큼 저장,모든 적이 저장이 되면 턴 시작

    public int sum;//이번 행동에 행동해야 할 행동의 총 합, 이 값이 클수록 행동과 행동사이의 시간이 줄어든다.


    public bool isEarlyActing; //적이 선 행동을 하는 중
    bool isLateActing; //적이 후 행동을 하는 중
    List<ActStruct> ActList = new List<ActStruct>();
    List<ActStruct> lateActList = new List<ActStruct>();
    List<ActStruct> earlyActList = new List<ActStruct>();
   
    public void EarlyAct()
    {
        earlyCount++;       
        if (earlyCount == BM.Enemys.Length)
        {
           
            TM.PlayerTurnStart();

            earlyCount = 0;
        }
    }

    public struct ActStruct
    {
        public int type;
        public int no;
        
      
        public int mount;//mount가 필요한 패시브만 적용
        public Enemy myE;
        public Enemy targetE;
        public Character myC;
        public Character targetC;
     
        public int count;

        public ActStruct(int type, int no, int mount, Enemy myE, Enemy targetE, Character myC, Character targetC, int count)
        {
            this.type = type;
            this.no = no;
            this.mount = mount;
            this.myE = myE;
            this.targetE = targetE;
            this.myC = myC;
            this.targetC = targetC;
            this.count = count;
        }
    }
   
    public void earlyAct()
    {
        for (int i = 0; i < earlyActList.Count; i++)
        {
            if (earlyActList[i].no == 1)
            {
                MakeAct(1, 1, earlyActList[i].mount, earlyActList[i].myE, null, null, earlyActList[i].targetC, 1);
                earlyActList[i].targetC.myPassive.ActMinus(earlyActList[i].mount);
            }
            if (earlyActList[i].no== 4)
            {
                MakeAct(1, 4, earlyActList[i].mount, earlyActList[i].myE, null, null, null, 1);
            }
            if (earlyActList[i].no == 6)
            {               
                BM.FormationCollapse(earlyActList[i].myE.Name);
            }
        }
        earlyActList.Clear();
        Act();
    }
    public void LateAct()
    {
        isLateActing = true;
        for (int i = 0; i < lateActList.Count; i++)
        {
            if (lateActList[i].no == 0)
            {
                MakeAct(1, 0, lateActList[i].mount, lateActList[i].myE, null, null, lateActList[i].targetC, 1);
                lateActList[i].targetC.onHit(lateActList[i].mount, lateActList[i].myE);
            }
            else if (lateActList[i].no == 2)
            {
                MakeAct(1, 2, lateActList[i].mount, lateActList[i].myE, lateActList[i].targetE, null, null, 1);
            }

            else if (lateActList[i].no == 3)
            {
                MakeAct(1, 3, lateActList[i].mount, lateActList[i].myE, lateActList[i].targetE, null, null, 1);
            }
        }
        Act();
        lateActList.Clear();
    }
    public void MakeLateAct(int no,int mount,Character target,Enemy myE,Enemy targetE)
    {
        ActStruct newActStruct = new ActStruct(1, no, mount, myE, targetE, null,target, 1);
        lateActList.Add(newActStruct);
    }
    public void MakeEarlyAct(int no, int mount, Character target, Enemy myE, Enemy targetE)
    {
        ActStruct newActStruct = new ActStruct(1, no, mount, myE, targetE, null, target, 1);
        earlyActList.Add(newActStruct);
    }
    public void MakeAct(int type,int no, int mount, Enemy myE,Enemy targetE, Character myC, Character targetC, int count) 
    {
       sum+=count;
       ActStruct newActStruct = new ActStruct(type,no, mount, myE,targetE, myC, targetC, count);
       ActList.Add(newActStruct);
    }

    public void Act()
    {
        StartCoroutine(ActCo());
    }

  
  
    IEnumerator ActCo()
    {
       
        while (BM.otherCanvasOn)
        {
            yield return new WaitForSeconds(0.01f);
        }
        while (BM.otherCor)
        {
            yield return new WaitForSeconds(0.01f);
        }

        BM.otherCor = true;  
        
        yield return new WaitForSeconds(0.2f);    
        while (ActList.Count != 0)
        {
          
            int c = 0;          
            if (ActList[0].type == 0) //아군 패시브 발동
            {
                BM.log.setPassive(ActList[0].no, ActList[0].count); //패시브를 로그에 기록
                while (ActList[0].count != c)
                {
                    c++;
                    ActList[0].myC.SelectBox.SetActive(true);

                    if (ActList[0].no == 1)
                    {
                        ActList[0].myC.myPassive.Q1();
                    }
                    if (ActList[0].no == 2)
                    {
                        ActList[0].myC.myPassive.Q2(ActList[0].mount);
                    }
                    if (ActList[0].no == 3)
                    {
                        ActList[0].myC.myPassive.Q3(ActList[0].mount);
                    }
                    if (ActList[0].no == 4)
                    {
                        ActList[0].myC.myPassive.Q4();
                    }
                    if (ActList[0].no == 5)
                    {
                        ActList[0].myC.myPassive.Sparky1();
                    }
                    if (ActList[0].no == 6)
                    {

                        ActList[0].myC.myPassive.Sparky2(ActList[0].mount, ActList[0].targetE);
                    }
                    if (ActList[0].no == 7)
                    {
                        ActList[0].myC.myPassive.Sparky3();
                    }
                    if (ActList[0].no == 8)
                    {
                        ActList[0].myC.myPassive.Sparky4();
                    }
                    if (ActList[0].no == 9)
                    {
                        ActList[0].myC.myPassive.Vangara1();
                    }
                    if (ActList[0].no == 10)
                    {
                        ActList[0].myC.myPassive.Vangara2(ActList[0].mount);
                    }

                    if (ActList[0].no == 11)
                    {
                        ActList[0].myC.myPassive.Vangara3(ActList[0].mount, ActList[0].targetE);
                    }
                    if (ActList[0].no == 12)
                    {
                        ActList[0].myC.myPassive.Vangara4();
                    }
                    if (ActList[0].no == 13)
                    {
                        ActList[0].myC.myPassive.Porte1();

                    }
                    if (ActList[0].no == 14)
                    {
                        ActList[0].myC.myPassive.Porte2();
                    }
                    if (ActList[0].no == 15)
                    {
                        // myActList[0].myC.myPassive.Porte3();
                    }
                    if (ActList[0].no == 16)
                    {
                        ActList[0].myC.myPassive.Porte4();
                    }

                    yield return new WaitForSeconds(2f / sum);

                    ActList[0].myC.SelectBox.SetActive(false);
                    yield return new WaitForSeconds(0.5f / sum);
                } //패시브 횟수만큼

                c = 0;
            }
            else if(ActList[0].type==1) //적의 행동
            {
                yield return new WaitForSeconds(0.2f);
                if (ActList[0].no == 0) //적의 공격
                {
                    ActList[0].targetC.onDamage(ActList[0].mount);
                    ActList[0].myE.transform.DOMove(ActList[0].myE.transform.position + new Vector3(0, -0.5f, 0), 0.2f);
                    yield return new WaitForSeconds(0.2f);
                    ActList[0].myE.transform.DOMove(ActList[0].myE.transform.position + new Vector3(0, 0.5f, 0), 0.2f);                 
                    yield return new WaitForSeconds(0.2f);
                }
                if (ActList[0].no == 1) //적의 행동력 감소
                {
                    ActList[0].targetC.onMinusAct(ActList[0].mount);
                    ActList[0].myE.transform.DOMove(ActList[0].myE.transform.position + new Vector3(0, -0.5f, 0), 0.2f);
                    yield return new WaitForSeconds(0.2f);
                    ActList[0].myE.transform.DOMove(ActList[0].myE.transform.position + new Vector3(0, 0.5f, 0), 0.2f);
                    yield return new WaitForSeconds(0.2f);
                }
                if (ActList[0].no == 2) //적의 방어력 획득
                {
                    ActList[0].targetE.GetArmorStat(ActList[0].mount);
                    ActList[0].myE.transform.DOMove(ActList[0].myE.transform.position + new Vector3(0, -0.5f, 0), 0.2f);
                    yield return new WaitForSeconds(0.2f);
                    ActList[0].myE.transform.DOMove(ActList[0].myE.transform.position + new Vector3(0, 0.5f, 0), 0.2f);
                    yield return new WaitForSeconds(0.2f);
                }
                if (ActList[0].no == 3) //적의 체력 회복
                {
                    ActList[0].targetE.GetHp(ActList[0].mount);
                    ActList[0].myE.transform.DOMove(ActList[0].myE.transform.position + new Vector3(0, -0.5f, 0), 0.2f);
                    yield return new WaitForSeconds(0.2f);
                    ActList[0].myE.transform.DOMove(ActList[0].myE.transform.position + new Vector3(0, 0.5f, 0), 0.2f);
                    yield return new WaitForSeconds(0.2f);
                }
                if (ActList[0].no == 4) //적의 상태 변화
                {
                    if (ActList[0].mount == 0) //은신
                    {
                        ActList[0].myE.onShadow();
                        for (int i = 0; i < 10; i++)
                        {
                          ActList[0].myE.myImage.color -= new Color(0, 0, 0, 0.07f);
                            yield return new WaitForSeconds(0.05f);
                        }
                    }
                   
                }
            }
            else if (ActList[0].type == 2)
            {
                ActList[0].targetE.onHit(ActList[0].mount); //적이 데미지를 입는 모션(데미지 계산은 이미 적용)
            }
            ActList.RemoveAt(0);
          
        }
        if (isEarlyActing) //적의 선 행동이 끝나면 내 턴이 된다.
        {
            isEarlyActing = false;
            TM.turnEndImage.color = new Color(1, 1, 1);
        }
        if (BM.turnStarting)
        {
            earlyAct();
            isEarlyActing = true;
        }
        if (isLateActing)
        {
            TM.PlayerTurnEnd();
            isLateActing = false;
        }
     
        sum = 1;
        BM.log.writePassiveInLog();
        BM.turnStarting = false;
        BM.otherCor = false;
        yield return null;
     
    }
}
