using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniRunScrollingObject : MonoBehaviour
{
    // 화면 이동속도
    public float speed = 10f; 

    void Update()
    {
        if (UniRunManager.instance.isGameover)
        {
            //speed = 0f;
            return;
        }
        //1초마다 x좌표로 -1 만큼 움직인다.
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        
    }
}
