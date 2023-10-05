using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenButton : MonoBehaviour
{
    public void QuitGame(){
        Application.Quit();
    }

    public void RestartGame(){
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>().DestroyControls();
        SceneManager.LoadScene(0);
        UIManager.Instance.RestartTimer();
        UIManager.Instance.ResetScene();
    }

    public void Play(){
        UIManager.Instance.Pause();
    }
}
