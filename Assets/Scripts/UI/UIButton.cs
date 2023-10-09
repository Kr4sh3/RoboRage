using System;
using System.Collections;
using UnityEngine;

[Obsolete]
public class UIButton : UIElement
{
    [SerializeField] GameObject _rightButton, _leftButton, _upButton, _downButton;

    public virtual void Submit()
    {
    }
    public void SelectRight()
    {
        GameManager.Instance.UIManager.UnselectElement(this);
        GameManager.Instance.UIManager.SelectElement(_rightButton.GetComponent<UIButton>());
    }
    public void SelectLeft()
    {
        GameManager.Instance.UIManager.UnselectElement(this);
        GameManager.Instance.UIManager.SelectElement(_leftButton.GetComponent<UIButton>());
    }
    public void SelectUp()
    {
        GameManager.Instance.UIManager.UnselectElement(this);
        GameManager.Instance.UIManager.SelectElement(_upButton.GetComponent<UIButton>());
    }
    public void SelectDown()
    {
        GameManager.Instance.UIManager.UnselectElement(this);
        GameManager.Instance.UIManager.SelectElement(_downButton.GetComponent<UIButton>());
    }
}
