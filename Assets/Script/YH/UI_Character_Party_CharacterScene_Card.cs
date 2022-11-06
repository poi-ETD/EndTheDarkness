using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Character_Party_CharacterScene_Card : MonoBehaviour
{
    public int characterCode;
    public TextMeshProUGUI text_Name;
    public TextMeshProUGUI text_Count_Card;
    public Image image_Face;

    [HideInInspector] public int count_SelectedCard;

    void Start()
    {

    }

    void Update()
    {

    }
}