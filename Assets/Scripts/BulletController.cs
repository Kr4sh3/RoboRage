using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class BulletController : MonoBehaviour
{
    //Bullet attributes, obtained from gun class
    public float bulletSpeed;
    public float spread;
    public int bulletDamage;
    public float knockbackMultiplier;
    public bool isGunBullet = false;
    public float bulletLife;
    //References
    protected Rigidbody2D rb;
    protected GameObject player;
    public GameObject impact;
    protected Vector3 target;
    protected Camera cameraMain;
    private SideScrollerMovementController playerMoveController;
    private float time;

    [SerializeField] float squashInterpolation;

    protected virtual void Start()
    {
        //Assign Private Variables
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");
            playerMoveController = player.GetComponent<SideScrollerMovementController>();
            cameraMain = Camera.main;
        }


        //Set Velocity Towards Mouse Pos
        {
            if (playerMoveController.LookDirection == 1)
            {
                target = new Vector3(0, 1, 0);
                transform.localScale = new Vector3(.1f, 1, 1);
            }
            else
            if (playerMoveController.LookDirection == -1)
            {
                target = new Vector3(0, -1, 0);
                transform.localScale = new Vector3(.1f, 1, 1);
            }
            else
            if (playerMoveController.FacingDirection)
            {
                target = new Vector3(1, 0, 0);
                transform.localScale = new Vector3(1, .1f, 1);
            }
            else
            {
                target = new Vector3(-1, 0, 0);
                transform.localScale = new Vector3(1, .1f, 1);
            }
            //Add Inaccuracy
            if (isGunBullet == false)
            {
                target = target.normalized + new Vector3(UnityEngine.Random.Range(-spread, spread), UnityEngine.Random.Range(-spread, spread), 0);
            }
            //Normalize And Set Speed Of Velocity
            target = target.normalized * bulletSpeed;
            //Set Velocity
            rb.velocity = target;

            //Destroy after 1/5th of a second
            Destroy(gameObject, bulletLife);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.CompareTag("Wall") || collision.CompareTag("Door") || collision.CompareTag("Obstacle"))
        {
            //Spawn impact FX
            GameObject fx = Instantiate(impact, transform.position, Quaternion.identity);
            //Destroy them after .1 seconds
            Destroy(fx, .1f);
            //Destroy the bullet
            Destroy(gameObject);
        }*/
        if (collision.GetComponent<HealthController>() != null && !collision.CompareTag("Player"))
        {
            collision.GetComponent<HealthController>().Damage(bulletDamage);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Solid"))
            Destroy(gameObject);
    }
    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(.2f,.2f,1f), Time.deltaTime * squashInterpolation);
        if (isGunBullet)
        {
            time += Time.deltaTime * 1000;
            transform.localRotation = Quaternion.Euler(0, 0, time);
        }
    }
}

