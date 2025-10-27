using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class MoveBee : MonoBehaviour
{
    [SerializeField] private GameObject visionRange;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Transform playerTransform;

    private EnemyVisionRange enemyVisionRange;
    private Rigidbody2D rb;
    private EnemyController enemyController;

    private bool playerSighted = false;
    private bool inUsualPathing;
    private Coroutine followCoroutine;
    private Coroutine returnToOriginPoint;
    private Coroutine usualPathing;
    private Coroutine hitBee;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        enemyController.onEnemyHit += EnemyController_onEnemyHit;
        rb = GetComponent<Rigidbody2D>();
        enemyVisionRange = visionRange.gameObject.GetComponent<EnemyVisionRange>();
        enemyVisionRange.onPlayerSighted += EnemyVisionRange_onPlayerSighted;
    }


    private void OnDestroy()
    {
        enemyVisionRange.onPlayerSighted -= EnemyVisionRange_onPlayerSighted;
    }

    private void OnEnable()
    {
        if (followCoroutine == null)
            followCoroutine = StartCoroutine(MoveBeeTowardsPlayer());

        if (returnToOriginPoint == null)
            returnToOriginPoint = StartCoroutine(ReturnToOriginPoint());

        if (usualPathing == null)
            usualPathing = StartCoroutine(UsualPathing());


    }

    private void OnDisable()
    {
        if (followCoroutine != null)
        {
            StopCoroutine(followCoroutine);
            followCoroutine = null;
        }
        if (returnToOriginPoint != null)
        {
            StopCoroutine(returnToOriginPoint);
            returnToOriginPoint = null;
        }
        if (usualPathing != null)
        {
            StopCoroutine(usualPathing);
            usualPathing = null;
        }
        if (hitBee != null)
        {
            StopCoroutine(hitBee);
            hitBee = null;
        }
    }
    private void EnemyController_onEnemyHit()
    {
        if (hitBee == null)
            hitBee = StartCoroutine(HitBee());
    }

    private void EnemyVisionRange_onPlayerSighted(bool sighted)
    {
        playerSighted = sighted;
    }

    private IEnumerator MoveBeeTowardsPlayer()
    {
        while (gameObject.activeSelf)
        {
            if (playerSighted)
            {
                Vector2 newPosition = Vector2.MoveTowards(
                    transform.position,
                    playerTransform.position,
                    enemyData.EnemySpeed * Time.fixedDeltaTime
                );

                rb.MovePosition(newPosition);
                inUsualPathing = false;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator ReturnToOriginPoint()
    {
        Vector2 startPosition = transform.position;
        while (gameObject.activeSelf)
        {
            if (!playerSighted)
            {
                Vector2 goOriginalPosition = Vector2.MoveTowards(transform.position, startPosition, enemyData.EnemySpeed * Time.fixedDeltaTime);
                rb.MovePosition(goOriginalPosition);
                if (Vector3.Distance(transform.position, new Vector3(goOriginalPosition.x, goOriginalPosition.y, 0)) < 0.05f && !inUsualPathing)
                {
                    inUsualPathing = true;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator UsualPathing()
    {
        
        Vector2 startPosition = transform.position;
        Vector2 leftPosition = new Vector2(startPosition.x - enemyData.DistanceOnSurveillance, startPosition.y);
        Vector2 rightPosition = new Vector2(startPosition.x + enemyData.DistanceOnSurveillance, startPosition.y);

        bool goingLeft = true;         

        while (gameObject.activeSelf)
        {
            if (inUsualPathing && !playerSighted)
            {
                Vector2 target = goingLeft ? leftPosition : rightPosition;
                Vector2 newPosition = Vector2.MoveTowards(rb.position, target, enemyData.EnemySpeed * Time.fixedDeltaTime);

                rb.MovePosition(newPosition);

                if (Vector2.Distance(rb.position, target) < 0.05f)
                {
                    goingLeft = !goingLeft; 

                    float flipX = goingLeft ? 1.3f : -1.3f;
                    transform.localScale = new Vector3(flipX, 1.3f, 1.3f);
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator HitBee()
    {
        StopCoroutine(usualPathing);
        StopCoroutine(returnToOriginPoint);

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
        usualPathing = StartCoroutine(UsualPathing());
        returnToOriginPoint = StartCoroutine(ReturnToOriginPoint());
        rb.MovePosition(targetPos);
    }
}
