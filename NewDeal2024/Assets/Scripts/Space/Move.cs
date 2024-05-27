using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //�ڽ� ������Ʈ�� Transform ����
    public Transform _childTransform;
    // Start is called before the first frame update
    void Start()
    {
        //�ڽ��� ������ġ�� 0,-1,0 ���� �̵�
        transform.position = new Vector3(0, -1, 0);

        //�ڽ��� ������ġ�� (0, 2, 0) ���� �̵�
        _childTransform.localPosition = new Vector3(0, 2, 0);

        //�ڽ��� ����ȸ���� (0, 0, 30) ��ŭ ������.
        transform.rotation = Quaternion.Euler(new Vector3 (0, 0, 30));

        //�ڽ��� ����ȸ���� (0, 60, 0) ��ŭ ������.
        _childTransform.localRotation = Quaternion.Euler(new Vector3(0, 60, 0));
        //_childTransform.rotation = Quaternion.Euler(new Vector3(0, 60, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow)) 
        { 
            //transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime);
            transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime, Space.World);
        }
        
        if(Input.GetKey(KeyCode.DownArrow)) 
        { 
            //transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime);
            transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.LeftArrow)) 
        { 
            //transform.Rotate(new Vector3(0, 0, 180) * Time.deltaTime);
            transform.Rotate(new Vector3(0, 0, 180) * Time.deltaTime, Space.World);
            //_childTransform.Rotate(new Vector3(0, 180, 0) * Time.deltaTime);
            _childTransform.Rotate(new Vector3(0, 180, 0) * Time.deltaTime, Space.World);
            
        }
        
        if(Input.GetKey(KeyCode.RightArrow)) 
        { 
            //transform.Rotate(new Vector3(0, 0, -180) * Time.deltaTime);
            transform.Rotate(new Vector3(0, 0, -180) * Time.deltaTime, Space.World);
            //_childTransform.Rotate(new Vector3(0, -180, 0) * Time.deltaTime);
            _childTransform.Rotate(new Vector3(0, -180, 0) * Time.deltaTime, Space.World);
        }
        

    }
}
