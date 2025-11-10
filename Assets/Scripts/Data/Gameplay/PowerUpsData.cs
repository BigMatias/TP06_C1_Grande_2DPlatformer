using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpsSettings", menuName = "PowerUps/Data")]

public class PowerUpsData : ScriptableObject
{
    public int healthAmount;
    public int invulnerabilityDuration;
    public int tripleJumpDuration;
    public int damageBuffDuration;
    public int damageBuff;
}
