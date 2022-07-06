using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy, IEnemy
{

    [SerializeField] private Vector2 flyForce = new Vector2(10f,0f);
    [SerializeField] private float flyDuration = 5f;
    public void Bonked()
    {
        StopAllCoroutines();
        GetComponent<BoxCollider2D>().enabled = false;
        animator.SetTrigger("isBonked");
        myRigidBody2D.gravityScale = 1;
        myRigidBody2D.constraints = RigidbodyConstraints2D.None;
        myRigidBody2D.AddTorque(7f);
        myRigidBody2D.AddForce(bonkForce);
        GetComponentInChildren<BoxCollider2D>().enabled = false;
    }
    
    protected override void Movement()
    {
        myRigidBody2D.AddForce(flyForce * Time.deltaTime);
        StartCoroutine(Fly());
    }
    IEnumerator Fly()
    {
        yield return new WaitForSecondsRealtime(flyDuration);
        transform.localScale = new Vector2(-transform.localScale.x, 1);
        flyForce = new Vector2(-flyForce.x, 0f);
    }

}
