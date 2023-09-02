using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Controller controls;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Rigidbody2D rb;
    bool isMoving;

    Vector2 moveDirection;
    bool isDashing;
    bool canDash = true;
    public Animator animator;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] private Transform playerSpriteTransform;



    void Awake()
    {
        controls = new Controller();
        controls.Gameplay.Movement.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        controls.Gameplay.Movement.canceled += ctx => moveDirection = Vector2.zero;

        controls.Gameplay.Dash.performed += ctx => Dash();

        controls.Gameplay.Enable();
    }

    private void Start()
    {
        canDash = true;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }
        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        

        if ((Input.GetKeyDown(KeyCode.Space) || controls.Gameplay.Dash.triggered) && canDash)
        {
            StartCoroutine(Dash());
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
        FlipPlayerSprite(moveX);

        // // Flip character based on movement direction
        // if (moveDirection.x > 0)
        // {
        //     FlipCharacter(false);
        // }
        // else if (moveDirection.x < 0)
        // {
        //     FlipCharacter(true);
        // }
        animator.SetBool("Move", isMoving);
        
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
        }
        else
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
            
            isMoving = rb.velocity.magnitude > 0;
            
        }
        float moveX = controls.Gameplay.Movement.ReadValue<Vector2>().x;

        FlipPlayerSprite(moveX);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void FlipCharacter(bool faceLeft)
    {
        Vector3 newScale = rb.transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * (faceLeft ? -1 : 1);
        rb.transform.localScale = newScale;
    }
    private void FlipPlayerSprite(float moveX)
    {
        Vector3 newScale = playerSpriteTransform.localScale;
        if (moveX > 0)
        {
            newScale.x = Mathf.Abs(newScale.x);
        }
        else if (moveX < 0)
        {
            newScale.x = -Mathf.Abs(newScale.x);
        }
        playerSpriteTransform.localScale = newScale;
    }


    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
