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
    public string defaultMessage;
    [TextArea(2, 3)]
    public string questMessage;
    [TextArea(2, 3)]
    public string completedMessage;
    
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
        //UpdateQuestIndicator();
    }

    
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }

        //UpdateQuestIndicator();
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
                //HandleRescueTarget();
                break;
                
            case NPCRole.QuestCompleter:
                //HandleQuestCompleter();
                break;
        }
    }
    
    
    private void HandleQuestGiver()
    {
        // 이미 퀘스트가 완료됐으면
        if (QuestManager.Instance.GetCurrentQuest()?.data.questId == questId && 
            QuestManager.Instance.GetCurrentQuest().status == QuestStatus.Finished)
        {
            //ShowMessage(completedMessage);
            return;
        }
        
        // 이미 퀘스트가 진행중이면
        if (QuestManager.Instance.HasActiveQuest())
        {
            //ShowMessage(questMessage);
            return;
        }
        
        // 새 퀘스트 시작
        if (QuestManager.Instance.StartQuest(questId))
        {
            //ShowMessage(questMessage);
        }
        
        else
        {
            //ShowMessage(defaultMessage);
        }
    }
}
