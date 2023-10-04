using UnityEngine;

public class InventoryContainer : UIContainer
{
    private UISlot[] _slots;
    private ItemStack[] _inventory;

    public override void Start()
    {
        base.Start();
        if (_inventory == null)
            Debug.LogError(gameObject + "Has no attached inventory! Did you forget to Initialize It?");
    }

    public void Init(ItemStack[] inventory)
    {
        _inventory = inventory;

        if (inventory.Length <= 0)
            return;

        _slots = new UISlot[transform.childCount];
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            //Disable Unneccesary Slots
            if (i > inventory.Length - 1)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(true);
                //Set Local Array References
                _slots[i] = transform.GetChild(i).GetComponent<UISlot>();
                //Set slot id if not set
                if (_slots[i].SlotID == null)
                    _slots[i].SetSlotID(i);
                if (_inventory != null)
                    _slots[i].SetInventory(_inventory);
            }
        }
    }
}
