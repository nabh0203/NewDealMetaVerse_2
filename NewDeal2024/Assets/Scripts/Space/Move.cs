using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //자식 오브젝트의 Transform 변수
    public Transform _childTransform;
    // Start is called before the first frame update
    void Start()
    {
        //자신의 전역위치를 0,-1,0 으로 이동
        transform.position = new Vector3(0, -1, 0);

        //자식의 지역위치를 (0, 2, 0) 으로 이동
        _childTransform.localPosition = new Vector3(0, 2, 0);

        //자신의 전역회전을 (0, 0, 30) 만큼 돌린다.
        transform.rotation = Quaternion.Euler(new Vector3 (0, 0, 30));

        //자식의 지역회전을 (0, 60, 0) 만큼 돌린다.
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
