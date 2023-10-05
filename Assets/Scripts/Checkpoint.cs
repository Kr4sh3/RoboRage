using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using Unity.Mathematics;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public bool isCraftingStation = true;
    public GameObject checkpointText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isCraftingStation)
                collision.gameObject.GetComponent<PlayerController>().CanCraft = true;
            if (collision.gameObject.GetComponent<PlayerController>().lastCheckpoint != transform)
            {
                collision.gameObject.GetComponent<PlayerController>().SetLastCheckpoint(transform);
                if (isCraftingStation)
                {
                    Instantiate(AssetManager.Instance.CheckPointText, transform.position + new Vector3(0, 1.25f, 0), Quaternion.identity);
                    VolatileSound.Create(AssetManager.Instance.CheckpointSound);
                }
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().CanCraft = false;
        }
    }
}
