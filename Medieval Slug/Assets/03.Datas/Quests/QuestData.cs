using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestStatus
{
    NotStarted,
    InProgress,
    Completed,
    Finished
}

public enum QuestType
{
    Rescue,
    Kill,
}

[CreateAssetMenu(fileName = "NewQuestData", menuName = "QuestSystem/QuestData")]
public class QuestData : ScriptableObject
{
    [Header("Quest Info")]
    public string questId;
    public string questName;
    [TextArea(3, 5)]
    public string description;
    public QuestType questType;
    
    [Header("Goals")]
    public string targetId;  // 구출할 NPC 도는 처치할 몬스터 이름
    public int targetCount;
    
    [Header("Rewards")]
    public ItemData[] rewardItems;
}