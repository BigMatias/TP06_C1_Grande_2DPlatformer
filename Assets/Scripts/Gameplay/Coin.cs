using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private PlayerPrefs playerPrefsSo;

    public static event Action onCoinPickedUp;

    private void Awake()
    {
        playerPrefsSo.currentCoins = 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == (int)LayersEnum.Layers.Player)
        {
            playerPrefsSo.playerCoins += 1;
            playerPrefsSo.currentCoins += 1;
            onCoinPickedUp?.Invoke();
            CoinPool.Instance.ReturnCoin(this.gameObject);
        }
    }
}
