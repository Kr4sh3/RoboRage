using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class TimerResetter : MonoBehaviour
{

    public bool hasResetTime = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !hasResetTime)
        {
            GameManager.Instance.UIManager.StopTimer();
            CheckpointText text = Instantiate(GameManager.Instance.AssetManager.CheckPointText, transform.position + new Vector3(0, 1.25f, 0), Quaternion.identity).GetComponent<CheckpointText>();
            text.congrats = true;
            VolatileSound.Create(GameManager.Instance.AssetManager.WinSound);
            hasResetTime = true;
        }
    }
}
