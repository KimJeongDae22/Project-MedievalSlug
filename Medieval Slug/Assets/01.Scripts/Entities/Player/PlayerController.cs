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
    [SerializeField] private PlayerRangedHandler playerEquip;
    [SerializeField] private PlayerMeleeHandler playerMelee;

    [Header("Mount Settings")]
    [SerializeField] float mountCheckRadius = 1f;
    [SerializeField] LayerMask mountLayer;
    [SerializeField] float mountJumpForce = 4f;

    VehicleController currentVehicle;
    bool isMounted;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool jumpRequest;
    private bool isFacingRight = true;

    void Awake() => rb = GetComponent<Rigidbody2D>();


    public void OnMovement(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        if (isMounted) currentVehicle.Move(moveInput);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started && IsGrounded()) jumpRequest = true;
    }

    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        if (isMounted) currentVehicle.Fire(GetAimDir());
        else
        {
            Vector2 aim = GetAimDir();
            playerEquip.Fire(aim);
        }
    }

    public void OnMount(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;

        if (isMounted)            // F키로 하차
            currentVehicle.Dismount();
        else                      // F키로 탑승
            TryMountNearestTank();
    }

    public void OnMelee(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        if(isMounted)
        {
            currentVehicle.Melee(ctx);
        }
        else
        {
            playerMelee.OnMelee();
        }
    }
    public void SetMountedState(bool mounted, VehicleController tank)
    {
        isMounted = mounted;
        currentVehicle = tank;
        rb.simulated = !mounted;              // 탑승 중엔 Rigidbody2D 정지
        GetComponent<Collider2D>().enabled = !mounted;
        //animator.SetBool("Mounted", mounted);
    }

    void TryMountNearestTank()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, mountCheckRadius, mountLayer);
        if (hit && hit.TryGetComponent(out VehicleController tank))
        {
            // 점프 애니메이션 & 힘
            rb.velocity = new Vector2(rb.velocity.x, mountJumpForce);
            StartCoroutine(MountAfterDelay(tank, 0.25f)); // 살짝 뜀 → 착지 시 탑승
        }
    }

    IEnumerator MountAfterDelay(VehicleController tank, float delay)
    {
        yield return new WaitForSeconds(delay);
        tank.Mount(this);
    }

    Vector2 GetAimDir()
    {
        return moveInput.sqrMagnitude < 0.01f
            ? (isFacingRight ? Vector2.right : Vector2.left)
            : moveInput;
    }


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


    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale; scale.x *= -1; transform.localScale = scale;
    }

    private bool IsGrounded() => Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
}
