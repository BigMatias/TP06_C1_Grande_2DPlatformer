using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    [SerializeField] private PlayerDataSo playerDataSo;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == (int)LayersEnum.Layers.Enemy)
        {
            if (gameObject.layer == (int)LayersEnum.Layers.SlashAttack)
            {
                if (other.TryGetComponent(out HealthSystem healthSystem))
                {
                    healthSystem.DoDamage(playerDataSo.slashDamage);
                }
            }
            if (gameObject.layer == (int)LayersEnum.Layers.PunchAttack)
            {
                if (other.TryGetComponent(out HealthSystem healthSystem))
                {
                    healthSystem.DoDamage(playerDataSo.punchDamage);
                }
            }
        }   
    }
}
