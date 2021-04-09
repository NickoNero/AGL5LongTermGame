﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetBullet : MonoBehaviour
{
    private float damage = 1;
    private int reflectionsRemaining = 5;
    public Rigidbody2D rb;
    public Collider2D coll;

    void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        coll = this.gameObject.GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Entity entity = other.gameObject.GetComponent<Entity>();
        if (entity != null)
        {
            entity.TakeDamage(damage, rb.velocity);
            GameObject.Destroy(this.gameObject, 0);
        }
        else
        {
            if (reflectionsRemaining > 0)
            {
                Vector2 normal = other.contacts[0].normal;
                rb.velocity = Vector2.Reflect(rb.velocity, normal);
                --reflectionsRemaining;
            }
            else
            {
                GameObject.Destroy(this.gameObject, 0);
            }
        }
    }
}