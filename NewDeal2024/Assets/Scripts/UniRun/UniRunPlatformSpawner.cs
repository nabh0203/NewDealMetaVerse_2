using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniRunPlatformSpawner : MonoBehaviour
{
    //생성되는 발판
    public GameObject platformPrefab;
    //3개의 오브젝트로 계속 돌려 쓸거임
    public int count = 3;

    #region 생성주기 변수
    //다음 생성주기까지 시간간격 최솟값
    public float timeBetSpawnMin = 1.25f;
    //다음 생성주기까지 시간간격 최댓값
    public float timeBetSpawnMax = 2.25f;
    //다음 생성주기 시간간격
    private float timeBetSpawn;
    #endregion

    #region 
    //배치 위치의 최소 y값
    public float yMin = -3.5f;
    //배치 위치의 최대 y값
    public float yMax = 1.5f;
    //배치 위치 x값
    private float xPos = 20f;
    #endregion
    //미리생성된 발판
    private GameObject[] platfroms;
    //발판 순번
    private int currentIndex = 0;

    //발판 숨길 위치
    private Vector2 poolPosition = new Vector2(0, -25);
    //마지막 배치 시점
    private float lastSpawnTime;
    
    void Start()
    {
        platfroms = new GameObject[count];

        for(int i = 0; i < count; i++)
        {
            //새로 생성되는 발판을 현재 발판 위치에 생성
            //생성된 발판을 배열에 해당
            platfroms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
        }

        lastSpawnTime = 0f;
        timeBetSpawn = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //게임오버시 실행 x
        if (UniRunManager.instance.isGameover)
        {
            return;
        }
        //만약 마지막 배치 시점에서 timeBetSpawn 이상만큼 시간이 흘렀다면
        if (Time.time >= lastSpawnTime + timeBetSpawn)
        {
            //기록된 마지막 배치 시점을 현재 시점으로 저장
            lastSpawnTime = Time.time;

            //다음 배치까지 시간 간격을 1.25 ~ 2.25 사이값을 출력
            timeBetSpawn = Random.Range(timeBetSpawnMin,timeBetSpawnMax);

            //Y 값을 -3.5 ~ 1.5 로 출력
            float yPos = Random.Range(yMin,yMax);

            platfroms[currentIndex].SetActive(false);
            platfroms[currentIndex].SetActive(true);

            //현재 순번의 발판 오른쪽에 재배치
            platfroms[currentIndex].transform.position = new Vector2(xPos,yPos);

            //발판 순번 넘기기
            currentIndex++;

            //발판순번 초기화
            if (currentIndex >= count)
            {
                currentIndex = 0;
            }

        }
    
    
    }
}
