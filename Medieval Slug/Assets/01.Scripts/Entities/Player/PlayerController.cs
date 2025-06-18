using Entities.Player;
using System.Collections;
using Unity.VisualScripting;
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
    public float a;
    [Header("[Movement Settings]")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private bool isFacingRight = true;

    [Header("[Ground Check & WallCheck]")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask VehicleLayer;
    [SerializeField] float wallRayRadius = 0.15f;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] LayerMask wallLayer;

    [Header("[Dependencies]")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerRangedHandler playerRanged;
    [SerializeField] private PlayerMeleeHandler playerMelee;

    [Header("[Mount Settings]")]
    [SerializeField] float mountCheckRadius = 1f;
    [SerializeField] LayerMask mountLayer;
    [SerializeField] float mountJumpForce = 4f;
    [SerializeField] VehicleController currentVehicle;
    public VehicleController CurrentVehicle => currentVehicle;

    bool isMounted;
    public bool IsMounted => isMounted;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool jumpRequest;

    int cachedSignBeforeMount = 1;   // 탑승 직전 부호
    bool blockSelfFlip = false; // 탑승 중엔 Update → Flip 금지

    void Awake() => rb = GetComponent<Rigidbody2D>();


    public void OnMovement(InputAction.CallbackContext ctx)
    {
        if (CharacterManager.Instance.StatHandler.IsDied) return;

        moveInput = ctx.ReadValue<Vector2>();
        if (isMounted) currentVehicle.ReceiveInput(moveInput, moveInput.x);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (CharacterManager.Instance.StatHandler.IsDied) return;
        if (!ctx.started) return;

        if (isMounted && currentVehicle.IsGrounded()) currentVehicle.RequestJump();
        else if (IsGrounded() || IsOnVehicle()) jumpRequest = true;
    }

    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (CharacterManager.Instance.StatHandler.IsDied) return;
        if (!ctx.started) return;

        if (isMounted) currentVehicle.Fire(GetAimDir());
        else
        {
            playerRanged.Fire(GetAimDir());
        }
    }

    public void OnMount(InputAction.CallbackContext ctx)
    {
        if (CharacterManager.Instance.StatHandler.IsDied) return;
        if (!ctx.started) return;

        if (isMounted)
        {
            // F키로 하차
            currentVehicle.Dismount();
        }
        else
        {
            // F키로 탑승
            TryMountNearestTank();
        }
    }

    public void OnMelee(InputAction.CallbackContext ctx)
    {
        if (CharacterManager.Instance.StatHandler.IsDied) return;
        if (!ctx.started) return;

        if (isMounted)
        {
            currentVehicle.Melee(ctx);
        }
        else
        {
            playerMelee.OnMelee();
        }
    }
    public void SetMountedState(bool mounted, VehicleController vehicle)
    {
        if (CharacterManager.Instance.StatHandler.IsDied) return;

        if (mounted)
        {
            cachedSignBeforeMount = transform.localScale.x >= 0 ? 1 : -1;
            blockSelfFlip = true;
        }
        else
        {
            blockSelfFlip = false;
            SetFacing(cachedSignBeforeMount > 0);
            playerRanged.SetWeaponEnabled(true);
        }

        isMounted = mounted;
        currentVehicle = vehicle;
        rb.simulated = !mounted;
        GetComponent<Collider2D>().enabled = !mounted;

    }

    void TryMountNearestTank()
    {
        if (CharacterManager.Instance.StatHandler.IsDied) return;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, mountCheckRadius, mountLayer);
        if (hit && hit.TryGetComponent(out VehicleController vehicle))
        {
            // 점프 애니메이션 & 힘
            rb.velocity = new Vector2(rb.velocity.x, mountJumpForce);
            playerRanged.SetWeaponEnabled(false);
            StartCoroutine(MountAfterDelay(vehicle, 0.25f)); // 살짝 뜀 → 착지 시 탑승
        }
        else 
        {
            Debug.Log("탈 것이 없습니다.");
            return;
        } 
            
    }

    IEnumerator MountAfterDelay(VehicleController vehicle, float delay)
    {
        yield return new WaitForSeconds(delay);
        vehicle.Mount(this);
        UIManager.Instance.UIUpdate_TankUI();
    }

    Vector2 GetAimDir()
    {
        // 방향키를 조금이라도 움직였으면 항상 입력값을 사용
        if (moveInput.sqrMagnitude >= 0.01f)
            return moveInput.normalized;

        // 탑승 중이면 전차 기준
        if (isMounted && currentVehicle != null)
            return currentVehicle.transform.localScale.x >= 0
                   ? Vector2.right
                   : Vector2.left;

        // 평상시엔 플레이어 본인 기준
        return isFacingRight ? Vector2.right : Vector2.left;
    }


    void Update()
    {
        if (isMounted) return;          // 탑승 중엔 방향 제어 안 함

        // 기존 이동 방향 체크 → Flip 로직은 그대로
        float h = moveInput.x;
        if (h > 0 && !isFacingRight) SetFacing(true);
        else if (h < 0 && isFacingRight) SetFacing(false);
    }

    void FixedUpdate()
    {
        int sign = moveInput.x > 0 ? 1 : moveInput.x < 0 ? -1 : 0;

        // 벽에 붙었고 공중이면 X속도 강제로 0
        if (!IsGrounded() && sign != 0 && IsAgainstWall(sign))
            rb.velocity = new Vector2(0, rb.velocity.y);
        else
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        if (jumpRequest)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpRequest = false;
        }

        if (CharacterManager.Instance.StatHandler.IsDied)
        {
            rb.velocity = Vector2.zero;
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
    }
    public void SetFacing(bool right)
    {
        isFacingRight = right;
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (right ? 1 : -1);
        transform.localScale = s;
    }

    private bool IsGrounded()
    {   
        return Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);  
    }

    private bool IsOnVehicle()
    {
        return Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, VehicleLayer);
    }

    bool IsAgainstWall(int sign)
    {
            return Physics2D.OverlapCircle(wallCheckPoint.position + Vector3.right * wallRayRadius *sign, wallRayRadius, wallLayer);
    }
}
