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
    [SerializeField] Transform seatPoint;   // 플레이어 앉는 위치
    [SerializeField] Animator animator;

    [Header("Combat")]
    //[SerializeField] PlayerRangedHandler crossbow;      // 탱크 석궁

    [Header("Stat")]
    [SerializeField] int maxHp = 250;

    [Header("Melee Weapon Setting")]
    [SerializeField] private int meleeDamage = 10;
    [SerializeField] private float meleeRange = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float windupTime = 0.2f;


    int currentHp;
    Rigidbody2D rb;
    PlayerController rider;
    private bool isAttacking;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHp = maxHp;
    }

    #region IMountable
    public bool IsMounted => rider != null;

    public void Mount(PlayerController p)
    {
        rider = p;
        // 플레이어를 전차 자식으로 두고 위치·회전 고정
        p.transform.SetParent(seatPoint);
        p.transform.localPosition = Vector3.zero;
        p.SetMountedState(true, this);   // ↓ 3) 참고
        animator.SetBool("Riding", true);
    }

    public void Dismount(bool exploded = false)
    {
        if (rider == null) return;
        rider.transform.SetParent(null);
        rider.transform.position = seatPoint.position + Vector3.up * 0.8f; // 전차 위로 점프
        rider.SetMountedState(false, null);
        rider = null;
        animator.SetBool("Riding", false);

        if (exploded) Destroy(gameObject);
    }
    #endregion

    #region Movement & Attack (플레이어 입력이 전달됨)
    public void Move(Vector2 input)
    {
        rb.velocity = new Vector2(input.x * moveSpeed, rb.velocity.y);
        if (input.x != 0) transform.localScale = new Vector3(Mathf.Sign(input.x), 1, 1);
        animator.SetFloat("Speed", Mathf.Abs(input.x));
    }

    public void Fire(Vector2 dir) { }//=> crossbow.Fire(dir);
    public void Melee(InputAction.CallbackContext ctx)
    {
        if (!ctx.started || isAttacking) return;
        StartCoroutine(PerformMelee());
    }
    private IEnumerator PerformMelee()
    {
        isAttacking = true;
        // 1 또는 2 중 랜덤으로 선택
        int idx = Random.Range(1, 3); // 1 또는 2
        string triggerName =  "Tank_Attack";
        animator.SetTrigger(triggerName);

        yield return new WaitForSeconds(windupTime);

        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector2 origin = (Vector2)transform.position + dir * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, meleeRange, enemyLayer);
        if (hit.collider != null && hit.collider.TryGetComponent<IDamagable>(out var target))
            target.TakeDamage(meleeDamage);
    }
    #endregion

    #region IDamagable
    public void TakeDamage(int dmg)
    {
        currentHp -= dmg;
        animator.SetTrigger("Hurt");
        if (currentHp <= 0) Die();
    }
    public void Die() => Dismount(true);


    #endregion

}
