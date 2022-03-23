using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CharacterSetting : EventTrigger
{
    int no;
    [SerializeField] TextMeshProUGUI[] CharacterInfo;//0->이름 1->설명 2,3,4,5->패시브 6->패시브상세이름 7->패시브 상세 내용
    CharacterData2 cd = new CharacterData2();
    [SerializeField] Image myImage;
    [SerializeField] EventTrigger myEventTrigger;
    public void SetCharacter(int i,Sprite s)
    {
        myImage.sprite = s;
        no = i;
        CharacterInfo[0].text = cd.cd[i].Name;
        CharacterInfo[1].text = "공격력:" + cd.cd[i].Atk + "\n코스트:" + cd.cd[i].Cost + "\nHP:" + cd.cd[i].maxHp;
        CharacterInfo[2].text = cd.cd[i].passive1;
        CharacterInfo[3].text = cd.cd[i].passive2;
        CharacterInfo[4].text = cd.cd[i].passive3;
        CharacterInfo[5].text = cd.cd[i].passive4;
      
    }
    public override void OnPointerClick(PointerEventData data)
    {
        GameObject.Find("CharacterManager").GetComponent<CharacterManager>().setCharacter(no);
    }
    public void SetPassiveContent(int i)
    {
        if(i == 1){
            CharacterInfo[6].text = cd.cd[no].passive1;
            CharacterInfo[7].text = cd.cd[no].passiveContent1;
        }
        if (i == 2)
        {
            CharacterInfo[6].text = cd.cd[no].passive2;
            CharacterInfo[7].text = cd.cd[no].passiveContent2;
        }
        if (i == 3)
        {
            CharacterInfo[6].text = cd.cd[no].passive3;
            CharacterInfo[7].text = cd.cd[no].passiveContent3;
        }
        if (i == 4)
        {
            CharacterInfo[6].text = cd.cd[no].passive4;
            CharacterInfo[7].text = cd.cd[no].passiveContent4;
        }
    }
}
