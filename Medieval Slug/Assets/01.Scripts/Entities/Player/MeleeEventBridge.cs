using UnityEngine;

public class MeleeEventBridge : MonoBehaviour
{
    // Body에서 부모 Player 검색해 캐싱
    PlayerMeleeHandler handler;
    void Awake() => handler = GetComponentInParent<PlayerMeleeHandler>();

    // Animation Event가 호출하는 함수
    public void UnlockMelee() => handler.UnlockMelee();
}
