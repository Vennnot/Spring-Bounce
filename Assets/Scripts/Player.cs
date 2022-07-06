using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody2D myRigidBody2D;
    private Animator animator;
    private bool levelEnd;
    
    [Header("Movement")] [SerializeField] private float runSpeed;

    [SerializeField] private float jumpSpeed;

    [SerializeField] private double coyoteBuffer = 0.2f;

    [SerializeField] private double jumpBuffer = 0.2f;

    private bool wasJustOnGround;

    private bool justPressedJump;
    [Header("Death")]
    [SerializeField] private bool died;
    [SerializeField] Vector2 deathForce = new Vector2(0f, 100f);

    //[SerializeField] private PauseMenu pauseMenu;
    

    public event Action OnPowerUpUse;

    private void Start()
    {
        animator = GetComponent<Animator>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!PauseMenu.GameIsPaused)
        {
            OnGround();
            Jump();
            Run();
            isIdle();
            isFalling();
            FlipSprite();
            Hurt();
        }
    }

    private void Die()
    {
        died = true;
        if(!levelEnd)
        {
            animator.SetTrigger("isHit");
            myRigidBody2D.velocity = new Vector2(0f, 0f);
            StartCoroutine(DelayDeath());
            GetComponent<CapsuleCollider2D>().enabled = false;
            myRigidBody2D.constraints = RigidbodyConstraints2D.None;
            myRigidBody2D.AddTorque(7f);
            myRigidBody2D.AddForce(deathForce);
        }
    }

    private void Hurt()
    {
        if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Hazards")) ||
            (transform.position.y < -15))
        {
            Die();
        }
    }

    IEnumerator DelayDeath()
    {
        yield return new WaitForSeconds((float)0.5);
        FindObjectOfType<GameController>().ResetLevel();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemy"))
        {
            Die();
        }
        else if(col.CompareTag("Bounce"))
        {
            justPressedJump = true;
            StartCoroutine(JumpBufferTimer());
            wasJustOnGround = true;
            Jump();
            col.gameObject.GetComponentInParent<IEnemy>().Bonked();
        }
        else if(col.CompareTag("Finish"))
        {
            levelEnd = true;
            myRigidBody2D.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy") && !died)
        {
            Die();
        }
    }

    void OnFire(InputValue value)
    {
        if(!PauseMenu.GameIsPaused)
        {
            OnPowerUpUse?.Invoke();
        }
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    void OnReset(InputValue value)
    {
        FindObjectOfType<GameController>().ResetLevel();
    }
    void OnPause(InputValue value)
    {
        if(!PauseMenu.GameIsPaused)
        {
            PauseMenu.GameIsPaused = true;
        }
        else
        {
            PauseMenu.GameIsPaused = false;
        }
    }
    void OnJump(InputValue value)
    {
        if(!PauseMenu.GameIsPaused)
        {
            if (value.isPressed)
            {
                justPressedJump = true;
                StartCoroutine(JumpBufferTimer());
            }
        }
    }
//||
    void Jump()
    {
         if(justPressedJump && wasJustOnGround)
        {
            animator.SetTrigger("isJumping");
            myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, 0);
            myRigidBody2D.velocity += new Vector2(0f, jumpSpeed);
            wasJustOnGround = false;
        }
    }
    
    private void OnGround()
    {
        if(!wasJustOnGround)
        {
            if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                wasJustOnGround = true;
                StartCoroutine(CoyoteTimer());
            }
        }
    }

    IEnumerator CoyoteTimer()
    {
        yield return new WaitForSeconds((float)coyoteBuffer);
        wasJustOnGround = false;
    }
    
    IEnumerator JumpBufferTimer()
    {
        yield return new WaitForSeconds((float)jumpBuffer);
        justPressedJump = false;
    }
    private void FlipSprite()
    {
        if(IsMovingHorizontally())
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody2D.velocity.x), 1);
        }
    }
    void Run()
    {
        if(!died)
        {
            // Adds speed to the horizontal velocity
            myRigidBody2D.velocity = new Vector2(moveInput.x * runSpeed, myRigidBody2D.velocity.y);
        }
        if (IsMovingHorizontally())
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    bool IsMovingHorizontally()
    {
        return Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;
    }
    
    bool IsMovingVertically()
    {
        return (Mathf.Abs(myRigidBody2D.velocity.y) > Mathf.Epsilon) && GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    void isFalling()
    {
        if ((myRigidBody2D.velocity.y < Mathf.Epsilon) && !(GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground"))))
        {
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }
    }

    bool isIdle()
    {
        if (!(IsMovingHorizontally() && IsMovingVertically()) && GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            animator.SetBool("isIdling", true);
        }
        else
        {
            animator.SetBool("isIdling", false);
        }

        return (!(IsMovingHorizontally() && IsMovingVertically()) &&
                GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")));
    }
}
