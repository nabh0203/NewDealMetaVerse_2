using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniRunScrollingObject : MonoBehaviour
{
    // ȭ�� �̵��ӵ�
    public float speed = 10f; 

    void Update()
    {
        if (UniRunManager.instance.isGameover)
        {
            //speed = 0f;
            return;
        }
        //1�ʸ��� x��ǥ�� -1 ��ŭ �����δ�.
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        
    }
}
