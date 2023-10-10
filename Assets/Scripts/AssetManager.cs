using UnityEngine;

public class AssetManager : MonoBehaviour
{
    #region Prefabs
    public GameObject VolatileSoundPrefab;
    public GameObject CheckPointText;
    #endregion

    #region Sounds
    public AudioClip ResourceDestroyedSound;
    public AudioClip ResourceHitSound;
    public AudioClip JumpSound;
    public AudioClip ShootSound;
    public AudioClip PlayerDamage;
    public AudioClip Heal;
    public AudioClip CheckpointSound;
    public AudioClip WinSound;
    public AudioClip ReloadSound;
    #endregion

    //Shaders
    public Shader DefaultSpriteShader;
    public Shader WhiteFlashShader;

    //Sprites
    public Sprite ArmorPip;
    public Sprite BrokenArmorPip;
    public Sprite[] ArmorBarBackground;
    public Sprite[] CheckpointSprites;
    public Sprite[] CongratsSprites;
}
