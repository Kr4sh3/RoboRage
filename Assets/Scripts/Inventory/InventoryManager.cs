using System.Collections;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    #region Instance
    private static InventoryManager s_instance;
    public static InventoryManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = Instantiate(AssetManager.Instance.InventoryManagerPrefab).GetComponent<InventoryManager>();
            return s_instance;
        }
    }
    #endregion

    public ItemStack HeldItem { get { return _heldItem; } }
    private ItemStack _heldItem;

    public ItemStack[] BurnerItem { get { return _burnerItem; } }
    private ItemStack[] _burnerItem = new ItemStack[1];

    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (_burnerItem[0] != null && _burnerItem[0].StackSize >= _burnerItem[0].ItemData.MaxStackSize)
        {
            //Burn item and heal
            _burnerItem[0] = null;
            _playerTransform.GetComponent<PlayerController>().Heal(1);
        }
    }

    private void DropItem(ItemStack item)
    {
        GameObject collectableItem = CollectableItem.Create(item);
        collectableItem.transform.position = _playerTransform.position + new Vector3(0, .1f, 0);
        bool facing = _playerTransform.GetComponent<SideScrollerMovementController>().FacingDirection;
        //Set Direction of velocity of dropped item
        if (facing)
            collectableItem.GetComponent<Rigidbody2D>().velocity = new Vector2(10, 0);
        else
            collectableItem.GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 0);
        collectableItem.GetComponent<CollectableItem>().DroppedBy(_playerTransform.gameObject);
    }

    public ref ItemStack GetHeldItemRef()
    {
        return ref _heldItem;
    }

    public void Split(ref ItemStack item)
    {
        if (_heldItem == null)
        {
            int newStackSize = item.StackSize / 2;
            _heldItem = new ItemStack(item.ItemData, item.StackSize - newStackSize);
            item.SetStackSize(newStackSize);
            if (item.StackSize == 0)
                item = null;
        }
    }

    public void AddOneFromHeldItem(ref ItemStack item)
    {
        if (item == null)
        {
            item = new ItemStack(_heldItem.ItemData, 1);
            _heldItem.SetStackSize(_heldItem.StackSize - 1);
        }
        else
        {
            if (_heldItem.ItemData == item.ItemData)
            {
                int possibleToAdd = item.ItemData.MaxStackSize - item.StackSize;
                if (possibleToAdd > 0)
                {
                    item.SetStackSize(item.StackSize + 1);
                    _heldItem.SetStackSize(_heldItem.StackSize - 1);
                }
            }
        }
        if (_heldItem.StackSize == 0)
            _heldItem = null;
    }

    public void AddFromHeldItem(ref ItemStack item)
    {
        int possibleToAdd = item.ItemData.MaxStackSize - item.StackSize;
        while (possibleToAdd > 0 && _heldItem != null && _heldItem.StackSize > 0)
        {
            AddOneFromHeldItem(ref item);
            possibleToAdd = item.ItemData.MaxStackSize - item.StackSize;
        }
    }

    public void SwapHeldItem(ref ItemStack item)
    {
        ItemStack copy;
        if (item != null)
            copy = new ItemStack(item.ItemData, item.StackSize);
        else
            copy = null;

        item = _heldItem;
        _heldItem = copy;
    }

    public void DropItem(ref ItemStack item)
    {
        if (item == null)
            return;
        DropItem(item);
        item = null;
    }

    public void DropHeldItem()
    {
        if (_heldItem != null)
            DropItem(_heldItem);
        _heldItem = null;
    }

    public void DropBurnerItem()
    {
        if (_burnerItem[0] != null)
            DropItem(_burnerItem[0]);
        _burnerItem[0] = null;
    }

    public void DropOneOfHeldItem()
    {
        if (_heldItem != null)
        {
            ItemStack newItem = new ItemStack(_heldItem.ItemData, 1);
            DropItem(newItem);
            _heldItem.SetStackSize(_heldItem.StackSize - 1);
            if (_heldItem.StackSize == 0)
                _heldItem = null;
        }
    }
}
