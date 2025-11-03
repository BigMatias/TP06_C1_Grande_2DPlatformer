using System;
using UnityEngine;

public class KeyBlue : MonoBehaviour
{
    public static event Action onKeyBluePickedUp;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)LayersEnum.Layers.Player)
        {
            onKeyBluePickedUp?.Invoke();
            gameObject.SetActive(false);
        }
    }

}
