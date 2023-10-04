using UnityEngine;

public class VolatileSound : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource.playOnAwake && _audioSource.isPlaying == false)
            _audioSource.Play();
    }

    private void Update()
    {
        if (_audioSource.isPlaying == false)
            Destroy(gameObject);
    }

    public static GameObject Create(AudioClip clip)
    {
        GameObject volatileSound = Instantiate(AssetManager.Instance.VolatileSoundPrefab);
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
