using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHeap : MonoBehaviour
{
    private static DataHeap instance;

    public static DataHeap Instance
    {
        get
        {
            return instance;
        }
    }

    [HideInInspector] public string[] passiveName;
    [HideInInspector] public string[] passiveDescription;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        passiveName = new string[25];
        passiveDescription = new string[25];
    }

    private void Start()
    {
        Init_PassiveName();
        Init_PassiveDescription();
    }

    private void Init_PassiveName()
    {
        passiveName[0] = "";
        passiveName[1] = "백옥의 왕";
        passiveName[2] = "군단";
        passiveName[3] = "흑백";
        passiveName[4] = "절망";
        passiveName[5] = "지치지 않는 폭주";
        passiveName[6] = "독단적인 팀플레이";
        passiveName[7] = "부서진 족쇄";
        passiveName[8] = "몰아치기";
        passiveName[9] = "굳건한 위치";
        passiveName[10] = "선봉의 호령";
        passiveName[11] = "무장";
        passiveName[12] = "독불장군";
        passiveName[13] = "창조의 잠재력";
        passiveName[14] = "미라클 드로우";
        passiveName[15] = "스타카티시모";
        passiveName[16] = "평균율";
        passiveName[17] = "조우하는 혼령들";
        passiveName[18] = "부유 천하";
        passiveName[19] = "혼령의 정기";
        passiveName[20] = "바람 같은 존재";
        passiveName[21] = "폭호";
        passiveName[22] = "산중호걸";
        passiveName[23] = "타오르는 가죽";
        passiveName[24] = "직전의 대재앙";
    }

    private void Init_PassiveDescription()
    {
        passiveDescription[0] = "";
        passiveDescription[1] = "행동 종료 시 망자가 50명일 때. [백옥의 왕 Q]로 변신한다.";
        passiveDescription[2] = "해당 전투 동안 망자가 4명 이상 부활할 때마다 후방 진형의 아군 1명은 공격력 +1 또는 SP -0.1 (4의 배수 판정)";
        passiveDescription[3] = "해당 전투 동안 드로우된 카드에 [모든 적에게 데미지 : 1]을 부여";
        passiveDescription[4] = "행동 시작 시 무덤에서 임의로 카드를 패로 가져온다. 해당 전투 동안 가져온 카드에 [모든 적에게 데미지 : 1]을 부여";
        passiveDescription[5] = "해당 턴 동안 카드를 3장 사용할 때마다 스파키에게 SP -1.0 (최대 2.0)";
        passiveDescription[6] = "해당 전투 동안 아군이 공격이 성공할 때마다 해당 적에게 데미지 : 자신의 공격력";
        passiveDescription[7] = "스파키의 행동 종료 시 패에 있는 임의로 카드 1장의 소비 코스트를 전 종료까지 0으로 만든다";
        passiveDescription[8] = "해당 전투 동안 스파키가 적에게 데미지를 10번이상 성공했을 때마다 공격력 +1";
        passiveDescription[9] = "자신을 제외한 아군, 적의 체력이 감소할 때마다 자신에게 방어도 : 1";
        passiveDescription[10] = "턴 시작 시 방어도가 있을 경우 부여 코스트 + 1, 방어도가 없을 경우 자신에게 방어도 : 3";
        passiveDescription[11] = "적의 공격이 성공할 때마다 방어도가 존재하면 깎인 방어도 절반만큼(소수점 버림) 해당 적에게 데미지를 준다";
        passiveDescription[12] = "턴 시작 시 한 진형에 반가라 혼자 있을 경우, 반가라에게 방어도 : 7 그리고 SP -0.2 (최대 5.0)";
        passiveDescription[13] = "행동 종료 시 패가 5장 이상 남아 있을 경우 다음 턴 시작할 때 드로우 : 2";
        passiveDescription[14] = "해당 턴 동안 카드를 통해 드로우 했을 때 포르테의 부여 코스트 +2";
        passiveDescription[15] = "턴 시작 시 패 한 장을 선택해서 덱으로 되돌리고 드로우 : 1, 드로우된 카드의 소비 코스트는 2 감소한다";
        passiveDescription[16] = "턴 시작시 묘지와 덱 중 카드가 많은 쪽에서 적은 쪽으로 카드 1장을 선택하고 이동시킨다.";
        passiveDescription[17] = "령의 행동 시작 시 묘지에 있는 카드 1장을 무작위로 패에 가지고 온다. 해당 카드는 행동 종료 시 혹은 사용했을 때 소멸된다";
        passiveDescription[18] = "3번의 전투 종료 시 1000이그넘을 획득한다";
        passiveDescription[19] = "해당 턴 동안 약화가 부여 혹은 카드가 소멸될 경우 아군의 SP -0.1";
        passiveDescription[20] = "령이 사용하는 카드의 소비 코스트는 1로 고정된다. 사용한 카드는 소멸된";
        passiveDescription[21] = "공격력이 2배가 되고 카드 사용시 적이 무작위로 선택된다";
        passiveDescription[22] = "해당 전투 동안 턴 시작 시 임의의 아군 공격력을 1 감소하고 흉귀의 공격력을 2 증가시킨다";
        passiveDescription[23] = "흉귀의 방어력이 10 이상일 때 공격력이 1 증가하고 방어력을 0으로 한다";
        passiveDescription[24] = "전투 시작 시 공물의 10%를 감소하고 감소한 공물의 10%만큼 공격력이 증가한다";
    }
}