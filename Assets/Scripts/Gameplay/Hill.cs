using System;
using UnityEngine;

public class Hill : MonoBehaviour
{
    [SerializeField] private PlayerPrefs playerPrefsSo;

    public static event Action onKeyBought;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)LayersEnum.Layers.Player)
        {
            if (playerPrefsSo.currentCoins >= 30)
            {
                playerPrefsSo.currentCoins -= 30;
                playerPrefsSo.playerCoins -= 30;
                gameObject.SetActive(false);
            }
        }
    }

}
