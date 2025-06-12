using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab; // 범용 Item 프리팹 하나만 사용
    [SerializeField] private ItemData[] availableItems; // 드롭 가능한 아이템들
    
    /// <summary>
    /// 기본적으로 랜덤 아이템 드랍
    /// </summary>
    /// <param name="position">드랍할 위치</param>
    public void DropRandomItem(Vector3 position)
    {
        if (availableItems.Length > 0)
        {
            ItemData randomItem = availableItems[Random.Range(0, availableItems.Length)];
            DropItem(position, randomItem);
        }
    }
    
    /// <summary>
    /// 특정 아이템 드랍할 때 사용
    /// </summary>
    /// <param name="position">드랍할 위치</param>
    /// <param name="itemData">드랍할 아이템 SO 데이터</param>
    public void DropItem(Vector3 position, ItemData itemData)
    {
        GameObject itemObj = Instantiate(itemPrefab, position, Quaternion.identity);
        Item item = itemObj.GetComponent<Item>();
        item.Init(itemData);
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