using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Obsolete]
public class UIManager : MonoBehaviour
{
    private void Awake()
    {
        _selectedElements = new List<UIElement>();
    }

    public List<UIElement> SelectedElements { get { return _selectedElements; } }
    private List<UIElement> _selectedElements;

    public UIButton SelectedButton { get { return _selectedButton; } }
    private UIButton _selectedButton;

    private float timer;
    private float maxTime = 0;
    private bool paused = false;
    private bool stopTimer = false;
    public bool hasBeatenGame = false;

    private GameObject _playerInventoryContainer;
    private GameObject _burnerInventoryContainer;
    private GameObject _craftingInventoryContainer;
    private GameObject _craftingInterface;
    public GameObject pauseOverlay;

    private void Start()
    {
        pauseOverlay = GameObject.FindGameObjectWithTag("PauseOverlay");
        SetPaused(paused);
        maxTime = PlayerPrefs.GetFloat("HighScore");
    }



    public void Update()
    {
        if (!stopTimer)
            timer += Time.deltaTime;
        if (!pauseOverlay)
            pauseOverlay = GameObject.FindGameObjectWithTag("PauseOverlay");
        if (!paused && pauseOverlay.activeSelf)
            pauseOverlay.SetActive(false);
    }

    public void Pause()
    {
        paused = !paused;
        SetPaused(paused);
    }

    public bool IsPaused()
    {
        return paused;
    }

    public void SetPaused(bool pause)
    {
        if (!pause)
        {
            Time.timeScale = 1;
            pauseOverlay.SetActive(false);
            paused = false;
        }
        else
        {
            Time.timeScale = 0;
            pauseOverlay.SetActive(true);
            paused = true;
        }
    }

    public void RestartTimer()
    {
        SetPaused(false);
        stopTimer = false;
        timer = 0;
    }
    public void StopTimer()
    {
        stopTimer = true;
        if (timer < maxTime || maxTime == 0)
        {
            maxTime = timer;
            PlayerPrefs.SetFloat("HighScore", maxTime);
            PlayerPrefs.SetInt("HasBeatenGame", 1);
        }

    }

    public float GetTime()
    {
        return Mathf.Round(timer * 100) / 100;
    }

    public float GetMaxTime()
    {
        return Mathf.Round(maxTime * 100) / 100;
    }

    public void Submit()
    {
        foreach (UIElement element in _selectedElements)
        {
            if (element is UIButton)
            {
                element.GetComponent<UIButton>().Submit();
            }
        }

    }

    public void SelectElement(UIElement uiElement)
    {
        if (uiElement is UIButton)
            _selectedButton = (UIButton)uiElement;
        _selectedElements.Add(uiElement);
    }

    public void UnselectAll()
    {
        _selectedElements.Clear();
        _selectedButton = null;
    }

    public void UnselectElement(UIElement uiElement)
    {
        if (uiElement is UIButton)
            _selectedButton = null;
        _selectedElements.Remove(uiElement);
    }
}
