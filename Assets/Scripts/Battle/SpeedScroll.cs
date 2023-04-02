using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedScroll : MonoBehaviour
{
    [SerializeField] RectTransform content;
  public void PointUp()
    {
       // GetComponent<ScrollRect>().
        //content.offsetMin = new Vector2(0, 0);
    }
}
