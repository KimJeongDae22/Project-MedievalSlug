using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour, IPoolable
{
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private Faction faction;
    [SerializeField] private EffectType curEffectType;

    private Vector2 direction;
    private Rigidbody2D rigidbody;
    private float curduration;

    private Action<GameObject> returnToPool;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 shootDircetion)
    {
        this.direction = shootDircetion.normalized;
        this.curduration = 0f;

        rigidbody.velocity = direction * projectileData.ShotSpeed;
    }

    void Update()
    {
        curduration += Time.deltaTime;
        
        if (curduration >= projectileData.Range)
        {
            OnDespawn();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInChildren<IDamagable>() is IDamagable target) // IDamagable 있는지 확인
        {
            int playerLayer = LayerMask.NameToLayer("Player");
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            Debug.Log(target);
            if ((faction == Faction.Player && collision.gameObject.layer == enemyLayer) || // Layer에 따른 화살 공격 여부
                (faction == Faction.Enemy && collision.gameObject.layer == playerLayer))
            {
                if (AudioManager.Instance.SFXClip != null)
                {
                    AudioManager.PlaySFXClip(AudioManager.Instance.SFXClip[6]);
                }
                target.TakeDamage((int)projectileData.Damage);
                target.ApplyEffect(curEffectType);
                OnDespawn();
            }
        }
        //else
        //{
        //    if (attackSoundClip != null)
        //    {
        //        Debug.Log("소리 발생");
        //        AudioManager.PlaySFXClip(attackSoundClip[0]);
        //    }
        //    OnDespawn();
        //}
    }

    public void Initialize(Action<GameObject> returnAction)
    {
        returnToPool = returnAction;
    }

    public void OnSpawn()
    {

    }

    public void OnDespawn()
    {
        returnToPool?.Invoke(gameObject);
    }

}
