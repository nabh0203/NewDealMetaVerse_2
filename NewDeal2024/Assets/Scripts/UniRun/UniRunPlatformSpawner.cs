using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniRunPlatformSpawner : MonoBehaviour
{
    //�����Ǵ� ����
    public GameObject platformPrefab;
    //3���� ������Ʈ�� ��� ���� ������
    public int count = 3;

    #region �����ֱ� ����
    //���� �����ֱ���� �ð����� �ּڰ�
    public float timeBetSpawnMin = 1.25f;
    //���� �����ֱ���� �ð����� �ִ�
    public float timeBetSpawnMax = 2.25f;
    //���� �����ֱ� �ð�����
    private float timeBetSpawn;
    #endregion

    #region 
    //��ġ ��ġ�� �ּ� y��
    public float yMin = -3.5f;
    //��ġ ��ġ�� �ִ� y��
    public float yMax = 1.5f;
    //��ġ ��ġ x��
    private float xPos = 20f;
    #endregion
    //�̸������� ����
    private GameObject[] platfroms;
    //���� ����
    private int currentIndex = 0;

    //���� ���� ��ġ
    private Vector2 poolPosition = new Vector2(0, -25);
    //������ ��ġ ����
    private float lastSpawnTime;
    
    void Start()
    {
        platfroms = new GameObject[count];

        for(int i = 0; i < count; i++)
        {
            //���� �����Ǵ� ������ ���� ���� ��ġ�� ����
            //������ ������ �迭�� �ش�
            platfroms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
        }

        lastSpawnTime = 0f;
        timeBetSpawn = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //���ӿ����� ���� x
        if (UniRunManager.instance.isGameover)
        {
            return;
        }
        //���� ������ ��ġ �������� timeBetSpawn �̻�ŭ �ð��� �귶�ٸ�
        if (Time.time >= lastSpawnTime + timeBetSpawn)
        {
            //��ϵ� ������ ��ġ ������ ���� �������� ����
            lastSpawnTime = Time.time;

            //���� ��ġ���� �ð� ������ 1.25 ~ 2.25 ���̰��� ���
            timeBetSpawn = Random.Range(timeBetSpawnMin,timeBetSpawnMax);

            //Y ���� -3.5 ~ 1.5 �� ���
            float yPos = Random.Range(yMin,yMax);

            platfroms[currentIndex].SetActive(false);
            platfroms[currentIndex].SetActive(true);

            //���� ������ ���� �����ʿ� ���ġ
            platfroms[currentIndex].transform.position = new Vector2(xPos,yPos);

            //���� ���� �ѱ��
            currentIndex++;

            //���Ǽ��� �ʱ�ȭ
            if (currentIndex >= count)
            {
                currentIndex = 0;
            }

        }
    
    
    }
}
