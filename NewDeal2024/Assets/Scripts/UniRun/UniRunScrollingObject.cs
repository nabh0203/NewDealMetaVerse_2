using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniRunScrollingObject : MonoBehaviour
{
    // 화면 이동속도
    public float speed = 10f; 

    void Update()
    {
        //1초마다 x좌표로 -1 만큼 움직인다.
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
