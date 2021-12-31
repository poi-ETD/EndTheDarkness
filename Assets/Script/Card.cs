using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    CardManager CM;
    public bool isUsed;
    public bool isDestroy;
    public Text Content;
    private void Awake()
    {
        CM = GameObject.Find("CardManager").GetComponent<CardManager>();
    }
    private void Update()
    {
        if (isUsed&&!isDestroy)
        {           
            CM.UseCard(gameObject);
            if (GetComponent<BlackWhite>()!= null)
            {
                GetComponent<BlackWhite>().onDamage();
            }
            isDestroy = true;
        }
    }
    
}
