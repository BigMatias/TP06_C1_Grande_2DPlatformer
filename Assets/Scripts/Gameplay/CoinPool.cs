using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    public static CoinPool Instance;

    [Header("Configuración del pool")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int initialSize = 30;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.Find("EnemyCoins"));
            coin.SetActive(false);
            pool.Enqueue(coin);
        }
    }

    public GameObject GetCoin()
    {
        if (pool.Count == 0)
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.SetActive(false);
            pool.Enqueue(coin);
        }

        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnCoin(GameObject coin)
    {
        coin.SetActive(false);
        pool.Enqueue(coin);
    }
}
