using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UniRunManager : MonoBehaviour
{
    public static UniRunManager instance;
    
    public bool isGameover = false;
    public Text scoreText;
    public GameObject gameoverUI;

    private int score = 0;

    public GameObject[] lifeObject;
    private void Awake()
    {
        if (instance == null) 
        { 
            instance = this;
            //�ϳ��� �������� �������� ������ �����Ҷ� ��ü������ ������ ����־�� �Ҷ� �ı��ǵ��� ����
            //DontDestroyOnLoad(gameObject);
        }
        else 
        { 
            Debug.LogWarning("�ſ� �ΰ� �̻��� ���ӸŴ����� �����մϴ�.");
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if(isGameover && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    public void AddScore(int newScore)
    {
        if (!isGameover) 
        {
            score += newScore;
            scoreText.text = "Score : " + score;
        }
    }

    public void OnPlayerDead()
    {
        isGameover = true;
        gameoverUI.SetActive(true);

    }


    public void RefreshLifeCount (int lifeCount)
    {

        //for (int i = 0; i < lifeObject.Length; i ++)
        //{
        //    if(i < lifeCount)
        //    {
        //        Debug.Log("-1");
        //        lifeObject[i].SetActive(true);
        //    }
        //    else
        //    {
        //        lifeObject[i].SetActive(false);
        //    }
        //}


        for (int i = 0; i < lifeObject.Length; i++)
        {
            if (i >= lifeCount)
            {
                lifeObject[i].SetActive(false);
            }
        }
    }

    public void BonusLifeCount(int lifeCount)
    {
        for (int i = 0; i < lifeObject.Length; i++)
        {
            if (i <= lifeCount)
            {
                lifeObject[i].SetActive(true);
            }
        }
    }
}
