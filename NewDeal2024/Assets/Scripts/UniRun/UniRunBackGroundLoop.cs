using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniRunBackGroundLoop : MonoBehaviour
{
    private float width;

    private void Awake()
    {
        //배경의 가로길이가 얼마인지 할당
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        width = backgroundCollider.size.x;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //현재 위치가 원점에서 왼쪽으로 width 이상 이동했을 때 위치를 재배치 하기 위한 코드
        if (transform.position.x <= -width)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        Vector2 offset = new Vector2(width * 2, 0);
        transform.position = (Vector2)transform.position + offset;

        //Vector3 offset = new Vector3(width * 2, 0, 0);
        //transform.position = transform.position + offset;


    }
}
