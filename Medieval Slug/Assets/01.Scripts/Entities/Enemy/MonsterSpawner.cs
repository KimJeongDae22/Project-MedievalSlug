using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int[] monsterPrefabIndices; // ObjectPoolManager의 프리팹 인덱스들
    [SerializeField] private float spawnRadius;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxMonsters = 10;
    
    private int currentMonsterCount;
    private float lastSpawnTime;
    
    private void Update()
    {
        if (ShouldSpawn())
        {
            SpawnRandomMonster();
        }
    }
    
    private bool ShouldSpawn()
    {
        return currentMonsterCount < maxMonsters && 
               Time.time - lastSpawnTime >= spawnInterval;
    }
    
    private void SpawnRandomMonster()
    {
        if (spawnPoints.Length == 0 || monsterPrefabIndices.Length == 0) return;
        
        // 랜덤 스폰 포인트 선택
        Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        // 위치 계산
        Vector2 spawnPosition = GetRandomSpawnPosition(selectedSpawnPoint);
        int monsterIndex = Random.Range(0, monsterPrefabIndices.Length);
        
        SpawnMonster(monsterPrefabIndices[monsterIndex], spawnPosition);
    }
    
    
    private Vector2 GetRandomSpawnPosition(Transform spawnPoint)
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        return (Vector2)spawnPoint.position + randomCircle;
    }
    
    
    public void SpawnMonster(int prefabIndex, Vector2 position)
    {
        GameObject monsterObj = ObjectPoolManager.Instance.GetObject(
            prefabIndex, 
            position, 
            Quaternion.identity
        );
        
        if (monsterObj)
        {
            // 몬스터 설정
            Monster monster = monsterObj.GetComponentInChildren<Monster>();
            if (monster)
            {
                monster.SetPrefabIndex(prefabIndex);
            }
            
            currentMonsterCount++;
            lastSpawnTime = Time.time;
        }
    }
    
    /// <summary>
    /// 디버그용 기즈모 표시
    /// </summary>
    private void OnDrawGizmos()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;
        
        Gizmos.color = Color.red;
        
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint == null) continue;
            
            Gizmos.DrawWireSphere(spawnPoint.position, spawnRadius);
        }
    }
}