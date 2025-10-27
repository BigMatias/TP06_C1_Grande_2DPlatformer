using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Player/Data")]
public class PlayerDataSo : ScriptableObject
{
    [Header("Attacks")]
    public GameObject[] playerAttacks;

    [Header("Attacks Config")]
    public float slashCooldown;
    public int slashDamage;
    public float slashSize;
    public float punchCooldown;
    public int punchDamage;
    public float punchSize;

    [Header("Controls")]
    public KeyCode Jump = KeyCode.Space;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    public KeyCode Down = KeyCode.DownArrow;
    public KeyCode SlashAttack = KeyCode.C;
    public KeyCode PunchAttack = KeyCode.X;

    [Header("Configs")]
    public float Speed;
    public float JumpSpeed;
    public float GravityScaleJump;
    public float GravityScaleFall;
    public float GravityScaleDead;

}