using Entities.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// 플레이어 컨트롤 담당 클래스
/// 움직임, 공격을 제어합니다.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] float groundCheckRadius = 1f;
    [SerializeField] LayerMask groundLayer;

    [Header("Animator")]

    [Header("Melee Weapon Setting")]
    [SerializeField] private int meleeDamage = 10;
    [SerializeField] private float meleeRange = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float windupTime = 0.2f;

    [Header("Dependencies")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerEquip playerEquip;
    [SerializeField] private RangeWeaponHandler weaponHandler;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool jumpRequest;
    private bool isFacingRight = true;
    private PlayerStatHandler statHandler;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        statHandler = GetComponent<PlayerStatHandler>();
    }

    void Start()
    {
        weaponHandler = GetComponentInChildren<RangeWeaponHandler>();
    }

    #region InputSystem 바인딩

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
    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Vector2 aim = moveInput;
            if (aim.sqrMagnitude < 0.01f)
                aim = isFacingRight ? Vector2.right : Vector2.left;
            weaponHandler.FireRange(aim);
        }
    }
    #endregion

    void Update()
    {
        // 캐릭터 방향 전환 처리
        float horizontal = moveInput.x;
        if (horizontal > 0 && !isFacingRight) Flip();
        else if (horizontal < 0 && isFacingRight) Flip();
    }

    void FixedUpdate()
    {
        //이동 물리 계산
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

        //점프 실행
        if (jumpRequest)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpRequest = false;
        }

        // 이동 애니메이션 속도 설정
        animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
    }

    /// <summary>
    /// 근접 무기 공격 코루틴
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// 캐릭터 플립
    /// </summary>
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    /// <summary>
    /// 지면에 있는지 검사하는 메소드
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheckPoint.position,
            groundCheckRadius,
            groundLayer
        );
    }
}
