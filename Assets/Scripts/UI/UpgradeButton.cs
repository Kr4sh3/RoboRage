using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : UIButton
{
    public string upgradePathName;
    private UpgradeController correspondingController;
    private InventoryContainer displayContainer;
    private SpriteRenderer _sRenderer;

    private void Start()
    {
        correspondingController = GameObject.FindGameObjectWithTag(upgradePathName).GetComponent<UpgradeController>();
        displayContainer = GetComponentInChildren<InventoryContainer>();
        displayContainer.Init(correspondingController.CurrentUpgradePath);
        _sRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (correspondingController.CheckIfCanUpgrade())
        {
            //Highlight
            if (UIManager.Instance.SelectedElements.Contains(this))
            {
                switch (upgradePathName)
                {
                    case "InventoryUpgrade":
                        _sRenderer.sprite = AssetManager.Instance.HighlightedUpgradeSprites[0];
                        break;
                    case "HealthUpgrade":
                        _sRenderer.sprite = AssetManager.Instance.HighlightedUpgradeSprites[1];
                        break;
                    case "SpeedUpgrade":
                        _sRenderer.sprite = AssetManager.Instance.HighlightedUpgradeSprites[2];
                        break;
                    case "PowerUpgrade":
                        _sRenderer.sprite = AssetManager.Instance.HighlightedUpgradeSprites[3];
                        break;
                    case "FireRateUpgrade":
                        _sRenderer.sprite = AssetManager.Instance.HighlightedUpgradeSprites[4];
                        break;
                    case "RangeUpgrade":
                        _sRenderer.sprite = AssetManager.Instance.HighlightedUpgradeSprites[5];
                        break;
                }
            }
            else
            {
                switch (upgradePathName)
                {
                    case "InventoryUpgrade":
                        _sRenderer.sprite = AssetManager.Instance.UpgradeSprites[0];
                        break;
                    case "HealthUpgrade":
                        _sRenderer.sprite = AssetManager.Instance.UpgradeSprites[1];
                        break;
                    case "SpeedUpgrade":
                        _sRenderer.sprite = AssetManager.Instance.UpgradeSprites[2];
                        break;
                    case "PowerUpgrade":
                        _sRenderer.sprite = AssetManager.Instance.UpgradeSprites[3];
                        break;
                    case "FireRateUpgrade":
                        _sRenderer.sprite = AssetManager.Instance.UpgradeSprites[4];
                        break;
                    case "RangeUpgrade":
                        _sRenderer.sprite = AssetManager.Instance.UpgradeSprites[5];
                        break;
                }
            }

            _sRenderer.color = Color.white;
        }
        else
        {
            switch (upgradePathName)
            {
                case "InventoryUpgrade":
                    _sRenderer.sprite = AssetManager.Instance.UpgradeSprites[0];
                    break;
                case "HealthUpgrade":
                    _sRenderer.sprite = AssetManager.Instance.UpgradeSprites[1];
                    break;
                case "SpeedUpgrade":
                    _sRenderer.sprite = AssetManager.Instance.UpgradeSprites[2];
                    break;
                case "PowerUpgrade":
                    _sRenderer.sprite = AssetManager.Instance.UpgradeSprites[3];
                    break;
                case "FireRateUpgrade":
                    _sRenderer.sprite = AssetManager.Instance.UpgradeSprites[4];
                    break;
                case "RangeUpgrade":
                    _sRenderer.sprite = AssetManager.Instance.UpgradeSprites[5];
                    break;
            }

            _sRenderer.color = Color.gray;
        }
    }

    public override void Submit()
    {
        correspondingController.Upgrade();
        if (correspondingController.PathFinished)
            displayContainer.gameObject.SetActive(false);
        else
            displayContainer.Init(correspondingController.CurrentUpgradePath);
    }
}
