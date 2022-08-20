using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMaker : MonoBehaviour
{
   
    private static CardMaker instance;

    public static CardMaker Instance
    {
        get
        {
            if (null == instance)
            {

                instance = new CardMaker();
            }
            return instance;
        }
    }
    //0->스탠다드,1->에디셔널 2->토큰
    public int MakeSpecificDeckCard(int deck,int type) //특정 덱의 카드를 만드는 함수
    {
        List<int> randomList = new List<int>();
        if (type == 1)
        {
            for(int i = 1; i < CardInfo.Instance.cd.Length; i++)
            {
                if (CardInfo.Instance.cd[i].Deck == deck && CardInfo.Instance.cd[i].type == 1)
                {
                    randomList.Add(i);
                }
            }
        }
        for (int i = 1; i < CardInfo.Instance.cd.Length; i++)
        {
            if (CardInfo.Instance.cd[i].Deck == deck && CardInfo.Instance.cd[i].type == 0)
            {
                randomList.Add(i);
            }
        }
        return randomList[Random.Range(0,randomList.Count)];
    }
    public int MakeAllCard(int[] myDecks,int type) //특정 타입의 카드를 만드는 함수,덱 풀은 모든 덱
    {
        bool[] flag = new bool[10];
        for(int i = 0; i <myDecks.Length; i++)
        {
            flag[myDecks[i]] = true;
        }
        List<int> randomList = new List<int>();
        if (type == 1)
        {
            for (int i = 1; i < CardInfo.Instance.cd.Length; i++)
            {
                if (flag[CardInfo.Instance.cd[i].Deck]&& CardInfo.Instance.cd[i].type == 1)
                {
                    randomList.Add(i);
                }
            }
        }
        for (int i = 1; i < CardInfo.Instance.cd.Length; i++)
        {
            if (flag[CardInfo.Instance.cd[i].Deck] && CardInfo.Instance.cd[i].type == 0)
            {
                randomList.Add(i);
            }
        }
        return randomList[Random.Range(0, randomList.Count)];
    }
}
