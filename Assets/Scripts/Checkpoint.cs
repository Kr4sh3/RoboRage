using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public bool isCraftingStation = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isCraftingStation)
                collision.gameObject.GetComponent<PlayerController>().CanCraft = true;
            collision.gameObject.GetComponent<PlayerController>().SetLastCheckpoint(transform);
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
