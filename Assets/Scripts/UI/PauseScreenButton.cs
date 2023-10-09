using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Obsolete]
public class PauseScreenButton : MonoBehaviour
{
    public void QuitGame(){
        Application.Quit();
    }

    public void RestartGame(){
        SceneManager.LoadScene(1);
        GameManager.Instance.UIManager.RestartTimer();
    }

    public void Play(){
        GameManager.Instance.UIManager.Pause();
    }
}
