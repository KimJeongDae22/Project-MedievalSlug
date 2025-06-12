using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    // private field
    private SpriteRenderer spriteRenderer;
    private Collider2D collider;
    private Animator animator;
    private bool isCollected;

    // animator Hash
    private static readonly int Get = Animator.StringToHash("Get");
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        if (spriteRenderer == null) Debug.LogError("Item SpriteRenderer not found");
        if (collider == null) Debug.LogError("Item Collider not found");
        if (animator == null) Debug.LogError("Item Animator not found");
    }

    /// <summary>
    /// 아이템 데이터 초기화 (ItemDropManger에서 호출됨)
    /// </summary>
    public void Init(ItemData data)
    {
        Debug.Log("=== Initialize 시작 ===");
    
        if (data == null)
        {
            Debug.LogError("ItemData가 null입니다!");
            return;
        }

        Debug.Log($"받은 ItemData: {data.name}");
        Debug.Log($"ItemData.itemName: {data.itemName}");
        Debug.Log($"ItemData.icon: {data.icon}");
        Debug.Log($"ItemData.icon.name: {data.icon?.name}");

        itemData = data;
    
        // SpriteRenderer 상태 확인
        Debug.Log($"SpriteRenderer 현재 sprite: {spriteRenderer.sprite?.name}");
        Debug.Log($"SpriteRenderer enabled: {spriteRenderer.enabled}");
        Debug.Log($"GameObject active: {gameObject.activeInHierarchy}");
    
        if (data.icon != null)
        {
            spriteRenderer.sprite = data.icon;
            Debug.Log($"스프라이트 할당 후: {spriteRenderer.sprite?.name}");
        }
        else
        {
            Debug.LogError($"ItemData '{data.itemName}'의 icon이 null!");
        }
    
        collider.enabled = true;
        isCollected = false;
    
        Debug.Log("=== Initialize 완료 ===");
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
        collider.enabled = false;

        ApplyItemEffect();
        animator.SetTrigger(Get);
    }

    /// <summary>
    /// 아이템 타입에 따라 효과 적용
    /// </summary>
    private void ApplyItemEffect()
    {
        //player = CharacterManager.Player;
        
        switch (itemData.itemType)
        {
            case ItemType.Score:
                // TODO
                // GameManager.Instance.AddScore(itemData.value);
                Debug.Log($"Score +{itemData.value}");
                break;
            
            case ItemType.Weapon:
                // TODO
                // 플레이어 무기 강화 로직
                Debug.Log($"Weapon changed: {itemData.itemName}");
                break;
            
            case ItemType.Health:
                // TODO
                // 플레이어 목숨 증가
                Debug.Log($"Health +{itemData.value}");
                break;
        }
    }

    /// <summary>
    /// 애니메이션 event로 호출. 애니메이션 종료 시 아이템 삭제
    /// </summary>
    public void OnCollectAnimationEnd()
    {
        Destroy(gameObject);
    }
}