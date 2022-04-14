using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnemyInfo : MonoBehaviour
{
    public Enemy SelectedEnemy;
    [SerializeField] Slider HpSlider;
    [SerializeField] TextMeshProUGUI[] t; // 0->이름 1->hp 2->아머 
    public GameObject g;

    private void Update()
    {
        if (SelectedEnemy != null)
        {
            t[1].text = SelectedEnemy.Hp + "/" + SelectedEnemy.maxHp + "";
            t[2].text = SelectedEnemy.Armor + "";
            HpSlider.value = SelectedEnemy.Hp / (float)SelectedEnemy.maxHp;
        }
    }

    public void setNull()
    {
        SelectedEnemy.myImage.transform.localScale = new Vector2(1, 1);

        // YH
        if (SelectedEnemy.gameObject.GetComponent<Dagger001>() != null)
        {
            Dagger001 dagger001 = SelectedEnemy.gameObject.GetComponent<Dagger001>();
            dagger001.image_character.sprite = dagger001.sprite_idle;
        }

        SelectedEnemy.hpSlider.gameObject.SetActive(true);

        SelectedEnemy = null;

        g.SetActive(false);
    }

    public void setThis(Enemy e)
    {
        if (SelectedEnemy != null)
            setNull();

        SelectedEnemy = e;
        HpSlider.value = SelectedEnemy.Hp / (float)SelectedEnemy.maxHp;
        SelectedEnemy.myImage.transform.localScale = new Vector2(1.15f, 1.15f);

        // YH
        if (SelectedEnemy.gameObject.GetComponent<Dagger001>() != null)
        {
            Dagger001 dagger001 = SelectedEnemy.gameObject.GetComponent<Dagger001>();
            dagger001.image_character.sprite = dagger001.sprite_highlight;
        }

        g.SetActive(true);
        SelectedEnemy.hpSlider.gameObject.SetActive(false);
        t[0].text = e.Name;
        t[1].text = e.Hp + "/" + e.maxHp + "";
        t[2].text = e.Armor + "";
    }
}
