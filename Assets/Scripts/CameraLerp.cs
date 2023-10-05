using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    [SerializeField] private float _lerpMultiplier;
    [SerializeField] private float _cameraHeight;

    public GameObject Target { get { return _target; } set { _target = value; } }
    [SerializeField] private GameObject _target;

    private void Update()
    {
        Vector3 target = Target.transform.position + new Vector3(0, _cameraHeight, 0);
        Vector3 lerpPosition = Vector3.Lerp(transform.position, target, Time.deltaTime * _lerpMultiplier);
        transform.position = new Vector3(lerpPosition.x, lerpPosition.y, -10);
    }
}
