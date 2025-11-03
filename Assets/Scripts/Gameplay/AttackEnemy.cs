using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    [SerializeField] private PlayerDataSo playerDataSo;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == (int)LayersEnum.Layers.Enemy)
        {
            if (gameObject.layer == (int)LayersEnum.Layers.SlashAttack)
            {
                audioSource.PlayOneShot(playerDataSo.slashAttackConnected);
                if (other.TryGetComponent(out HealthSystem healthSystem))
                {
                    Debug.Log("Slashed");
                    healthSystem.DoDamage(playerDataSo.slashDamage);
                }
            }
            if (gameObject.layer == (int)LayersEnum.Layers.PunchAttack)
            {
                audioSource.PlayOneShot(playerDataSo.punchAttackConnected);
                if (other.TryGetComponent(out HealthSystem healthSystem))
                {
                    Debug.Log("Punched");
                    healthSystem.DoDamage(playerDataSo.punchDamage);
                }
            }
        }   
    }
}
