using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : Singleton<ItemDropManager>
{
    [SerializeField] private GameObject itemPrefab; // 범용 Item 프리팹 하나만 사용
    [SerializeField] private ItemData[] availableItems; // 드롭 가능한 아이템들
    
    [Header("Scatter Effect Settings")]
    [SerializeField] private float scatterForce = 3f;
    [SerializeField] private float upwardForce = 1f;
    [SerializeField] private float scatterDelay = 0.1f;
    [SerializeField] private float physicsDisableTime = 1f; // 물리엔진 비활성화


    protected override void Awake()
    {
        base.Awake();
        LoadAvailableItems();
    }

    private void LoadAvailableItems()
    {
        availableItems = Resources.LoadAll<ItemData>("Items");
        
        if (availableItems.Length == 0) Debug.LogError("No Items Available");
    }
    
    /// <summary>
    /// 특정 아이템 드롭할 때 사용
    /// </summary>
    public void DropRandomItem(Vector3 position, int count, float scatterRadius = 1.5f)
    {
        StartCoroutine(ScatterDropCoroutine(position, count, scatterRadius, true));
    }
    
    /// <summary>
    /// 특정 아이템 드롭할 때 사용
    /// </summary>
    public void DropSpecificItem(Vector3 position, int count, ItemData itemData, float scatterRadius = 1.5f)
    {
        StartCoroutine(ScatterDropCoroutine(position, count, scatterRadius, false, itemData));
    }

    /// <summary>
    /// 흩뿌리면서 드롭되는 아이템 효과 포함
    /// </summary>
    /// <param name="centerPosition">기준 위치</param>
    /// <param name="count">몇 개인지</param>
    /// <param name="scatterRadius">아이템 드롭 범위</param>
    /// <param name="useRandomItems">랜덤일 경우 true</param>
    /// <param name="dropItemData">랜덤 아닐 경우 설정된 아이템 데이터</param>
    private IEnumerator ScatterDropCoroutine(Vector3 centerPosition, int count, float scatterRadius, bool useRandomItems, ItemData dropItemData = null)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 scatterDirection = Random.insideUnitCircle * scatterRadius;
            scatterDirection.y = Mathf.Abs(scatterDirection.y);
            Vector3 dropPosition = centerPosition + scatterDirection;

            GameObject itemObj;
            if (useRandomItems && availableItems.Length > 0)
            {
                ItemData randomItem = availableItems[Random.Range(0, availableItems.Length)];
                itemObj = CreateItemObject(dropPosition, randomItem);
            }
            
            else if (dropItemData != null)
            {
                itemObj = CreateItemObject(dropPosition, dropItemData);
            }

            else continue;
            
            ApplyScatterPhysics(itemObj, scatterDirection);

            if (i < count - 1)
            {
                yield return new WaitForSeconds(scatterDelay);
            }
        }
    }

    private GameObject CreateItemObject(Vector3 position, ItemData itemData)
    {
        GameObject itemObj = Instantiate(itemPrefab, position, Quaternion.identity);
        Item item = itemObj.GetComponent<Item>();

        if (item)
        {
            item.Init(itemData);
            return itemObj;
        }

        else
        {
            Debug.LogError("아이템 컴포넌트 없음 !!!");
            Destroy(itemObj);
            return null;
        }
    }
    
    /// <summary>
    /// 흩뜨리는 물리 효과 적용
    /// </summary>
    private void ApplyScatterPhysics(GameObject itemObj, Vector2 scatterDirection)
    {
        if (itemObj == null) return;
        
        Rigidbody2D rb = itemObj.GetComponent<Rigidbody2D>();
        
        // 힘 방향 계산
        Vector2 forceDirection = scatterDirection;
        forceDirection.y += upwardForce * 0.5f;
        float forceMagnitude = scatterForce * Random.Range(0.8f, 1.2f);
        
        // 힘 적용
        rb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);
        
        // 일정 시간 후 물리 효과 제거
        StartCoroutine(DisablePhysicsAfterDelay(rb, physicsDisableTime));
    }

    /// <summary>
    /// 일정 시간 후 물리 효과 비활성화
    /// </summary>
    private IEnumerator DisablePhysicsAfterDelay(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = 0f;
            rb.isKinematic = true;
        }
    }
    
    #region 테스트용 코드

    [SerializeField] private GameObject player;
    
    public void TestDrop()
    {
        Vector3 testPos = player.transform.position + (Vector3.right * 2);
        DropRandomItem(testPos, 1);
    }
    
    #endregion
}