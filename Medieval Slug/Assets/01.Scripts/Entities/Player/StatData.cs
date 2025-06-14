using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stat/StatData")]
public class StatData : ScriptableObject
{
    [Header("Base Stat")]
    public List<StatEntry> stat;
}

