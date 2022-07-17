using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SetCardInGM : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI count;
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] TextMeshProUGUI noT;
    CardInfo CaInfo = new CardInfo();
    GMmode gm;
    int no;
    int size;
    public void set(int no, GMmode gm,int size)
    {
        this.gm = gm;
        noT.text = no+"";
        title.text = CaInfo.cd[no].Name;
        content.text = CaInfo.cd[no].Content;
        this.no = no;
        this.size = size;
        count.text = this.size + "";
    }
    public void plusCard()
    {
        size++;
        gm.CardChange(no, size);
        count.text = size + "";
    }
    public void minusCard()
    {
        size--;
        if (size < 0) size = 0;
        gm.CardChange(no, size);
        count.text = size + "";
    }
}
