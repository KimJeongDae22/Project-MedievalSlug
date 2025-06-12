using System;
using UnityEngine;

public class Monster : MonoBehaviour, IDamagable
{
    [SerializeField] private MeleeMonsterStateMachine stateMachine;
    [field : SerializeField] public SpriteRenderer Sprite { get; private set; }
    [field : SerializeField] public Animator Animator { get; private set; }
    [field : SerializeField] public MonsterAnimationHash AnimationHash {get; private set;}
    [field : SerializeField] public bool HasAnimator { get; private set; } = false;

    /// <summary>
    /// 아래 값들을 SO 관리할 경우 삭제 가능
    /// </summary>
    /// <param name="damage"></param>
    [Header("Monster States")] 
    [SerializeField] private int health = 1;
    [field : SerializeField] public int damage {get; private set;} = 1;
    [field : SerializeField] public float AttackCooldown { get; private set; } = 0.5f;
    [field: SerializeField] public float MoveSpeed { get; private set; } = 2f;
    [field : SerializeField] public float DetectRange { get; private set; } = 10f;
    [field : SerializeField] public Vector3 DetectOffset { get; private set; } = Vector3.zero;

    [field: SerializeField] public float AttackRange { get; private set; } = 1f;

    private void Reset()
    {
        Sprite = GetComponentInChildren<SpriteRenderer>();
        
        Animator = GetComponentInChildren<Animator>();
        
        if (Animator != null) 
            HasAnimator = true;
        
        stateMachine = GetComponent<MeleeMonsterStateMachine>();
    }

    private void Awake()
    {
        AnimationHash.Initialize();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void Die()
    {
        
    }
    
    /// <summary>
    /// 감지 확인용 범위 그리기
    /// </summary>
    void OnDrawGizmos()
    {
        if (stateMachine == null || stateMachine.target == null)
            return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(stateMachine.transform.position, stateMachine.Monster.DetectRange);
        
        Gizmos.color = Color.red;
        Vector2 origin = stateMachine.transform.position + DetectOffset;
        float directionX = stateMachine.target.position.x - origin.x;
        Vector2 direction = directionX > 0 ? Vector2.right : Vector2.left;
        float attackRange = stateMachine.Monster.AttackRange;

        Gizmos.DrawLine(origin, origin + direction * attackRange);
    }
}