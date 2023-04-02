using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_CharacterList", menuName = "Scriptable Objects/Character/Character List")]
public class SO_CharacterList : ScriptableObject
{
    [SerializeField]
    public List<CharacterDetails> characterDetails;
}