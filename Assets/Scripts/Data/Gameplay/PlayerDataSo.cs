using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Player/Data")]
public class PlayerDataSo : ScriptableObject
{
    [Header("Attacks")]
    public GameObject[] playerAttacks;

    [Header("Attack Sounds")]
    public AudioClip swooshAttackSound;
    public AudioClip slashAttackConnected;
    public AudioClip punchAttackConnected;
    public AudioClip uppercutAttackConnected;

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
    public KeyCode UppercutAttack = KeyCode.Z;

    [Header("Configs")]
    public float Speed;
    public float JumpSpeed;
    public float GravityScaleJump;
    public float GravityScaleFall;
    public float GravityScaleDead;
    public float InvulnerabilityAfterHit;

}