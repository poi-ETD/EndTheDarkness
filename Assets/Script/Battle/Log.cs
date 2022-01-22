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
}
