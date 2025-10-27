using System;
using UnityEngine;

public class EnemyVisionRange : MonoBehaviour
{
    public event Action <bool> onPlayerSighted;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)LayersEnum.Layers.Player)
            onPlayerSighted?.Invoke(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)LayersEnum.Layers.Player)
            onPlayerSighted?.Invoke(false);
    }
}
