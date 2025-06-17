using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class DestuctibleObject : MonoBehaviour, IDamagable
{
    [Header("Health Setting")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    
    [Header("Visual Effects")]
    [SerializeField] private float hitFlashDuration = 0.15f;
    [SerializeField] private Color hitFlashColor = Color.red;
    [SerializeField] private GameObject destructObject;
    
    [Header("Drop Settings")]
    [SerializeField] private int minDropCount = 1;
    [SerializeField] private int maxDropCount = 3;
    [SerializeField] private float dropRadius = 1f;
    [SerializeField] private bool useRandomDrop = true; // true면 랜덤, false면 특정 아이템 드랍
    [SerializeField] private ItemData dropItemData; // 특정 아이템 드랍 시 사용
    [SerializeField] private Tilemap targetTilemap;
    
    
    // Components
    private SpriteRenderer spriteRenderer;
    private Collider2D destructCollider;
    private Color originalColor;
    private bool isDestroyed;
    
    private Coroutine flashCoroutine;
    private Vector3Int tilePosition;

    private void Reset()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        destructCollider = GetComponent<BoxCollider2D>();
        
        if (GetComponentInParent<Tilemap>() != null)
        {
            targetTilemap = GetComponentInParent<Tilemap>();
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        destructCollider = GetComponent<BoxCollider2D>();
        
        if (spriteRenderer == null) Debug.LogError("sprite renderer is null");
        if (destructCollider == null) Debug.LogError("cage collider is null");
        
        if (targetTilemap != null)
        {
            tilePosition = targetTilemap.WorldToCell(transform.position);
        }

    }

    private void Start()
    {
        currentHealth = maxHealth;
        originalColor = spriteRenderer.color;
    }

    #region IDamagable
    public void TakeDamage(int damage)
    {
        if (isDestroyed) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        ShowHitEffect();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplyEffect(EffectType effectType)
    {
    }

    public void Die()
    {
        DestroyCage();
    }
    #endregion

    /// <summary>
    /// 히트 시 빨간색 플래시 효과
    /// </summary>
    private void ShowHitEffect()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashEffect());
    }

    private IEnumerator FlashEffect()
    {
        spriteRenderer.color = hitFlashColor;
        yield return new WaitForSeconds(hitFlashDuration);

        if (!isDestroyed)
        {
            spriteRenderer.color = originalColor;
        }
    }

    private void DestroyCage()
    {
        if (isDestroyed) return;
        
        targetTilemap.SetTile(tilePosition, null);
        isDestroyed = true;
        destructCollider.enabled = false;
        Destroy(destructObject);
        
        DropItems();
    }
    
    /// <summary>
    /// NPC 아이템 드롭 로직
    /// </summary>
    private void DropItems()
    {
        int dropCount = Random.Range(minDropCount, maxDropCount + 1);
        
        // 랜덤 드롭
        if (useRandomDrop)
        {
            ItemDropManager.Instance.DropRandomItem(transform.position, dropCount, dropRadius);
        }
        
        // 특정 아이템 드롭
        else if (dropItemData)
        {
            ItemDropManager.Instance.DropSpecificItem(transform.position, dropCount, dropItemData, dropRadius);
        }
        
        Debug.Log($"{name}에서 {dropCount}개 아이템 드롭됨");
    }
}
