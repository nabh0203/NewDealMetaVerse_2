using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleManager : MonoBehaviour
{
    //TextMeshPro�� ����Ͽ� ������ �����Ҷ� Ÿ���� TextMeshProUGUI �� �����Ѵ�.
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
