using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject _gameOverText;
    public Text _timeText;
    public Text _recordText;

    private float _surviveTime;
    private bool _isGameOver;
    // Start is called before the first frame update
    void Start()
    {
        _surviveTime = 0f;
        _isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGameOver)
        {
            _surviveTime += Time.deltaTime;

            _timeText.text = "Time : " + (int)_surviveTime;
        }
        else
        {
            if(Input .GetKeyUp(KeyCode.R)) 
            {
                SceneManager.LoadScene("Dodge");
            }
        }
    }

    public void EndGame()
    {
        _isGameOver = true;
        _gameOverText.SetActive(true);

        float highsocre = PlayerPrefs.GetInt("BestTime");

        if (_surviveTime > highsocre)
        {
            highsocre = _surviveTime;
            PlayerPrefs.SetInt("BestTime",(int)_surviveTime) ;
        }
        _recordText.text = "Best Time : " + (int)highsocre;
    }
}
