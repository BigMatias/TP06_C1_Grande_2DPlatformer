using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpsSettings", menuName = "PowerUps/Data")]

public class PowerUpsDataSo : ScriptableObject
{
    public int healthAmount;
    public int invulnerabilityDuration;
    public int tripleJumpDuration;
    public int damageBuffDuration;
    public int damageBuff;
}
