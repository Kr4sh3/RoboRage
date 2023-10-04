using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public int Size { get { return _size; } }
    [SerializeField] private int _size;

    public ItemStack[] Inventory { get { return _inventory; } }
    [SerializeField] private ItemStack[] _inventory;

    public ItemStack[] CraftingInventory { get { return _craftingInventory; } }
    [SerializeField] private ItemStack[] _craftingInventory;

    public InventoryContainer ConnectedContainer { get { return _connectedContainer; } set { _connectedContainer = value; } }
    private InventoryContainer _connectedContainer;

    private void Start()
    {
        _inventory = new ItemStack[_size];
        _craftingInventory = new ItemStack[20];
    }

    private void Update()
    {
        //Rebuild Inventory
        if (_inventory.Length != _size && _size > 0)
        {
            ItemStack[] newInventory = new ItemStack[_size];
            for (int i = 0; i < _inventory.Length; i++)
            {
                if (i >= _size)
                    InventoryManager.Instance.DropItem(ref _inventory[i]);
                else
                    newInventory[i] = _inventory[i];
            }
            _inventory = newInventory;
            if (_connectedContainer != null)
                _connectedContainer.Init(_inventory);
        }
    }

    public ItemStack GetItemInSlot(int i) { return _inventory[i]; }

    public ItemStack Add(ItemStack item)
    {
        if (IsFull(item))
            return item;

        int amount = item.StackSize;
        //Search for stackables
        for (int i = 0; i < _inventory.Length; i++)
        {
            if (_inventory[i] != null)
            {
                if (_inventory[i].ItemData == item.ItemData && _inventory[i].StackSize < item.ItemData.MaxStackSize)
                {
                    int possibleToAdd = item.ItemData.MaxStackSize - _inventory[i].StackSize;
                    if (amount > possibleToAdd)
                    {
                        _inventory[i].SetStackSize(_inventory[i].StackSize + possibleToAdd);
                        amount -= possibleToAdd;
                    }
                    else
                    {
                        _inventory[i].SetStackSize(_inventory[i].StackSize + amount);
                        return null;
                    }
                }
            }
        }
        item.SetStackSize(amount);
        //Search for empty slots
        for (int i = 0; i < _inventory.Length; i++)
        {
            if (_inventory[i] == null)
            {
                Set(i, item);
                return null;
            }
        }
        return item;

    }

    public void Set(int slot, ItemStack item)
    {
        if (_inventory[slot] != null)
            Clear(slot);
        _inventory[slot] = item;
    }

    public void Clear(int slot)
    {
        if (_inventory[slot] != null)
            _inventory[slot] = null;
    }

    public bool IsFull(ItemStack comparison)
    {
        for (int i = 0; i < _inventory.Length; i++)
        {
            if (_inventory[i] == null)
                return false;
            if (_inventory[i].ItemData == comparison.ItemData && _inventory[i].StackSize < comparison.ItemData.MaxStackSize)
                return false;
        }
        return true;
    }

    public void DumpInventory()
    {
        for (int i = 0; i < _inventory.Length; i++)
        {
            if (_inventory[i] != null)
            {
                InventoryManager.Instance.DropItem(ref _inventory[i]);
            }
        }
        InventoryManager.Instance.DropHeldItem();
        InventoryManager.Instance.DropBurnerItem();
    }

    public void AddNewSlot()
    {
        _size++;
    }
}
