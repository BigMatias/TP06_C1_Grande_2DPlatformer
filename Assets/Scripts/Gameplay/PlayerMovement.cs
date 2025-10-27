using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerDataSo playerDataSo;
    [SerializeField] private AudioClip[] audioClips;

    [Header("Ground check")]
    [SerializeField] private Transform groundCheck;
    private float groundRadius = 0.2f;

    public static event Action playerDied;

    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;
    private HealthSystem healthSystem;
    private AudioSource audioSource;
    private Animator animator;
    private SpriteRenderer sprite;
    private GameObject[] attacks;
    private bool isSlashOnCd;
    private bool isPunchOnCd;
    private bool wasHit;

    [SerializeField] private LayerMask groundLayer;

    private static readonly int State = Animator.StringToHash("State");
    enum PlayerState
    {
        Idle,
        Jump,
        Run,
        Hit
    }

    [SerializeField] private PlayerState playerState = PlayerState.Idle;

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.onDie += HealthSystem_onDie;
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        attacks = new GameObject[2];
    }

    private void OnDestroy()
    {
        healthSystem.onDie -= HealthSystem_onDie;
    }

    private void Start()
    {
        animator.SetInteger(State, (int)playerState);
        rb.gravityScale = playerDataSo.GravityScaleJump;
        InstantiateAttacks();
    }


    private void Update()
    {
        Fall();
        if (!wasHit)
        {
            Attack();
        }
        if (!IsGrounded() && !wasHit)
        {
            playerState = PlayerState.Jump;
            animator.SetInteger(State, (int)playerState);
        }
        if (IsGrounded() && rb.velocity.x == 0 && !wasHit)
        {
            playerState = PlayerState.Idle;
            animator.SetInteger(State, (int)playerState);
        }

    }

    private void FixedUpdate()
    {
        if (!wasHit)
        {
            MovePlayer();
        }
    }

    private void InstantiateAttacks()
    {
        for (int i = 0; i <= attacks.Length - 1; i++)
        {
            attacks[i] = Instantiate(playerDataSo.playerAttacks[i], transform.Find("Attacks"));
            attacks[i].SetActive(false);
        }
    }

    private void Fall()
    {
        if (Input.GetKeyUp(playerDataSo.Jump) && !wasHit)
        {
            rb.gravityScale = playerDataSo.GravityScaleFall;
        }
    }

    private void MovePlayer()
    {
        Vector2 velocity = rb.velocity;

        if (Input.GetKey(playerDataSo.Left))
        {
            sprite.flipX = true;
            playerState = PlayerState.Run;
            animator.SetInteger(State, (int)playerState);
            velocity.x = -playerDataSo.Speed;
        }
        else if (Input.GetKey(playerDataSo.Right))
        {
            sprite.flipX = false;
            playerState = PlayerState.Run;
            animator.SetInteger(State, (int)playerState);
            velocity.x = playerDataSo.Speed;
        }
        else
        {
            velocity.x = 0f;
        }

        if (Input.GetKey(playerDataSo.Jump) && IsGrounded())
        {
            audioSource.PlayOneShot(audioClips[0]);
            rb.gravityScale = playerDataSo.GravityScaleJump;
            rb.AddForce(playerDataSo.JumpSpeed * Time.fixedDeltaTime * Vector2.up);
        }

        rb.velocity = velocity;
    }

    private void Attack()
    {
        if (Input.GetKey(playerDataSo.SlashAttack) && !isSlashOnCd)
        {

            attacks[0].SetActive(true);
            if (!sprite.flipX)
            {
                attacks[0].transform.position = transform.position + new Vector3(1, 0, 0);
                attacks[0].transform.rotation = Quaternion.Euler(0, 0, 0);
                attacks[0].transform.localScale = new Vector3(playerDataSo.slashSize, playerDataSo.slashSize, playerDataSo.slashSize);
            }
            if (sprite.flipX)
            {
                attacks[0].transform.position = transform.position + new Vector3(-1, 0, 0);
                attacks[0].transform.rotation = Quaternion.Euler(0, 0, 0);
                attacks[0].transform.localScale = new Vector3(-playerDataSo.slashSize, playerDataSo.slashSize, playerDataSo.slashSize);
            }
            

            if (!IsGrounded() && Input.GetKey(playerDataSo.Down))
            {
                attacks[0].SetActive(true);
                attacks[0].transform.position = transform.position + new Vector3(0, -1, 0);
                attacks[0].transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            StartCoroutine(AttackCooldown("Slash", playerDataSo.slashCooldown));
        }

        if (Input.GetKey(playerDataSo.PunchAttack) && !isPunchOnCd)
        {
            attacks[1].SetActive(true);
            if (!sprite.flipX)
            {
                attacks[1].transform.position = transform.position + new Vector3(2, 0, 0);
                attacks[1].transform.localScale = new Vector3(playerDataSo.punchSize, playerDataSo.punchSize, playerDataSo.punchSize);
            }
            if (sprite.flipX)
            {
                attacks[1].transform.position = transform.position + new Vector3(-2, 0, 0);
                attacks[1].transform.localScale = new Vector3(-playerDataSo.punchSize, playerDataSo.punchSize, playerDataSo.punchSize);
            }
            StartCoroutine(AttackCooldown("Punch", playerDataSo.punchCooldown));
        }
    }

    private IEnumerator AttackCooldown(string attack, float cooldown)
    {
        if (attack == "Slash")
        {
            isSlashOnCd = true;
            yield return new WaitForSeconds(cooldown);
            isSlashOnCd = false;
        }
        if (attack == "Punch")
        {
            isPunchOnCd = true;
            yield return new WaitForSeconds(cooldown);
            isPunchOnCd = false;
        }
    }

    private void HealthSystem_onDie()
    {
        audioSource.PlayOneShot(audioClips[1]);
        playerDied?.Invoke();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.IsTouching(playerCollider))
        {
            if (collision.gameObject.layer == (int)LayersEnum.Layers.Enemy)
            {
                Transform enemyTransform = collision.gameObject.GetComponent<Transform>();
                StartCoroutine(WasHit(enemyTransform));
            }
        }
    }

    private IEnumerator WasHit(Transform enemyTransform)
    {
        wasHit = true;
        float direction = (transform.position.x - enemyTransform.position.x) >= 0 ? 1f : -1f;
        rb.velocity = Vector2.zero;
        float knockbackForceX = 8f; 
        float knockbackForceY = 7f;  
        Vector2 knockbackDir = new Vector2(direction * knockbackForceX, knockbackForceY);

        rb.AddForce(knockbackDir, ForceMode2D.Impulse);
        playerState = PlayerState.Hit;
        animator.SetInteger(State, (int)playerState);
        playerCollider.excludeLayers = (int)LayersEnum.Layers.Enemy;
        yield return new WaitForSeconds(0.3f);
        wasHit = false;
        yield return new WaitForSeconds(1f);
        playerCollider.excludeLayers = (int)LayersEnum.Layers.Nothing;
        
    }

}
