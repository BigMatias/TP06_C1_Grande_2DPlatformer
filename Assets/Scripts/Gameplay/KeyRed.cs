using System;
using UnityEngine;

public class KeyRed : MonoBehaviour
{
    public static event Action onKeyRedPickedUp;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)Layers.Player)
        {
            onKeyRedPickedUp?.Invoke();
            gameObject.SetActive(false);
        }
    }

}
