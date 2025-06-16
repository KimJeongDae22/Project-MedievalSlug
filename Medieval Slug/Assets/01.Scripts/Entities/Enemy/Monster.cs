using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Monster : MonoBehaviour, IDamagable
{
    [field: SerializeField] public SpriteRenderer Sprite { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public MonsterAnimationHash AnimationHash { get; private set; }
    [field: SerializeField] public bool HasAnimator { get; private set; } = false;

    [field: Header("Monster States")] [SerializeField]
    protected MonsterStateMachine stateMachine;

    [field: SerializeField] public virtual MonsterSO MonsterData { get; private set; }
    [SerializeField] private int health;
    [SerializeField] private float speed;
    private bool isSlowed;
    public float Speed => speed;

    protected virtual void Reset()
    {
        Sprite = GetComponent<SpriteRenderer>();

        Animator = GetComponent<Animator>();

        if (Animator != null)
            HasAnimator = true;

        stateMachine = transform.parent.GetComponent<MonsterStateMachine>();
    }

    protected virtual void Awake()
    {
        health = MonsterData.Health;
        speed = MonsterData.MoveSpeed;
        AnimationHash.Initialize();
    }
    
    public void ApplyEffect(EffectType effectType)
    {
        switch (effectType)
        {
            case EffectType.Nomal:
                Debug.Log("피격");
                break;
            case EffectType.Burn:
                TakeDamage(1); // 추가 1 데미지
                break;
            case EffectType.Deceleration:
                if (!isSlowed)
                {
                    StartCoroutine(SlowSpeedCoroutine(2)); // 속도 절반으로 감속
                }
                break;
            case EffectType.Poisoning:
                StartCoroutine(PoisoningCoroutine()); // 중족 데미지
                break;
        }
    }

    private IEnumerator PoisoningCoroutine()
    {
        int tick = 3;

        for (int i = 0; i < tick; i++)
        {
            // Sprite.color = new Color(30f / 255f, 180f / 255f, 30f / 255f);  // 적용 안됨 아마도 애니메이션에다 넣어야할듯?
            TakeDamage(1);
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator SlowSpeedCoroutine(float degree)
    {
        isSlowed = true;
        speed /= degree;

        yield return new WaitForSeconds(3f);

        speed = MonsterData.MoveSpeed;
        isSlowed = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
        else
        {
            stateMachine.ChangeState(stateMachine.HitState);
        }
    }

    public void Die()
    {
        stateMachine.ChangeState(stateMachine.DeadState);
    }

    public void DisableGameObject()
    {
        transform.parent.gameObject.SetActive(false);
        //몬스터도 오브젝트 풀링 한다면 이곳에 코드 추가
    }
}