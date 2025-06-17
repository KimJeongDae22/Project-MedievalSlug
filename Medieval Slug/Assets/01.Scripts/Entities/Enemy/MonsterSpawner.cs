using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int[] monsterPrefabIndices; // ObjectPoolManager의 프리팹 인덱스들
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
        
        // 랜덤 스폰 포인트, 몬스터 타입 선택
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        int monsterIndex = Random.Range(0, monsterPrefabIndices.Length);
        
        SpawnMonster(monsterPrefabIndices[monsterIndex], spawnPoint.position);
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
    
    // 몬스터 죽었을 때
    public void OnMonsterDied()
    {
        currentMonsterCount = Mathf.Max(0, currentMonsterCount - 1);
    }
}