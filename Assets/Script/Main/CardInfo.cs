using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardInfo : MonoBehaviour
{
    private static CardInfo instance;

    public static CardInfo Instance
    {
        get
        {
            if (null == instance)
            {

                instance = new CardInfo();
            }
            return instance;
        }
    }
    public Sprite[] CardSpr = {
         Resources.Load<Sprite>("CardSprite/Card-13"), //0
         Resources.Load<Sprite>("CardSprite/Card-13"), //1
         Resources.Load<Sprite>("CardSprite/Card-13"), //2
         Resources.Load<Sprite>("CardSprite/Card-13"), //3
         Resources.Load<Sprite>("CardSprite/Card-13"), //4
         Resources.Load<Sprite>("CardSprite/Card-13"), //5
         Resources.Load<Sprite>("CardSprite/Card-5"), //6
         Resources.Load<Sprite>("CardSprite/Card-6"), //7
         Resources.Load<Sprite>("CardSprite/Card-7"), //8
         Resources.Load<Sprite>("CardSprite/Card-13"), //9
         Resources.Load<Sprite>("CardSprite/Card-13"), //10
         Resources.Load<Sprite>("CardSprite/Card-13"), //11
         Resources.Load<Sprite>("CardSprite/Card-13"), //12
         Resources.Load<Sprite>("CardSprite/Card-13"), //13
         Resources.Load<Sprite>("CardSprite/Card-13"), //14
         Resources.Load<Sprite>("CardSprite/Card-13"), //15
         Resources.Load<Sprite>("CardSprite/Card-13"), //16
         Resources.Load<Sprite>("CardSprite/Card-13"), //17
         Resources.Load<Sprite>("CardSprite/Card-13"), //18
         Resources.Load<Sprite>("CardSprite/Card-13"), //19
         Resources.Load<Sprite>("CardSprite/Card-20"), //20
         Resources.Load<Sprite>("CardSprite/Card-13"), //21
         Resources.Load<Sprite>("CardSprite/Card-13"), //22
         Resources.Load<Sprite>("CardSprite/Card-13"), //23
         Resources.Load<Sprite>("CardSprite/Card-24"), //24
         Resources.Load<Sprite>("CardSprite/Card-25"), //25
         Resources.Load<Sprite>("CardSprite/Card-26"), //26
         Resources.Load<Sprite>("CardSprite/Card-13"), //27
         Resources.Load<Sprite>("CardSprite/Card-13"), //28
         Resources.Load<Sprite>("CardSprite/Card-13"), //29
         Resources.Load<Sprite>("CardSprite/Card-13"), //30
         Resources.Load<Sprite>("CardSprite/Card-13"), //31

         };
    public struct cardData
    {
        public string Name;
        public string Content;
        public int No;
        public int Cost;
        public int Deck; // 이 카드가 어떤 캐릭터 소유의 덱인지 나타내는 변수 0:기본, 1:Q, 2:스파키, 3:반가라, 4:포르테, 5:령
        public int type; // 기본&스타터,에디셔녈,토큰
        public int select; // 0->선택X 1->적 선택2->묘지on,묘지 선택 3 ->덱 on,덱 선택 4->스케치반복 5->아군 선택

        public cardData(string Name, string Content, int No, int Cost, int Deck,int type,int select)
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

       new cardData("기본공격","-적 한명에게 데미지: <color=red><b>7</color></b>",1,1,0,0,1),

       new cardData("통상방어","-아군 한명에게 방어도:10",2,1,0,0,5),

       new cardData("연계공격","-적 한명에게 데미지:7\n-드로우:1",3,2,0,0,1),

       new cardData("준비태세","-드로우:2\n-자신에게 SP-0.1",4,2,0,0,0),

       new cardData("심연속 요동","-망자부활:8\n-덱에 이 카드 한장을 복사해 넣는다.",5,1,1,0,0),

       new cardData("그림자의 그림자","-망자부활:6\n-드로우:1\n-다음 턴 시작시 부여 코스트+2",6,2,1,0,0),

       new cardData("엮어만든 시체","-망자부활:6\n-묘지에서 패로 카드 2장을 선택해 가지고 온다.",7,1,1,0,2),

       new cardData("방어전술","-현재 남은 코스트 1당 자신에게\n 방어도:12",8,0,3,0,0),

       new cardData("인고의 노력","-다음 턴 시작시 자신에게 방어도:20",9,1,3,0,0),

       new cardData("우렁찬 표효","-턴 종료시까지 모든 아군의 공격력:+1\n-자신의 SP-0.1",10,0,2,0,0),

       new cardData("스트레이트 펀치","-카드를 4장 이상 사용했을 때 패에 있는 이 카드의 소비 코스트를 3감소한다\n-적 한명에게 데미지:15",11,3,2,0,1),
       new cardData("칠흑/핏빛","-패에 있는 카드를 덱에 넣는다\n-드로우:덱에 넣은 카드 장 수",12,1,4,0,0),

       new cardData("검은 아리아","-묘지에 패로 카드 1장을 선택해 가지고 온다.\n-소비 코스트+2",13,0,4,0,2),

       new cardData("절대적인 주인: 복종","-적 한명에게 데미지:제거한 망자의 수\n-망자부활:30",14,2,1,2,1),

       new cardData("거대한 어둠:지배","-적 한명에게 데미지:묘지의 카드 수\n-망자부활:묘지의 카드 수",15,1,1,2,1),

       new cardData("맞받아치기","-이번 전투 동안 적에게 공격을 받을 시 해당 적에게 데미지:1(중첩 가능)",16,2,3,1,0),

       new cardData("집중과 휴식","-아군 한명에게 방어도:20\n-다음 턴 시작 시 반가라의 방어도:-30",17,1,3,1,5),

       new cardData("피스트범프","-턴 종료시까지 스파키의 SP-1.0\n-턴 종료시까지 스파키의 공격력:+1",18,1,2,1,0),

       new cardData("콤비네이션!","-적 한명에게 데미지:4(7번)",19,3,2,1,1),

       new cardData("자비로운 앙코르","-이전 카드의 발동을 반복한다.(중첩 불가능)",20,2,4,1,4),

       new cardData("찢겨진 커튼콜","-묘지에서 패로 카드 3장을 무작위로 가지고 온다.\n가지고 온 카드의 소비 코스트를 0으로 고정한다.",21,1,4,1,0),

       new cardData("결의","-아군 전체에게 방어도:4\n-방어도가 제일 낮은 아군 한명에게 방어도:10",22,2,3,1,0),

       new cardData("매치 포인트","-스파키를 제외한 파티 전체의 행동력이 0이 된다.\n-턴 종료시까지 스파키의 공격력:+3",23,2,2,1,0),

       new cardData("끔찍한 화음, 비명","-패에있는 카드를 묘지에 버린다.\n-덱에 있는 카드를 1장 선택해 가지고 온다.",24,2,4,1,3),

       new cardData("들어낸 하얀 뼈","-망자부활:현재 남은 코스트.\n-묘지와 패에 있는 카드에 [드로우:1]를 부여한다..",25,1,1,1,0),

       new cardData("화염 속 절규","-망자부활:10.\n-적 전체에게 데미지:6",26,2,1,1,0),

       new cardData("음을 읊조리고 금을 연주한다.","-적 한명에게 약화부여:10.\n-약화가 부여된 적에게 데미지:15.",27,3,5,0,1),

       new cardData("불타는 금은보패","-소지한 이그넘이 50감소한다.\n-령이 사용할 시,적 전체에게 약화부여:2.\n-적 전체에게 데미지:2",28,2,5,0,0),

       new cardData("쌓이는 금붙이,쌓이는 업보","-소지한 이그넘이 50감소한다.\n-적 전체에게 쌓인 해로운 효과를 두배로 한다.",29,2,5,1,0),

       new cardData("무(舞)","-적 전체에게 쌓인 해로운 효과를 제거한다.\n-령이 사용할 시,적 전체에게 데미지:5(제거한 해로운 효과 스택 횟수)",30,5,5,1,0),

       new cardData("내려오는 귀신님","패에 있는 카드에 [적 전체에게 약화를 1부여한다]를 부여한다.\n-령이 사용할 시,적 전체에게 데미지:2",31,2,5,1,0),
    };
}
