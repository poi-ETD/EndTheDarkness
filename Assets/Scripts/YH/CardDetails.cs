using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CardDetails
{
    public int code;
    public Sprite sprite_Card;
    public string name;
    public int cost;
    public Owner ownerCharacter;
    public Thema thema;

    [Space(10f)]
    [TextArea(3, 5)]
    public string description_KR;
}    
