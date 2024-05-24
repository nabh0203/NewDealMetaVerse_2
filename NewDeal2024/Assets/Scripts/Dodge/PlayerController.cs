using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Contoroller Option")]
    [Tooltip("�̵��ӵ�")]
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
        //GetAxis : ������� �������� �Է��� �����ϸ� ������ ����
        //�� , �Ʒ� Ű�� flaot �� ����
        float xInput = Input.GetAxis("Horizontal");
        // �� , �� Ű�� float �� ����
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
