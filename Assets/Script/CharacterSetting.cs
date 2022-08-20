using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CharacterSetting :MonoBehaviour
{
    int no;
    [SerializeField] TextMeshProUGUI[] Info;//0->이름 1->설명 2,3,4,5->패시브 6->패시브상세이름 7->패시브 상세 내용

    [SerializeField] Image myImage;
    bool isSet;

    public void SetCharacter(int i) //캐릭터 프리펩에 텍스트 적용
    {
        isSet = true;
        myImage.sprite = CharacterInfo.Instance.cd[i].characterSprtie;
        no = i;
        Info[0].text = CharacterInfo.Instance.cd[i].Name;
        Info[1].text = "공격력:" + CharacterInfo.Instance.cd[i].Atk + "\n코스트:" + CharacterInfo.Instance.cd[i].Cost + "\nHP:" + CharacterInfo.Instance.cd[i].maxHp;        
        Info[2].text = CharacterInfo.Instance.cd[i].passive[0];
        Info[3].text = CharacterInfo.Instance.cd[i].passive[1];
        Info[4].text = CharacterInfo.Instance.cd[i].passive[2];
        Info[5].text = CharacterInfo.Instance.cd[i].passive[3];
      
    }
    //0->이름 1->내용 2->위치 3~5->패시브
    public void SetCharacterInLobby(int no, Sprite s,int atk,int endur,int cost,int curHp,int maxHp,int formation,int[] passive,string s1,string s2,Sprite equipSprite)
    {
        Debug.Log(CharacterInfo.Instance.cd[no].Name);
        //로비에서 파티 정보 버튼을 눌렀을 때, 현재 캐릭터의 상태를 띄움.
        myImage.sprite = s;
        this.no = no;
        Info[0].text = CharacterInfo.Instance.cd[no].Name;
        Info[1].text = "공격력:" + atk + "\n코스트:" + cost + "\nHP:"+curHp+"/" + maxHp;
        if (formation == 0)
            Info[2].text = "전방";
        else
        {
            Info[2].text = "후방";
        }
        int counter = 0;
        Info[3].text = "";
        while (counter < passive[0])
        {
            Info[3].text += "<b>"+CharacterInfo.Instance.cd[no].passive[0] + "</b>\n";
            Info[3].text +=   CharacterInfo.Instance.cd[no].passiveContent[0]+"\n";
            counter++;
        }
        counter = 0;
        while (counter < passive[1])
        {
            Info[3].text += "<b>" + CharacterInfo.Instance.cd[no].passive[1] + "</b>\n";
            Info[3].text += CharacterInfo.Instance.cd[no].passiveContent[1] + "\n";
            counter++;
        }
        counter = 0;
        while (counter < passive[2])
        {
            Info[3].text += "<b>" + CharacterInfo.Instance.cd[no].passive[2] + "</b>\n";
            Info[3].text +=  CharacterInfo.Instance.cd[no].passiveContent[2] + "\n";
            counter++;
        }
        counter = 0;
        while (counter < passive[3])
        {
            Info[3].text += "<b>" + CharacterInfo.Instance.cd[no].passive[3] + "</b>\n";
            Info[3].text += CharacterInfo.Instance.cd[no].passiveContent[3] + "\n";
            counter++;
        }
        counter = 0;


        transform.GetChild(1).GetComponent<Image>().sprite = equipSprite;
        
        transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = s1;
        transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = s2;
    }
   /*public override void OnPointerClick(PointerEventData data)
    {   //파티 만들기 형태에서 캐릭터를 누르면 적용됨
        //이미 파티에 있는 캐릭터면 목록에서 없애고, 파티에 없는 캐릭터면 목록에 추가함
        if (isSet)
        GameObject.Find("CharacterManager").GetComponent<CharacterManager>().setCharacter(no);
   }*/
    public void SetPassiveContent(int i)
    {
       
            Info[6].text = CharacterInfo.Instance.cd[no].passive[i-1];
            Info[7].text = CharacterInfo.Instance.cd[no].passiveContent[i-1];      
      
    }
}
