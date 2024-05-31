using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도
   //public Transform mapTransform;

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    //private Camera mainCamera; // 메인 카메라
    public Camera mainCamera; // 메인 카메라


    private void Start() {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        //맵 값을 받아와 이동 로직 구현
        //GameObject levelArt = GameObject.Find("Level Art");

        //if(levelArt != null )
        //{
        //    mapTransform = levelArt.transform;
        //}
        //else
        //{
        //    Debug.Log("맵이 없음");
        //}

        //카메라 기준을 통해 이동 로직 구현
        mainCamera = Camera.main;
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate() {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
        ////회전 로직
        Rotate();

        // 회전 로직
        //RotateToMouseDirection();
        //이동 로직
        Move();

        float move = playerInput.moveVertical == 0 ? playerInput.moveHorizontal : playerInput.moveVertical;
        playerAnimator.SetFloat("Move", move);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move() {

        /*
         * 기본적 이동 로직
        //상대적으로 이동할 거리 계산
        Vector3 moveDistonce = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        //리지드바디를 이용해 게임오브젝트 위치 변경
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistonce);
        */

        // 플레이어 전방의 시점으로 상 하 좌 우 이동 로직
        //Vector3 moveDistonce = (playerInput.moveVertical * transform.forward + playerInput.moveHorizontal * transform.right).normalized * moveSpeed * Time.deltaTime;


        // 키보드의 상 하 좌 우 키를 입력받아 화면상의 상 하 좌 우 로 움직인다.

        //Vector3 moveDistonce = new Vector3(playerInput.moveHorizontal,0 ,playerInput.moveVertical).normalized * moveSpeed *Time.deltaTime;

        //맵의 Transform 값을 이용하여 보정
        //Vector3 moveVertical = playerInput.moveVertical * mapTransform.forward * moveSpeed * Time.deltaTime;
        //Vector3 moveHorizontal = playerInput.moveHorizontal * mapTransform.right * moveSpeed * Time.deltaTime;
        //Vector3 moveDistonce = moveVertical + moveHorizontal;


        // 카메라 방향을 기준으로 좌우 를 보정할때
        Vector3 cameraFoward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraFoward.y = 0;
        cameraRight.y = 0;

        Vector3 moveVertical = playerInput.moveVertical * cameraFoward.normalized;
        Vector3 moveHorizontal = playerInput.moveHorizontal* cameraRight.normalized;
        Vector3 moveDistonce = (moveVertical + moveHorizontal).normalized * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistonce);
    }

    
    private void Rotate()
    {
        /*
         * 기본 회전 로직
        // 입력값에 따라 캐릭터를 좌우로 회전
        //상대적으로 회전할 거리 계산
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        //리지드바디를 이용해 게임 오브젝트 회전 변경
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0);
        */

        //float turn = playerInput.rotateMouse * rotateSpeed * Time.deltaTime;
        //playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0);

        //마우스 위치로 플레이어 캐릭터가 방향을 트는 방법

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
        {
            // 광선과 평면의 교차점 계산
            Vector3 target = ray.GetPoint(distance);
            // 교차점에서 플레이어 위치까지의 방향 계산
            Vector3 direction = (target - transform.position).normalized;
            direction.y = 0f; // 평면에서만 회전하도록 y 축 제거

            // 해당 방향을 바라보는 회전값 계산
            Quaternion rotation = Quaternion.LookRotation(direction);
            // 리지드바디의 회전 설정
            playerRigidbody.MoveRotation(rotation);
        }

    }

    //private void RotateToMouseDirection()
    //{
    //    // 마우스 위치를 화면 좌표에서 월드 좌표로 변환
    //    //ScreenPointToRay : Camera 클래스 메서드 마우스 위치를 매개변수로 받아 해당 위치에서 Ray 를 발사하여 월드 좌표 변환으로 사용됨
    //    Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition); // 카메라에서 마우스 위치를 향하는 광선 생성
    //    Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // y축 방향으로 위를 가리키는 평면 생성
    //    float rayLength;

    //    // 광선이 평면과 교차하는지 확인
    //    if (groundPlane.Raycast(cameraRay, out rayLength))
    //    {
    //        // 광선과 평면의 교차점 계산
    //        Vector3 pointToLook = cameraRay.GetPoint(rayLength);
    //        // 교차점에서 플레이어 위치까지의 방향 계산
    //        Vector3 directionToLook = pointToLook - transform.position;
    //        directionToLook.y = 0f; // 평면에서만 회전하도록 y 축 제거

    //        // 해당 방향을 바라보는 회전값 계산
    //        Quaternion newRotation = Quaternion.LookRotation(directionToLook);
    //        // 리지드바디의 회전 설정
    //        playerRigidbody.MoveRotation(newRotation);
    //    }
    //}
}