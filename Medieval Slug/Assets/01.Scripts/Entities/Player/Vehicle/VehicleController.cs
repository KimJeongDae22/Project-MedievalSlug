using Entities.Player;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// 탑승물 컨트롤러
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class VehicleController : MonoBehaviour, IDamagable, IMountalbe
{
    [Header("[Movement / Seat]")]
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] Transform seatPoint;   // 플레이어 앉는 위치
    [SerializeField] private bool facingRight = true;

    [Header("[Reference]")]
    [SerializeField] private PlayerController rider;
    [SerializeField] private RangeWeaponHandler crossbow;      // 석궁
    [SerializeField] private VehicleItemCollector collector;
    [SerializeField] private MountIndicater indicator;

    [Header("[Animations]")]
    [SerializeField] Animator animator;
    [SerializeField] private Animator MeleeFx;
    [SerializeField] private Animator MeleeFx2;
    [SerializeField] private Animator MoveFx;
 
    
    [Header("[Stat]")]
    [SerializeField] float maxHp = 250; //HP를 별도의 필드로 정의
    public float MaxHp => maxHp;

    [SerializeField] private float currentHp;

    [Header("[Melee Weapon Setting]")]
    [SerializeField] private int meleeDamage = 10;
    [SerializeField] private float meleeRange = 1f;
    [SerializeField] private float meleeWidth;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float windupTime = 0.5f;
    [SerializeField] private Vector2 meleeOffset = new Vector2(1.0f, 0.0f);

    [Header("[Ground Check & WallCheck]")]
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] float groundRadius = 0.18f;
    [SerializeField] float wallRadius = 0.15f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;

    [SerializeField] string layerPlayer = "Player";
    [SerializeField] string layerVehicle = "Vehicle";
    [SerializeField] VehicleItemCollector vehicleItemCollector;

    //유틸
    Rigidbody2D rb;
    private bool isAttacking;
    public bool jumpRequest;
    Vector2 cachedInput;
    int playerLayer;   // 런타임에 미리 계산
    int vehicleLayer;

    // 원거리 무기 관련 필드
    private ProjectileData currentArrowData;
    private int currentAmmo = 1;
    private float nextFireTime = 0f;
    private bool isBursting;

    public void ReceiveInput(Vector2 input, float ctx)
    {
        cachedInput = input;
        var c= Mathf.Abs(ctx);
        MoveFx.SetFloat("IsMoving", c);
    }

    public void RequestJump() => jumpRequest = true;
    public bool IsGrounded() =>
    Physics2D.OverlapCircle(groundCheckPoint.position,groundRadius, groundLayer);

    bool IsAgainstWall(int sign) =>
        Physics2D.OverlapCircle(wallCheckPoint.position + Vector3.right * wallRadius * sign, wallRadius, wallLayer);

    public float VehicleHP() => currentHp;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        playerLayer = LayerMask.NameToLayer(layerPlayer);
        vehicleLayer = LayerMask.NameToLayer(layerVehicle);
        vehicleItemCollector.gameObject.SetActive(false);
        currentHp = maxHp;
        currentArrowData = crossbow.projectileData;
        indicator.Show(true);
    }
    void FixedUpdate()
    {
        int sign = cachedInput.x > 0 ? 1 : cachedInput.x < 0 ? -1 : 0;

        
        if (!IsGrounded() && sign != 0 && IsAgainstWall(sign))
            rb.velocity = new Vector2(0, rb.velocity.y);
        else
            rb.velocity = new Vector2(cachedInput.x * moveSpeed, rb.velocity.y);

        // 점프
        if (jumpRequest)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpRequest = false;
        }

        // 좌우 플립
        if (cachedInput.x > 0 && !facingRight) Flip();
        else if (cachedInput.x < 0 && facingRight) Flip();

    }
    private void Update()
    {
        //테스트 코드
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(1);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(Exploeded());
        }
    }
    void Flip()
    {
        facingRight = !facingRight;

        Vector3 s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }

    /*---------------------------------------------------------------------------- */
    #region IMountable
    public bool IsMounted => rider != null;

    /// <summary>
    /// 탑승
    /// </summary>
    /// <param name="p"></param>
    public void Mount(PlayerController p)
    {
        rider = p;
        crossbow.Setting(gameObject);
        collector.Setup();
        // 플레이어를 전차 자식으로 두고 위치·회전 고정
        p.transform.SetParent(seatPoint);
        p.transform.localPosition = Vector3.zero;

        Vector3 rs = p.transform.localScale;
        rs.x = Mathf.Abs(rs.x);
        p.transform.localScale = rs;

        SetLayerRecursively(transform, playerLayer);
        p.SetMountedState(true, this);
        indicator.Show(false);
        vehicleItemCollector.gameObject.SetActive(true);
        UIManager.Instance.UIUpdate_TankUI();
    }

    /// <summary>
    /// 탑승 해제
    /// </summary>
    /// <param name="exploded"></param>
    public void Dismount(bool exploded = false)
    {
        rider.transform.SetParent(null);
        if (exploded)
        {
            rider.transform.position = seatPoint.position + Vector3.up * 5f; // 전차 위로 점프
            //플레이어에게 데미지를 입히기
        }
        else
        {
            rider.transform.position = seatPoint.position + Vector3.up * 0.8f; // 전차 위로 점프
        }
        rider.SetMountedState(false, null);
        rider = null;
        indicator.Show(true);
        SetLayerRecursively(transform, vehicleLayer);
        collector.ResetSetup();
        vehicleItemCollector.gameObject.SetActive(false);
        UIManager.Instance.UIUpdate_TankUI();
    }
    #endregion

    /*---------------------------------------------------------------------------- */
    #region  Attack (플레이어 입력이 전달됨)

    /// <summary>
    /// 전차의 석궁 발사
    /// </summary>
    /// <param name="aimDir"></param>
    public void Fire(Vector2 aimDir)
    {
        if (aimDir.sqrMagnitude < 0.01f)
            aimDir = facingRight ? Vector2.right : Vector2.left;

        //발사 시퀀스 쿨타임 검사 
        float interval = (currentArrowData.AttackSpeed > 0f)
                       ? 1f / currentArrowData.AttackSpeed
                       : 0;
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + interval;
        if (!isBursting) StartCoroutine(FireBurst(aimDir));
    }

    IEnumerator FireBurst(Vector2 dir)
    {
        isBursting = true;
        crossbow.animator.SetTrigger("Attack");
        
        int burst = Mathf.Max(1, currentArrowData.ProjecileCount);
        for (int i = 0; i < burst && currentAmmo > 0; i++)
        {
            crossbow.Fire(dir);        // 실제 투사체 생성

            /* 연사 간 미세 지연 */
            if (i < burst - 1)
                yield return new WaitForSeconds(0.11f);
        }
        isBursting = false;
    }

    /// <summary>
    /// 근접 공격
    /// </summary>
    /// <param name="ctx"></param>
    public void Melee(InputAction.CallbackContext ctx)
    {
        if (!ctx.started || isAttacking) return;
        StartCoroutine(PerformMelee());
    }
    private IEnumerator PerformMelee()
    {
        isAttacking = true;
        animator.SetTrigger("Tank_MeleeAttack");
        StartMeleeFx("Attack");
        yield return new WaitForSeconds(windupTime);

        float sign = Mathf.Sign(transform.lossyScale.x);     // +1 오른쪽, -1 왼쪽
        Vector2 dir = Vector2.right * sign;
        Vector2 center = (Vector2)transform.position           // 전차 기준
                        + dir * meleeOffset.x                   // 앞뒤 오프셋
                        + Vector2.up * meleeOffset.y;           // 높이
        Vector2 size = new Vector2(meleeRange, meleeWidth);  // ★ 가로·세로
        float angle = 0f;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(center, size, angle, dir, 0f, enemyLayer);
        foreach (var h in hits)
            if (h.collider.TryGetComponent<IDamagable>(out var target))
                target.TakeDamage(meleeDamage);

        DebugDrawBoxCast(center, size, angle, Color.red, 0.2f); // 개발용 시각화
        isAttacking = false;
    }
    void DebugDrawBoxCast(Vector2 center, Vector2 size, float angle, Color col, float dur)
    {
        Vector2 half = size * 0.5f;

        // Vector3 배열로 선언해 타입 충돌 원천 차단
        Vector3[] p = new Vector3[4] {
        new Vector3(-half.x, -half.y, 0),
        new Vector3(-half.x,  half.y, 0),
        new Vector3( half.x,  half.y, 0),
        new Vector3( half.x, -half.y, 0)
    };

        Quaternion rot = Quaternion.Euler(0, 0, angle);
        Vector3 center3 = new Vector3(center.x, center.y, 0);

        for (int i = 0; i < 4; i++)
            p[i] = center3 + rot * p[i];

        Debug.DrawLine(p[0], p[1], col, dur);
        Debug.DrawLine(p[1], p[2], col, dur);
        Debug.DrawLine(p[2], p[3], col, dur);
        Debug.DrawLine(p[3], p[0], col, dur);
    }

    private void StartMeleeFx(string name)
    {
        MeleeFx.SetTrigger(name);
        MeleeFx2.SetTrigger(name);
    }
    #endregion

    /*---------------------------------------------------------------------------- */
    #region IDamagable
    public void TakeDamage(int dmg)
    {
        currentHp -= dmg;
        animator.SetTrigger("Hurt");
        if (currentHp <= 0) StartCoroutine(Exploeded());
        UIManager.Instance.UIUpdate_TankUI();
    }

    public IEnumerator Exploeded()
    {
        crossbow.gameObject.SetActive(false);
        Dismount(true);
        animator.SetTrigger("IsExploded");
        yield return new WaitForSeconds(0.5f);
        Die();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    public void Die()
    {
        animator.SetTrigger("Die");
    }

    public void ApplyEffect(EffectType effectType)
    {

    }
    void SetLayerRecursively(Transform root, int layer)
    {
        root.gameObject.layer = layer;

        foreach (Transform child in root)
            SetLayerRecursively(child, layer);
    }

    #endregion

}
