using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    public float inaccuracy;
    public Quaternion Rotation;
    public LayerMask LayerMask;
    //References
    protected Rigidbody2D rb;
    protected GameObject player;
    public GameObject impact;
    public Vector3 Direction;
    protected Camera cameraMain;
    private MovementController playerMoveController;
    private float time;

    [SerializeField] float squashInterpolation;

    protected virtual void Start()
    {
        //Assign Private Variables
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");
            playerMoveController = player.GetComponent<MovementController>();
            cameraMain = Camera.main;
        }


        //Set Velocity Towards Mouse Pos
        {
            transform.localScale = new Vector3(1, .1f, 1);
            transform.localRotation = Rotation;
            //Add inaccuracy 
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(-inaccuracy, inaccuracy)) + transform.localRotation.eulerAngles);
            //Normalize And Set Speed Of Velocity
            Direction = transform.right * bulletSpeed;
            //Set Velocity
            rb.velocity = Direction;

            //Destroy after 1/5th of a second
            Destroy(gameObject, bulletLife);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<HealthController>() != null && !collision.CompareTag("Player"))
        {
            collision.GetComponent<HealthController>().Damage(bulletDamage);
            Destroy(gameObject);
        }
        else
        if (LayerMask == (LayerMask | (1 << collision.gameObject.layer)) && !collision.CompareTag("Platform"))
        {
            /*
            //Spawn impact FX
            GameObject fx = Instantiate(impact, transform.position, Quaternion.identity);
            //Destroy them after .1 seconds
            Destroy(fx, .1f);
            */
            //Destroy the bullet
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(.2f, .2f, 1f), Time.deltaTime * squashInterpolation);
        if (isGunBullet)
        {
            time += Time.deltaTime * 1000;
            transform.localRotation = Quaternion.Euler(0, 0, time);
        }
    }
}

