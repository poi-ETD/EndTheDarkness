using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Enemy_Battle : MonoBehaviour // YH
{
    public Image image_Face;
    public TextMeshProUGUI text_Name;
    public TextMeshProUGUI text_Hp;
    public TextMeshProUGUI text_Attack;
    public TextMeshProUGUI text_Defense;
    public TextMeshProUGUI text_Speed;

    public TextMeshProUGUI text_Armor; // YH : 배틀 씬 내의 우측 적 정보 인스턴스 내의 방어도 텍스트
    public TextMeshProUGUI text_Blood; // YH : 배틀 씬 내의 우측 적 정보 인스턴스 내의 출혈 텍스트
    public TextMeshProUGUI text_Weak; // YH : 배틀 씬 내의 우측 적 정보 인스턴스 내의 약화 텍스트

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
