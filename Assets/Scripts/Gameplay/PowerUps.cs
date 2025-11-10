using UnityEngine;

public partial class Powerup : MonoBehaviour
{
    public PlayerActionType type;

    public void PowerUpPickedUp()
    {
        gameObject.SetActive(false);
    }
}