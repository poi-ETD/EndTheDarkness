using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    int layerMask;

    void Start()
    {
        
    }

    void Update()
    {
        //특정 Layer만 raycast하기
        RaycastHit hit;
        float distance = 10f;
        int layerMask = 1 << LayerMask.NameToLayer("Default");  // Player 레이어만 충돌 체크함
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance, layerMask))
        {
            Debug.Log(hit.collider.gameObject);
        }
    }
}
