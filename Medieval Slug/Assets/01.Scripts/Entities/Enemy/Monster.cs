using System;
using UnityEngine;

public class Monster : MonoBehaviour, IDamagable
{
    [SerializeField] private MeleeMonsterStateMachine stateMachine;
    [field : SerializeField] public SpriteRenderer Sprite { get; private set; }
    [field : SerializeField] public Animator Animator { get; private set; }
    [field : SerializeField] public MonsterAnimationHash AnimationHash {get; private set;}
    [field : SerializeField] public bool HasAnimator { get; private set; } = false;
    
    [field : Header("Monster States")] 
    [field : SerializeField] public MonsterSO MonsterData { get; private set; }
    [SerializeField] private int health;
    
    /// <summary>
    /// 근접몬스터에만 넣어야 하는데 방법 모르겠음.
    /// </summary>
    [field : Header("Melee Monster")]
    [field : SerializeField] public MeleeMonsterCollider meleeCollider { get; private set; }

    private void Reset()
    {
        Sprite = GetComponentInChildren<SpriteRenderer>();
        
        Animator = GetComponentInChildren<Animator>();
        
        if (Animator != null) 
            HasAnimator = true;
        
        stateMachine = GetComponent<MeleeMonsterStateMachine>();
        
        health = MonsterData.Health;
    }

    private void Awake()
    {
        AnimationHash.Initialize();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    
    public void Die()
    {
        Animator.SetTrigger("Dead");
        meleeCollider.DisableCollider();
    }

    public void DisableGameObject()
    {
        transform.parent.gameObject.SetActive(false);
        //몬스터도 오브젝트 풀링 한다면 이곳에 코드 추가
    }
    
    /// <summary>
    /// 감지 확인용 범위 그리기
    /// </summary>
    void OnDrawGizmos()
    {
        if (stateMachine == null || stateMachine.target == null)
            return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(stateMachine.transform.position, MonsterData.DetectRange);
        
        Gizmos.color = Color.red;
        Vector2 origin = stateMachine.transform.position + MonsterData.RayOffset;
        float directionX = stateMachine.target.position.x - origin.x;
        Vector2 direction = directionX > 0 ? Vector2.right : Vector2.left;
        float attackRange = MonsterData.AttackRange;

        Gizmos.DrawLine(origin, origin + direction * attackRange);
    }
}