using System;
using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using Unity.Mathematics;
using UnityEngine;

[Obsolete]
public class Checkpoint : MonoBehaviour
{
    public GameObject checkpointText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().LastCheckpoint != transform)
            {
                collision.gameObject.GetComponent<PlayerController>().SetLastCheckpoint(transform);
                Instantiate(GameManager.Instance.AssetManager.CheckPointText, transform.position + new Vector3(0, 1.25f, 0), Quaternion.identity);
                VolatileSound.Create(GameManager.Instance.AssetManager.CheckpointSound);
            }

        }
    }
}
