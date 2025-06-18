using UnityEngine;

public enum ItemType
{
    Score, // 점수
    Weapon, // 무기 효과 변경
    Health, // 체력 회복
    Quest, // 퀘스트용 아이템
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public int value;
    
    [Header("Weapon Info")]
    public ProjectileData projectileData;
}