using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

[Obsolete]
public class EnemyController : MonoBehaviour
{
    public float LaunchForce;

    public Transform target;
    public bool mindless; //enables just moving back and forth between two points
    public float speed;
    public bool isFlier = false;
    public float nextWaypointDistance = 3;
    [SerializeField] private Vector2 _box1Size;
    [SerializeField] private Vector2 _box1Offset;
    [SerializeField] private Vector2 _box2Size;
    [SerializeField] private Vector2 _box2Offset;
    [SerializeField] private LayerMask _groundLayer;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if (!mindless)
        {
            seeker = GetComponent<Seeker>();
            InvokeRepeating("UpdatePath", 0f, .5f);
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<HealthController>().Health > 1)
            {
                Vector3 direction = collision.transform.position - transform.position; //direction to launch player
                Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direction2D * LaunchForce);
                collision.gameObject.GetComponent<MovementController>().ForceJump(1.5f);
            }
            collision.gameObject.GetComponent<HealthController>().Damage(1);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            speed *= -1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(_box1Offset.x, _box1Offset.y), _box1Size);
        Gizmos.DrawWireCube(transform.position + new Vector3(_box2Offset.x, _box2Offset.y), _box2Size);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mindless)
        {
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
                speed *= -1;
            if (!isFlier)
            {
                if (rb.velocity.x < 0 && !Physics2D.BoxCast(transform.position + new Vector3(_box1Offset.x, _box1Offset.y), _box1Size, 0, -transform.up, 0, _groundLayer))
                    speed *= -1;
                if (rb.velocity.x > 0 && !Physics2D.BoxCast(transform.position + new Vector3(_box2Offset.x, _box2Offset.y), _box2Size, 0, -transform.up, 0, _groundLayer))
                    speed *= -1;
            }
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            if (path == null)
                return;

            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }


    }
}
