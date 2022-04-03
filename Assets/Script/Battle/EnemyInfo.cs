using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnemyInfo : MonoBehaviour
{
    public Enemy SelectedEnemy;
    [SerializeField] Image Hp;
    [SerializeField] TextMeshProUGUI[] t; // 0->이름 1->hp 2->아머 
    public GameObject g;
    private void Update()
    {
        if (SelectedEnemy != null)
        {
           
            t[1].text = SelectedEnemy.Hp + "/" + SelectedEnemy.maxHp + "";
            t[2].text = SelectedEnemy.Armor + ""; 
            Hp.fillAmount = SelectedEnemy.Hp / (float)SelectedEnemy.maxHp; }
    }

    public void setNull()
    {
       
            SelectedEnemy.myImage.transform.localScale = new Vector2(1, 1);
            SelectedEnemy.HpImage[0].color = new Color(1, 1, 1, 1);
            SelectedEnemy.HpImage[1].color = new Color(1, 1, 1, 1);
        
            SelectedEnemy = null;
       
        g.SetActive(false);
    }
    public void setThis(Enemy e)
    {if(SelectedEnemy!=null)
        setNull();
  
        SelectedEnemy = e;
        Hp.fillAmount = SelectedEnemy.Hp / (float)SelectedEnemy.maxHp;
        SelectedEnemy.myImage.transform.localScale = new Vector2(1.3f, 1.3f);
        g.SetActive(true);
        SelectedEnemy.HpImage[0].color = new Color(1, 1, 1, 0);
        SelectedEnemy.HpImage[1].color = new Color(1, 1, 1, 0);
        t[0].text = e.Name;
        t[1].text = e.Hp + "/" + e.maxHp+"";
        t[2].text = e.Armor+"";
    }
}
