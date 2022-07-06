using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class ActManager : MonoBehaviour
{
    BattleManager BM;
    TurnManager TM;
    List<Character> characters = new List<Character>();
    List<Enemy> enemys = new List<Enemy>();
    List<ord> orderList = new List<ord>();
    [SerializeField] Transform speedLineTrans;
    public int curOrder;
    bool isTurn;
    public bool isStartAct;
    public float curSpeed;
    public struct ord{
        public float value;
        public int type;
        public int obj;

        public ord(float value, int type,int obj) 
        {
    
            this.value = value;
            this.type = type;
            this.obj = obj;
           
        }
      
    }
    
    void Start()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        characters = BM.characters;
        for(int i = 0; i < BM.Enemys.Length; i++)
        {
            enemys.Add(BM.Enemys[i].GetComponent<Enemy>());
        }
    }
    public void ActComplete() //행동 종료 패시브를 불러 일으키기 위한 함수.
    {
        BM.actCharacter.myPassive.myAct();
    }
    public void SpeedChangeByEffect(int type,int no)
    {
        if (!isTurn) return;
        Debug.Log("속도 변화");
        if (type == 0) //팀이 속도가 변했을 경우
        {
            for(int i = 0; i < orderList.Count; i++)
            {
                if (orderList[i].type == 0 && orderList[i].obj == no)
                {
                    orderList.RemoveAt(i);
                }
            } //현재 선택된 팀의 예정 행동을 다 없앰
            float previousActTime = characters[no].curTurnActTime;
            if (characters[no].curTurnActTime+characters[no].curSpeed<curSpeed)
            {
                characters[no].curTurnActTime = curSpeed - characters[no].curSpeed;
            }

            float characterActing = characters[no].curTurnActTime + characters[no].curSpeed;
          
            ord newOrd = new ord(characterActing, 0, no);         
            if (characterActing <= 10 || previousActTime == 0)
            {
                Debug.Log(characterActing);
                orderList.Add(newOrd);
            }
            characterActing += characters[no].curSpeed;
            while (characterActing <= 10)
            {
                Debug.Log(characterActing);
                ord newOrd2 = new ord(characterActing, 0, no);
                characterActing += characters[no].curSpeed;
                orderList.Add(newOrd2);
            }
            orderList = orderList.OrderBy(obj =>
            {
                return obj.value;
            }).ToList();
            SpeedImageChange();
        }
        else //적이 속도가 변했을 경우
        {

        }
    }
    public void SetOrder()
    {
        isTurn = true;
        curOrder = 0;
        orderList.Clear();
       
        for (int i = 0; i < characters.Count; i++)
        {
            float s = characters[i].speed;
            if (s <= 1) characters[i].speed=1;
            ord newOrd = new ord(s, 0, i);
            orderList.Add(newOrd);
            while (s + characters[i].speed <= 10)
            {
                s += characters[i].speed;
                ord newOrd2 = new ord(s, 0, i);
                orderList.Add(newOrd2);
            }
        }
        for(int i = 0; i < enemys.Count; i++)
        {
            float s = enemys[i].speed;
            if (s <= 1) enemys[i].speed = 1;
            ord newOrd = new ord(s, 1, i);
            orderList.Add(newOrd);
            while (s + enemys[i].speed <= 10)
            {
                s += enemys[i].speed;
                ord newOrd2 = new ord(s, 1, i);
                orderList.Add(newOrd2);
            }
        }
        orderList = orderList.OrderBy(obj =>
        {
            return obj.value;
        }).ToList();
        SpeedImageChange();
        ActByOrder();
    }
    public void SpeedImageChange()
    {
       
        for (int i = 0; i < 11; i++)
        {
            speedLineTrans.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < orderList.Count; i++)
        {

            speedLineTrans.GetChild(i).gameObject.SetActive(true);
            if (orderList[i].type == 0)
            {
                speedLineTrans.GetChild(i).GetComponent<Image>().sprite = characters[orderList[i].obj].myImage.sprite;
            }
            else
            {
                speedLineTrans.GetChild(i).GetComponent<Image>().sprite = enemys[orderList[i].obj].face;
            }
        }
    }
    public void ActByOrder()
    {
        if (orderList.Count == 0)
        {
            isTurn = false;
            TM.PlayerTurnEndButton();
            return;
        }
      
        SpeedImageChange();
        int or = 0;
        curSpeed = orderList[or].value;
        BM.CancleCharacter();   
        curOrder = or;
        
        if (orderList[or].type == 0)
        {
            if (!characters[orderList[or].obj].isDie)
            {
                BM.ShowCharacterHaveTurn(characters[orderList[or].obj].gameObject);
            }
            else
            {
                orderList.RemoveAt(0);
                ActByOrder();
            }
        }
        else
        {
            if (!enemys[orderList[or].obj].isDie)
            {
                enemys[orderList[or].obj].isAct = true;
            }
            else
            {
                orderList.RemoveAt(0);
                ActByOrder();
            }
        }
    }
    public int earlyCount;//적이 행동을 정한 만큼 저장,모든 적이 저장이 되면  시작

    public int sum;//이번 행동에 행동해야 할 행동의 총 합, 이 값이 클수록 행동과 행동사이의 시간이 줄어든다.


    public bool isEarlyActing; //적이 선 행동을 하는 중
    bool isLateActing; //적이 후 행동을 하는 중
    List<ActStruct> ActList = new List<ActStruct>();
    List<ActStruct> lateActList = new List<ActStruct>();
    List<ActStruct> earlyActList = new List<ActStruct>();
   
   /* public void EarlyAct()
    {
        earlyCount++;       
        if (earlyCount == BM.Enemys.Length)
        {       
            TM.PlayerTurnStart();
            earlyCount = 0;
        }
    }*/

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
        //no=>-1 리플렉트
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
                        ActList[0].myC.myPassive.Q3();
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
                    if (ActList[0].no == 17)
                    {
                        ActList[0].myC.myPassive.Ryung1();
                    }
                    if (ActList[0].no == -1)
                    { //캐릭터가 타겟에게 공격
                        ActList[0].targetE.OnHitCal(ActList[0].mount, ActList[0].myC.curNo, false);
                    }
                    if (ActList[0].no == -2)
                    {
                        for(int i=0;i<BM.Enemys.Length;i++) //캐릭터가 모든 타겟에게 공격
                        BM.Enemys[i].GetComponent<Enemy>().OnHitCal(ActList[0].mount, ActList[0].myC.curNo, false);
                    }
                    if (ActList[0].no >=100) //상태이상은 100번부터
                    {
                        ActList[0].targetE.StatusChange(ActList[0].no-100, ActList[0].mount);
                    }
                    if (sum == 1)
                    {
                        yield return new WaitForSeconds(1);
                    }
                    else
                        yield return new WaitForSeconds(2f / sum);
                    if(BM.actCharacter!=ActList[0].myC)
                    ActList[0].myC.SelectBox.SetActive(false);
                    yield return new WaitForSeconds(0.5f / sum);
                } //패시브 횟수만큼

                c = 0;
            }
            else if (ActList[0].type == 1) //적의 행동
            {
                if (ActList[0].myE.isDie) yield return null;
                else
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
            }
            else if (ActList[0].type == 2)
            {
                ActList[0].targetE.onHit(ActList[0].mount); //적이 데미지를 입는 모션(데미지 계산은 이미 적용)
                yield return new WaitForSeconds(0.2f);
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
            //earlyAct();
            //isEarlyActing = true;
            TM.turnEndImage.color = new Color(1, 1, 1);
            SetOrder();
        }
        else if (isTurn)
        {
            if (isStartAct)
            {
                isStartAct = false;
            }
            else
            {
                if (orderList.Count>0)
                {
                    if (orderList[0].type == 0)
                    {
                        characters[orderList[0].obj].curTurnActTime = curSpeed;
                    }
                    orderList.RemoveAt(0);
                    ActByOrder();
                }
                else
                {
                    isTurn = false;
                    TM.PlayerTurnEndButton();
                }
            }
        }
       
     
        sum = 1;
        BM.log.writePassiveInLog();
        BM.turnStarting = false;
        BM.otherCor = false;
        yield return null;
     
    }
}
