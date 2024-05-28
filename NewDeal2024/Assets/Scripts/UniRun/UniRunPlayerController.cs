using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniRunPlayerController : MonoBehaviour
{
    private const int MAX_JUMP_COUNT = 2;

    //����� ����Ǵ� ����
    public AudioClip dethClip;
    //���� ��
    public float jumpforce = 70f;
    //���� ȹ��
    private int jumpCount = 0;
    //���ٴڿ� ��Ҵ��� Ȯ��
    private bool isGrounded = false;
    //�׾����� Ȯ��
    private bool isDead = false;

    #region �÷��̾�� �Ҵ�Ǵ� Audio,RigidBody,Animator ����

    private Rigidbody2D playerRigidbody;
    private Animator animator;
    private AudioSource playerAudio;
    # endregion

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        

    
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < MAX_JUMP_COUNT && !isDead)
        {
            //����Ƚ�� �߰�
            jumpCount++;

            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jumpforce));

            playerAudio.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space) && playerRigidbody.velocity.y > 0)
        {
            //���� Ű���� ���� ���� ����&&�÷��̾��� y���� 0���� ū ������
            //���� �ӵ��� ������ ����
            playerRigidbody.velocity = playerRigidbody.velocity * 0.5f;
        }

        // �ִϸ������� Grounded �Ķ���͸� isGrounded ������ �����Ѵ�. (��������)
        animator.SetBool("Grounded", isGrounded);


    }
    private void Die()
    {
        //�ִϸ������� Die Ʈ���� �Ķ���͸� ����
        animator.SetTrigger("Die");
        //����� �ҽ��� �Ҵ�� Ŭ���� dethClip ���� ����
        playerAudio.clip = dethClip;
        //�ٲ� ���� ������ ���
        playerAudio.Play();

        //ĳ������ �ӵ��� (0,0) ���� ����
        playerRigidbody.velocity = Vector2.zero;
        //isDead �� ������ ��ȯ
        isDead = true;

        UniRunManager.instance.OnPlayerDead();        
        //gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //ĳ���Ͱ� �Ʒ��� �������� �״� ����
        if (other.tag == "Dead" && !isDead) //�浹�� ������ �±װ� Dead && ����� ���°� �ƴ϶��
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //ĳ���Ͱ� ���� �� ���������� ����
        //�������� : normal �� ����ϴ� ���� �̸� 0~1������ ���� ��Ÿ����.
        if (other.contacts[0].normal.y >0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
            Debug.Log("����");
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
