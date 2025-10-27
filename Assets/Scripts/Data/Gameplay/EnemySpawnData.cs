using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnSettings", menuName = "EnemySpawn/Data")]

public class EnemySpawnData : ScriptableObject
{

    [Header("Config")]
    public float EnemyMinRandomSpawn = 2f;
    public float EnemyMaxRandomSpawn = 4f;
    public float SpawnerSpeed = 1f;
    public float SpawnLimitLeft = 1f;
    public float SpawnLimitRight = 1f;
}
