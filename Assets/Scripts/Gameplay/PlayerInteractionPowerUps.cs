using System;
using System.Collections;
using UnityEngine;

public class PlayerInteractionPowerUps : MonoBehaviour
{
    [SerializeField] private PowerUpsDataSo powerUpsDataSo;
    [SerializeField] private PlayerDataSo playerDataSo;

    public static event Action<int, float> onPowerUpPickedUp;

    private HealthSystem healthSystem;
    private Coroutine powerUpPickedUpCoroutine;
    public bool invulnerabilityPickedUp = false;

    private void Awake()
    {
        playerDataSo.slashCurrentDamage = playerDataSo.slashDamage;
        playerDataSo.punchCurrentDamage = playerDataSo.punchDamage;
        healthSystem = GetComponent<HealthSystem>();
    }

    private void OnDisable()
    {
        if (powerUpPickedUpCoroutine != null)
        {
            StopCoroutine(powerUpPickedUpCoroutine);
            powerUpPickedUpCoroutine = null;
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
                    powerUpPickedUpCoroutine = StartCoroutine(PowerUpPickedUp((int)PlayerActionType.Invulnerability, powerUpsDataSo.damageBuffDuration));
                    break;
                case PlayerActionType.TripleJump:
                    onPowerUpPickedUp?.Invoke((int)PlayerActionType.TripleJump, powerUpsDataSo.tripleJumpDuration);
                    break;
                case PlayerActionType.Damage:
                    onPowerUpPickedUp?.Invoke((int)PlayerActionType.Damage, powerUpsDataSo.damageBuffDuration);
                    powerUpPickedUpCoroutine = StartCoroutine(PowerUpPickedUp((int)PlayerActionType.Damage, powerUpsDataSo.damageBuffDuration));
                    break;
                case PlayerActionType.Health:
                    onPowerUpPickedUp?.Invoke((int)PlayerActionType.Health, 0);
                    healthSystem.Heal(powerUpsDataSo.healthAmount);
                    break;
            }
            powerup.PowerUpPickedUp();
        }
    }
    private IEnumerator PowerUpPickedUp(int powerUp, int duration)
    {
        switch (powerUp)
        {
            case (int)PlayerActionType.Damage:
                playerDataSo.slashCurrentDamage += powerUpsDataSo.damageBuff;
                playerDataSo.punchCurrentDamage += powerUpsDataSo.damageBuff;
                yield return new WaitForSeconds(duration);
                playerDataSo.slashCurrentDamage -= powerUpsDataSo.damageBuff;
                playerDataSo.punchCurrentDamage -= powerUpsDataSo.damageBuff;
                break;
            case (int)PlayerActionType.Invulnerability:
                invulnerabilityPickedUp = true;
                yield return new WaitForSeconds(duration);
                invulnerabilityPickedUp = false;
                break;
        }
    }
}
