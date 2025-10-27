using UnityEngine;

[CreateAssetMenu(fileName = "EnemySettings", menuName = "Enemy/Data")]

public class EnemyData : ScriptableObject
{
    public int EnemyDamage;
    public float EnemySpeed;
    public float DistanceOnSurveillance;
    public float BlockFallSpeed;
}
