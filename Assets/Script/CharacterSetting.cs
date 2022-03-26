using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CharacterSetting :EventTrigger
{
    int no;
    [SerializeField] TextMeshProUGUI[] CharacterInfo;//0->이름 1->설명 2,3,4,5->패시브 6->패시브상세이름 7->패시브 상세 내용
    CharacterData2 cd = new CharacterData2();
    [SerializeField] Image myImage;
    bool isSet;

    public void SetCharacter(int i,Sprite s)
    {
        isSet = true;
        myImage.sprite = s;
        no = i;
        CharacterInfo[0].text = cd.cd[i].Name;
        CharacterInfo[1].text = "공격력:" + cd.cd[i].Atk + "\n코스트:" + cd.cd[i].Cost + "\nHP:" + cd.cd[i].maxHp;
        
        CharacterInfo[2].text = cd.cd[i].passive[0];
        CharacterInfo[3].text = cd.cd[i].passive[1];
        CharacterInfo[4].text = cd.cd[i].passive[2];
        CharacterInfo[5].text = cd.cd[i].passive[3];
      
    }
    //0->이름 1->내용 2->위치 3->패시브
    public void SetCharacterInLobby(int i, Sprite s,int atk,int cost,int curHp,int maxHp,int formation,int[] passive)
    {
        myImage.sprite = s;
        no = i;
        CharacterInfo[0].text = cd.cd[i].Name;
        CharacterInfo[1].text = "공격력:" + atk + "\n코스트:" + cost + "\nHP:"+curHp+"/" + maxHp;
        if (formation == 0)
            CharacterInfo[2].text = "전방";
        else
        {
            CharacterInfo[2].text = "후방";
        }
        int counter = 0;
        CharacterInfo[3].text = "";
        while (counter < passive[0])
        {
            CharacterInfo[3].text += "<b>"+cd.cd[i].passive[0] + "</b>\n";
            CharacterInfo[3].text +=   cd.cd[i].passiveContent[0]+"\n";
            counter++;
        }
        counter = 0;
        while (counter < passive[1])
        {
            CharacterInfo[3].text += "<b>" + cd.cd[i].passive[1] + "</b>\n";
            CharacterInfo[3].text += cd.cd[i].passiveContent[1] + "\n";
            counter++;
        }
        counter = 0;
        while (counter < passive[2])
        {
            CharacterInfo[3].text += "<b>" + cd.cd[i].passive[2] + "</b>\n";
            CharacterInfo[3].text +=  cd.cd[i].passiveContent[2] + "\n";
            counter++;
        }
        counter = 0;
        while (counter < passive[3])
        {
            CharacterInfo[3].text += "<b>" + cd.cd[i].passive[3] + "</b>\n";
            CharacterInfo[3].text += cd.cd[i].passiveContent[3] + "\n";
            counter++;
        }
        counter = 0;

    }
   public override void OnPointerClick(PointerEventData data)
    {if(isSet)
        GameObject.Find("CharacterManager").GetComponent<CharacterManager>().setCharacter(no);
   }
    public void SetPassiveContent(int i)
    {
       
            CharacterInfo[6].text = cd.cd[no].passive[i-1];
            CharacterInfo[7].text = cd.cd[no].passiveContent[i-1];
      
      
      
    }
}
