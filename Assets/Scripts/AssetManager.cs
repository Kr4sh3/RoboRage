using UnityEngine;

public class AssetManager : MonoBehaviour
{
    #region Instance
    private static AssetManager s_instance;
    public static AssetManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = Instantiate(Resources.Load<GameObject>("Prefabs/AssetManager")).GetComponent<AssetManager>();
            return s_instance;
        }
    }
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    #region Prefabs
    public GameObject PlayerInventoryContainerPrefab;
    public GameObject VolatileSoundPrefab;
    public GameObject CollectableItemPrefab;
    public GameObject UIManagerPrefab;
    public GameObject InventoryManagerPrefab;
    public GameObject HeldItemRendererPrefab;
    public GameObject BurnerContainerPrefab;
    public GameObject CraftingInventoryPrefab;
    public GameObject CraftingInterfacePrefab;
    public GameObject CheckPointText;
    #endregion

    #region Sounds
    public AudioClip InventoryOpenSound;
    public AudioClip InventoryCloseSound;
    public AudioClip ResourceDestroyedSound;
    public AudioClip ResourceHitSound;
    public AudioClip PickupSound;
    public AudioClip JumpSound;
    public AudioClip ShootSound;
    public AudioClip PlayerDamage;
    public AudioClip Heal;
    public AudioClip CheckpointSound;
    public AudioClip UpgradeSound;
    public AudioClip WinSound;
    #endregion

    //Shaders
    public Shader DefaultSpriteShader;
    public Shader WhiteFlashShader;

    //Sprites
    public Sprite[] AttackSprites;
    public Sprite ArmorPip;
    public Sprite BrokenArmorPip;
    public Sprite[] ArmorBarBackground;
    public Sprite[] UpgradeSprites;
    public Sprite[] HighlightedUpgradeSprites;
    public Sprite[] CheckpointSprites;
    public Sprite[] CongratsSprites;
}
