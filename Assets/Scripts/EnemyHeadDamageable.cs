using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeadDamageable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.GetComponentInParent<IDamageable>().DamageNonLethal(1);
            collision.gameObject.GetComponent<SideScrollerMovementController>().ForceJump(1.5f);
        }
    }
}
