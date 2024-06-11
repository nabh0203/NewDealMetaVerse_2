using System.Collections;
using System.Security.Cryptography;
using Photon.Pun;
using UnityEngine;

// 총을 구현
public class Gun : MonoBehaviourPun, IPunObservable {
    // 총의 상태를 표현하는 데 사용할 타입을 선언
    public enum State {
        Ready,
        Empty,
        Reloading
    }

    public State state { get; private set; } // 현재 총의 상태

    public Transform fireTransform; // 탄알이 발사될 위치

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과

    private LineRenderer bulletLineRenderer; // 탄알 궤적을 그리기 위한 렌더러

    private AudioSource gunAudioPlayer; // 총 소리 재생기

    public GunData gunData; // 총의 현재 데이터

    private float fireDistance = 50f; // 사정거리

    public int ammoRemain = 100; // 남은 전체 탄알
    public int magAmmo; // 현재 탄알집에 남아 있는 탄알

    private float lastFireTime; // 총을 마지막으로 발사한 시점

    //주기적으로 자동 실행되는 동기화 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //로컬 오브젝트라면 쓰기 부분이 실행
        if (stream.IsWriting)
        {
            //남은 탄알 수를 네트워크를 통해 보내기
            stream.SendNext(ammoRemain);
            //탄찬의 탄알 수를 네트워크를 통해 보내기
            stream.SendNext(magAmmo);
            //현재 총의 상태를네트워크를 통해 보내기
            stream.SendNext(state);
        }
        else
        {
            //리모트 오브젝트라면 읽기 부분이 실행됨
            //남은 탄알 수를 네트워크를 통해 받기
            ammoRemain = (int)stream.ReceiveNext();
            //탄창의 탄알 수를 네트워크를 통해 받기
            magAmmo = (int)stream.ReceiveNext();
            //현재 총의 상태를 네트워크를 통해 받기
            state = (State)stream.ReceiveNext();
        }
    }
    //남은 탄알을 추가하는 메서드
    [PunRPC]
    public void AddAmmo(int ammo)
    {
        ammoRemain += ammo;
    }
    private void Awake() {
        // 사용할 컴포넌트의 참조 가져오기
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable() {
        // 총 상태 초기화
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;

        state = State.Ready;
        lastFireTime = 0;
    }

    // 발사 시도
    public void Fire()
    {
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            // 마지막 총 발사 시점을 갱신해서 다음시간까지 총을 쏠수없게 하는 시간값을 저장
            lastFireTime = Time.time;

            // 실제 발사 처리
            Shot();
        }
    }

    // 실제 발사 처리
    private void Shot()
    {
        photonView.RPC("ShotOnSever", RpcTarget.MasterClient);
        magAmmo--;
        if (magAmmo <= 0)
        {
            // 탄창에 남은 탄약이 없다면, 총의 현재상태를 Empty로 갱신
            state = State.Empty;
        }
    }
    [PunRPC]
    private void ShotOnServer()
    {
        // 레이캐스트를 사용할 변수 선언(컨테이너)
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        // Raycast 함수는 충돌이 일어난 경우 True를 반환 / 설정한 거리 이내에 충돌되는 collider가 없을 경우 False를 반환
        // 레이캐스트 파라미터(시작지점, 방향, 충돌정보반환할 컨테이너, 최대사정거리)
        bool isHit = Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance);

        if (isHit)
        {
            // 레이가 어떤 물체와 충돌했을때 여기로 들어옴

            // 충돌한 사물로부터 IDamageable 컴포넌트를 가져오기 시도(충돌한 오브젝트에 IDamageable이 없다면 Null이 됨)
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            // 히트가 일어난 대상으로부터 IDamageable을 가져왔을 경우에만 if문 안의 내용을 처리
            if (target != null)
            {
                // 상대방의 OnDamage 함수를 수행하여 데미지 주기
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }

            // 레이가 충돌한 위치를 저장
            hitPosition = hit.point;
        }
        else
        {
            // 레이에 충돌하는 오브젝트가 없다면 여기로 들어옴

            // 레이의 최대 거리로 설정해둔 firstDistance 뒤의 좌표를 hit좌표로 설정하여 거기까지 총알(?) LineRenderer을 그림
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }
        photonView.RPC("ShotEffectOnClients", RpcTarget.All, hitPosition);
    }
    //전체 클라이언트에 샷에 대한 이펙트 처리를 하려고 명령하기 위한 메서드
    //타 클라이언트의 코루틴을 직접 실행할 수 없기에 코루틴을 담아줄 함수
    [PunRPC]
    private void ShotEffectOnClients(Vector3 hitPosition)
    {
        // 발사 이펙트 코루틴함수 실행
        StartCoroutine(ShotEffect(hitPosition));
    }
    // 발사 이펙트와 소리를 재생하고 탄알 궤적을 그림
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // 총구 위치의 화염 효과를 재생
        muzzleFlashEffect.Play();

        // 탄피 배출 효과 재생
        shellEjectEffect.Play();

        // 총격 소리 재생
        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        bulletLineRenderer.SetPosition(0, fireTransform.position);      // 총알효과의 선의 시작점 위치
        bulletLineRenderer.SetPosition(1, hitPosition);                 // 총알효과의 선의 끝점(충돌이 일어난 지점의 위치)

        // 라인 렌더러를 활성화하여 탄알 궤적을 그림
        bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기 ( 0.03초동안만 탄알궤적을 보여주기위해 )
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 탄알 궤적을 지움
        bulletLineRenderer.enabled = false;
    }

    // 재장전 시도
    public bool Reload()
    {
        // 재장전을 할 수 없을때
        // 이미 재장전중이거나, 남아있는 탄창이 없거나, 탄알이 탄창에 가득 차있거나
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
        {
            return false;
        }

        // 재장전 코드 호출
        StartCoroutine(ReloadRoutine());

        return true;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine() {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;

        // 재장전 소리를 재생
        gunAudioPlayer.PlayOneShot(gunData.reloadClip);

        // 재장전 소요 시간 만큼 처리 쉬기
        yield return new WaitForSeconds(gunData.reloadTime);

        // 탄창에 몇발을 채워야 하는지 계산한다. = 탄창에들어갈수 있는 최대 탄알개수(25) - 현재 채워져있는 탄알 개수
        int ammoToFill = gunData.magCapacity - magAmmo;

        // 탄창에 채워야할 탄약이 남은 탄약보다 많다면,
        // 채워야할 탄약 수를 남아있는 탄약수로 맞춘다.
        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;      // 채울 수 있는 탄약 수를 채운다.
        ammoRemain -= ammoToFill;   // 남아있는 탄약 수를 채운만큼 뺀다.


        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;


    }
    


    // Enum 참고용 코드

    //public enum State
    //{
    //    Ready = 1, // 발사 준비됨                // 00000001
    //    Empty = 2, // 탄알집이 빔                // 00000010
    //    Reloading = 4, // 재장전 중              // 00000100
    //    Test = 8                                //  00001000
    //}
    //public void SetState(State state)
    //{
    //    bool isReady = state.HasFlag(State.Ready);  // false;
    //    bool isReloading = state.HasFlag(State.Reloading); // true;
    //    bool isEmpty = state.HasFlag(State.Empty); // true;
    //}

    //public void TestFunction()
    //{
    //    SetState(State.Reloading | State.Empty);

    //}
}