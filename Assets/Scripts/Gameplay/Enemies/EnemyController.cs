using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyData enemyDataSo;
    [SerializeField] private ParticleSystem enemyDeathParticles;

    public event Action onEnemyHit;

    private Collider2D collider;
    public static event Action onEnemyDie;
    private HealthSystem healthSystem;

    private void Awake ()
    {
        collider = GetComponent<Collider2D>();
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
        //Instantiate(enemyDeathParticles, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        onEnemyDie?.Invoke();

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.IsTouching(collider))
        {
            if (other.TryGetComponent(out HealthSystem healthSystem))
            {
                healthSystem.DoDamage(enemyDataSo.EnemyDamage);
            }
        }
    }
}
