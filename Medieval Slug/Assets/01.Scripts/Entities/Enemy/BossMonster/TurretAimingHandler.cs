using System;
using UnityEngine;

public class TurretAimingHandler : MonoBehaviour
{
    [SerializeField] private BossTurretStateMachine stateMachine;
    [SerializeField] private BossTurret boss;
    [SerializeField] private Transform rotatingPivot;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform target;
    [SerializeField] private float angleOffset;
    public bool IsAniming = false;

    private void Reset()
    {
        stateMachine = transform.parent.GetComponent<BossTurretStateMachine>();
        boss = GetComponent<BossTurret>();
        rotatingPivot = transform.Find("RotatingPivot").transform;
        shootPoint = transform.Find("RotatingPivot/ShootPoint").transform;
    }

    void Start()
    {
        Vector2 initialDirection = shootPoint.position - rotatingPivot.position;
        angleOffset = (Mathf.Atan2(initialDirection.y, initialDirection.x) * Mathf.Rad2Deg) - 45f;
    }

    void Update()
    {
        if (IsAniming || boss.IsHalfHealth)
        {
            Vector2 direction = target.position - rotatingPivot.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float finalAngle = targetAngle - angleOffset;

            Quaternion targetRotation = Quaternion.Euler(0f, 0f, finalAngle);
            
            rotatingPivot.rotation = Quaternion.Lerp
            (
                rotatingPivot.rotation,
                targetRotation,
                Time.deltaTime *
                (
                    boss.BossData.AimingSpeed *
                    (boss.IsHalfHealth ? (float)boss.BossData.Health / boss.Health : 1f)
                )
            );
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}