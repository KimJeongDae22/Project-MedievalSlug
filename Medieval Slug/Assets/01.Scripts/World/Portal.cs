using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField] private string targetSceneName;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    
    [Header("UI")]
    [SerializeField] private GameObject interactionUI;
    
    private bool playerInRange;

    private void Start()
    {
        ShowInteractionUI(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            Debug.Log("보스 씬 전환");
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            ShowInteractionUI(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            ShowInteractionUI(false);
        }
    }
    
    private void ShowInteractionUI(bool show)
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(show);
        }
    }
}