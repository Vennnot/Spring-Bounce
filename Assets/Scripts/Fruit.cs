using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Fruit : MonoBehaviour
{
    protected int delayTime = 5;
    private bool pickedUp;
    private Animator animator;
    protected Player player;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
    }

    protected abstract void PowerUp();

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player") && !pickedUp)
        {
            PickedUp();
            player.OnPowerUpUse += PowerUp;
        }
    }
    
    protected void PickedUp()
    {
        pickedUp = true;
        animator.SetTrigger("isPickedUp");
        StartCoroutine(DestroyPickUp());
        StartCoroutine(DestroySpriteRenderer());
    }

    IEnumerator DestroyPickUp()
    {
        yield return new WaitForSecondsRealtime(delayTime);
        player.OnPowerUpUse -= PowerUp;
        Destroy(gameObject);
    }
    
    IEnumerator DestroySpriteRenderer()
    {
        yield return new WaitForSecondsRealtime((float)0.5);
        Destroy(spriteRenderer);
    }

}
