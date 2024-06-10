using UnityEngine;
using UnityEngine.UI; // UI 관련 코드

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity {
    public Slider healthSlider; // 체력을 표시할 UI 슬라이더

    public AudioClip deathClip; // 사망 소리
    public AudioClip hitClip; // 피격 소리
    public AudioClip itemPickupClip; // 아이템 습득 소리

    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    private PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트

    private void Awake() {
        // 사용할 컴포넌트를 가져오기
        playerAudioPlayer = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();

        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable() {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);    // 체력 슬라이더 게이지 활성화
        healthSlider.maxValue = startingHealth;     // 체력 슬라이더 최대값을최대치로 변경
        healthSlider.value = health;                // 체력 슬라이더의 현재 값을 현재 체력으로 변경

        playerMovement.enabled = true;              // playerMovement 활성화
        playerShooter.enabled = true;               // playerShooter 활성화
    }

    // 체력 회복
    public override void RestoreHealth(float newHealth) {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);

        healthSlider.value = health;
    }

    // 데미지 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (!dead)
        {
            // 죽지 않았을 경우에만 피격음 재생
            playerAudioPlayer.PlayOneShot(hitClip);
        }
        
        // LivingEntity의 OnDamage() 실행(데미지 적용)
        base.OnDamage(damage, hitPoint, hitDirection);
        // 갱신된 체력 슬라이더에 반영
        healthSlider.value = health;
    }

    // 사망 처리
    public override void Die() {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();

        healthSlider.gameObject.SetActive(false);   // 죽었을 때 체력 슬라이더 없앰
        playerAudioPlayer.PlayOneShot(deathClip);   // 사망했을 때 소리 재생
        playerAnimator.SetTrigger("Die");


        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        if (!dead)
        {
            IItem item = other.GetComponent<IItem>();       // 충돌한 상대방으로부터 Item 컴포넌트를 가져오기 시도

            if (item != null) 
            {
                item.Use(gameObject);                           //  Use 메서드를 실행하여 아이템 사용
                playerAudioPlayer.PlayOneShot(itemPickupClip);  // 아이템 줍는 소리 재생 
            }
        }
    }
}