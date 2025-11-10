using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    [SerializeField] private PlayerDataSo playerDataSo;
    [SerializeField] private PowerUpsData powerUpsDataSo;
    [SerializeField] private TypeOfAttack typeOfAttack;

    private enum TypeOfAttack
    {
        Slash,
        Punch
    }

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == (int)Layers.Enemy)
        {
            if (typeOfAttack == TypeOfAttack.Slash)
            {
                audioSource.PlayOneShot(playerDataSo.slashAttackConnected);
                if (other.TryGetComponent(out HealthSystem healthSystem))
                {
                    healthSystem.DoDamage(playerDataSo.slashCurrentDamage);
                }
            }
            if (typeOfAttack == TypeOfAttack.Punch)
            {
                audioSource.PlayOneShot(playerDataSo.punchAttackConnected);
                if (other.TryGetComponent(out HealthSystem healthSystem))
                {
                    healthSystem.DoDamage(playerDataSo.punchCurrentDamage);
                }
            }
        }
    }
}
