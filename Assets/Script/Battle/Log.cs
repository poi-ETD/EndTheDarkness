using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Log : MonoBehaviour
{
    public TextMeshProUGUI logContent;
    [SerializeField] BattleManager BM;
    [SerializeField] GameObject OnButton;
    [SerializeField] GameObject logTextPosition;
    [SerializeField] GameObject logText;
    bool done;
    int[] passive = new int[30];

    private void Update()
    {
        //if (done)
        //{
        //    done = false;
        //    loadLog();
        //} YH : 굳이 없어도 되는 로직 같아서 주석 처리
    }

    public void onLog()
    {    
        OnButton.SetActive(false);
        BM.otherCanvasOn = true;

        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(1950f, 0f);
        gameObject.GetComponent<RectTransform>().DOMove(new Vector2(0f, 0f), 1f).SetEase(Ease.OutExpo);
        //gameObject.transform.DOMove(new Vector3(-1f, 0f, 0f), 1f).SetEase(Ease.OutExpo);

        done = true;
    }

    void loadLog()
    {
        float a = logText.GetComponent<RectTransform>().rect.height - 1080;
        logTextPosition.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, a, 0);
    }

    public void offLog() // YH : 로그가 완전히 사라지지 않았는데 otherCanvasOn이 false가 되어 문제점이 발견된다면 현재 함수를 코루틴으로 변경하여
                         // 1초(DOMove 2번째 인자) 후 otherCanvasOn을 false 시키는 것 고려할 것
    {
        OnButton.SetActive(true);
        BM.otherCanvasOn = false;

        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        gameObject.GetComponent<RectTransform>().DOMove(new Vector2(43f, 0f), 1f).SetEase(Ease.OutExpo);
        //gameObject.transform.DOMove(new Vector3(1950f, 0f, 0f), 1f).SetEase(Ease.OutExpo);
    }

    public void setPassive(int i,int count)
    {
        if (i > 0 && i < 100)
            passive[i] += count;
    }

    public void writePassiveInLog()
    {
        for(int i = 0; i < passive.Length; i++)
        {
            if (passive[i] > 0)
            {
                logContent.text += SetPassiveName(i, passive[i]);
                passive[i] = 0;
            }
        }
    }

    string SetPassiveName(int i,int c) //패시브 번호에 따라 로그 출력을 위한 함수
    {
        string s = "";
        
        int k = i % 4 - 1;
        int m = i / 4 + 1;
        if (k == -1) { k = 3; m--; }
        s ="\n"+ CharacterInfo.Instance.cd[m].name+"이 "+ CharacterInfo.Instance.cd[m].passive[k]+" 발동("+c+")";
        return s;
    }
}
