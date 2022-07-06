using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private bool active;
    private Animator animator;
    private int animLayer = 0;
    [SerializeField] GameObject child;
    [SerializeField] private float duration = 4;

    private void Start()
    {
        animator = GetComponent<Animator>();
        child.SetActive(false);
    }

    private void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !active)
        {
            Active();
            StartCoroutine(SteppedOn());
            StartCoroutine(TurnOn());
        }
    }

    void Active()
    {
        animator.SetTrigger("isActivated");
        animator.SetBool("isOn", true);
        animator.SetBool("isOff", false);
        active = true;
    }
    private IEnumerator SteppedOn()
    {
        yield return new WaitForSecondsRealtime(duration);
        animator.SetBool("isOff", true);
        animator.SetBool("isOn", false);
        active = false;
        child.SetActive(false);
    }
    
    private IEnumerator TurnOn()
    {
        yield return new WaitForSecondsRealtime((float)0.8);
        // After the activation animation is finished...
        if (!isPlaying(animator, "Activation"))
        {
            child.SetActive(true);
        }
    }
    
    bool isPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(animLayer).IsName(stateName) &&
            anim.GetCurrentAnimatorStateInfo(animLayer).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
}
