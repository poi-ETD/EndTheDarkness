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
    Character c;

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
        myCard.Content.text += "\n-모든 적에게 1의 데미지 부여:(" + 1 + ")";
    }
    public string content;
    public void PlusStack()
    {
        BlackWhiteStack++;
        string newstring = myCard.Content.text;
        newstring = newstring.Replace("\n모든 적에게 1의 데미지 부여:(" + (BlackWhiteStack-1) + ")", "\n모든 적에게 1의 데미지 부여:(" + BlackWhiteStack + ")");
        myCard.Content.text = newstring;
       
    }   
    public void onDamage()
    {
        c = BM.actCharacter;
        Invoke("BWattack", 0.3f);
    }
    void BWattack()
    {
        BM.log.logContent.text += "\n흑백의 효과로 모든 적에게 " + "1 " + "의 데미지!(" + BlackWhiteStack + ")";
      //  BM.curMessage.text = "흑백의 효과로 모든 적에게 " + "1 " + "의 데미지!(" + BlackWhiteStack + ")";

    
        BM.AllAttack(1,c, BlackWhiteStack);

    }
}
