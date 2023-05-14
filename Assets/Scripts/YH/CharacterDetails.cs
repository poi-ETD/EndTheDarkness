using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterDetails
{
    public int characterCode;
    public string name;
    public string aka;

    public Sprite sprite_Face;
    public Sprite sprite_All;

    public bool isReviver;

    public int hp;
    public int attackPower;
    public int cost;
    public int endurance;
    public int inventorySize;
    public float speed;

    public string passive0_Name;
    public string passive0_Description;
    public string passive1_Name;
    public string passive1_Description;
    public string passive2_Name;
    public string passive2_Description;
    public string passive3_Name;
    public string passive3_Description;
}
