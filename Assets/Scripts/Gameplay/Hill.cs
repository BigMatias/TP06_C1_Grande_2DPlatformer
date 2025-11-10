using System;
using UnityEngine;

public class Hill : MonoBehaviour
{
    [SerializeField] private PlayerPrefsSo playerPrefsSo;
    [SerializeField] private GameDataSo gameDataSo;

    public static event Action<int> onKeyBought;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)Layers.Player)
        {
            if (playerPrefsSo.currentCoins >= gameDataSo.keyPrice)
            {
                onKeyBought?.Invoke(gameDataSo.keyPrice);
                playerPrefsSo.currentCoins -= gameDataSo.keyPrice;
                playerPrefsSo.playerCoins -= gameDataSo.keyPrice;
                gameObject.SetActive(false);
            }
        }
    }

}
