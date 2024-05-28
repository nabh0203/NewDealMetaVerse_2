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
            //하나의 콘텐츠에 여러개의 씬으로 동작할때 전체씬에서 무조건 살아있어야 할때 파괴되도록 선언
            //DontDestroyOnLoad(gameObject);
        }
        else 
        { 
            Debug.LogWarning("신에 두개 이상의 게임매니저가 존재합니다.");
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
