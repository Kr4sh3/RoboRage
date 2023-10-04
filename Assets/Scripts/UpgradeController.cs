using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public bool devMode = false;
    public ItemStack[] CurrentUpgradePath { get { return currentUpgradePath; } }
    private ItemStack[] currentUpgradePath;
    public List<CraftingRequirement> upgradePath;
    private int currentUpgradePathIndex;
    private ItemStack[] craftingInventory;
    private ItemStack[] inventory;
    public bool PathFinished { get { return pathFinished; } }
    private bool pathFinished = false;


    // Start is called before the first frame update
    void Start()
    {
        if (upgradePath != null && upgradePath.Count > 0)
        {
            currentUpgradePath = CopyPath(upgradePath[0].Items);
            currentUpgradePathIndex = 0;
        }
        craftingInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().CraftingInventory;
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().Inventory;
    }

    private ItemStack[] CopyPath(ItemStack[] original)
    {
        ItemStack[] copy = new ItemStack[original.Length];
        for (int i = 0; i < copy.Length; i++)
        {
            if (original[i] != null)
                copy[i] = new ItemStack(original[i].ItemData, original[i].StackSize);
        }
        return copy;
    }

    public bool CheckIfCanUpgrade()
    {
        if (devMode)
            return true;

        if (pathFinished)
            return false;

        if (craftingInventory == null)
            craftingInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().CraftingInventory;
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().Inventory;
        bool hasAllRequirements = true;
        foreach (ItemStack reqItem in currentUpgradePath)
        {
            if (reqItem.ItemData != null)
            {
                bool hasEnoughOfItem = false;
                ItemStack itemComparison = new ItemStack(reqItem.ItemData, 0);
                foreach (ItemStack craftingInvItem in craftingInventory)
                {
                    if (craftingInvItem != null && craftingInvItem.ItemData == reqItem.ItemData)
                        itemComparison.SetStackSize(itemComparison.StackSize + craftingInvItem.StackSize);
                }
                foreach (ItemStack invItem in inventory)
                {
                    if (invItem != null && invItem.ItemData == reqItem.ItemData)
                        itemComparison.SetStackSize(itemComparison.StackSize + invItem.StackSize);
                }
                if (itemComparison.StackSize >= reqItem.StackSize)
                    hasEnoughOfItem = true;
                if (!hasEnoughOfItem)
                    hasAllRequirements = false;
            }
        }
        return hasAllRequirements;
    }

    public void Upgrade()
    {
        if (!CheckIfCanUpgrade())
            return;

        if (!devMode)
        {
            //Remove Items
            foreach (ItemStack reqItem in currentUpgradePath)
            {
                if (reqItem.ItemData != null)
                {
                    foreach (ItemStack craftingInvItem in craftingInventory)
                    {
                        if (craftingInvItem != null && craftingInvItem.ItemData == reqItem.ItemData)
                        {
                            while (reqItem.StackSize > 0 && craftingInvItem.StackSize > 0)
                            {
                                reqItem.SetStackSize(reqItem.StackSize - 1);
                                craftingInvItem.SetStackSize(craftingInvItem.StackSize - 1);
                            }
                        }
                    }
                    foreach (ItemStack invItem in inventory)
                    {
                        if (invItem != null && invItem.ItemData == reqItem.ItemData)
                        {
                            while (reqItem.StackSize > 0 && invItem.StackSize > 0)
                            {
                                reqItem.SetStackSize(reqItem.StackSize - 1);
                                invItem.SetStackSize(invItem.StackSize - 1);
                            }
                        }
                    }
                }
            }

            //Fix zeroed items
            for (int i = 0; i < craftingInventory.Length; i++)
            {
                if (craftingInventory[i] != null && craftingInventory[i].StackSize <= 0)
                    craftingInventory[i] = null;
            }
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != null && inventory[i].StackSize <= 0)
                    inventory[i] = null;
            }
            for (int i = 0; i < currentUpgradePath.Length; i++)
            {
                if (currentUpgradePath[i] != null && currentUpgradePath[i].StackSize <= 0)
                    currentUpgradePath[i] = null;
            }
        }


        //Do Upgrade Code
        switch (gameObject.tag)
        {
            case "InventoryUpgrade":
                switch (currentUpgradePathIndex)
                {
                    case 0:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().AddNewSlot();
                        break;
                    case 1:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().AddNewSlot();
                        break;
                    case 2:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().AddNewSlot();
                        break;
                    case 3:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().AddNewSlot();
                        break;
                    case 4:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().AddNewSlot();
                        break;
                    case 5:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().AddNewSlot();
                        break;
                    case 6:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().AddNewSlot();
                        break;
                    case 7:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().AddNewSlot();
                        break;
                    case 8:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().AddNewSlot();
                        break;
                }
                break;
            case "HealthUpgrade":
                switch (currentUpgradePathIndex)
                {
                    case 0:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>().AddMaxHealth();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Heal(5);
                        break;
                    case 1:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>().AddMaxHealth();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Heal(5);
                        break;
                }
                break;
            case "SpeedUpgrade":
                switch (currentUpgradePathIndex)
                {
                    case 0:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<SideScrollerMovementController>().AddMoveSpeed();
                        break;
                    case 1:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<SideScrollerMovementController>().AddMoveSpeed();
                        break;
                    case 2:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<SideScrollerMovementController>().AddMoveSpeed();
                        break;
                }
                break;
            case "PowerUpgrade":
                switch (currentUpgradePathIndex)
                {
                    case 0:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>().AddPower(1);
                        GameObject.FindGameObjectWithTag("Player").GetComponent<SideScrollerMovementController>().AddRecoil();
                        break;
                    case 1:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>().AddPower(2);
                        GameObject.FindGameObjectWithTag("Player").GetComponent<SideScrollerMovementController>().AddRecoil();
                        break;
                    case 2:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>().AddPower(2);
                        GameObject.FindGameObjectWithTag("Player").GetComponent<SideScrollerMovementController>().AddRecoil();
                        break;
                    case 3:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>().AddPower(2);
                        GameObject.FindGameObjectWithTag("Player").GetComponent<SideScrollerMovementController>().AddRecoil();
                        break;
                }
                break;
            case "FireRateUpgrade":
                switch (currentUpgradePathIndex)
                {
                    case 0:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>().AddFireRate();
                        break;
                    case 1:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>().AddFireRate();
                        break;
                    case 2:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>().AddFireRate();
                        break;
                }
                break;
            case "RangeUpgrade":
                switch (currentUpgradePathIndex)
                {
                    case 0:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>().SetRange(25, .2f);
                        break;
                    case 1:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>().SetRange(30, .225f);
                        break;
                    case 2:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>().SetRange(50, .25f);
                        break;
                }
                break;
        }

        //Set next path
        currentUpgradePathIndex++;
        if (currentUpgradePathIndex + 1 > upgradePath.Count)
            pathFinished = true;
        else
        {
            currentUpgradePath = CopyPath(upgradePath[currentUpgradePathIndex].Items);
        }
    }
}
