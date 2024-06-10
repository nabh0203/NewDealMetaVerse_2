using Photon.Pun;
using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviourPun
{
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    public Transform mapTransform;      // levelArt map의 트랜스폼을 저장

    public Camera mainCamera;       // 메인카메라를 저장
    private void Start()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        // 4번 방법을 위한 Map의 트랜스폼 값을 가져온다.
        //GameObject levelArt = GameObject.Find("Level Art");
        //if (levelArt != null)
        //{
        //    mapTransform = levelArt.transform;
        //}
        //else
        //{
        //    Debug.LogError("Level Art 오브젝트를 찾을 수 없습니다.");
        //}

        // 5번방법 카메라를 기준으로 축이동을 하기위해 카메라 할당
        mainCamera = Camera.main;
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행

        // 회전을 실행
        Rotate();

        // 움직임을 실행
        Move();

        //    변수 = 조건 ? 참일때값 : 거짓일때 값       // 조건에 따라 참일때 값이나 거짓일때 값 둘중에 하나가 변수에 저장됨.
        float move = playerInput.moveVertical == 0 ? playerInput.moveHorizontal : playerInput.moveVertical;
        playerAnimator.SetFloat("Move", move);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move()
    {
        /*
        // 1. 기본 교재의 내용으로 작성한 이동 스크립트
        // 키 입력에 맞춰 속도값을 적용하여 상대적으로 이동할 거리 계산
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        // 리지드바디를 이용해 현재좌표에서 설정한 좌표까지 자연스럽게 이동하는 명령
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
        */

        // 2. 키보드 상하좌우키로 캐릭터의 전방을 기준으로 앞뒤좌우로 움직인다.
        //Vector3 moveDistance = (playerInput.moveVertical * transform.forward + playerInput.moveHorizontal * transform.right).normalized
        //    * moveSpeed * Time.deltaTime;

        // 3. 키보드의 상하좌우키로 캐릭터를 화면상의 상하좌우로 움직이고싶은데 대각선으로 움직이네?
        //Vector3 moveDistance = new Vector3(playerInput.moveHorizontal, 0, playerInput.moveVertical).normalized * moveSpeed * Time.deltaTime;

        // 4. map의 Transform 값을 이용하여 보정한다.
        //Vector3 moveVertical = playerInput.moveVertical * mapTransform.forward * moveSpeed * Time.deltaTime;
        //Vector3 moveHorizontal = playerInput.moveHorizontal * mapTransform.right * moveSpeed * Time.deltaTime;
        //Vector3 moveDistance = moveVertical + moveHorizontal;

        // 5. 카메라의 방향을 기준으로 좌우를 보정할때
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        Vector3 moveVertical = playerInput.moveVertical * cameraForward.normalized;
        Vector3 moveHorizontal = playerInput.moveHorizontal * cameraRight.normalized;
        Vector3 moveDistance = (moveVertical + moveHorizontal).normalized * moveSpeed * Time.deltaTime;

        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    private void Rotate()
    {
        /*
        // 1. 기본 교재의 내용으로 작성한 회전 스크립트
        // 회전할 상대 수치 계산
        // 한 프레임동안 회전할 각도를 저장하는 변수
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;

        // 리지드바디의 y축 회전값을 변경
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0);
        */

        // 2,3,4번의 move에서 사용했던 방법
        //float turn = playerInput.rotateMouse * rotateSpeed * Time.deltaTime;
        //playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0);

        // 5번 마우스의 위치로 플레이어 캐릭터가 방향을 트는 방법
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = (target - transform.position).normalized;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);

            playerRigidbody.MoveRotation(rotation);
        }
    }
}
