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
    private bool stopTimer = false;
    public bool hasBeatenGame = false;

    private GameObject _playerInventoryContainer;
    private GameObject _burnerInventoryContainer;
    private GameObject _craftingInventoryContainer;
    private GameObject _craftingInterface;

    private void Start()
    {
        maxTime = PlayerPrefs.GetFloat("HighScore");
    }



    public void Update()
    {
        if (!stopTimer)
            timer += Time.deltaTime;
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
