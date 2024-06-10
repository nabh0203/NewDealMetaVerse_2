using System.Collections;
using UnityEngine;
using UnityEngine.AI; // AI, 내비게이션 시스템 관련 코드 가져오기

// 좀비 AI 구현
public class Zombie : LivingEntity
{
    public LayerMask whatIsTarget; // 추적 대상 레이어
    public ParticleSystem hitEffect; // 피격 시 재생할 파티클 효과
    public AudioClip deathSound; // 사망 시 재생할 소리
    public AudioClip hitSound; // 피격 시 재생할 소리

    private LivingEntity targetEntity; // 추적 대상
    private NavMeshAgent navMeshAgent; // 경로 계산 AI 에이전트
  
    private Animator zombieAnimator; // 애니메이터 컴포넌트
    private AudioSource zombieAudioPlayer; // 오디오 소스 컴포넌트
    private Renderer zombieRenderer; // 렌더러 컴포넌트

    public float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점

    // 추적할 대상이 존재하는지 알려주는 프로퍼티
    private bool hasTarget {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            // 그렇지 않다면 false
            return false;
        }
    }

    private void Awake() {
        // 초기화

        // 게임 오브젝트로부터 사용할 컴포넌트들을 가져오기
        navMeshAgent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioPlayer = GetComponent<AudioSource>();

        // 렌더러 컴포넌트는 자식 게임오브젝트 Zombie_Cylinder에 있는 Skinned Mesh Renderer를 가져옴
        // 자식객체에서 컴포넌트를 가져올 때는 GetComponentChildren() 메서드를 사용
        zombieRenderer = GetComponentInChildren<Renderer>();
    }

    // 좀비 AI의 초기 스펙을 결정하는 셋업 메서드
    public void Setup(ZombieData zombieData)
    {
        // 좀비의 체력 설정
        startingHealth = zombieData.health;
        health = zombieData.health;

        // 공격력 설정
        damage = zombieData.damage;
        navMeshAgent.speed = zombieData.speed;  // 내비메시 에이전트의 이동속도
        zombieRenderer.material.color = zombieData.skinColor;   // 렌더러가 사용중인 머테리얼 컬러를 변경, 외형 색이 변함
    }

    private void Start() {
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }

    private void Update() {
        // 추적 대상의 존재 여부에 따라 다른 애니메이션 재생
        zombieAnimator.SetBool("HasTarget", hasTarget);
    }

    // 주기적으로 추적할 대상의 위치를 찾아 경로 갱신
    private IEnumerator UpdatePath() {
        // 살아 있는 동안 무한 루프
        while (!dead)
        {
            if (hasTarget)
            {
                // 추적할 대상(플레이어)가 존재할 때 : 경로를 갱신하고 AI 이동을 계속 진행
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                // 추적 대상이 없을 경우 : AI 이동 중지
                navMeshAgent.isStopped = true;

                // 20의 반지름을 가진 가상의 구를 그려서, 구와 겹치는 모든 콜라이더들을 가져옴(3번째 파라미터 타겟에대한 레이어마스크를 사용 안했을 때)
                // 단, WhatIsTarget 레이어를 가져오도록 3번째 파라미터로 필터링
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);

                for (int i = 0; i < colliders.Length; i++)
                {
                    // 콜라이더로부터 LivingEntity 컴포넌트 가져오기
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    // LivingEntity 컴포넌트가 있으며, 해당 LivingEntity가 죽은 상태가 아니라면 타겟으로 설정
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity;        // 추적 대상을 livingEntity로 설정
                        break;                              // for문 루프 즉시 정지
                    }
                }
                
            }

            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    // 데미지를 입었을 때 실행할 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // 아직 사망하지 않은 경우에만 피격 효과 재생
        if (!dead)
        {
            // 공격 받은 지점과 방향으로 파티클 효과를 재생
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            // 피격 효과음 재생
            zombieAudioPlayer.PlayOneShot(hitSound);
        }
        
        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // 사망 처리
    public override void Die() {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        Collider[] zombieColliders = GetComponents<Collider>();
        for (int i = 0; i < zombieColliders.Length; i++)
        {
            zombieColliders[i].enabled = false;         // 좀비에게 할당된 모든 콜라이더들을 비활성화(다른 AI들의 방해를 받지 않기 위해)
        }

        navMeshAgent.isStopped = true;          // AI의 추적 중지
        navMeshAgent.enabled = false;           // 내비메쉬 컴포넌트 비활성화

        zombieAnimator.SetTrigger("Die");       // Die 애니메이션 재생
        zombieAudioPlayer.PlayOneShot(deathSound);  // 사망시 효과음 재생
    }

    private void OnTriggerStay(Collider other)
    {
        // OnTriggerStay는 콜라이더 충돌이 발생하는동안 계속 진입
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();     // 부딛힌 콜라이더로부터 LivingEntity를 가져옴

            // attackTarget이 null이 아니면(콜라이더가 livingEntity요소를 가지고 있으면) && attackTarget이 targetEntity와 같으면
                                                                                        // UpdatePath에서 Player Layer를 가진 녀석만 targetEntity에 할당
                                                                                        // 좀비도 LivingEntity를 가지고 있으므로
                                                                                        // 좀비와 충돌로 발생한 타겟을 필터링하기위해
            if (attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;         // 마지막 공격시간 갱신

                Vector3 hitPoint = other.ClosestPoint(transform.position);          // 상대 콜라이더와 가장 가까운 피격위치를 계산
                Vector3 hitNormal = transform.position - other.transform.position;  // 방향벡터를 가져오기 위한 공식

                // 공격 실행
                attackTarget.OnDamage(damage, hitPoint, hitNormal);                 // Player안에 있는 OnDamage 호출
            }
        }
    }
}