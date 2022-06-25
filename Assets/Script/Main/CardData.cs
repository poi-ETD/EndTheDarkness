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
        public int type;//기본&스타터,에디셔녈,토큰
        public int No;
        public int select; //0->선택X 1->적 선택2->묘지on,묘지 선택 3 ->덱 on,덱 선택 4->스케치반복 5->아군 선택
     
        public cardData(string Name, string Content, int Cost, int Deck,int type,int No,int select)
        {
            this.Name = Name;
            this.Content = Content;
            this.Cost = Cost;
            this.Deck = Deck;
            this.type = type;
            this.No = No;
            this.select = select;
        }
    }
    public cardData[] cd = new cardData[32]
    {  new cardData("이름자리입니다","내용입니다",0,0,0,0,0),

       new cardData("기본공격","-적 한명에게 데미지: <color=red><b>7</color></b>",1,0,0,1,1),

       new cardData("통상방어","-아군 한명에게 방어도:10",1,0,0,2,5),

       new cardData("연계공격","-적 한명에게 데미지:7\n-드로우:1",2,0,0,3,1),

       new cardData("준비태세","-드로우:2\n-자신에게 SP-0.1",2,0,0,4,0),

       new cardData("심연속 요동","-망자부활:8\n-덱에 이 카드 한장을 복사해 넣는다.",1,1,0,5,0),

       new cardData("그림자의 그림자","-망자부활:6\n-드로우:1\n-다음 턴 시작시 부여 코스트+2",2,1,0,6,0),

       new cardData("엮어만든 시체","-망자부활:6\n-묘지에서 패로 카드 2장을 선택해 가지고 온다.",1,1,0,7,2),

       new cardData("방어전술","-현재 남은 코스트 1당 자신에게\n 방어도:12",0,3,0,8,0),

       new cardData("인고의 노력","-다음 턴 시작시 자신에게 방어도:20",1,3,0,9,0),

       new cardData("우렁찬 표효","-턴 종료시까지 모든 아군의 공격력:+1\n-자신의 SP-0.1",0,2,0,10,0),

       new cardData("스트레이트 펀치","-카드를 4장 이상 사용했을 때 패에 있는 이 카드의 소비 코스트를 3감소한다\n-적 한명에게 데미지:15",3,2,0,11,1),

       new cardData("칠흑/핏빛","-패에 있는 카드를 덱에 넣는다\n-드로우:덱에 넣은 카드 장 수",1,4,0,12,0),

       new cardData("검은 아리아","-묘지에 패로 카드 1장을 선택해 가지고 온다.\n-소비 코스트+2",0,4,0,13,2),

       new cardData("절대적인 주인: 복종","-적 한명에게 데미지:제거한 망자의 수\n-망자부활:30",2,1,2,14,1),

       new cardData("거대한 어둠:지배","-적 한명에게 데미지:묘지의 카드 수\n-망자부활:묘지의 카드 수",1,1,2,15,1),

       new cardData("맞받아치기","-이번 전투 동안 적에게 공격을 받을 시 해당 적에게 데미지:1(중첩 가능)",2,3,1,16,0),

       new cardData("집중과 휴식","-아군 한명에게 방어도:20\n-다음 턴 시작 시 반가라의 방어도:-30",1,3,1,17,5),

       new cardData("피스트범프","-턴 종료시까지 스파키의 SP-1.0\n-턴 종료시까지 스파키의 공격력:+1",1,2,1,18,0),

       new cardData("콤비네이션!","-적 한명에게 데미지:4(7번)",3,2,1,19,1),

       new cardData("자비로운 앙코르","-이전 카드의 발동을 반복한다.(중첩 불가능)",2,4,1,20,4),

       new cardData("찢겨진 커튼콜","-묘지에서 패로 카드 3장을 무작위로 가지고 온다.\n가지고 온 카드의 소비 코스트를 0으로 고정한다.",1,4,1,21,0),

       new cardData("결의","-아군 전체에게 방어도:4\n-방어도가 제일 낮은 아군 한명에게 방어도:10",2,3,1,22,0),
    
       new cardData("매치 포인트","-스파키를 제외한 파티 전체의 행동력이 0이 된다.\n-턴 종료시까지 스파키의 공격력:+3",2,2,1,23,0),

       new cardData("끔찍한 화음, 비명","-패에있는 카드를 묘지에 버린다.\n-덱에 있는 카드를 1장 선택해 가지고 온다.",2,4,1,24,3),
       new cardData("들어낸 하얀 뼈","-망자부활:현재 남은 코스트.\n-묘지와 패에 있는 카드에 [드로우:1]를 부여한다..",1,1,1,25,0),
       new cardData("화염 속 절규","-망자부활:10.\n-적 전체에게 데미지:6",2,1,1,26,0),
       new cardData("음을 읊조리고 금을 연주한다.","-적 한명에게 약화부여:10.\n-약화가 부여된 적에게 데미지:15.",3,5,0,27,1),
       new cardData("불타는 금은보패","-소지한 이그넘이 50감소한다.\n-령이 사용할 시,적 전체에게 약화부여:2.\n-적 전체에게 데미지:2",2,5,0,28,0),
   new cardData("쌓이는 금붙이,쌓이는 업보","-소지한 이그넘이 50감소한다.\n-적 전체에게 쌓인 해로운 효과를 두배로 한다.",2,5,1,29,0),
   new cardData("무(舞)","-적 전체에게 쌓인 해로운 효과를 제거한다.\n-령이 사용할 시,적 전체에게 데미지:5(제거한 해로운 효과 스택 횟수)",5,5,1,30,0),
   new cardData("내려오는 귀신님","패에 있는 카드에 [적 전체에게 약화를 1부여한다]를 부여한다.\n-령이 사용할 시,적 전체에게 데미지:2",2,5,1,31,0),

    };
    


    
}
