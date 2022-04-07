using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ActManager : MonoBehaviour
{
    BattleManager BM;
    TurnManager TM;
    void Start()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
    }
    public struct EnemyAct
    {
        public int type; /*
                    0->데미지 1->데미지+행감 2->방어도 3->체력 회복 4->상태이상 5->은신/무적 
        그 외 는 해당 스크립트 가 아닌 각각 개별 스크립트에서 즉발로 처리 ex)공격력 증가
        */
       public int mount; //데미지 , 방어도, 회복 량,상태이상 종류*10+상태이상 양(여기는 변경가능성 多,0>은신 1>무적 2>불사)
       public int target; //10일 시 모든 타겟
       public int myEnemy;
        public EnemyAct(int type, int mount, int target, int myEnemy)
        {
            this.type = type;
            this.mount = mount;
            this.target = target;
            this.myEnemy = myEnemy;
        }
    }
   public struct CharacterAct
    {
        public int type;
        public int mount;
        public int target;
        public int myCharacer;
    }
    List<EnemyAct> enemyAct = new List<EnemyAct>();
    int earlyCount;
    public void EarlyAct()
    {
        earlyCount++;
        if (earlyCount == BM.Enemys.Length)
        {
            TM.PlayerTurnStart();
            earlyCount = 0;
        }
    }
    public void LateAct()
    {

    }
   
}
