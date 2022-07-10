using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

sealed public class Player : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody2D myRigidBody2D;
    private Animator animator;
    private bool levelEnd;
    
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private AudioClip pauseClip;
    
    [Header("Movement")] [SerializeField] private float runSpeed;

    [SerializeField] private float jumpSpeed;

    [SerializeField] private double coyoteBuffer = 0.2f;

    [SerializeField] private double jumpBuffer = 0.2f;

    [SerializeField] private OptionsMenu optionsMenu;

    private bool wasJustOnGround;

    private bool justPressedJump;
    [Header("Death")]
    [SerializeField] private bool died;
    [SerializeField] Vector2 deathForce = new Vector2(0f, 100f);

    private PauseMenu pauseMenu;
    

    public event Action OnPowerUpUse;

    private void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
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
            GameController.Instance.IncrementDeaths();
            SoundManager.Instance.PlayPlayerSound(deathClip);
            animator.SetTrigger("isHit");
            myRigidBody2D.velocity = new Vector2(0f, 0f);
            StartCoroutine(DelayDeath());
            GetComponent<CapsuleCollider2D>().enabled = false;
            myRigidBody2D.constraints = RigidbodyConstraints2D.None;
            myRigidBody2D.AddTorque(30f);
            myRigidBody2D.AddForce(deathForce*4);
        }
    }

    private void Hurt()
    {
        if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Hazards")) ||
            (transform.position.y < -15))
        {
            if(!died)
            {
                Die();
            }
        }
    }

    IEnumerator DelayDeath()
    {
        yield return new WaitForSeconds((float)0.75);
        GameController.Instance.ResetLevel();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemy") || col.CompareTag("Trap"))
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
    
    // private void OnTriggerStay2D(Collider2D col)
    // {
    //     if(col.CompareTag("Enemy") || col.CompareTag("Trap"))
    //     {
    //         Die();
    //     }
    // }

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
        if(!PauseMenu.GameIsPaused && !optionsMenu.isEnabled)
        {
            SoundManager.Instance.PlayPlayerSound(pauseClip);
            pauseMenu.Pause();
        }
        else if(!optionsMenu.isEnabled)
        {
            SoundManager.Instance.PlayPlayerSound(pauseClip);
            pauseMenu.Resume();
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
                if (wasJustOnGround)
                {
                    SoundManager.Instance.PlayPlayerSound(jumpClip);
                }
            }
        }
    }
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
            // SoundManager.Instance.PlayPlayerSound(walkClip);
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
        // if(SoundManager.Instance.playerSource.)
        // {
        //     SoundManager.Instance.PlayPlayerSound(walkClip);
        // }
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
