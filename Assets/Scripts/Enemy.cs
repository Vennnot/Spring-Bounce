using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Vector2 bonkForce = new Vector2(0, 100f);
    protected Vector2 gravity = new Vector2(0, -5f);
    protected bool bonked;
    protected Rigidbody2D myRigidBody2D;
    protected Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Despawn();
        Movement();
    }

    protected virtual void Movement()
    {
        return;
    }
    protected void Despawn()
    {
        if(transform.position.y < -15)
        {
            Destroy(gameObject);
        }
    }
}
