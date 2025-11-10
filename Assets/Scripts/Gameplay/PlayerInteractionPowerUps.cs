using System;
using System.Collections;
using UnityEngine;

public class PlayerInteractionPowerUps : MonoBehaviour
{
    [SerializeField] private PowerUpsData powerUpsDataSo;
    [SerializeField] private PlayerDataSo playerDataSo;

    public static event Action<int, float> onPowerUpPickedUp;

    private HealthSystem healthSystem;
    private Coroutine damagePickedUpCoroutine;

    private void Awake()
    {
        playerDataSo.slashCurrentDamage = playerDataSo.slashDamage;
        playerDataSo.punchCurrentDamage = playerDataSo.punchDamage;
        healthSystem = GetComponent<HealthSystem>();
    }

    private void OnDisable()
    {
        if (damagePickedUpCoroutine != null)
        {
            StopCoroutine(damagePickedUpCoroutine);
            damagePickedUpCoroutine = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Layers.PowerUps)
        {
            Powerup powerup = collision.gameObject.GetComponent<Powerup>();
            switch (powerup.type)
            {
                case PlayerActionType.Invulnerability:
                    onPowerUpPickedUp?.Invoke((int)PlayerActionType.Invulnerability, powerUpsDataSo.invulnerabilityDuration);
                    powerup.PowerUpPickedUp();
                    break;

                case PlayerActionType.TripleJump:
                    onPowerUpPickedUp?.Invoke((int)PlayerActionType.TripleJump, powerUpsDataSo.tripleJumpDuration);
                    powerup.PowerUpPickedUp();
                    break;

                case PlayerActionType.Damage:
                    onPowerUpPickedUp?.Invoke((int)PlayerActionType.Damage, powerUpsDataSo.damageBuffDuration);
                    if (damagePickedUpCoroutine == null)
                        damagePickedUpCoroutine = StartCoroutine(DamagePickedUp(powerUpsDataSo.damageBuffDuration));
                    powerup.PowerUpPickedUp();
                    break;

                case PlayerActionType.Health:
                    healthSystem.Heal(powerUpsDataSo.healthAmount);
                    powerup.PowerUpPickedUp();
                    break;
            }
        }
    }
    private IEnumerator DamagePickedUp(int duration)
    {
        playerDataSo.slashCurrentDamage += powerUpsDataSo.damageBuff;
        playerDataSo.punchCurrentDamage += powerUpsDataSo.damageBuff;
        yield return new WaitForSeconds(duration);
        playerDataSo.slashCurrentDamage -= powerUpsDataSo.damageBuff;
        playerDataSo.punchCurrentDamage -= powerUpsDataSo.damageBuff;
    }
}
