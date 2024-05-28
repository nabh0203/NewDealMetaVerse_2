using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniRunPlatform : MonoBehaviour
{
    //0���� 100���� �����߿� Ȯ������ ����
    private const int PERCENTAGE_OBSTACLES_CREATE = 30;
    //�߰� ���ʽ� ������
    private const int PERCENTAGE_BONUSLIFE_CREATE = 100;
    //��ֹ� ������Ʈ 3���� �����ϴ� �迭
    public GameObject[] obstacles;
    //���ʽ� ������ ������Ʈ 3���� �����ϴ� �迭
    public GameObject[] BonusLife;
    //�÷��̾ ��ֹ��� ��Ҵ��� üũ
    public bool isStepped = false;


    public bool isNotStepped = true;

    private void OnEnable()
    {
        //������ �����ϴ� �ڵ�
        //������ ���� �������� ���� ���¸� �ʱ�ȭ
        isStepped = false;
        //������ ���� �ʾҴٴ°� �˼� �ְ����ִ� ����
        isNotStepped = true;

        for (int i = 0;i < obstacles.Length; i++)
        {
            //Random.Range(min,max) �Լ� : min �̻� max �̸� �� ���ڸ� �����ϰ� ��ȯ�Ѵ�.
            //Ǯ�� : 0~100 ������ ������ �����ϰ� ��ȯ�Ҷ� 30���� �۴ٸ� �����ض�
            //30%�� Ȯ���� ��ֹ� ����
            if (Random.Range(0, 100) < PERCENTAGE_OBSTACLES_CREATE)
            {
                obstacles[i].SetActive(true);
            }
            else
            {
                obstacles[i].SetActive(false);
            }
            
        }
        foreach(GameObject Bonus in BonusLife) 
        { 
            Bonus.SetActive(false);
        }

        if (Random.Range(0, 100) < PERCENTAGE_BONUSLIFE_CREATE)
        {
            int index = Random.Range(0,BonusLife.Length);    
            BonusLife[index].SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //�÷��̾� ĳ���Ͱ� �ڽ��� ����� �� �״� ����

        //Player �±׸� ������ �ְ� !isStepped ��� �ڵ带 ���� 

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
