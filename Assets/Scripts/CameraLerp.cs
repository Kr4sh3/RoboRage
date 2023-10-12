using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    [SerializeField] private Vector2 _cameraBoundaryMin;
    [SerializeField] private Vector2 _cameraBoundaryMax;
    [SerializeField] private float _lerpMultiplier;
    [SerializeField] private float _cameraHeight;

    public GameObject Target { get { return _target; } set { _target = value; } }
    [SerializeField] private GameObject _target;

    private void Update()
    {
        Vector3 target = Target.transform.position + new Vector3(0, _cameraHeight, 0);
        target = new Vector3(Mathf.Clamp(target.x, _cameraBoundaryMin.x, _cameraBoundaryMax.x), Mathf.Clamp(target.y, _cameraBoundaryMin.y, _cameraBoundaryMax.y), 0);
        Vector3 lerpPosition = Vector3.Lerp(transform.position, target, Time.deltaTime * _lerpMultiplier);
        transform.position = new Vector3(lerpPosition.x, lerpPosition.y, -10);
    }
}
