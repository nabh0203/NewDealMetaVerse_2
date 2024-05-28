using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UniRunPlayerController : MonoBehaviour
{
    private const int MAX_JUMP_COUNT = 2;

    //사망시 재생되는 음악
    public AudioClip dethClip;
    //점프 힘
    public float jumpforce = 70f;
    //점프 획수
    private int jumpCount = 0;
    //땅바닥에 닿았는지 확인
    private bool isGrounded = false;
    //죽었는지 확인
    private bool isDead = false;

    public int currentLifeCount;

    #region 플레이어에게 할당되는 Audio,RigidBody,Animator 변수

    private Rigidbody2D playerRigidbody;
    private Animator animator;
    private AudioSource playerAudio;
    #endregion

    public float invincibilityDuration = 2.0f; // 무적 시간 설정
    private SpriteRenderer playerSprite; // Player의 Sprite Renderer 컴포넌트

    private bool isInvincible = false; // 무적 상태 여부

    private Coroutine Blink;



    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();

        currentLifeCount = 5;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < MAX_JUMP_COUNT && !isDead)
        {
            //점프횟수 추가
            jumpCount++;

            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jumpforce));

            playerAudio.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space) && playerRigidbody.velocity.y > 0)
        {
            //점프 키에서 손을 떼는 순간&&플레이어의 y값이 0보다 큰 순간에
            //현재 속도를 반으로 감소
            playerRigidbody.velocity = playerRigidbody.velocity * 0.5f;
        }

        // 애니메이터의 Grounded 파라미터를 isGrounded 값으로 갱신한다. (매프레임)
        animator.SetBool("Grounded", isGrounded);


    }
    private void Die()
    {
        //애니메이터의 Die 트리거 파라미터를 보냄
        animator.SetTrigger("Die");
        //오디오 소스에 할당된 클립을 dethClip 으로 변경
        playerAudio.clip = dethClip;
        //바뀐 음악 파일을 재생
        playerAudio.Play();

        //캐릭터의 속도를 (0,0) 으로 변경
        playerRigidbody.velocity = Vector2.zero;
        //isDead 를 참으로 반환
        isDead = true;

        UniRunManager.instance.OnPlayerDead();        
        //gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //캐릭터가 아래로 떨어져서 죽는 로직
        if (other.tag == "Dead" && !isDead) //충돌한 상대방의 태그가 Dead && 사망한 상태가 아니라면
        {
            currentLifeCount = 0;
        }
        else if (other.tag == "obstacles" && !isDead && !isInvincible)
        {
            TaskDamage();
            //currentLifeCount -- ;
            //StartCoroutine(InvincibilityRoutine());
            //조건연산자 : 조건 ? 참 일때 값 : 거짓 일때 값
            //currentLifeCount = currentLifeCount < 0 ? 0 : currentLifeCount;
            //if (isInvincible)
            //{
                
            //}
        }
        
        UniRunManager.instance.RefreshLifeCount(currentLifeCount);
        if (currentLifeCount <= 0)
        {
            Die();
        }
        if (other.CompareTag("BonusLife") && currentLifeCount < 5)
        {
            currentLifeCount++;
            other.gameObject.SetActive(false);
            UniRunManager.instance.BonusLifeCount(currentLifeCount);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //캐릭터가 점프 후 착지했을때 로직
        //법선벡터 : normal 을 사용하는 벡터 이며 0~1사이의 값을 나타낸다.
        if (other.contacts[0].normal.y >0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
            Debug.Log("착지");
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    // 무적 상태를 처리하는 코루틴
    //private IEnumerator InvincibilityRoutine()
    //{
    //    // 무적 상태 시작
    //    isInvincible = true; 
    //    for (float i = 0; i < invincibilityDuration; i += 0.5f)
    //    {
    //        // Sprite Renderer의 활성 상태를 전환하여 깜빡임 효과 생성
    //        playerSprite.enabled = !playerSprite.enabled;
    //        yield return new WaitForSeconds(0.2f);
    //    }
    //    // 깜빡임 종료 후 Sprite Renderer 활성화
    //    playerSprite.enabled = true;
    //    // 무적 상태 종료
    //    isInvincible = false; 
    //}
    private void TaskDamage()
    {
        currentLifeCount--;
        if (Blink != null)
        {
            StopCoroutine(Blink);
        }
        Blink = StartCoroutine(BlinkEffect());
    }

    private IEnumerator BlinkEffect()
    {
        isInvincible = true;
        float endTime = Time.time + 5f;

        while (Time.time < endTime) 
        {
            playerSprite.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(0.2f);

            playerSprite.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.2f);

        }
        playerSprite.color = new Color(1, 1, 1, 1);
        isInvincible = false;
    }
}



