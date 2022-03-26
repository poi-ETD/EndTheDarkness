using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData2
{
    public struct characterData
    {
        public string Name;
        public int No;
        public int Cost;
        public int Atk;
        public int maxHp;
        public string[] passive;
        public string[] passiveContent;

        public characterData(string name, int no, int cost, int atk, int maxHp, string[] passive, string[] passiveContent)
        {
            Name = name;
            No = no;
            Cost = cost;
            Atk = atk;
            this.maxHp = maxHp;
            this.passive = passive;
            this.passiveContent = passiveContent;
        }
    }
    public characterData[] cd = new characterData[5]
    {
        new characterData("이름",0,0,0,0,new string[]{"패시브1","패시브2","패시브3","패시브4"},new string[]{"패시브1설명","패시브2설명","패시브3설명","패시브4설명" }),
         new characterData("Q",1,2,1,30,new string[]{"백옥의 왕","군단","흑백","절망" },new string[]{"턴 종료시 망자가 50명일 때, [백옥의왕Q]로 변신한다." ,
             "매 턴 망자가 4명이상 부활할 때마다 Q는 현재 행동력+1을 얻는다. ","해당 전투동안 카드를 통해 드로우된 카드들에게 [모든 적에게 데미지:1]을 부여","Q가 행동력이 감소될 때, 무덤에서 임의로 카드를 패로 가져온다. 해당 전투동안 가져온 카드에게 [모든 적에게 데미지:1]을 부여" }),
          new characterData("스파키",2,1,1,50,new string[]{"지치지 않는 폭주","독단적인 팀플레이", "부서진 족쇄","몰아치기" },new string[]{"매턴 카드를 3장 사용할 때마다 리플리의 현재 행동력 +1","적에게 데미지가 줄 때마다 해당 적에게 데미지: 자신의공격력",
              "스파키의 현재 행동력이 0이 될 때 턴 종료시까지 패에 있는 무작위 카드 1장의 코스트를 0으로 만든다.","해당 전투 동안 스파키가 적에게 데미지를 20번이상 데미지를 줄 때 공격력+2" }),
           new characterData("반가라",3,1,0,60,new string[]{"굳건한 위치","선봉의 호령","무장","독불장군" },new string[]{"자신을 제외한 아군,적에게 데미지가 들어갈 때 마다 자신에게 방어도:1","매 턴 시작시 방어도가 있을 경우 코스트+1  , 방어도가 없을 경우 자신에게 방어도:3","적이 데미지를 주었을 때, 방어도가 존재하면 깎인 방어도/2만큼 해당 적에게 데미지를 준다.","턴 시쟉시  한 진영에 반가라 혼자 있을 경우, 자신에게 방어도:7 행동력+1" }),
            new characterData("포르테",4,1,0,10,new string[]{"창조의 잠재력","미라클 드로우","스타키티시모","평균율" },new string[]{"매 턴 종료 시 패가 3장이상 남아있을 경우 다음 턴 시작할 때 드로우:2","매 턴마다 한번 카드를 통해 드로우 했을 때 포르테의 현재 행동력 +1 ,코스트 +1","매 턴 시작 시 패 한 장을 선택해서 덱으로 되돌리고 드로우한다. 해당 카드의 코스트는 2 감소한다.","매 턴 시작시 묘지와 덱 중 카드가 많은 쪽에서 적은 쪽으로 카드 1장을 선택해 이동시킨다." }),
    };



}
