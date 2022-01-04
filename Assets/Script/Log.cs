using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Log : MonoBehaviour
{
    public TextMeshProUGUI logContent;
    [SerializeField] BattleManager BM;
    [SerializeField] GameObject OnButton;
    public void onLog()
    {
        OnButton.SetActive(false);
        BM.otherCanvasOn = true;
        gameObject.SetActive(true);
    }
    public void offLog()
    {
        OnButton.SetActive(true);
        BM.otherCanvasOn = false;
        
        gameObject.SetActive(false);
    }
}
