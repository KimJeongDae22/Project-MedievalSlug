using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCRole
{
    QuestGiver,
    RescueTarget,
    QuestCompleter,
}

public class NPC : MonoBehaviour
{
    [Header("NPC setting")]
    public string npcId;
    public string npcName;
    public NPCRole role;
    
    [Header("Quest")]
    public string questId;
    
    [Header("Dialogue")]
    [TextArea(2, 3)]
    public string defaultMessage; // 기본 메세지
    [TextArea(2, 3)]
    public string questMessage; // 퀘스트 중 메세지
    [TextArea(2, 3)]
    public string completedMessage; // 완료 메세지
    
    [Header("Status")]
    public bool isRescued;
    
    [Header("UI")]
    public GameObject questIndicator;
    public GameObject interactionPrompt;

    private bool playerInRange;

    private void Reset()
    {
        questIndicator = GameObject.Find("Quest Indicator");
        interactionPrompt = GameObject.Find("InteractionPrompt");
    }

    private void Awake()
    {
        if (questIndicator == null) Debug.LogError($"questIndicator is null");
        if (interactionPrompt == null) Debug.LogError($"interactionPrompt is null");
    }

    private void Start()
    {
        interactionPrompt.SetActive(false);
        UpdateQuestIndicator();
    }

    
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        UpdateQuestIndicator();
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        playerInRange = true;
        interactionPrompt.SetActive(true);
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        playerInRange = false;
        interactionPrompt.SetActive(false);
    }
    
    /// <summary>
    /// NPC 역할별 상호작용
    /// </summary>
    private void Interact()
    {
        switch (role)
        {
            case NPCRole.QuestGiver:
                HandleQuestGiver();
                break;
                
            case NPCRole.RescueTarget:
                HandleRescueTarget();
                break;
                
            case NPCRole.QuestCompleter:
                HandleQuestCompleter();
                break;
        }
    }
    
    
    private void HandleQuestGiver()
    {
        Quest currentQuest = QuestManager.Instance.GetCurrentQuest();

        if (currentQuest != null)
        {
            // 이미 퀘스트가 완료됐으면
            if (currentQuest?.data.questId == questId && 
                currentQuest?.status == QuestStatus.Finished)
            {
                ShowMessage(completedMessage);
                return;
            }
        
            // 이미 퀘스트가 진행중이면
            ShowMessage(QuestManager.Instance.HasActiveQuest() ? questMessage : defaultMessage);
        }
        
        // 새 퀘스트 시작
        if (QuestManager.Instance.StartQuest(questId))
        {
            ShowMessage(questMessage);
        }
    }
    
    
    private void HandleRescueTarget()
    {
        if (isRescued)
        {
            ShowMessage(completedMessage);
            return;
        }
        
        // 구출 퀘스트가 활성화되어 있는지 확인
        var currentQuest = QuestManager.Instance.GetCurrentQuest();
        if (currentQuest != null && 
            currentQuest.data.questType == QuestType.Rescue &&
            currentQuest.data.targetId == npcId)
        {
            // 구출 완료
            isRescued = true;
            QuestManager.Instance.UpdateProgress(npcId);
            ShowMessage(completedMessage);
            Debug.Log($"{npcName} 구출 완료!");
        }
        else
        {
            ShowMessage(defaultMessage);
        }
    }
    
    private void HandleQuestCompleter()
    {
        // 완료 가능한 퀘스트가 있는지 확인
        if (QuestManager.Instance.HasCompletedQuest())
        {
            var currentQuest = QuestManager.Instance.GetCurrentQuest();
            if (currentQuest.data.questId == questId)
            {
                QuestManager.Instance.CompleteQuest();
                ShowMessage(completedMessage);
                return;
            }
        }
        
        ShowMessage(defaultMessage);
    }
    
    
    private void ShowMessage(string message)
    {
        Debug.Log($"{npcName}: {message}");
        // TODO: UI 연결
    }
    
    
    private void UpdateQuestIndicator()
    {
        if (!questIndicator || !QuestManager.Instance) return;
        
        bool showIndicator = false;
        
        switch (role)
        {
            case NPCRole.QuestGiver:
                // 진행 중인 퀘스트 없으면 표시
                showIndicator = !QuestManager.Instance.HasActiveQuest();
                break;
                
            case NPCRole.QuestCompleter:
                // 완료된 퀘스트가 있으면 표시
                Quest currentQuest = QuestManager.Instance.GetCurrentQuest();
                showIndicator = QuestManager.Instance.HasCompletedQuest() &&
                                currentQuest?.data.questId == questId;
                break;
        }
        
        questIndicator.SetActive(showIndicator);
    }
}
