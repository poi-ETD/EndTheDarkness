using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SetCardInGM : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI count;
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] TextMeshProUGUI noT;
    Image myImage;
    GMmode gm;
    int no;
    int size;
	[SerializeField] private SO_CardList so_CardList;
	public void set(int no, GMmode gm,int size)
    {
        myImage = transform.GetChild(0).GetComponent<Image>();
		myImage.sprite = so_CardList.cardDetails[no].sprite_Card;

		this.gm = gm;
        noT.text = no+"";
        title.text = CardInfo.Instance.cd[no].Name;
        content.text = CardInfo.Instance.cd[no].Content;
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
