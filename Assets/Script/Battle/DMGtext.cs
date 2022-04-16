using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DMGtext : MonoBehaviour
{
    int type; //0>데미지 ->빨강 1>행동력 파랑 2>방어도 노랑 3>체력 회복>초록 etc)
    TextMeshProUGUI t;

    public void GetType(int i,int value)
    {
        t = GetComponent<TextMeshProUGUI>();
        t.text = value + "";
        type = i;
        if (i == 0)
        {
            t.color = Color.red;
        }
        if (i == 1)
        {
            t.color = Color.blue;
        }
        if (i == 2)
        {
            t.color = Color.yellow;
        }
        if (i == 3)
        {
            t.color = Color.green;
        }
        StartCoroutine("TextChange");
    }

    IEnumerator TextChange()
    {
        int c = 0;
        while (c < 120)
        {
           
            t.color = new Color(t.color.r,t.color.g,t.color.b,t.color.a-0.01f);
            transform.position += new Vector3(0, 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
    // Update is called once per frame
    
}
