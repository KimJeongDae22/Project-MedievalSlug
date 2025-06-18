using Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeHandler : MonoBehaviour
{
    [Header("[Melee Weapon Setting]")]
    [SerializeField] private int meleeDamage = 10;
    [SerializeField] private float meleeRange = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float windupTime = 0.2f;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerRangedHandler prh;
    private bool isAttacking;
    [SerializeField] private AudioClip attackAudioClip;
    public void OnMelee()
    {
        if (isAttacking) return;  
        StartCoroutine(PerformMelee());

    }
    private IEnumerator PerformMelee()
    {
        prh.SetWeaponEnabled(false);
        isAttacking = true;            
        // 1 또는 2 중 랜덤으로 선택
        int idx = Random.Range(1, 3); // 1 또는 2
        string triggerName = (idx == 1) ? "MeleeAttack1" : "MeleeAttack2";
        animator.SetTrigger(triggerName);

        yield return new WaitForSeconds(windupTime);

        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector2 origin = (Vector2)transform.position + dir * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, meleeRange, enemyLayer);
        if (hit.collider != null && hit.collider.TryGetComponent<IDamagable>(out var target))
            target.TakeDamage(meleeDamage);

        if(attackAudioClip != null)
        {
            AudioManager.PlaySFXClip(attackAudioClip);
        }
        prh.SetWeaponEnabled(true);

        yield return new WaitForSeconds(0.21f); //방어코드
        UnlockMelee();
    }
    public void UnlockMelee() => isAttacking = false;

    // 히트박스 시각화 (에디터용)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
