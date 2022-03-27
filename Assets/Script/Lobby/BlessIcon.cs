using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BlessIcon : MonoBehaviour
{
    [SerializeField] int i;
    blessData d = new blessData();
    [SerializeField] TextMeshProUGUI[] t;
    void Start()
    {
        t[0].text = d.bd[i].Name;
        t[1].text = d.bd[i].content;    }

  
    
}
