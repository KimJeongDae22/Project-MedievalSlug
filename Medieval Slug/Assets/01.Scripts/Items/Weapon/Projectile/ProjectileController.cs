using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private ProjectileData projectileData;

    private Vector2 direction;
    private Rigidbody2D rigidbody;
    private float curduration;

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
            // 오브젝트 풀링
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 데미지 주기
    }



}
