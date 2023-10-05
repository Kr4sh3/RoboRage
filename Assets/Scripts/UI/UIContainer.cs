using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

public class UIContainer : UIElement
{
    [SerializeField] private bool _interpolateToOnscreenPoint;

    [SerializeField] [HideIf("_interpolateToOnscreenPoint")] private Vector2 _position;

    [SerializeField] [ShowIf("_interpolateToOnscreenPoint")] private float _lerpSpeed;
    [SerializeField] [ShowIf("_interpolateToOnscreenPoint")] private Vector2 _startPos;
    [SerializeField] [ShowIf("_interpolateToOnscreenPoint")] private Vector2 _endPos;
    private bool _opening;

    public virtual void Start()
    {
        _opening = true;
        if (_interpolateToOnscreenPoint)
            transform.localPosition = _startPos;
        else
            transform.localPosition = _position;
    }

    public virtual void Update()
    {
        if (_interpolateToOnscreenPoint)
        {
            if (_opening)
                transform.localPosition = Vector3.Lerp(transform.localPosition, ToVector3(_endPos), _lerpSpeed * Time.deltaTime);
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, ToVector3(_startPos), _lerpSpeed * Time.deltaTime);
            if (transform.localPosition == ToVector3(_startPos) && !_opening)
                Destroy(gameObject);
        }
        else
        {
            if (!_opening)
                Destroy(gameObject);
        }

    }

    public void Open()
    {
        _opening = true;
    }

    public void Close()
    {
        _opening = false;
    }
}
