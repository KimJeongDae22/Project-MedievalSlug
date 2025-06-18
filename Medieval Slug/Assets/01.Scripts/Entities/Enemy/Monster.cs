using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

public class Monster : MonoBehaviour, IDamagable, IPoolable
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public MonsterAnimationHash AnimationHash { get; private set; }
    [field: SerializeField] public bool HasAnimator { get; private set; } = false;

    [field: Header("Monster States")] [SerializeField]
    protected MonsterStateMachine stateMachine;
    
    [Header("VFX")]
    [SerializeField] private GameObject burnVFX;
    [SerializeField] private GameObject freezeVFX;
    [SerializeField] private GameObject poisonVFX;

    [field: SerializeField] public virtual MonsterSO MonsterData { get; private set; }
    [SerializeField] private int health;
    [SerializeField] private float speed;
    private bool isSlowed;
    public float Speed => speed;
    
    // 풀링용 추가 변수
    private Action<GameObject> returnToPool;

    protected virtual void Reset()
    {
        Animator = GetComponentInChildren<Animator>();

        if (Animator != null)
            HasAnimator = true;

        stateMachine = GetComponent<MonsterStateMachine>();
        
        burnVFX = transform.Find("Sprite/EffectParticles/BurnEffect").gameObject;
        freezeVFX = transform.Find("Sprite/EffectParticles/FreezeEffect").gameObject;
        poisonVFX = transform.Find("Sprite/EffectParticles/PoisonEffect").gameObject;
    }

    protected virtual void Awake()
    {
        if (Animator == null)
        {
            Animator = GetComponentInChildren<Animator>();
            if (Animator != null)
            {
                HasAnimator = true;
            }
        }

        if (stateMachine == null)
        {
            stateMachine = GetComponent<MonsterStateMachine>();
        }
        
        burnVFX?.SetActive(false);
        freezeVFX?.SetActive(false);
        poisonVFX?.SetActive(false);
        
        Initialize();
    }

    public void Initialize()
    {
        health = MonsterData.Health;
        speed = MonsterData.MoveSpeed;
        isSlowed = false;
        AnimationHash.Initialize();
    }
    
    #region IPoolable
    
    public void Initialize(Action<GameObject> returnAction)
    {
        returnToPool = returnAction;
        Initialize();
    }

    public void OnSpawn()
    {
        Initialize();
        StopAllCoroutines();
    }

    public void OnDespawn()
    {
        StopAllCoroutines();
        burnVFX?.SetActive(false);
        freezeVFX?.SetActive(false);
        poisonVFX?.SetActive(false);
        returnToPool?.Invoke(gameObject);
    }
    
    public void SetPrefabIndex(int index)
    {
        
    }
    
    #endregion
    
    private Coroutine burnVFXCoroutine;
    private Coroutine lastPoisonVFXCoroutine;
    
    public void ApplyEffect(EffectType effectType)
    {
        switch (effectType)
        {
            case EffectType.Nomal:
                break;
            case EffectType.Burn:
                if (burnVFXCoroutine != null)
                {
                    StopCoroutine(burnVFXCoroutine);
                }
                burnVFXCoroutine = StartCoroutine(BurningEffectCoroutine());
                TakeDamage(1); // 추가 1 데미지
                break;
            case EffectType.Deceleration:
                if (!isSlowed)
                {
                    freezeVFX?.SetActive(true);
                    StartCoroutine(SlowSpeedCoroutine(2)); // 속도 절반으로 감속
                }
                break;
            case EffectType.Poisoning:
                poisonVFX?.SetActive(true);
                lastPoisonVFXCoroutine = StartCoroutine(PoisoningCoroutine()); // 중족 데미지
                break;
        }
    }

    private IEnumerator BurningEffectCoroutine()
    {
        burnVFX?.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        burnVFX?.SetActive(false);
        burnVFXCoroutine = null;
    }

    private IEnumerator PoisoningCoroutine()
    {
        Coroutine myCoroutine = lastPoisonVFXCoroutine;
        
        int tick = 3;

        for (int i = 0; i < tick; i++)
        {
            TakeDamage(1);
            yield return new WaitForSeconds(1f);
        }

        if (myCoroutine == lastPoisonVFXCoroutine)
        {
            poisonVFX?.SetActive(false);
        }
    }

    private IEnumerator SlowSpeedCoroutine(float degree)
    {
        isSlowed = true;
        speed /= degree;

        yield return new WaitForSeconds(3f);

        speed = MonsterData.MoveSpeed;
        isSlowed = false;
        freezeVFX?.SetActive(false);
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

    public void OnDie()
    {
        
    }
}