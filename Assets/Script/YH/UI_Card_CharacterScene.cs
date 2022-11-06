using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Card_CharacterScene : MonoBehaviour
{
    public int cardCode;
    public int ownerCode;

    public Image image_BackGround;
    public TextMeshProUGUI text_Cost;
    public TextMeshProUGUI text_Number;
    public TextMeshProUGUI text_Name;
    public TextMeshProUGUI text_Owner;
    public TextMeshProUGUI text_Thema;
    public TextMeshProUGUI text_Description;

    [HideInInspector] public int count;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
