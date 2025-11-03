using UnityEngine;

[CreateAssetMenu(fileName = "EnemySettings", menuName = "Enemy/Data")]

public class EnemyData : ScriptableObject
{
    public GameObject coin;
    public AudioClip EnemyDead;
    public int EnemyDamage;
    public float EnemySpeed;
    public float DistanceOnSurveillance;
    public float BlockFallSpeed;
}
