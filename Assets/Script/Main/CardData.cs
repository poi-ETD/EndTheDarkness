using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData2
{
   public struct cardData
    {
        public string Name;
        public string Content;
        public int Cost;
        public int Deck;
        public int type;
        public int No;
     
        public cardData(string Name, string Content, int Cost, int Deck,int type,int No)
        {
            this.Name = Name;
            this.Content = Content;
            this.Cost = Cost;
            this.Deck = Deck;
            this.type = type;
            this.No = No;
        }
    }
    public cardData[] cd = new cardData[25]
    {  new cardData("이름자리입니다","내용입니다",0,0,0,0),
       new cardData("기본공격","-적 한명에게 데미지:5",1,0,0,1),
       new cardData("통상방어","-자신에게 방어도:7",1,0,0,2),
       new cardData("백스텝","-적 한명에게 데미지:7\n-드로우:1",2,0,0,3),
       new cardData("준비태세","-드로우:2",2,0,0,4),
       new cardData("심연속 요동","-망자부활:6\n-덱에 이 카드 한장을 복사해 넣는다.",1,1,0,5),
       new cardData("푸른 화염체","-망자부활:3\n-적 한명에게 2번의 데미지:7\n-다음 턴 시작시 코스트+1",3,1,0,6),
       new cardData("누더기 제조","-망자부활:4\n-묘지에서 패로 카드 2장을 선택해 가지고 온다.",2,1,0,7),
       new cardData("방어전술","-자신에게 방어도:5\n-이번 턴 종료시 남은 코스트 1당 자신에게 방어도:10",1,3,0,8),
       new cardData("인고의 노력","-다음 턴 시작시 자신에게 방어도:16",1,3,0,9),
       new cardData("우렁찬 표효","-턴 종료시까지 모든 아군의 공격력:+1",0,2,0,10),
       new cardData("스트레이트 펀치","-카드를 4장 이상 사용한 턴에는 해당 카드의 코스트가 2감소한다\n-턴 종료시까지 자신의 공격력:+1\n-적 한명에게 데미지:5",3,2,0,11),
       new cardData("리셋","-패에 있는 카드를 덱에 넣는다\n-드로우:덱에 넣은 카드 장 수",1,4,0,12),
       new cardData("완벽한 발굴","-드로우:1\n-묘지에 패로 카드 1장을 선택해 가지고 온다.",1,4,0,13),
       new cardData("시체 폭탄","-적 한명에게 데미지:제거한 망자의 수\n-망자부활:30",2,1,2,14),
       new cardData("묘지","-적 한명에게 데미지:묘지의 카드 수\n-망자부활:묘지의 카드 수",1,1,2,15),
       new cardData("맞받아치기","-자신에게 방어도:3\n-적에게 공격을 받을 시 해당 적에게 데미지:1(중첩 가능)",2,3,1,16),
       new cardData("집중과 휴식","-자신에게 방어도:15\n-다음 턴 시작 시 자신에게 방어도:-15",1,3,1,17),
       new cardData("피스트범프","-행동력이 0이여도 사용할 수 있다.\n-적 한명에게 데미지:2",0,2,1,18),
       new cardData("슬리핑","-드로우:1\n-적 한명에게 2번의 데미지:6\n-스파키의 현재 행동력:+1",3,2,1,19),
       new cardData("스케치 반복","-이전 카드의 발동을 반복한다.(중복 불가능)",2,4,1,20),
          new cardData("구겨진 습작","-묘지에서 무작위로 카드를 3장 패로 가져옵니다.",1,4,1,21),
       new cardData("결의","-아군 전체에게 방어도:6\n다음 턴 시작 시 아군 전체의 방어도를 제거한다\n-자신에게 방어도:이 카드의 효과로 제거한 방어도",2,3,1,22),
    
       new cardData("매치 포인트","-파티 전체의 행동력이 0이 됩니다.\n다음 턴 시작 시 파티 전체의 공격력이 해당 턴 종료시 까지 2상승\n-적 전체에게 3번의 데미지:1",2,2,1,23),
        new cardData("사이코키네시스","-패에있는 카드를 모두 묘지에 버린다.\n덱에 있는 카드를 1장 선택해 가지고 온다.",2,4,1,24)
    };
    


    
}
