using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniRunTitleScroll : MonoBehaviour
{
    public float speed = 10f;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
