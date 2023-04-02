using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_CardList", menuName = "Scriptable Objects/Card/Card List")]
public class SO_CardList : ScriptableObject
{
    [SerializeField]
    public List<CardDetails> cardDetails;
}