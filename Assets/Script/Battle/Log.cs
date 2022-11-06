using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        if (done)
        {
            done = false;
            loadLog();
        }
    }
    public void onLog()
    {    
        OnButton.SetActive(false);
        BM.otherCanvasOn = true;
        gameObject.SetActive(true);
        done = true;
      
    }
    void loadLog()
    {
        float a = logText.GetComponent<RectTransform>().rect.height - 1080;
        logTextPosition.GetComponent<RectTransform>().anchoredPosition =
      new Vector3(0, a
      , 0);

    }
    public void offLog()
    {
        OnButton.SetActive(true);
        BM.otherCanvasOn = false;       
        gameObject.SetActive(false);
    }
    public void setPassive(int i,int count)
    {if(i>0&&i<100)
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
