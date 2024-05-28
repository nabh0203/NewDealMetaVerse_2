using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniRunBackGroundLoop : MonoBehaviour
{
    private float width;

    private void Awake()
    {
        //����� ���α��̰� ������ �Ҵ�
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
        //���� ��ġ�� �������� �������� width �̻� �̵����� �� ��ġ�� ���ġ �ϱ� ���� �ڵ�
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
