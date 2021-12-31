using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlackWhite : MonoBehaviour
{
    public int BlackWhiteStack=1;
    Card myCard;
    GameObject[] enemys;
    public Enemy[] enemyScript;
    public Text newText;
    private void Awake()
    {
       
    }
    private void Update()
    {
   
    }
    public void birth()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemyScript = new Enemy[enemys.Length];
        for(int i = 0; i < enemys.Length; i++)
        {
            enemyScript[i] = enemys[i].GetComponent<Enemy>();
        }
        myCard = GetComponent<Card>();
        myCard.Content.text += "\n흑백(" + 1 + ")";
    }
    
    public void PlusStack()
    {           
        myCard.Content.text.Replace("흑백(" + BlackWhiteStack + ")", "흑백(" + BlackWhiteStack+1 + ")");
        BlackWhiteStack++;
    }
    public void onDamage()
    {
        Debug.Log("온댐");
        for (int i = 0; i < enemys.Length; i++)
        {            
            enemyScript[i].onHit(BlackWhiteStack);
        }
    }
}
