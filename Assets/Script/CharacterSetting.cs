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
    CharacterInfo cd = new CharacterInfo();
    [SerializeField] Image myImage;
    bool isSet;

    public void SetCharacter(int i,Sprite s) //캐릭터 프리펩에 텍스트 적용
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
    //0->이름 1->내용 2->위치 3~5->패시브
    public void SetCharacterInLobby(int no, Sprite s,int atk,int endur,int cost,int curHp,int maxHp,int formation,int[] passive,string s1,string s2,Sprite equipSprite)
    {
        //로비에서 파티 정보 버튼을 눌렀을 때, 현재 캐릭터의 상태를 띄움.
        myImage.sprite = s;
        this.no = no;
        CharacterInfo[0].text = cd.cd[no].Name;
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
            CharacterInfo[3].text += "<b>"+cd.cd[no].passive[0] + "</b>\n";
            CharacterInfo[3].text +=   cd.cd[no].passiveContent[0]+"\n";
            counter++;
        }
        counter = 0;
        while (counter < passive[1])
        {
            CharacterInfo[3].text += "<b>" + cd.cd[no].passive[1] + "</b>\n";
            CharacterInfo[3].text += cd.cd[no].passiveContent[1] + "\n";
            counter++;
        }
        counter = 0;
        while (counter < passive[2])
        {
            CharacterInfo[3].text += "<b>" + cd.cd[no].passive[2] + "</b>\n";
            CharacterInfo[3].text +=  cd.cd[no].passiveContent[2] + "\n";
            counter++;
        }
        counter = 0;
        while (counter < passive[3])
        {
            CharacterInfo[3].text += "<b>" + cd.cd[no].passive[3] + "</b>\n";
            CharacterInfo[3].text += cd.cd[no].passiveContent[3] + "\n";
            counter++;
        }
        counter = 0;


        transform.GetChild(1).GetComponent<Image>().sprite = equipSprite;
        
        transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = s1;
        transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = s2;
    }
   public override void OnPointerClick(PointerEventData data)
    {   //파티 만들기 형태에서 캐릭터를 누르면 적용됨
        //이미 파티에 있는 캐릭터면 목록에서 없애고, 파티에 없는 캐릭터면 목록에 추가함
        if (isSet)
        GameObject.Find("CharacterManager").GetComponent<CharacterManager>().setCharacter(no);
   }
    public void SetPassiveContent(int i)
    {
       
            CharacterInfo[6].text = cd.cd[no].passive[i-1];
            CharacterInfo[7].text = cd.cd[no].passiveContent[i-1];      
      
    }
}
