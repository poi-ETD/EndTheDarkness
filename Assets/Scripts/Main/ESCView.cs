using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCView : MonoBehaviour
{
    public void onExit()
    {
        Application.Quit();
    }
    public void notExit()
    {
        Destroy(gameObject);
    }
}
