using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
using Newtonsoft.Json;

public class CharacterManager : MonoBehaviour
{
    CharacterData CD = new CharacterData();
    float timer;

    [SerializeField] Sprite[] CharacterImgae;
    [SerializeField] Sprite[] CharacterFaceImgae;
    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject Prefebs;
    List<int> CharacterList = new List<int>();
    [SerializeField] Image[] listImage;
    int counter;
    List<int> FrontCharacter = new List<int>();
    List<int> BackCharacter = new List<int>();
    [SerializeField] TextMeshProUGUI[] SetText;//1->이름 2~5 -> 패시브 1 6->패시브 팝업 텍스트 7->경고 팝업 텍스트
    [SerializeField] GameObject PassivePopup;
    int curPassive;
    [SerializeField] GameObject[] Arrows; //현재 선택된 패시브 가르키는 화살표
    [SerializeField] Image[] FrontButton; //전방, 후방
    int curFormation;
    [SerializeField] CardSetManager cardManager;
    [SerializeField] Image SubImage;
    [SerializeField] GameObject warnPopup;

    [SerializeField] private GameObject go_Window_SlotName;
    [SerializeField] private Text text_SlotName;

    public void ToMain() //저장 후 로비로 게임 시작
    {
        cardManager.SaveCard();

        for(int i = 0; i < CharacterList.Count; i++)
        {
            if (CD.characterDatas[i].curFormation == 0)
                CD.line++;   
        }

        CD.size = CharacterList.Count; //캐릭터의 수
        string characterData = JsonConvert.SerializeObject(CD);       
        string path = Path.Combine(Application.persistentDataPath, GameManager.Instance.slot_CharacterDatas[GameManager.Instance.selectedSlot_Main]);                       
        File.WriteAllText(path, characterData);

        GameManager.Instance.nowPlayingSlot = GameManager.Instance.selectedSlot_Main; // 현재 플레이중인 슬롯 번호를 선택한 슬롯 번호로 초기화

        GameManager.Instance.Save();

        StartCoroutine(SceneControllerManager.Instance.SwitchScene("Scene3_Lobby"));
        //SceneManager.LoadScene("Scene2_Lobby");
    }

    public void RemoveCharacter(int i) //리스트에서 캐릭터를 누르면, 리스트에서 없앰.
    {
        if (i < CharacterList.Count)
            CharacterList.RemoveAt(i);
        setListImage();
    }

    private void Awake()
    {
        curPassive = -1;
        curFormation = -1;
        counter = -1; //초기 값 세팅
        for (int i = 1; i < CharacterInfo.Instance.cd.Length; i++)
        {
            GameObject cha = Instantiate(Prefebs, Canvas.transform);
            cha.GetComponent<CharacterSetting>().SetCharacter(i);
        }
    }

    public void setCharacter(int n) //리스트가 아닌, 캐릭터 일러를 누르면 이미 있는 캐릭터면 리스트에서 없애고,
    {//존재하지 않는 캐릭터면 리스트에 추가함
        bool isThere=false;

        for(int i = 0; i < CharacterList.Count; i++)
        {
            if (CharacterList[i] == n)
            {
                CharacterList.RemoveAt(i);
                isThere = true;
            }
        }

        if (!isThere && CharacterList.Count < 4)
            CharacterList.Add(n);

        setListImage();
    }

    void setListImage() //리스트가 변경함에 따라 리스트에 보이는 캐릭터의 이미지를 다시 나타냄
    {
        for(int i = 0; i < CharacterList.Count; i++)
        {
            listImage[i].sprite = CharacterInfo.Instance.cd[CharacterList[i]].characterFace;
            listImage[i].color = new Color(1, 1, 1, 1);
        }

        for(int i = CharacterList.Count; i < 4; i++)
            listImage[i].color = new Color(1, 1, 1, 0);
    } 

    public bool noSelect;

    public void noSelectMakeFalse()
    {
        noSelect = false;
    }

    [SerializeField] GameObject MaxCardPopup;

    public void SetSubSetting() // 1->이름, 2~5->패시브 1, 6->패시브 팝업 텍스트
    { //캐릭터 최대 4명을 선택하고 세부적으로 선택을 할 때
        if (counter > -1)
        {
            int no = CharacterList[counter];
            if (curFormation == -1) //진형이 초기값이라면.
            {
                warnPopup.SetActive(true);
                SetText[6].text = "진형이 설정되지 않았습니다.";
                return;
            }

            if (curPassive == -1) //패시브가 초기값이라면
            {
                warnPopup.SetActive(true);
                SetText[6].text = "패시브가 설정되지 않았습니다.";
                return;
            }

            if (cardManager.curCardCount < cardManager.maxiumCardCount && !noSelect)//카드를 더 선택이 가능하다면
            {
                noSelect = true;
                MaxCardPopup.SetActive(true);
                return; 
            } 

            noSelect = false;
            MaxCardPopup.SetActive(false);
            // public curCharacterData(string name, int no, int cost, int atk, 
            //int maxHp, int curHp, int passive1, int passive2, int passive3, int passive4, int curFormation)
            int[] passiveO = new int[4];
            CD.characterDatas[counter] = new CharacterData.curCharacterData(
                CharacterInfo.Instance.cd[no].name, no, CharacterInfo.Instance.cd[no].cost, CharacterInfo.Instance.cd[no].atk, CharacterInfo.Instance.cd[no].def, CharacterInfo.Instance.cd[no].speed, CharacterInfo.Instance.cd[no].maxHp, CharacterInfo.Instance.cd[no].maxHp, passiveO, curFormation, -1);
            CD.characterDatas[counter].passive[curPassive]++;
        }

        if (counter+1 >= CharacterList.Count) //모든 캐릭터 서브 세팅을 마치면
        {
            go_Window_SlotName.SetActive(true); 
            return;
        }

        counter++; //다음 캐릭터 세팅을 위함.
        cardManager.clear();

        if(CharacterList[counter]==1)
            cardManager.getStarterCard(CharacterList[counter],5);
        else
            cardManager.getStarterCard(CharacterList[counter], 1);

        SubImage.sprite = CharacterInfo.Instance.cd[CharacterList[counter]].characterSprtie;
        SetText[0].text = CharacterInfo.Instance.cd[CharacterList[counter]].name;
        SetText[1].text = CharacterInfo.Instance.cd[CharacterList[counter]].passive[0];
        SetText[2].text = CharacterInfo.Instance.cd[CharacterList[counter]].passive[1];
        SetText[3].text = CharacterInfo.Instance.cd[CharacterList[counter]].passive[2];
        SetText[4].text = CharacterInfo.Instance.cd[CharacterList[counter]].passive[3];   
        //다음 캐릭터를 보여줌
        Canvas.SetActive(false);
        curPassive = -1;
        curFormation = -1; //캐릭터가 바꼈기 때문에 다시 초기값 세팅
        PassivePopup.SetActive(false);

        for(int i=0;i<4;i++)
            Arrows[i].SetActive(false);

        FrontButton[0].color = new Color(1, 1, 1);
        FrontButton[1].color = new Color(1, 1, 1);
    }

    public void setPassive(int selectedPassive) //서브 세팅에서 패시브를 눌렀을 때
    {
        if (curPassive != selectedPassive) //이미 눌러진 패시브가 아니라면,
        {
            PassivePopup.SetActive(true); //패시브 설명을 킴

            SetText[5].text = CharacterInfo.Instance.cd[CharacterList[counter]].passiveContent[selectedPassive];
            //설명에 있는 텍스트를 변경함
            if (curPassive != -1)
                Arrows[curPassive].SetActive(false); //이미 패시브 다른게 있었다면 화살표 없앰.
            curPassive = selectedPassive;

            Arrows[selectedPassive].SetActive(true);
        }
        else //이미 눌러진 패시브와 같다면 초기상태로 변경
        {
            Arrows[curPassive].SetActive(false);
            PassivePopup.SetActive(false);
            curPassive = -1;
        } 
    }

    public void setFormation(int i) //진형 설정
    {
        if (i == curFormation) //이미 눌린 진형을 또 누르면 초기로
        {
            FrontButton[i].color = new Color(1, 1, 1);
            curFormation = -1;
        }
        else //다를 시 진형 선택
        {
            if(curFormation!=-1)
                FrontButton[curFormation].color = new Color(1, 1, 1);

            curFormation = i;
            FrontButton[curFormation].color = new Color(1, 0, 0);
        }
    }

    public void Click_Button_OK_SlotName()
    {
        GameManager.Instance.slot_Names[GameManager.Instance.selectedSlot_Main] = text_SlotName.text;
        GameManager.Instance.Save();
        Debug.Log("슬롯의 이름이 " + text_SlotName.text + "(으)로 설정되었습니다.");

        ToMain();
    }
}

//public class CharacterData //json으로 저장 될 인게임 캐릭터 정보
//{
//    public struct curCharacterData
//    {
//        public string Name;
//        public int No;
//        public int Cost;
//        public int Atk;
//        public int maxHp;
//        public int curHp;
//        public int def;
//        public float speed;
//        public int[] passive;
//        public int curFormation; //0->전방 1->후방
//        public int curEquip;

//        public curCharacterData(string name, int no, int cost, int atk, int def ,float speed,int maxHp, int curHp, int[] passive, int curFormation,int curEquip)
//        {
//            Name = name;
//            No = no;
//            Cost = cost;
//            Atk = atk;
//            this.def = def;
//            this.speed = speed;
//            this.maxHp = maxHp;
//            this.curHp = curHp;
//            this.passive = passive;
//            this.curFormation = curFormation;
//            this.curEquip = curEquip;
//        }
//    }

//    public curCharacterData[] characterDatas = new curCharacterData[5];
//    public int line; //전, 후방을 나누는 기준
//    public int size; 
//}
