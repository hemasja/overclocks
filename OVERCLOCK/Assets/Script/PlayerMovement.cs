using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Controller controls;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Rigidbody2D rb;

    Vector2 moveDirection;
    bool isDashing;
    bool canDash = true;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCooldown = 1f;

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

        // Flip character based on movement direction
        if (moveDirection.x > 0)
        {
            FlipCharacter(false);
        }
        else if (moveDirection.x < 0)
        {
            FlipCharacter(true);
        }
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
        }
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

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
