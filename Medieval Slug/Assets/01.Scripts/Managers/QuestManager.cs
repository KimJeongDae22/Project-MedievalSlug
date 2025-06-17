using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField] private QuestData[] allQuests;
    [SerializeField] private Quest currentQuest;

    protected override void Awake()
    {
        base.Awake();
        LoadQuests();
    }
    
    private void LoadQuests()
    {
        allQuests = Resources.LoadAll<QuestData>("Quests");
        Debug.Log($"{allQuests.Length} quests loaded");
    }
    
    
    public bool StartQuest(string questId)
    {
        if (currentQuest != null)
        {
            Debug.LogWarning("이미 진행 중인 퀘스트가 있습니다!");
            return false;
        }
        
        QuestData questData = GetQuestData(questId);
        if (!questData) return false;
        
        currentQuest = new Quest(questData);
        currentQuest.status = QuestStatus.InProgress;
        
        Debug.Log($"퀘스트 시작: {questData.questName}");
        return true;
    }
    
    
    private QuestData GetQuestData(string questId)
    {
        return allQuests.FirstOrDefault(quest => quest.questId == questId);
    }
    
    
    public void UpdateProgress(string targetId, int amount = 1)
    {
        if (currentQuest == null || currentQuest.status != QuestStatus.InProgress) 
            return;
        
        if (currentQuest.data.targetId == targetId)
        {
            currentQuest.AddProgress(amount);
            Debug.Log($"퀘스트 진행: {currentQuest.currentProgress}/{currentQuest.data.targetCount}");
            
            if (currentQuest.IsComplete())
            {
                Debug.Log($"퀘스트 목표 달성: {currentQuest.data.questName}");
            }
        }
    }
    
    
    public bool CompleteQuest()
    {
        if (currentQuest == null || currentQuest.status != QuestStatus.Completed)
            return false;
        
        // 아이템 보상 지급
        foreach (ItemData item in currentQuest.data.rewardItems)
        {
            Vector3 dropPos = CharacterManager.Instance.Controller.transform.position + Vector3.forward * 2;
            ItemDropManager.Instance.DropSpecificItem(dropPos, 1, item);
        }
        
        Debug.Log($"퀘스트 완료: {currentQuest.data.questName}");
        currentQuest.status = QuestStatus.Finished;
        currentQuest = null;
        return true;
    }
    
    #region public Getters
    
    public Quest GetCurrentQuest() => currentQuest;
    public bool HasActiveQuest() => currentQuest != null && currentQuest.status == QuestStatus.InProgress;
    public bool HasCompletedQuest() => currentQuest != null && currentQuest.status == QuestStatus.Completed;
    
    #endregion
}
