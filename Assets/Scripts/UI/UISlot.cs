using System.Collections;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class UISlot : UIButton
{
    public int? SlotID { get { return _slotID; } }
    [SerializeField] private int? _slotID = null;

    public bool DisplaySlot = false;

    private ref ItemStack ItemSlot { get { return ref _inventory[_slotID.Value]; } }

    private ItemStack[] _inventory;
    private SpriteRenderer _sRenderer;
    private SpriteRenderer _itemRenderer;
    private TextMeshPro _textRenderer;
    private SpriteRenderer _burnableRenderer;

    private void Start()
    {
        _sRenderer = GetComponent<SpriteRenderer>();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("ItemDisplay"))
            {
                _itemRenderer = child.GetComponent<SpriteRenderer>();
            }
            if (child.CompareTag("BurnableDisplay"))
            {
                _burnableRenderer = child.GetComponent<SpriteRenderer>();
            }
        }
        _textRenderer = GetComponentInChildren<TextMeshPro>();
    }

    private void Update()
    {
        if (UIManager.Instance.IsPaused())
            return;

        if (_inventory == null)
        {
            Debug.LogError(gameObject.name + "does not have a connected inventory!");
            gameObject.SetActive(false);
            return;
        }

        if (_slotID != null)
        {
            //Sprite
            if (ItemSlot != null)
                _itemRenderer.sprite = ItemSlot.ItemData.Icon;
            else
                _itemRenderer.sprite = null;

            //Text
            if (ItemSlot != null && ItemSlot.StackSize > 1)
                _textRenderer.text = ItemSlot.StackSize.ToString();
            else
                _textRenderer.text = "";

            if (!DisplaySlot)
            {
                //Burnable Display
                if (ItemSlot != null && ItemSlot.StackSize >= ItemSlot.ItemData.MaxStackSize)
                    _burnableRenderer.enabled = true;
                else
                    _burnableRenderer.enabled = false;
            }
        }

        if (!DisplaySlot)
        {
            //Highlight
            if (UIManager.Instance.SelectedElements.Contains(this))
                _sRenderer.material.shader = AssetManager.Instance.WhiteFlashShader;
            else
                _sRenderer.material.shader = AssetManager.Instance.DefaultSpriteShader;
        }
        else
        {
            _sRenderer.sprite = null;
        }

    }

    public void SetInventory(ItemStack[] inventory)
    {
        _inventory = inventory;
    }

    public void SetSlotID(int id)
    {
        _slotID = id;
    }

    public ref ItemStack GetSlotRef()
    {
        return ref ItemSlot;
    }

    public override void Submit()
    {
        if (ItemSlot != null && InventoryManager.Instance.HeldItem != null
            && ItemSlot.ItemData == InventoryManager.Instance.HeldItem.ItemData
            && ItemSlot.StackSize < ItemSlot.ItemData.MaxStackSize)
            InventoryManager.Instance.AddFromHeldItem(ref ItemSlot);
        else
            InventoryManager.Instance.SwapHeldItem(ref ItemSlot);
    }

    public void Cancel()
    {
        if (ItemSlot != null && InventoryManager.Instance.HeldItem == null)
        {
            InventoryManager.Instance.Split(ref ItemSlot);
            return;
        }
        if (ItemSlot != null && InventoryManager.Instance.HeldItem != null)
        {
            if (ItemSlot.ItemData == InventoryManager.Instance.HeldItem.ItemData)
            {
                InventoryManager.Instance.AddOneFromHeldItem(ref ItemSlot);
                return;
            }
            else
            {
                InventoryManager.Instance.SwapHeldItem(ref ItemSlot);
                return;
            }
        }
        if (ItemSlot == null && InventoryManager.Instance.HeldItem != null)
        {
            InventoryManager.Instance.AddOneFromHeldItem(ref ItemSlot);
            return;
        }
    }


}
