using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = Instantiate(AssetManager.Instance.UIManagerPrefab).GetComponent<UIManager>();
            return s_instance;
        }
    }
    private static UIManager s_instance;

    private bool _inMenu = false, _craftingMenuOpen = false;

    public List<UIElement> SelectedElements { get { return _selectedElements; } }
    private List<UIElement> _selectedElements;

    public UIButton SelectedButton { get { return _selectedButton; } }
    private UIButton _selectedButton;

    private GameObject _playerInventoryContainer;
    private GameObject _burnerInventoryContainer;
    private GameObject _craftingInventoryContainer;
    private GameObject _craftingInterface;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _selectedElements = new List<UIElement>();
    }

    public void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().CanCraft && _inMenu && !_craftingMenuOpen)
            OpenCrafting();
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().CanCraft && _inMenu && _craftingMenuOpen)
            CloseCrafting();
    }

    public void OpenPlayerInventory()
    {
        _inMenu = true;

        //Burner Inventory
        if (_burnerInventoryContainer == null)
        {
            _burnerInventoryContainer = Instantiate(AssetManager.Instance.BurnerContainerPrefab, Camera.main.transform);
            _burnerInventoryContainer.GetComponent<InventoryContainer>().Init(InventoryManager.Instance.BurnerItem);
        }
        else
        {
            _burnerInventoryContainer.GetComponent<UIContainer>().Open();
        }

        //Player Inventory
        if (_playerInventoryContainer == null)
        {
            //Instantiate Inventory Renderer and Connect Player Data Inventory
            _playerInventoryContainer = Instantiate(AssetManager.Instance.PlayerInventoryContainerPrefab, Camera.main.transform);
            _playerInventoryContainer.GetComponent<InventoryContainer>().Init(GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().Inventory);
            GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().ConnectedContainer = _playerInventoryContainer.GetComponent<InventoryContainer>();
        }
        else
            _playerInventoryContainer.GetComponent<UIContainer>().Open();

        //Open Crafting
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().CanCraft)
            OpenCrafting();

        VolatileSound.Create(AssetManager.Instance.InventoryOpenSound);
        UnselectAll();
    }

    public void OpenCrafting()
    {
        _craftingMenuOpen = true;
        if (_craftingInventoryContainer == null)
        {
            _craftingInventoryContainer = Instantiate(AssetManager.Instance.CraftingInventoryPrefab, Camera.main.transform);
            _craftingInventoryContainer.GetComponent<InventoryContainer>().Init(GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>().CraftingInventory);
        }
        else
        {
            _craftingInventoryContainer.GetComponent<UIContainer>().Open();
        }
        if (_craftingInterface == null)
        {
            _craftingInterface = Instantiate(AssetManager.Instance.CraftingInterfacePrefab, Camera.main.transform);
        }
        else
        {
            _craftingInterface.GetComponent<UIContainer>().Open();
        }
    }

    public void CloseCrafting()
    {
        _craftingMenuOpen = false;
        if (_craftingInventoryContainer != null)
            _craftingInventoryContainer.GetComponent<UIContainer>().Close();
        if (_craftingInterface != null)
            _craftingInterface.GetComponent<UIContainer>().Close();
    }

    public void ClosePlayerInventory()
    {
        _inMenu = false;

        if (_playerInventoryContainer == null)
            return;

        _playerInventoryContainer.GetComponent<UIContainer>().Close();
        _burnerInventoryContainer.GetComponent<UIContainer>().Close();
        CloseCrafting();
        InventoryManager.Instance.DropBurnerItem();
        InventoryManager.Instance.DropHeldItem();
        VolatileSound.Create(AssetManager.Instance.InventoryCloseSound);
        UnselectAll();
    }

    public void Submit()
    {
        foreach (UIElement element in _selectedElements)
        {
            if (element is UIButton)
            {
                element.GetComponent<UIButton>().Submit();
            }
        }
        //Drop
        if (_selectedElements.Count == 0 && InventoryManager.Instance.HeldItem != null)
            InventoryManager.Instance.DropHeldItem();

    }

    public void Drop()
    {
        if (InventoryManager.Instance.HeldItem != null)
        {
            InventoryManager.Instance.DropHeldItem();
        }
        else
        {
            if (_selectedButton is UISlot && _selectedButton.GetComponent<UISlot>().GetSlotRef() != null)
                InventoryManager.Instance.DropItem(ref _selectedButton.GetComponent<UISlot>().GetSlotRef());
        }
    }

    public void Cancel()
    {
        foreach (UIElement element in _selectedElements)
        {
            if (element is UISlot)
            {
                element.GetComponent<UISlot>().Cancel();
            }
        }
        //Drop
        if (_selectedElements.Count == 0 && InventoryManager.Instance.HeldItem != null)
            InventoryManager.Instance.DropOneOfHeldItem();
    }

    public void SelectElement(UIElement uiElement)
    {
        if (uiElement is UIButton)
            _selectedButton = (UIButton)uiElement;
        _selectedElements.Add(uiElement);
    }

    public void UnselectAll()
    {
        _selectedElements.Clear();
        _selectedButton = null;
    }

    public void UnselectElement(UIElement uiElement)
    {
        if (uiElement is UIButton)
            _selectedButton = null;
        _selectedElements.Remove(uiElement);
    }
}
