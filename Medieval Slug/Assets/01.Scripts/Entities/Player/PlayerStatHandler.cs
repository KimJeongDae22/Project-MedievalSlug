using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{
    [Header("Stat Setting")]
    public StatData statData;
    private Dictionary<StatType, float> currentStats = new Dictionary<StatType, float>();

    public event Action<StatType, float> OnStatChanged;
    public event Action<int> OnCoinChanged;


    private void Awake()
    {
        InitializeStats();

    }

    private void InitializeStats()
    {
        currentStats.Clear();
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
}


