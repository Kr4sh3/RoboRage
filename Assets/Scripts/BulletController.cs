using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private PlayerInputController playerInput;
    private float time;

    protected virtual void Start()
    {
        //Assign Private Variables
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");
            playerInput = player.GetComponent<PlayerInputController>();
            cameraMain = Camera.main;
        }


        //Set Velocity Towards Mouse Pos
        {
            if (playerInput.LookDirection == 1)
                target = new Vector3(0, 1, 0);
            else
            if (playerInput.LookDirection == -1)
                target = new Vector3(0, -1, 0);
            else
            if (playerInput.FacingDirection)
                target = new Vector3(1, 0, 0);
            else
                target = new Vector3(-1, 0, 0);
            //Add Inaccuracy
            if (isGunBullet == false)
            {
                target = target.normalized + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);
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
        if (collision.GetComponent<IDamageable>() != null && !collision.CompareTag("Player"))
        {
            collision.GetComponent<IDamageable>().Damage(bulletDamage);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Solid"))
            Destroy(gameObject);
    }
    private void Update()
    {
        if (isGunBullet)
        {
            time += Time.deltaTime * 1000;
            transform.localRotation = Quaternion.Euler(0, 0, time);
        }
    }
}

