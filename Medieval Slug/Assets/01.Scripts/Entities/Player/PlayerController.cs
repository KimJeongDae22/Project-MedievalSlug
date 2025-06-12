using System.Collections;
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

    [Header("근접 공격 설정")]
    [SerializeField] private int meleeDamage = 10;
    [SerializeField] private float meleeRange = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float windupTime = 0.2f;

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
    public void OnMelee(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            StartCoroutine(PerformMelee());
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

    private IEnumerator PerformMelee()
    {
        // 1 또는 2 중 랜덤으로 선택
        int idx = Random.Range(1, 3); // 1 또는 2
        string triggerName = (idx == 1) ? "MeleeAttack1" : "MeleeAttack2";
        animator.SetTrigger(triggerName);

        yield return new WaitForSeconds(windupTime);

        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector2 origin = (Vector2)transform.position + dir * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, meleeRange, enemyLayer);
        if (hit.collider != null && hit.collider.TryGetComponent<IDamagable>(out var target))
            target.TakeDamage(meleeDamage);
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
