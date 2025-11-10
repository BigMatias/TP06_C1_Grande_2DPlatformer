using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerDataSo playerDataSo;
    [SerializeField] private AudioClip playerHitAudio;
    [SerializeField] private AudioClip playerDeadAudio;
    [SerializeField] private AudioClip playerJumpAudio;

    [Header("Ground check")]
    [SerializeField] private Transform groundCheck;
    private float groundRadius = 0.2f;

    public static event Action onPlayerDied;
    public static event Action<float, int> onPlayerPunch;

    private Rigidbody2D rb;
    private HealthSystem healthSystem;
    private AudioSource audioSource;
    private Animator animator;
    private SpriteRenderer sprite;
    private GameObject[] attacks;

    private bool isSlashOnCd;
    private bool isPunchOnCd;
    private bool wasHit;
    private bool isInvulnerable;

    private int jumpQuantity;
    private int jumpsLeft;

    private Coroutine powerUpPickedUp;

    // Keys / Locks
    private bool keyBluePickedUp = false;
    private bool keyRedPickedUp = false;
    private bool keyGreenPickedUp = false;
    private bool keyYellowPickedUp = false;

    private bool doorOpened = false;
    public static event Action onFinishedGame;

    public static event Action onLockBlueOpened;
    public static event Action onLockRedOpened;
    public static event Action onLockGreenOpened;
    public static event Action onLockYellowOpened;
    public static event Action onAllLocksOpened;

    [SerializeField] private LayerMask groundLayer;

    private static readonly int State = Animator.StringToHash("State");

    enum PlayerState
    {
        Idle,
        Jump,
        Run,
        Hit,
        RunHit,
        JumpHit,
        IdleHit
    }

    [SerializeField] private PlayerState playerState = PlayerState.Idle;

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    private void Awake()
    {
        KeyYellow.onKeyYellowPickedUp += KeyYellow_onKeyYellowPickedUp;
        KeyBlue.onKeyBluePickedUp += KeyBlue_onKeyBluePickedUp;
        KeyGreen.onKeyGreenPickedUp += KeyGreen_onKeyGreenPickedUp;
        KeyRed.onKeyRedPickedUp += KeyRed_onKeyRedPickedUp;

        PlayerInteractionPowerUps.onPowerUpPickedUp += PlayerInteractionPowerUps_onPowerUpPickedUp;

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.onDie += HealthSystem_onDie;
        EnemyController.onPlayerHit += EnemyController_onPlayerHit;

        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        attacks = new GameObject[2];
    }



    private void Start()
    {
        animator.SetInteger(State, (int)playerState);
        rb.gravityScale = playerDataSo.GravityScaleJump;
        jumpQuantity = playerDataSo.JumpQuantity;
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
            if (isInvulnerable)
            {
                playerState = PlayerState.JumpHit;
                animator.SetInteger(State, (int)playerState);
            }
            else if (!isInvulnerable)
            {
                playerState = PlayerState.Jump;
                animator.SetInteger(State, (int)playerState);
            }
        }
        if (IsGrounded() && rb.velocity.x == 0 && !wasHit)
        {
            if (isInvulnerable)
            {
                playerState = PlayerState.IdleHit;
                animator.SetInteger(State, (int)playerState);
            }
            else if (!isInvulnerable)
            {
                playerState = PlayerState.Idle;
                animator.SetInteger(State, (int)playerState);
            }
        }
        if (IsGrounded())//&& groundTime <= 0.1)
        {
            Debug.Log(jumpsLeft);
            jumpsLeft = jumpQuantity;
        }
        Jump();
    }

    private void FixedUpdate()
    {
        if (!wasHit)
        {
            MovePlayer();
        }
    }

    private void OnDestroy()
    {
        KeyYellow.onKeyYellowPickedUp -= KeyYellow_onKeyYellowPickedUp;
        KeyBlue.onKeyBluePickedUp -= KeyBlue_onKeyBluePickedUp;
        KeyGreen.onKeyGreenPickedUp -= KeyGreen_onKeyGreenPickedUp;
        KeyRed.onKeyRedPickedUp -= KeyRed_onKeyRedPickedUp;

        EnemyController.onPlayerHit -= EnemyController_onPlayerHit;
        PlayerInteractionPowerUps.onPowerUpPickedUp -= PlayerInteractionPowerUps_onPowerUpPickedUp;
        healthSystem.onDie -= HealthSystem_onDie;
    }

    private void OnDisable()
    {
        if (powerUpPickedUp != null)
        {
            StopCoroutine(powerUpPickedUp);
            powerUpPickedUp = null;
        }
    }

    private void HealthSystem_onDie()
    {
        audioSource.PlayOneShot(playerDeadAudio);
        onPlayerDied?.Invoke();
    }

    private void EnemyController_onPlayerHit(Transform enemyTransform)
    {
        StartCoroutine(WasHit(enemyTransform));
    }
    private void PlayerInteractionPowerUps_onPowerUpPickedUp(int id, float cooldownTime)
    {
        switch (id)
        {
            case (int)PlayerActionType.TripleJump:
                int tripleJump = (int)PlayerActionType.TripleJump;
                powerUpPickedUp = StartCoroutine(PowerUpPickedUp(tripleJump, cooldownTime));
                break;
        }
    }


    private void KeyYellow_onKeyYellowPickedUp()
    {
        keyYellowPickedUp = true;
    }

    private void KeyBlue_onKeyBluePickedUp()
    {
        keyBluePickedUp = true;
    }

    private void KeyGreen_onKeyGreenPickedUp()
    {
        keyGreenPickedUp = true;
    }
    private void KeyRed_onKeyRedPickedUp()
    {
        keyRedPickedUp = true;
    }


    private void InstantiateAttacks()
    {
        for (int i = 0; i <= attacks.Length - 1; i++)
        {
            attacks[i] = Instantiate(playerDataSo.playerAttacks[i], transform.Find("Attacks"));
            attacks[i].SetActive(false);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(playerDataSo.Jump) && jumpsLeft > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.velocity += Vector2.up * playerDataSo.JumpSpeed;
            audioSource.PlayOneShot(playerJumpAudio);
            jumpsLeft--;
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
            if (IsGrounded())
            {
                if (isInvulnerable)
                {
                    playerState = PlayerState.RunHit;
                    animator.SetInteger(State, (int)playerState);
                }
                else if (!isInvulnerable)
                {
                    playerState = PlayerState.Run;
                    animator.SetInteger(State, (int)playerState);
                }
            }
            velocity.x = -playerDataSo.Speed;
        }
        else if (Input.GetKey(playerDataSo.Right))
        {
            sprite.flipX = false;
            if (IsGrounded())
            {
                if (isInvulnerable)
                {
                    playerState = PlayerState.RunHit;
                    animator.SetInteger(State, (int)playerState);
                }
                else if (!isInvulnerable)
                {
                    playerState = PlayerState.Run;
                    animator.SetInteger(State, (int)playerState);
                }
            }
            velocity.x = playerDataSo.Speed;
        }
        else
        {
            velocity.x = 0f;
        }



        rb.velocity = velocity;
    }

    private void Attack()
    {
        if (Input.GetKey(playerDataSo.SlashAttack) && !isSlashOnCd)
        {
            attacks[0].SetActive(true);
            audioSource.PlayOneShot(playerDataSo.swooshAttackSound);
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
            audioSource.PlayOneShot(playerDataSo.swooshAttackSound);

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
            onPlayerPunch?.Invoke(playerDataSo.punchCooldown, (int)PlayerActionType.Punch);
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "LockYellow":
                {
                    if (keyYellowPickedUp)
                    {
                        onLockYellowOpened?.Invoke();
                    }
                    break;
                }
            case "LockBlue":
                {
                    if (keyBluePickedUp)
                    {
                        onLockBlueOpened?.Invoke();
                    }
                    break;
                }
            case "LockGreen":
                {
                    if (keyGreenPickedUp)
                    {
                        onLockGreenOpened?.Invoke();
                    }
                    break;
                }
            case "LockRed":
                {
                    if (keyRedPickedUp)
                    {
                        onLockRedOpened?.Invoke();
                    }
                    break;
                }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Layers.Door)
        {
            if (doorOpened)
            {
                onFinishedGame?.Invoke();
            }
        }
        if (collision.gameObject.layer == (int)Layers.FallDeadPoint)
        {
            onPlayerDied?.Invoke();
        }
    }


    private IEnumerator PowerUpPickedUp(int powerUp, float duration)
    {
        switch (powerUp)
        {
            case (int)PlayerActionType.TripleJump:
                jumpQuantity = 2;
                yield return new WaitForSeconds(duration);
                jumpQuantity = 1;
                break;
        }

    }

    private IEnumerator WasHit(Transform enemyTransform)
    {
        audioSource.PlayOneShot(playerHitAudio);

        wasHit = true;
        isInvulnerable = true;
        float direction = (transform.position.x - enemyTransform.position.x) >= 0 ? 1f : -1f;
        rb.velocity = Vector2.zero;
        float knockbackForceX = 8f;
        float knockbackForceY = 7f;
        Vector2 knockbackDir = new Vector2(direction * knockbackForceX, knockbackForceY);

        rb.AddForce(knockbackDir, ForceMode2D.Impulse);
        playerState = PlayerState.Hit;
        animator.SetInteger(State, (int)playerState);
        gameObject.layer = (int)Layers.PlayerInvulnerable;
        yield return new WaitForSeconds(0.3f);
        wasHit = false;
        yield return new WaitForSeconds(playerDataSo.InvulnerabilityAfterHit);
        isInvulnerable = false;
        gameObject.layer = (int)Layers.Player;

    }
}
