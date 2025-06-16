using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
public enum StatType
{
    Health,
    Attack,
    Defense
}
[System.Serializable]
public class StatEntry
{
    public StatType statType;
    public float basevalue;
}

