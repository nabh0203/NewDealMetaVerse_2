using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleManager : MonoBehaviour
{
    //TextMeshPro를 사용하여 변수를 지정할때 타입을 TextMeshProUGUI 로 지정한다.
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI noticeText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) { StartGame(); }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Dodge");
    }
}
