using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeldItemRenderer : UIElement
{
    public static HeldItemRenderer Instance
    {
        get
        {
            if (_instance != null)
                return _instance;
            _instance = Instantiate(AssetManager.Instance.HeldItemRendererPrefab).GetComponent<HeldItemRenderer>();
            return _instance;
        }
    }
    private static HeldItemRenderer _instance;

    private Vector3 _target;
    private ref ItemStack HeldItemRef { get { return ref InventoryManager.Instance.GetHeldItemRef(); } }
    private SpriteRenderer _sRenderer;
    private TextMeshPro _textRenderer;

    public void Start()
    {
        HeldItemRef = InventoryManager.Instance.GetHeldItemRef();
        _sRenderer = GetComponent<SpriteRenderer>();
        _textRenderer = GetComponentInChildren<TextMeshPro>();
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }

    public void Update()
    {
        if (HeldItemRef == null)
        {
            _sRenderer.sprite = null;
            _textRenderer.text = "";
        }
        else
        {
            transform.position = _target;
            _sRenderer.sprite = HeldItemRef.ItemData.Icon;
            if (HeldItemRef.StackSize > 1)
                _textRenderer.text = HeldItemRef.StackSize.ToString();
            else
                _textRenderer.text = "";
        }

    }
}
