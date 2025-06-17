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
    [Header("Aiming")]
    [SerializeField] private bool isLeft;
    [SerializeField] private float headOffset;
    public bool IsAniming = false;

    private Quaternion targetRotation;

    private void Reset()
    {
        stateMachine = transform.parent.parent.GetComponent<BossTurretStateMachine>();
        boss = GetComponent<BossTurret>();
        rotatingPivot = transform.Find("RotateForAnim/RotatingPivot").transform;
        shootPoint = transform.Find("RotateForAnim/RotatingPivot/ShootPoint").transform;
    }

    void Start()
    {
        Vector2 initialDirection = shootPoint.position - rotatingPivot.position;
        angleOffset = (Mathf.Atan2(initialDirection.y, initialDirection.x) * Mathf.Rad2Deg) -
                      (45f + (isLeft ? headOffset : -headOffset));
    }

    void Update()
    {
        if (!boss.IsDead && (IsAniming || boss.IsHalfHealth))
        {
            Vector2 direction = target.position - rotatingPivot.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float finalAngle = targetAngle - angleOffset;

            targetRotation = Quaternion.Euler(0f, 0f, finalAngle);
            
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

    public bool IsAimingComplete()
    {
        float angleDifference = Quaternion.Angle(rotatingPivot.rotation, targetRotation);
        return angleDifference < 10f;
    }
}