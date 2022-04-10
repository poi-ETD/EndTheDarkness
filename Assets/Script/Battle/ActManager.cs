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
        
        while (BM.earlyActList.Count != 0)
        {
            bool notPop = false;
            if (BM.earlyActList[0].type == 1)
            {
               
                BM.earlyActList[0].myEnemy.transform.DOMove(BM.earlyActList[0].myEnemy.transform.position+new Vector3(0, -0.5f,0), 0.3f);
                BM.earlyActList[0].target.myImage.color = Color.blue;
                yield return new WaitForSeconds(0.5f);
                BM.earlyActList[0].myEnemy.transform.DOMove(BM.earlyActList[0].myEnemy.transform.position + new Vector3(0, 0.5f, 0), 0.2f);
                int t = BM.earlyActList[0].target.myPassive.ActMinus(BM.earlyActList[0].mount);
                yield return new WaitForSeconds(0.3f + t);
                BM.earlyActList[0].target.myImage.color = Color.white;
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
        yield return null;
    }
    public void LateAct()
    {
        StartCoroutine(LateActCor());
    }
   IEnumerator LateActCor()
    {
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
                    float t = 0;
                    BM.lateActList[0].myEnemy.transform.DOMove(BM.lateActList[0].myEnemy.transform.position + new Vector3(0, -0.5f, 0), 0.3f);
                    BM.lateActList[0].target.myImage.color = Color.red;
                    yield return new WaitForSeconds(0.5f);
                    BM.lateActList[0].myEnemy.transform.DOMove(BM.lateActList[0].myEnemy.transform.position + new Vector3(0, 0.5f, 0), 0.2f);
                    t = BM.lateActList[0].target.onDamage(BM.lateActList[0].mount, BM.lateActList[0].myEnemy);
                    yield return new WaitForSeconds(0.3f + t);
                    if (!BM.lateActList[0].target.isDie)
                    {
                        BM.lateActList[0].target.myImage.color = Color.white;
                    }
                    else BM.lateActList[0].target.myImage.color = new Color(0.3f, 0.3f, 0.3f);
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
                BM.lateActList.RemoveAt(0);
            }
        }
        yield return null;
        TM.PlayerTurnEnd();
    }
}
