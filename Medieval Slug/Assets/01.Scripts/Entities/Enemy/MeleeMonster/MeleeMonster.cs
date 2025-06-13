using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonster : Monster
{
    [field : Header("Melee Monster")]
    [field : SerializeField] public MeleeMonsterCollider MeleeCollider { get; private set; }

    protected override void Reset()
    {
        base.Reset();
        MeleeCollider = GetComponent<MeleeMonsterCollider>();
    }

    protected override void Awake()
    {
        base.Awake();
        MeleeCollider.enabled = false;
    }
}
