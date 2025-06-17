using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 탑승물 탑승 가능 알림 화살표
/// </summary>
public class MountIndicater : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] float blinkSpeed = 5f; //깜빡임

    private void Reset()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Color c = sr.color;
        c.a = Mathf.PingPong(Time.time *  blinkSpeed, 1f);
        sr.color = c;
    }

    public void Show(bool on )
    {
        gameObject.SetActive( on );
    }
}
