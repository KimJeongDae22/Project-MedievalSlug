using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    // private field
    private SpriteRenderer spriteRenderer;
    private Collider2D itemCollider;
    
    private SpriteRenderer effectRenderer;
    private Animator effectAnimator;
    private bool isCollected;

    // animator Hash
    private static readonly int Get = Animator.StringToHash("Get");
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<Collider2D>();
        
        effectRenderer = transform.FindChild<SpriteRenderer>("Effect");
        effectAnimator = GetComponentInChildren<Animator>();

        if (spriteRenderer == null) Debug.LogError("Item SpriteRenderer not found");
        if (itemCollider == null) Debug.LogError("Item Collider not found");
        
        if (effectRenderer == null) Debug.LogError("Item Effect Renderer not found");
        if (effectAnimator == null) Debug.LogError("Item Effect Animator not found");
    }

    /// <summary>
    /// 아이템 데이터 초기화 (ItemDropManger에서 호출됨)
    /// </summary>
    public void Init(ItemData data)
    {
        itemData = data;
        spriteRenderer.sprite = data.icon;
        effectRenderer.enabled = false;
        itemCollider.enabled = true;
        isCollected = false;
    }
    
    /// <summary>
    /// 플레이어와 충돌 시 획득 처리
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected || !other.CompareTag("Player")) return;

        CollectItem();
    }

    /// <summary>
    /// 아이템 효과 적용, 획득 애니메이션 출력
    /// </summary>
    private void CollectItem()
    {
        // player = CharacterManager.Player
        isCollected = true;
        itemCollider.enabled = false;
        spriteRenderer.enabled = false;

        ApplyItemEffect();
        
        effectRenderer.enabled = true;
        effectAnimator.SetTrigger(Get);
        StartCoroutine(WaitForAnimAndDestroy());
    }

    /// <summary>
    /// 아이템 타입에 따라 효과 적용
    /// </summary>
    private void ApplyItemEffect()
    {
        switch (itemData.itemType)
        {
            case ItemType.Score:
                GameManager.Instance.AddScore(itemData.value);
                break;
            
            case ItemType.Weapon:
                CharacterManager.Instance.PlayerItemCollector.OnArrowPickup(itemData.projectileData);
                break;
            
            case ItemType.Health:
                CharacterManager.Instance.PlayerItemCollector.OnHealthPickup(itemData.value);
                break;
        }
    }

    /// <summary>
    /// 애니메이션 대기
    /// </summary>
    private IEnumerator WaitForAnimAndDestroy()
    {
        yield return null; // 애니메이션 상태 갱신 대기

        float length = effectAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);
        
        Destroy(gameObject);
    }
}