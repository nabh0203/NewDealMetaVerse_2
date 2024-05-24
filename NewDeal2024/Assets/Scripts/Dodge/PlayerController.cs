using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Contoroller Option")]
    [Tooltip("이동속도")]
    [SerializeField]
    private float moveSpeed = 5f;
    private Rigidbody playerRigidbody;


    void Start()
    {

        if (playerRigidbody == null)
        {
            playerRigidbody = GetComponent<Rigidbody>();
        }
    }


    private void Update()
    {
        //GetAxis : 수평축과 수직축의 입력을 감지하며 변수에 저장
        //위 , 아래 키값 flaot 형 변수
        float xInput = Input.GetAxis("Horizontal");
        // 좌 , 우 키값 float 형 변수
        float zInput = Input.GetAxis("Vertical");

        float xspeed = xInput * moveSpeed;
        float zspeed = zInput * moveSpeed;

        Vector3 newVelcity = new Vector3 (xspeed, 0f , zspeed);

        playerRigidbody.velocity = newVelcity;

        //if (Input.GetButtonDown("Jump"))
        //{
        //    playerRigidbody.AddForce(0f, 1000f, 0f);
        //}
    }

    public void Die()
    {
        gameObject.SetActive(false);
        GameManager GM = FindAnyObjectByType<GameManager>();
        GM.EndGame();
    }
}
