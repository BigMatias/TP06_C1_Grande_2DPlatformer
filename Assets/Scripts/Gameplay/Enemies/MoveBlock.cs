using System.Collections;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    [SerializeField] private GameObject visionRange;
    [SerializeField] private EnemyDataSo enemyData;
    [SerializeField] private Transform playerTransform;

    private EnemyVisionRange enemyVisionRange;
    private Rigidbody2D rb;
    private Animator animator;

    private bool playerSighted = false;
    private Coroutine blockFall;
    private EnemyController enemyController;

    enum BlockState
    {
        Sleep,
        Idle,
        Fall
    }

    private static readonly int State = Animator.StringToHash("State");
    [SerializeField] private BlockState blockState = BlockState.Sleep;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        enemyVisionRange = visionRange.gameObject.GetComponent<EnemyVisionRange>();
        enemyVisionRange.onPlayerSighted += EnemyVisionRange_onPlayerSighted;
    }

    private void Start()
    {
        animator.SetInteger(State, (int)blockState);
    }

    private void OnDestroy()
    {
        enemyVisionRange.onPlayerSighted -= EnemyVisionRange_onPlayerSighted;
    }

    private void OnEnable()
    {
        if (blockFall == null)
            blockFall = StartCoroutine(BlockFall());
    }

    private void OnDisable()
    {
        if (blockFall != null)
        {
            StopCoroutine(blockFall);
            blockFall = null;
        }

    }

    private void EnemyVisionRange_onPlayerSighted(bool sighted)
    {
        playerSighted = sighted;
    }

    private IEnumerator BlockFall()
    {
        while (gameObject.activeSelf)
        {
            if (playerSighted)
            {
                blockState = BlockState.Idle;
                animator.SetInteger(State, (int)blockState);

                yield return new WaitForSeconds(0.3f);

                blockState = BlockState.Fall;
                animator.SetInteger(State, (int)blockState);

                Vector2 floor = new Vector2(transform.position.x, transform.position.y - 2f);

                while (Vector2.Distance(transform.position, floor) > 0.05f)
                {
                    Vector2 fallPos = Vector2.MoveTowards(transform.position, floor, enemyData.BlockFallSpeed * Time.fixedDeltaTime);
                    rb.MovePosition(fallPos);
                    yield return new WaitForFixedUpdate();
                }

                yield return new WaitForSeconds(1f);

                blockState = BlockState.Idle;
                animator.SetInteger(State, (int)blockState);

                Vector2 upDir = new Vector2(transform.position.x, transform.position.y + 2f);

                while (Vector2.Distance(transform.position, upDir) > 0.05f)
                {
                    Vector2 goUp = Vector2.MoveTowards(transform.position, upDir, enemyData.BlockFallSpeed * Time.fixedDeltaTime);
                    rb.MovePosition(goUp);
                    yield return new WaitForFixedUpdate();
                }
                blockState = BlockState.Sleep;
                animator.SetInteger(State, (int)blockState);
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
