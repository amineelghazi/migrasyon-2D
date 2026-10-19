using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.05f;

    [Header("Animator State Names (must match the boxes in the Animator window)")]
    [SerializeField] private string idleState = "Idle";
    [SerializeField] private string walkState = "Walk";
    [SerializeField] private string runState = "Run";
    [SerializeField] private string jumpState = "Jump";
    [SerializeField] private string defenseState = "Defense";
    [SerializeField] private string attackState = "Attack";

    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;

    private float moveInput;
    private bool isRunning;
    private bool isDefending;
    private bool isGrounded;
    private bool jumpPressed;
    private string currentState = "";
    private float attackUntil;

    public bool IsGrounded => isGrounded;
    public bool IsAttacking => Time.time < attackUntil;
    public bool IsDefending => isDefending;

    public void PlayAttack(float duration)
    {
        attackUntil = Time.time + duration;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnDisable()
    {
        currentState = "";
        moveInput = 0f;
        jumpPressed = false;
        if (rb != null) rb.linearVelocity = Vector2.zero;
        if (anim != null) anim.Play(idleState);
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        isDefending = kb.fKey.isPressed && isGrounded && !IsAttacking;

        moveInput = 0f;
        if (!isDefending && !IsAttacking)
        {
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) moveInput -= 1f;
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) moveInput += 1f;

            if (kb.spaceKey.wasPressedThisFrame || kb.wKey.wasPressedThisFrame)
                jumpPressed = true;
        }

        isRunning = moveInput != 0f && kb.leftShiftKey.isPressed;

        if (moveInput != 0f)
        {
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveInput);
            transform.localScale = scale;
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (anim == null) return;

        string next;
        if (IsAttacking) next = attackState;
        else if (!isGrounded) next = jumpState;
        else if (isDefending) next = defenseState;
        else if (moveInput != 0f) next = isRunning ? runState : walkState;
        else next = idleState;

        if (next == currentState) return;

        if (!anim.HasState(0, Animator.StringToHash(next)))
        {
            Debug.LogWarning($"Animator has no state named '{next}'. Check the name in the Animator window.");
            return;
        }

        anim.Play(next);
        currentState = next;
    }

    private void FixedUpdate()
    {
        Bounds b = col.bounds;
        isGrounded = Physics2D.BoxCast(
            b.center,
            new Vector2(b.size.x * 0.9f, b.size.y),
            0f,
            Vector2.down,
            groundCheckDistance,
            groundLayer).collider != null;

        float speed = isRunning ? runSpeed : walkSpeed;
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (jumpPressed && isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        jumpPressed = false;
    }
}