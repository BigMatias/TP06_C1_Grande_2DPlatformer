using UnityEngine;

public class LayersEnum : MonoBehaviour
{
    public enum Layers
    {
        Nothing = 0,
        Floor = 7,
        Player = 8,
        Enemy = 9,
        SlashAttack = 10,
        PunchAttack = 11,
        Door = 12,
        FallDeadPoint = 15
    }
}
