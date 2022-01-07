using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlackWhite : MonoBehaviour
{
    public int BlackWhiteStack=1;
    [SerializeField]Card myCard;
    GameObject[] enemys;
    public Enemy[] enemyScript;
    public Text newText;
    BattleManager BM;
    private void Awake()
    {
       
    }
    private void Update()
    {
   
    }
    public void birth()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemyScript = new Enemy[enemys.Length];
        for(int i = 0; i < enemys.Length; i++)
        {
            enemyScript[i] = enemys[i].GetComponent<Enemy>();
        }
        myCard = GetComponent<Card>();
        myCard.Content.text += "\n흑백(" + 1 + ")";
    }
    public string content;
    public void PlusStack()
    {
        BlackWhiteStack++;
        myCard.Content.text += "\n흑백(" + BlackWhiteStack + ")";
       
    }   
    public void onDamage()
    {
        BM.log.logContent.text += "\n흑백의 효과로 모든 적에게 " + BlackWhiteStack + "의 데미지!"; 
        for (int i = 0; i < enemyScript.Length; i++)
        {            
            enemyScript[i].onHit(BlackWhiteStack);
        }
    }
}
