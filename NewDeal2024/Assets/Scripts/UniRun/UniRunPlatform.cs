using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniRunPlatform : MonoBehaviour
{
    //0부터 100개의 숫자중에 확률값을 설정
    private const int PERCENTAGE_OBSTACLES_CREATE = 30;
    //장애물 오브젝트 3개를 저장하는 배열
    public GameObject[] obstacles;
    //플레이어가 장애물을 밟았는지 체크
    public bool isStepped = false;


    public bool isNotStepped = true;

    private void OnEnable()
    {
        //발판을 리셋하는 코드
        //발판이 새로 켜질때는 밟은 상태를 초기화
        isStepped = false;
        //발판을 밟지 않았다는걸 알수 있게해주는 변수
        isNotStepped = true;

        for (int i = 0;i < obstacles.Length; i++)
        {
            //Random.Range(min,max) 함수 : min 이상 max 미만 의 숫자를 랜덤하게 반환한다.
            //풀이 : 0~100 사이의 숫자중 랜덤하게 반환할때 30보다 작다면 실행해라
            //30%의 확률로 장애물 등장
            if (Random.Range(0, 100) < PERCENTAGE_OBSTACLES_CREATE)
            {
                obstacles[i].SetActive(true);
            }
            else
            {
                obstacles[i].SetActive(false);
            }
        } 
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //플레이어 캐릭터가 자신을 밟았을 때 죽는 로직

        //Player 태그를 가지고 있고 !isStepped 라면 코드를 실행 

        //if (other.collider.tag == "Player" && isNotStepped == true)
        //if (other.collider.tag == "Player" && !isStepped)
        //if (other.collider.tag == "Player" && isStepped == false)
        if (other.collider.tag == "Player" && isNotStepped)
        {
            isNotStepped = false;
            UniRunManager.instance.AddScore(1);
        }
    }
}
