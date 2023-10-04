using System.Collections;
using UnityEngine;


public class UIButton : UIElement
{
    [SerializeField] GameObject _rightButton, _leftButton, _upButton, _downButton;

    public virtual void Submit()
    {
    }
    public void SelectRight()
    {
        UIManager.Instance.UnselectElement(this);
        UIManager.Instance.SelectElement(_rightButton.GetComponent<UIButton>());
    }
    public void SelectLeft()
    {
        UIManager.Instance.UnselectElement(this);
        UIManager.Instance.SelectElement(_leftButton.GetComponent<UIButton>());
    }
    public void SelectUp()
    {
        UIManager.Instance.UnselectElement(this);
        UIManager.Instance.SelectElement(_upButton.GetComponent<UIButton>());
    }
    public void SelectDown()
    {
        UIManager.Instance.UnselectElement(this);
        UIManager.Instance.SelectElement(_downButton.GetComponent<UIButton>());
    }
}
