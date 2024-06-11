using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 좀비 게임 오브젝트를 주기적으로 생성
public class ZombieSpawner : MonoBehaviourPun,IPunObservable {
    public Zombie zombiePrefab; // 생성할 좀비 원본 프리팹

    public ZombieData[] zombieDatas; // 사용할 좀비 셋업 데이터들
    public Transform[] spawnPoints; // 좀비 AI를 소환할 위치들

    private List<Zombie> zombies = new List<Zombie>(); // 생성된 좀비들을 담는 리스트
    private int zombieCount = 0;
    private int wave; // 현재 웨이브


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) 
        {
            //남은 좀비 수를 네트워크를 통해 보내기
            stream.SendNext(zombies.Count);
            //현재 웨이브를 네트워크를 통해 보내기
            stream.SendNext(wave);
        }else
        {
            //리모트 오브젝트라면 읽기 부분이 실행됨
            //남은 좀비 수를 네트워크를 통해 받기
            zombieCount = (int)stream.ReceiveNext();
            //현재 웨이브를 네트워크를 통해 받기
            wave = (int)stream.ReceiveNext();
        }
    }
    private void Awake()
    {
        PhotonPeer.RegisterType(typeof(Color), 128, ColorSerialization.SerializeColor, ColorSerialization.DeserializeColor);
    }
    private void Update() {
        if (PhotonNetwork.IsMasterClient)
        {
            if (GameManager.instance != null && GameManager.instance.isGameover)
            {
                return;
            }

            // 좀비를 모두 물리친 경우 다음 스폰 실행
            if (zombies.Count <= 0)
            {
                SpawnWave();
            }
        }

        // 게임 오버 상태일때는 생성하지 않음
        

        // UI 갱신
        UpdateUI();
    }

    // 웨이브 정보를 UI로 표시
    private void UpdateUI() {
        if (PhotonNetwork.IsMasterClient)
        {
            // 호스트는 직접 갱신한 좀비 리스트를 이용해 남은 좀비 수 표시
            UIManager.instance.UpdateWaveText(wave, zombies.Count);
        }
        else
        {
            //클라이언트는 좀비 리스트를 갱신할 수 없으므로
            //호스트가 보내준 zombirCount를 이용해 좀비 수 표시
            UIManager.instance.UpdateWaveText(wave, zombieCount);
        }
    }

    // 현재 웨이브에 맞춰 좀비들을 생성
    private void SpawnWave() {
        wave++;

        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        for (int i = 0; i < spawnCount; i++)
        {
            CreateZombie();
        }
    }

    // 좀비를 생성하고 생성한 좀비에게 추적할 대상을 할당
    private void CreateZombie()
    {
        ZombieData zombieData = zombieDatas[Random.Range(0, zombieDatas.Length)];   // Default , Heavy, Fast 3가지의 좀비데이터중 하나를 랜덤으로 선택

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];    // 스폰포인트 4개중에서 하나를 랜덤으로 결정해서 스폰위치러 결정

        //Zombie zombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);    // 좀비프리팹으로 해당위치에 좀비 생성
        //좀비 프리펩으로부터 좀비 생성, 네트워크 상의 모든 클라이언트들이 생성
        GameObject createdZombie = PhotonNetwork.Instantiate(zombiePrefab.name, spawnPoint.position, spawnPoint.rotation);
        Zombie zombie = createdZombie.GetComponent<Zombie>();
        zombie.photonView.RPC("SetUp", RpcTarget.All, zombieData.health, zombieData.damage,zombieData.speed, zombieData.skinColor);

        //zombie.Setup(zombieData);       // 생성한 좀비의 초기값 능력치 설정
        

        zombies.Add(zombie);            // 리스트에 새로 생성한 좀비 추가

        zombie.onDeath += () => zombies.Remove(zombie);

        zombie.onDeath += () => StartCoroutine(DestroyAfter(zombie.gameObject, 10f));
        zombie.onDeath += Zombie_onDeathAddScore;
    }
    IEnumerator DestroyAfter(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (target != null)
        {
            PhotonNetwork.Destroy(target);
        }
    }
    private void Zombie_onDeathAddScore()
    {
        GameManager.instance.AddScore(100);
    }

}