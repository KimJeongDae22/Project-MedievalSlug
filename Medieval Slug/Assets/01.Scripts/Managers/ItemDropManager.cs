using UnityEngine;

public class ItemDropManager : Singleton<ItemDropManager>
{
    [SerializeField] private GameObject itemPrefab; // 범용 Item 프리팹 하나만 사용
    [SerializeField] private ItemData[] availableItems; // 드롭 가능한 아이템들
    
    /// <summary>
    /// 기본적으로 랜덤 아이템 드롭
    /// </summary>
    /// <param name="position">드롭할 위치</param>
    public void DropRandomItem(Vector3 position)
    {
        if (availableItems.Length > 0)
        {
            ItemData randomItem = availableItems[Random.Range(0, availableItems.Length)];
            DropItem(position, randomItem);
        }
    }
    
    /// <summary>
    /// 특정 아이템 드롭할 때 사용
    /// </summary>
    /// <param name="position">드롭할 위치</param>
    /// <param name="itemData">드롭할 아이템 SO 데이터</param>
    public void DropItem(Vector3 position, ItemData itemData)
    {
        GameObject itemObj = Instantiate(itemPrefab, position, Quaternion.identity);
        Item item = itemObj.GetComponent<Item>();
        
        if (!item)
        {
            Debug.LogError("아이템 컴포넌트 없음 !!!");
            return;
        }
        
        item.Init(itemData);
    }
    
    /// <summary>
    /// 여러 개 아이템을 랜덤 위치에 드롭
    /// </summary>
    /// <param name="centerPosition">중심 위치</param>
    /// <param name="count">드롭할 개수</param>
    /// <param name="radius">드롭 반경</param>
    public void DropMultipleRandomItems(Vector3 centerPosition, int count, float radius = 1f)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * radius;
            Vector3 dropPosition = centerPosition + (Vector3)randomOffset;
            DropRandomItem(dropPosition);
        }
    }
    
    /// <summary>
    /// 특정 아이템을 여러 개 드롭
    /// </summary>
    /// <param name="centerPosition">중심 위치</param>
    /// <param name="itemData">드롭할 아이템</param>
    /// <param name="count">드롭할 개수</param>
    /// <param name="radius">드롭 반경</param>
    public void DropMultipleItems(Vector3 centerPosition, ItemData itemData, int count, float radius = 1f)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * radius;
            Vector3 dropPosition = centerPosition + (Vector3)randomOffset;
            DropItem(dropPosition, itemData);
        }
    }
    
    #region 테스트용 코드

    [SerializeField] private GameObject player;
    
    public void TestDrop()
    {
        Vector3 testPos = player.transform.position + (Vector3.right * 2);
        DropRandomItem(testPos);
    }
    
    #endregion
}