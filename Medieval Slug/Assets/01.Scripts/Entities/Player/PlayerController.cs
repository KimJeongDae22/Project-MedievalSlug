using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// 플레이어 컨트롤 담당 클래스
/// 움직임, 공격을 제어합니다.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IDamagable
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] float groundCheckRadius = 1f;
    [SerializeField] LayerMask groundLayer;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool jumpRequest;
    private bool isFacingRight = true;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started && IsGrounded())
            jumpRequest = true;
    }

    void Update()
    {
        float h = moveInput.x;
        if (h > 0 && !isFacingRight) Flip();
        else if (h < 0 && isFacingRight) Flip();

        //테스트 코드

        //마우스 왼쪽 클릭 시 데미지
        if (Input.GetMouseButtonDown(0))
        {
            TakeDamage(1);
        }
        //q 입력 시 죽음
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            Die();
        }

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

        if (jumpRequest)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpRequest = false;
        }

        float speed = Mathf.Abs(moveInput.x);  
        animator.SetFloat("Speed", speed);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheckPoint.position,
            groundCheckRadius,
            groundLayer
        );
    }

    //임시 작성, 데미지 처리는 플레이어 스탯 부분에서?
    //이후에 리펙토링으로 분리
    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hurt");
    }

    public void Die()
    {
        animator.SetTrigger("Die");
    }
}
