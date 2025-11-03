using System;
using UnityEngine;

public class KeyYellow : MonoBehaviour
{
    public static event Action onKeyYellowPickedUp;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)LayersEnum.Layers.Player)
        {
            onKeyYellowPickedUp?.Invoke();
            gameObject.SetActive(false);
        }
    }

}
