using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerResetter : MonoBehaviour
{

    public bool hasResetTime = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !hasResetTime)
        {
            UIManager.Instance.StopTimer();
            CheckpointText text = Instantiate(AssetManager.Instance.CheckPointText, transform.position + new Vector3(0, 1.25f, 0), Quaternion.identity).GetComponent<CheckpointText>();
            text.congrats = true;
            VolatileSound.Create(AssetManager.Instance.WinSound);
            hasResetTime = true;
        }
    }
}
