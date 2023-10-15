using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    private Camera _mainCamera;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private GameObject _reloadWheel;
    private GameObject _ammoCount;
    [SerializeField] private Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _canvas = GetComponentInParent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("AmmoCountUI"))
                _ammoCount = transform.GetChild(i).gameObject;
            else
                _reloadWheel = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.InputManager.PlayerController == null)
            return;

        //Place UI next to player
        _rectTransform.anchoredPosition = FindPos(GameManager.Instance.InputManager.PlayerController.transform.position) + offset;
        GunInfo info = GameManager.Instance.InputManager.PlayerController.GunController.GetGunInfo();
        _reloadWheel.SetActive(info.Reloading);
        _reloadWheel.GetComponent<Image>().fillAmount = info.ReloadPercentage;
        //Ammo Counters
        for (int i = 0; i < _ammoCount.transform.childCount; i++)
        {
            bool usingAmmoCounter = i < info.MaxAmmoCount;
            _ammoCount.transform.GetChild(i).gameObject.SetActive(usingAmmoCounter); //Disable uneeded ammo counters
            if (usingAmmoCounter)
            {
                //Set color of ammo counters
                if (i + 1 <= info.AmmoCount)
                    _ammoCount.transform.GetChild(i).GetComponent<Image>().color = Color.white;
                else
                    _ammoCount.transform.GetChild(i).GetComponent<Image>().color = Color.gray;
            }

        }

    }

    private Vector2 FindPos(Vector3 target)
    {
        Vector3 screenPos = _mainCamera.WorldToScreenPoint(target);
        float h = Screen.height;
        float w = Screen.width;
        float x = screenPos.x - (w / 2);
        float y = screenPos.y - (h / 2);
        float s = _canvas.scaleFactor;
        return new Vector2(x, y) / s;
    }
}
