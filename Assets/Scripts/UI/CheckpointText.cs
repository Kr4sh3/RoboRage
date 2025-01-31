using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class CheckpointText : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public float swapSpeed;
    public float lifeTime;
    public float speed;
    private float lifeTimer = 0;
    private float swapTimer = 0;
    private bool swapped = false;

    public bool congrats = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        if (congrats)
            spriteRenderer.sprite = GameManager.Instance.AssetManager.CongratsSprites[0];
        else
            spriteRenderer.sprite = GameManager.Instance.AssetManager.CheckpointSprites[0];

    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(0, speed);

        swapTimer += Time.deltaTime;
        if (swapTimer > swapSpeed)
        {
            swapTimer = 0;
            if (congrats)
            {
                if (swapped)
                    spriteRenderer.sprite = GameManager.Instance.AssetManager.CongratsSprites[0];
                else
                    spriteRenderer.sprite = GameManager.Instance.AssetManager.CongratsSprites[1];
            }
            else
            {
                if (swapped)
                    spriteRenderer.sprite = GameManager.Instance.AssetManager.CheckpointSprites[0];
                else
                    spriteRenderer.sprite = GameManager.Instance.AssetManager.CheckpointSprites[1];
            }
            swapped = !swapped;
        }
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifeTime)
            Destroy(gameObject);
    }
}
