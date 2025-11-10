using System;
using UnityEngine;

public class KeyGreen : MonoBehaviour
{
    public static event Action onKeyGreenPickedUp;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)Layers.Player)
        {
            onKeyGreenPickedUp?.Invoke();
            gameObject.SetActive(false);
        }
    }

}
