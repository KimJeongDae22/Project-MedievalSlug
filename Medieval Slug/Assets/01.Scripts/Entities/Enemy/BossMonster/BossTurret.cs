using UnityEngine;

public class BossTurret : MonoBehaviour
{
    public BossSO BossData;
    public Transform firePoint;
    public GameObject projectilePrefab;
    private float lastFireTime;

    public void FireAt(Vector3 targetPosition)
    {
        if (Time.time - lastFireTime < BossData.AttackCooldown)
            return;

        lastFireTime = Time.time;

        Vector2 direction = (targetPosition - firePoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 10f;
    }
}