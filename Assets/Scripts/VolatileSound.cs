using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VolatileSound : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
    }

    private void Update()
    {
        if (_audioSource.isPlaying == false)
            Destroy(gameObject);
    }

    public static GameObject Create(AudioClip clip)
    {
        GameObject volatileSound = Instantiate(GameManager.Instance.AssetManager.VolatileSoundPrefab);
        volatileSound.GetComponent<AudioSource>().clip = clip;
        return volatileSound;
    }

    public static GameObject Create(AudioClip clip, Vector3 position)
    {
        GameObject volatileSound = Create(clip);
        volatileSound.transform.position = position;
        return volatileSound;
    }
}
