using UnityEngine;

public class ItemStack
{

    public ItemData ItemData { get { return _item; } }
    [SerializeField] private ItemData _item;

    public int StackSize { get { return _stackSize; } }
    [SerializeField] private int _stackSize;

    public ItemStack(ItemData item)
    {
        _item = item;
        _stackSize = 1;
    }

    public ItemStack(ItemData item, int stackSize)
    {
        _item = item;
        _stackSize = stackSize;
    }

    public ItemStack(ItemStack stack)
    {
        _item = stack.ItemData;
        _stackSize = stack.StackSize;
    }

    public void SetStackSize(int stackSize)
    {
        _stackSize = stackSize;
    }

    public override string ToString() { return ItemData.DisplayName + " " + StackSize; }
}
