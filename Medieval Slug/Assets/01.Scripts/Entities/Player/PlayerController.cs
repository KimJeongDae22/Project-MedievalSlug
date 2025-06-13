using Entities.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// 플레이어 컨트롤 담당 클래스
/// 움직임, 공격을 제어합니다.
/// </summary>
/// <summary>
/// 이동·점프·입력 중계만 담당. 발사는 PlayerEquip에 위임.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Dependencies")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerEquip playerEquip;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool jumpRequest;
    private bool isFacingRight = true;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    /* ---------- InputSystem ---------- */
    public void OnMovement(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started && IsGrounded()) jumpRequest = true;
    }

    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        Vector2 aim = moveInput.sqrMagnitude < 0.01f ? (isFacingRight ? Vector2.right : Vector2.left) : moveInput;
        playerEquip.Fire(aim);
    }

    /* ---------- Unity Loop ---------- */
    void Update()
    {
        float h = moveInput.x;
        if (h > 0 && !isFacingRight) Flip();
        else if (h < 0 && isFacingRight) Flip();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        if (jumpRequest)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpRequest = false;
        }
        animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
    }

    /* ---------- Helper ---------- */
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale; scale.x *= -1; transform.localScale = scale;
    }

    private bool IsGrounded() => Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
}
