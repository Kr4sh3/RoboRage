using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
                if (gameManagerObject == null)
                    _instance = Instantiate(Resources.Load<GameObject>("Prefabs/GameManager")).GetComponent<GameManager>();
                else
                    _instance = gameManagerObject.GetComponent<GameManager>();
            }
            return _instance;
        }
    }
    private static GameManager _instance;
    #endregion

    #region Managers
    public AssetManager AssetManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public InputManager InputManager { get; private set; }
    public PlayerStatsManager PlayerStatsManager { get; private set; }
    #endregion
    
    private void Awake()
    {
        //Singleton Setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        //Managers
        AssetManager = GetComponentInChildren<AssetManager>();
        UIManager = GetComponentInChildren<UIManager>();
        InputManager = GetComponentInChildren<InputManager>();
        PlayerStatsManager = GetComponentInChildren<PlayerStatsManager>();
    }
}
