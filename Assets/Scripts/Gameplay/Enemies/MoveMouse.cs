using System.Collections;
using UnityEngine;

public class MoveMouse : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Transform playerTransform;

    private EnemyController enemyController;
    private Rigidbody2D rb;
    private Animator animator;
    private Coroutine moveMouse;
    private Coroutine hitMouse;

    private static readonly int State = Animator.StringToHash("State");
    enum MouseState
    {
        Walk,
        Sleep
    }

    [SerializeField] private MouseState mouseState = MouseState.Walk;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        enemyController.onEnemyHit += EnemyController_onEnemyHit;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetInteger(State, (int)mouseState);
    }

    private void OnEnable()
    {
        if (moveMouse == null)
            moveMouse = StartCoroutine(MovingMouse());
    }

    private void OnDisable()
    {
        if (moveMouse != null)
        {
            StopCoroutine(moveMouse);
            moveMouse = null;
        }
        if (hitMouse != null)
        {
            StopCoroutine(hitMouse);
            hitMouse = null;
        }

    }
    private void EnemyController_onEnemyHit()
    {
        if (hitMouse != null)
        {
            StopCoroutine(hitMouse);
            hitMouse = null;
        }

        hitMouse = StartCoroutine(HitMouse());
    }

    private IEnumerator MovingMouse()
    {
        Vector2 startPosition = transform.position;
        Vector2 leftPosition = new Vector2(startPosition.x - enemyData.DistanceOnSurveillance, startPosition.y);
        Vector2 rightPosition = new Vector2(startPosition.x + enemyData.DistanceOnSurveillance, startPosition.y);

        bool goingLeft = true;

        while (gameObject.activeSelf)
        {
            Vector2 target = goingLeft ? leftPosition : rightPosition;
            Vector2 newPosition = Vector2.MoveTowards(rb.position, target, enemyData.EnemySpeed * Time.fixedDeltaTime);

            rb.MovePosition(newPosition);

            if (Vector2.Distance(rb.position, target) < 0.05f)
            {
                goingLeft = !goingLeft;
            }
            float flipX = goingLeft ? 1.3f : -1.3f;
            transform.localScale = new Vector3(flipX, 1.3f, 1.3f);
            
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator HitMouse()
    {
        StopCoroutine(moveMouse);

        float direction = (transform.position.x - playerTransform.position.x) >= 0 ? 1f : -1f;

        float knockbackDistance = 1.2f;
        float knockbackTime = 0.15f;

        Vector2 startPos = transform.position;
        Vector2 targetPos = startPos + new Vector2(direction * knockbackDistance, 0f);

        float elapsed = 0f;

        while (elapsed < knockbackTime)
        {
            elapsed += Time.fixedDeltaTime;
            float t = elapsed / knockbackTime;

            Vector2 newPos = Vector2.Lerp(startPos, targetPos, t);
            rb.MovePosition(newPos);

            yield return new WaitForFixedUpdate();
        }

        moveMouse = StartCoroutine(MovingMouse());
        rb.MovePosition(targetPos);
    }
}
