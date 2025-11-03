using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyData enemyDataSo;
    [SerializeField] private ParticleSystem enemyDeathParticles;

    public event Action onEnemyHit;
    public static event Action onPlayerHit;
    public static event Action onEnemyDie;

    private Collider2D collider;
    private AudioSource audioSource;
    private HealthSystem healthSystem;

    private void Awake ()
    {
        collider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        healthSystem = GetComponent<HealthSystem>();   
        healthSystem.onDamageDealt += HealthSystem_onDamageDealt;
        healthSystem.onDie += HealthSystem_onDie;
    }

    private void HealthSystem_onDamageDealt()
    {
        onEnemyHit?.Invoke();
    }

    private void HealthSystem_onDie()
    {
        audioSource.PlayOneShot(enemyDataSo.EnemyDead);
        ActivateCoins();
        //Instantiate(enemyDeathParticles, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        onEnemyDie?.Invoke();

    }

    private void ActivateCoins()
    {
        int randomQuantity = UnityEngine.Random.Range(1 , 6);
        for (int i = 0; i <= randomQuantity - 1; i++)
        {
            GameObject newCoin = CoinPool.Instance.GetCoin();
            newCoin.transform.position = gameObject.transform.position;
            Rigidbody2D coinRb = newCoin.gameObject.GetComponent<Rigidbody2D>();
            coinRb.velocity = Vector2.zero;

            float direction = UnityEngine.Random.Range(-1, 2);
            float knockbackForceX = UnityEngine.Random.Range(1f, 4f);
            float knockbackForceY = UnityEngine.Random.Range(1f, 4f);
            Vector2 knockbackDir = new Vector2(direction * knockbackForceX, knockbackForceY);

            coinRb.AddForce(knockbackDir, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.IsTouching(collider))
        {
            if (other.TryGetComponent(out HealthSystem healthSystem))
            {
                onPlayerHit?.Invoke();
                healthSystem.DoDamage(enemyDataSo.EnemyDamage);
            }
        }
    }
}
