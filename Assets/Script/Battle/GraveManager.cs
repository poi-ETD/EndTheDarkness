using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveManager : MonoBehaviour
{
    public GameObject[] buttons;
    public GameObject button;
    private void Awake()
    {
        for(int i = 0; i < 5; i++)
        {
            buttons[i] = Instantiate(button, new Vector3(2000+40*i,180, 0), transform.rotation);
        }
    }
}
