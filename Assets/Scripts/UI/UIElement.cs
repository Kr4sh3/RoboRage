using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool Hovered { get { return _hovered; } }
    private bool _hovered;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hovered = true;
        UIManager.Instance.SelectElement(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _hovered = false;
        UIManager.Instance.UnselectElement(this);
    }

    protected Vector3 ToVector3(Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 10);
    }
}
