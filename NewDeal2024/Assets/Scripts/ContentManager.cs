using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContentManager : MonoBehaviour
{
    public string sceneName;
    
    public void SceneChange()
    {
        Debug.Log("�� �̵�");
        SceneManager.LoadScene(sceneName);
    }

    public void GoTitle()
    {
        Debug.Log("�� �̵�");
        SceneManager.LoadScene(sceneName);
    }
}
