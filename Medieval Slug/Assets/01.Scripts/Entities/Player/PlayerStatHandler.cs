using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour, IDamagable
{
    [Header("Stat Setting")]
    public StatData statData;
    [SerializeField]private Dictionary<StatType, float> currentStats = new Dictionary<StatType, float>();

    private bool isDied;
    public bool IsDied => isDied;
    public event Action<StatType, float> OnStatChanged;

    [SerializeField]private Animator animator;

    private void Awake()
    {
        InitializeStats();

    }

    public void InitializeStats()
    {
        currentStats.Clear();
        isDied = false;
        animator.SetTrigger("Idle");
        foreach (var entry in statData.stat)
            currentStats[entry.statType] = entry.basevalue;
    }
    public float GetStat(StatType type) =>
            currentStats.TryGetValue(type, out var v) ? v : 0f;

    public void ModifyStat(StatType type, float amount, bool isPermanent = true, float duration = 0f)
    {
        if (!currentStats.ContainsKey(type)) return;
        currentStats[type] += amount;
        OnStatChanged?.Invoke(type, currentStats[type]);
        if (!isPermanent && duration > 0f)
            StartCoroutine(RemoveStatAfterDuration(type, amount, duration));
    }

    private IEnumerator RemoveStatAfterDuration(StatType type, float amount, float duration)
    {
        yield return new WaitForSeconds(duration);
        currentStats[type] -= amount;
        OnStatChanged?.Invoke(type, currentStats[type]);
    }

    public void TakeDamage(int damage)
    {
        if (isDied) return;

        ModifyStat(StatType.Health, -damage);
        animator.SetTrigger("Hurt");
        UIManager.Instance.UIUpdate_PlayerHP();
        if (GetStat(StatType.Health) <= 0)
        {
            Die();
        }
    }

    public void ApplyEffect(EffectType effectType)
    {
    }

    public void Die()
    {
        isDied = true;
        animator.SetTrigger("Die");
        UIManager.Instance.ShowDeadUI();
    }
}


