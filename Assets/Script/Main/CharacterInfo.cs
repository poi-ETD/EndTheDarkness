using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo
{
    private static CharacterInfo instance;

    public static CharacterInfo Instance
    {
        get
        {
            if (null == instance)
            {

                instance = new CharacterInfo();
            }
            return instance;
        }
    }
    public struct characterData
    {
        public string name;
        public int code;
        public int cost;
        public int atk;
        public int def;
        public float speed;
        public int maxHp;
      
     
        public string[] passive;
        public string[] passiveContent;
        public string specialEquipName;
        public string specialEquipContent;

        public Sprite characterSprtie;
        public Sprite characterFace;
        public characterData(string name, int no, int cost, int atk, int def, float speed, int maxHp, string[] passive, string[] passiveContent, string specialEquipName, string specialEquipContent,Sprite characterSprtie,Sprite characterFace)
        {
            this.name = name;
            this.code = no;
            this.cost = cost;
            this.atk = atk;
            this.def = def;
            this.speed = speed;
            this.maxHp = maxHp;
            this.passive = passive;
            this.passiveContent = passiveContent;
            this.specialEquipName = specialEquipName;
            this.specialEquipContent = specialEquipContent;
            this.characterSprtie = characterSprtie;
            this.characterFace = characterFace;
        }
    }
    public characterData[] cd = new characterData[7]
    {
        new characterData("이름",0,0,0,0,0,0,new string[]{"패시브1","패시브2","패시브3","패시브4"},new string[]{"패시브1설명","패시브2설명","패시브3설명","패시브4설명" },"전용 장비 이름","전용 장비 설명",null,null),
        new characterData("Q",1,3,1,0,8.9f,35,new string[]{"백옥의 왕","군단","흑백","절망" },new string[]{"행동 종료시 망자가 50명일 때, [백옥의왕Q]로 변신한다." ,
             "해당 전투 동안 망자가 4명이상 부활할 때마다 후방 진형의 아군 1명은 공격력 +1 또는 SP-0.1","해당 전투동안 드로우된 카드들에게 [모든 적에게 데미지:1]을 부여","행동 시작 시 무덤에서 임의로 카드를 패로 가져온다. 해당 전투 동안 가져온 카드에 [모든 적에게 데미지:1]을 부여"}
        ,"깨진 유리 심장","전체:\n턴 시작 시 망자가 200명일 때,해당 전투에서 승리한다.\n턴 종료시 아군 전체의 HP가 5 회복한다.",Resources.Load<Sprite>("CharacterSprite/Q"),Resources.Load<Sprite>("CharacterSprite/Q_face")),
          new characterData("스파키",2,2,1,0,5.5f,45,new string[]{"지치지 않는 폭주","독단적인 팀플레이", "부서진 족쇄","몰아치기" },new string[]{"해당 턴 동안 카드를 3장 사용할 때마다 스파키에게 SP-1.0(최대 2.0)","해당 전투 동안 아군이 공격이 성공했을때마다 해당 적에게 데미지:자신의 공격력",
              "스파키의 행동 종료 시,패에 있는 임의로 카드 1장의 소비 코스트를 턴 종료까지 0으로 만든다.","해당 전투 동안 스파키가 적에게 데미지를 10번이상 성공했을 때마다 공격력+1" },
          "진실이 적혀있는 쪽지","전체: 공격력+10, SPD-0.5\n행동 종료 시 적 한명에게 데미지:1(3번)",Resources.Load<Sprite>("CharacterSprite/Sparky"),Resources.Load<Sprite>("CharacterSprite/Sparky_face")),
           new characterData("반가라",3,1,0,1,9.9f,60,new string[]{"굳건한 위치","선봉의 호령","무장","독불장군" },new string[]{"자신을 제외한 아군,적의 체력이 감소할 때마다 자신에게 방어도:1","턴 시작시 방어도가 있을 경우 부여 코스트+1,방어도가 없을 경우 자신에게 방어도:3","적의 공격이 성공할 때마다 방어도가 존재하면 깎인 방어도의 절반만큼 해당 적에게 데미지를 준다.","턴 시쟉시 한 진영에 반가라 혼자 있을 경우, 반가라에게 방어도:7 그리고 SP-0.2(최대 5.0)"}
          ,"피묻은 곡옥","전방:\n턴 시작 시 반가라를 제외한 아군 전체에게 방어도: 반가라의 방어도",Resources.Load<Sprite>("CharacterSprite/Vangara"),Resources.Load<Sprite>("CharacterSprite/Vangara_face")),
            new characterData("포르테",4,3,0,0,4.9f,20,new string[]{"창조의 잠재력","미라클 드로우","스타키티시모","평균율" },new string[]{"행동 종료 시 패가 5장이상 남아있을 경우 다음 턴 시작할 때 드로우:2","해당 턴 동안 카드를 통해 드로우 했을 때 포르테의 부여 코스트 +2","턴 시작 시 패 한 장을 선택해서 덱으로 되돌리고 드로우:1,드로우 된 카드의 소비 코스트는 2 감소한다.","턴 시작시 묘지와 덱 중 카드가 많은 쪽에서 적은 쪽으로 카드 1장을 선택하고 이동시킨다." }
            ,"퍼퓸 디 블러드 하프","전체: HP-5 , 공격력 +1 , 코스트 +1\n포르테의 행동 시작시 패에 있는 포르테 카드에 [적 전체에게 데미지: 적 현재 HP의 5%]를 부여\n포르테가 포르테 카드를 사용할 때마다 해당 장비의 공격력 +1",Resources.Load<Sprite>("CharacterSprite/Forte"),Resources.Load<Sprite>("CharacterSprite/Forte_face")),
     new characterData("령",5,2,0,0,6.0f,35,new string[]{ "조우하는 혼령들", "부유 천하", "혼령의 정기", "바람 같은 존재"},new string[]{ "령의 행동 시작 시 묘지에 있는 카드 1장을 무작위로 패에 가지고 온다. 해당 카드는 행동 종료 시 혹은 사용했을 때 소멸된다.", "3번의 전투 종료시 1000이그넘을 획득한다.", "해당 턴 동안 약화가 부여 혹은 카드가 소멸될 경우 아군의 SP-0.1", "령이 사용하는 카드의 소비 코스트는 1로 고정된다. 사용한 카드는 소멸된다." }
     ,"알 수없는 자의 머리","전체:\n모든 카드의 발동을 반복한다.",Resources.Load<Sprite>("CharacterSprite/Ryung"),Resources.Load<Sprite>("CharacterSprite/Ryung_face")),
      new characterData("흉귀",6,2,2,0,11.5f,25,new string[]{ "폭호", "산중호걸", "타오르는 가죽", "직전의 대재앙"},new string[]{ "공격력이 2배가 되고 카드 사용시, 적이 무작위로 선택된다.", "해당 전투 동안 턴 시작 시 임의의 아군 공격력을 1 감소하고 흉귀의 공격력을 2증가한다.", "흉귀의 방어력이 10이상일때, 공격력이 1 증가하고 방어력을 0으로 한다.", "전투 시작 시 공물의 10%를 감소하고 감소한 공물의 10%만큼 공격력이 증가합니다." }
     ,"알 수없는 자의 머리","전체:\n모든 카드의 발동을 반복한다.",Resources.Load<Sprite>("CharacterSprite/Hyunggwi"),Resources.Load<Sprite>("CharacterSprite/Hyunggwi_face")),
    };



}
