﻿using System.Collections.Generic;
using UnityEngine;

// 좀비 게임 오브젝트를 주기적으로 생성
public class ZombieSpawner : MonoBehaviour {
    public Zombie zombiePrefab; // 생성할 좀비 원본 프리팹

    public ZombieData[] zombieDatas; // 사용할 좀비 셋업 데이터들
    public Transform[] spawnPoints; // 좀비 AI를 소환할 위치들

    private List<Zombie> zombies = new List<Zombie>(); // 생성된 좀비들을 담는 리스트
    private int wave; // 현재 웨이브

    private void Update() {
        // 게임 오버 상태일때는 생성하지 않음
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        // 좀비를 모두 물리친 경우 다음 스폰 실행
        if (zombies.Count <= 0)
        {
            SpawnWave();
        }

        // UI 갱신
        UpdateUI();
    }

    // 웨이브 정보를 UI로 표시
    private void UpdateUI() {
        // 현재 웨이브와 남은 적 수 표시
        UIManager.instance.UpdateWaveText(wave, zombies.Count);
    }

    // 현재 웨이브에 맞춰 좀비들을 생성
    private void SpawnWave() {
        //웨이브 +1
        wave++;

        //현재 웨이브 *1.5 을 반올림한 수만큼 좀비 생성
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        //spawnCount만큼 좀비 생성
        for (int i = 0; i < spawnCount; i++)
        {
            CreateZombie();
        }
    }

    // 좀비를 생성하고 생성한 좀비에게 추적할 대상을 할당
    private void CreateZombie() {
        //사용되는 좀비 데이터랜덤 지정
        ZombieData zombieData = zombieDatas[Random.Range(0, zombieDatas.Length)];
        //좀비 생성 위치 랜덤 지정
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        //좀비 복제
        Zombie zombie = Instantiate(zombiePrefab, spawnPoint.position , spawnPoint.rotation);
        //생성한 좀비의 능력치 설정
        zombie.Setup(zombieData);
        //생성된 좀비 추가
        zombies.Add(zombie);

        //람다식을 사용한 익명 함수
        //zombie.onDeath 이벤트 안에서 일회용 메서드를 구현한뒤 실행 도중에 미리 정의되지 않은 메서드를 오브젝트 찍어내듯이 사용할수 있다.
        zombie.onDeath += () => zombies.Remove(zombie);
        zombie.onDeath += () => Destroy(zombie.gameObject, 10f);
        zombie.onDeath += () => GameManager.instance.AddScore(100);
        
    
    }
}