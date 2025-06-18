using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZoneTrigger : MonoBehaviour
{
    public GameObject boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        boss.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
