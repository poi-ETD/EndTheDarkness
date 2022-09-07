using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager_CharacterScene : MonoBehaviour
{
    private static HandManager instance;

    public static HandManager Instance
    {
        get
        {
            return instance;
        }
    }

    private int[] characterCodes_Party; // 파티의 구성원의 캐릭터 코드를 가지는 변수

    [SerializeField] private SO_CharacterList so_CharacterList;

    [SerializeField] private GameObject[] gos_Frame_party;
    [SerializeField] private TextMeshProUGUI[] texts_Party;
    [SerializeField] private Image[] images_Party;

    private int selectedPartyCode;
    private int selectedCharacterCode;

    [SerializeField] private GameObject go_Characters;
    [SerializeField] private GameObject go_SelectedCharacter;

    // # 선택된 캐릭터 세부 정보에 쓰이는 변수 목록
    [SerializeField] private TextMeshProUGUI text_Name_SelectedCharacter;
    [SerializeField] private TextMeshProUGUI text_Aka_SelectedCharacter;

    [SerializeField] private TextMeshProUGUI text_Hp;
    [SerializeField] private TextMeshProUGUI text_AttackPower;
    [SerializeField] private TextMeshProUGUI text_Cost;
    [SerializeField] private TextMeshProUGUI text_Endurance;
    [SerializeField] private TextMeshProUGUI text_InventorySize;
    [SerializeField] private TextMeshProUGUI text_Speed;

    [SerializeField] private TextMeshProUGUI text_CardCount_EnableSelect;

    [SerializeField] private TextMeshProUGUI text_Name_Passive0;
    [SerializeField] private TextMeshProUGUI text_Name_Passive1;
    [SerializeField] private TextMeshProUGUI text_Name_Passive2;
    [SerializeField] private TextMeshProUGUI text_Name_Passive3;
    // ##

    [SerializeField] private GameObject go_SubWindow_Passive;
    [SerializeField] private TextMeshProUGUI text_Name_SubWindow_Passive;
    [SerializeField] private TextMeshProUGUI text_Description_SubWindow_Passive;

    private void Awake()
    {
        characterCodes_Party = new int[4];
    }

    void Start()
    {
        Init_Main();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Debug.Log(characterCodes_Party[0] + ", " + characterCodes_Party[1] + ", " + characterCodes_Party[2] + ", " + characterCodes_Party[3] + ", ");
    }

    public void Click_Party(int partyCode) // 왼쪽 파티의 파티원을 누르면 실행되는 함수
    {
        gos_Frame_party[selectedPartyCode].SetActive(false);

        selectedPartyCode = partyCode;
        gos_Frame_party[partyCode].SetActive(true);

        if (characterCodes_Party[partyCode] == 0) // 부활자인 경우
        {
            go_Characters.SetActive(false);

            Select_Character(0);
        }
        else // 부활자가 아닌 경우
        {
            go_Characters.SetActive(true);

            if (characterCodes_Party[partyCode] == -1)
                go_SelectedCharacter.SetActive(false);
            else
                Select_Character(characterCodes_Party[partyCode]);
        }
    }

    public void Click_Select_Character(int characterCode) // 오른쪽 캐릭터 나열의 캐릭터를 누르면 실행되는 함수
    {
        for (int i = 0; i < 4; i++) // 현재 파티중
        {
            if (characterCodes_Party[i] == characterCode) // 이미 존재하는 파티원과 동일한 캐릭터라면 리턴
                return;
        }

        Select_Character(characterCode);

        characterCodes_Party[selectedPartyCode] = characterCode;

        images_Party[selectedPartyCode].sprite = so_CharacterList.characterDetails[characterCode].sprite_Face;
        texts_Party[selectedPartyCode].text = so_CharacterList.characterDetails[characterCode].name;
    }

    public void PointerEnter_Passive(int code) // 패시브 이름 위에 포인터를 진입시킬 때 실행되는 함수
    {
        go_SubWindow_Passive.SetActive(true);

        if (code == 0)
        {
            text_Name_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive0_Name.ToString();
            text_Description_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive0_Description.ToString();
        }
        else if (code == 1)
        {
            text_Name_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive1_Name.ToString();
            text_Description_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive1_Description.ToString();
        }
        else if (code == 2)
        {
            text_Name_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive2_Name.ToString();
            text_Description_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive2_Description.ToString();
        }
        else if (code == 3)
        {
            text_Name_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive3_Name.ToString();
            text_Description_SubWindow_Passive.text = so_CharacterList.characterDetails[selectedCharacterCode].passive3_Description.ToString();
        }
    }

    public void PointerExit_Passive() // 패시브 이름 위의 포인터를 퇴출시킬 때 실행되는 함수
    {
        go_SubWindow_Passive.SetActive(false);
    }

    private void Init_Main() // 캐릭터 세팅 씬의 초기화를 담당하는 함수
    {
        for (int i = 0; i < 4; i++)
        {
            characterCodes_Party[i] = -1;
        }

        characterCodes_Party[0] = 0;
    }

    private void Select_Character(int characterCode) // 선택한 캐릭터의 세부정보를 나타내는 함수
    {
        selectedCharacterCode = characterCode;

        go_SelectedCharacter.SetActive(true);

        text_Name_SelectedCharacter.text = so_CharacterList.characterDetails[characterCode].name;
        text_Aka_SelectedCharacter.text = so_CharacterList.characterDetails[characterCode].aka;

        text_Hp.text = so_CharacterList.characterDetails[characterCode].hp.ToString();
        text_AttackPower.text = so_CharacterList.characterDetails[characterCode].attackPower.ToString();
        text_Cost.text = so_CharacterList.characterDetails[characterCode].cost.ToString();
        text_Endurance.text = so_CharacterList.characterDetails[characterCode].endurance.ToString();
        text_InventorySize.text = so_CharacterList.characterDetails[characterCode].inventorySize.ToString();
        text_Speed.text = so_CharacterList.characterDetails[characterCode].speed.ToString();

        if (so_CharacterList.characterDetails[characterCode].isReviver)
            text_CardCount_EnableSelect.text = "5";
        else
            text_CardCount_EnableSelect.text = "1";

        text_Name_Passive0.text = so_CharacterList.characterDetails[characterCode].passive0_Name.ToString();
        text_Name_Passive1.text = so_CharacterList.characterDetails[characterCode].passive1_Name.ToString();
        text_Name_Passive2.text = so_CharacterList.characterDetails[characterCode].passive2_Name.ToString();
        text_Name_Passive3.text = so_CharacterList.characterDetails[characterCode].passive3_Name.ToString();
    }
}
