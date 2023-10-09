using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class EnemyHeadDamageable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.GetComponentInParent<HealthController>().DamageNonLethal(1);
            collision.gameObject.GetComponent<SideScrollerMovementController>().ForceJump(1.5f);
            collision.gameObject.GetComponent<SideScrollerMovementController>().SpriteScale();
        }
    }
}
