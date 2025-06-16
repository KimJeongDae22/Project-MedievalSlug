using Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// 탑승물 컨트롤러
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class VehicleController : MonoBehaviour, IDamagable, IMountalbe
{
    [Header("Movement / Seat")]
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] Transform seatPoint;   // 플레이어 앉는 위치
    [SerializeField] Animator animator;
    [SerializeField]private bool facingRight = true;

    [Header("Combat")]
    [SerializeField]PlayerController rider;
    //[SerializeField] PlayerRangedHandler crossbow;      // 탱크 석궁

    [Header("Stat")]
    [SerializeField] float maxHp = 250;

    [Header("Melee Weapon Setting")]
    [SerializeField] private int meleeDamage = 10;
    [SerializeField] private float meleeRange = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float windupTime = 0.2f;
    [SerializeField] private Vector2 meleeOffset = new Vector2(1.0f, 0.0f);


    private float currentHp;
    Rigidbody2D rb;
    private bool isAttacking;
    public bool jumpRequest;
    Vector2 cachedInput;


    public void ReceiveInput(Vector2 input) => cachedInput = input;

    public void RequestJump() => jumpRequest = true;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHp = maxHp;
    }
    void FixedUpdate()
    {
        // 이동
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
        // 플레이어를 전차 자식으로 두고 위치·회전 고정
        p.transform.SetParent(seatPoint);
        p.transform.localPosition = Vector3.zero;

        Vector3 rs = p.transform.localScale;
        rs.x = Mathf.Abs(rs.x);
        p.transform.localScale = rs;

        p.SetMountedState(true, this);   
        
    }

    /// <summary>
    /// 탑승 해제
    /// </summary>
    /// <param name="exploded"></param>
    public void Dismount(bool exploded = false)
    {

        rider.transform.SetParent(null);
        rider.transform.position = seatPoint.position + Vector3.up * 0.8f; // 전차 위로 점프
        rider.SetMountedState(false, null);
        rider = null;
        //animator.SetBool("Riding", false);

        if (exploded) Destroy(gameObject);
    }
    #endregion

    /*---------------------------------------------------------------------------- */
    #region  Attack (플레이어 입력이 전달됨)


    public void Fire(Vector2 dir) { }//=> crossbow.Fire(dir);
    public void Melee(InputAction.CallbackContext ctx)
    {
        if (!ctx.started || isAttacking) return;
        StartCoroutine(PerformMelee());
    }
    private IEnumerator PerformMelee()
    {
        isAttacking = true;
        animator.SetTrigger("Tank_MeleeAttack");

        yield return new WaitForSeconds(windupTime);


        Vector2 dir = (Vector2)transform.right;          // 지금 바라보는 세계 방향
        Vector2 origin = (Vector2)transform.position
                       + dir * meleeOffset.x                // 앞뒤
                       + Vector2.up * meleeOffset.y;        // 높이


        RaycastHit2D hit = Physics2D.Raycast(origin, dir, meleeRange, enemyLayer);
        if (hit.collider != null && hit.collider.TryGetComponent<IDamagable>(out var target))
            target.TakeDamage(meleeDamage);

        isAttacking = false;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector2 dir = Application.isPlaying ? (Vector2)transform.right
                                               : Vector2.right * Mathf.Sign(transform.lossyScale.x);
        Vector2 origin = (Vector2)transform.position
                       + dir * meleeOffset.x
                       + Vector2.up * meleeOffset.y;

        Gizmos.DrawWireSphere(origin, meleeRange);
        Gizmos.DrawLine(origin, origin + dir * meleeRange);
    }
    #endregion

    /*---------------------------------------------------------------------------- */
    #region IDamagable
    public void TakeDamage(int dmg)
    {
        currentHp -= dmg;
        animator.SetTrigger("Hurt");
        if (currentHp <= 0) Die();
    }
    public void Die()
    {
        Dismount(true);
        animator.SetTrigger("Die");
    }

    public void ApplyEffect(EffectType effectType)
    {
        throw new System.NotImplementedException();
    }


    #endregion

}
