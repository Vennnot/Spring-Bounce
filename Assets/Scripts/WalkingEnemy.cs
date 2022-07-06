using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy, IEnemy
{
    [SerializeField] Vector2 walkForce = new Vector2(-15f, 0f);
    [SerializeField] private float walkDuration = 2f;
    [SerializeField] private bool readyToWalk = true;
    [SerializeField] private float walkWait = 1f;
    [SerializeField] private bool coroutineStarted;
    public void Bonked()
    {
        StopAllCoroutines();
        GetComponent<BoxCollider2D>().enabled = false;
        animator.SetTrigger("isBonked");
        myRigidBody2D.constraints = RigidbodyConstraints2D.None;
        myRigidBody2D.AddTorque(7f);
        myRigidBody2D.AddForce(bonkForce);
        GetComponent<CapsuleCollider2D>().enabled = false;
        Debug.Log("Disabled Collider");
        GetComponentInChildren<BoxCollider2D>().enabled = false;
    }

    protected override void Movement()
    {
        if(readyToWalk)
        {
            myRigidBody2D.AddForce(walkForce * Time.deltaTime);
            if (!coroutineStarted)
            {
                animator.SetBool("isMoving", true);
                StartCoroutine(WalkDuration());
                coroutineStarted = true;
            }
        }
    }

    IEnumerator WaitToWalk()
    {
        yield return new WaitForSecondsRealtime(walkWait);
        transform.localScale = new Vector2(-transform.localScale.x, 1);
        walkForce = new Vector2(-walkForce.x, 0f);
        readyToWalk = true;
        animator.SetBool("isMoving", true);
        coroutineStarted = false;
    }

    IEnumerator WalkDuration()
    {
        yield return new WaitForSecondsRealtime(walkDuration);
        readyToWalk = false;
        animator.SetBool("isMoving", false);
        myRigidBody2D.velocity = Vector2.zero;
        StartCoroutine(WaitToWalk());
    }
}
