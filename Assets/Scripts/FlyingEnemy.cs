using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy, IEnemy
{

    [SerializeField] private Vector2 flyForce = new Vector2(10f,0f);
    [SerializeField] private float flyDuration = 5f;
    private bool coroutineStarted;
    [SerializeField] private AudioClip clip;
    public void Bonked()
    {
        SoundManager.Instance.PlaySound(clip);
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
        if(!coroutineStarted){
            coroutineStarted = true;
            StartCoroutine(Fly());
        }
    }
    IEnumerator Fly()
    {
        yield return new WaitForSecondsRealtime(flyDuration);
        transform.localScale = new Vector2(-transform.localScale.x, 1);
        myRigidBody2D.velocity = new Vector2(0,0);
        flyForce = new Vector2(-flyForce.x, 0f);
        coroutineStarted = false;
    }

}
