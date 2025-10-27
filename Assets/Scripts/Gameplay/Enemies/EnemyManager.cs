using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemySpawnData enemySpawnDataSo;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject[] EnemiesBee;
    [SerializeField] private GameObject[] EnemiesMouse;
    [SerializeField] private GameObject[] EnemiesBlock;
    [SerializeField] private GameObject[] EnemiesFrog;
    [SerializeField] private GameObject beeVision;
    [SerializeField] private GameObject mouseVision;
    [SerializeField] private GameObject blockVision;
    [SerializeField] private GameObject frogVision;
    
    private EnemyVisionRange enemyVisionRange;

    private enum Enemies
    {
        Bee,
        Block,
        Frog,
        Mouse
    }

    /*private float enemyTimeSpawn = 0;
    private float enemyCurrentRandomSpawn = 0f;
    private int enemySpawnQuantity = 0;
    private int enemiesSpawned = 0;
    private int enemiesStillAlive = 0;*/

    private void Awake()
    {
        enemyVisionRange = GetComponent<EnemyVisionRange>();
        //EnemyController.onEnemyDie += EnemyController_onEnemyDie;
    }

    void Start()
    {
        for (int i = 0; i < EnemiesBee.Length - 1; i++)
        {

        }

        /*enemySpawnQuantity = UnityEngine.Random.Range(5, 11);
        enemyCommons = new GameObject[5];
        enemyFliers = new GameObject[5];

        for (int i = 0; i <= enemyCommons.Length - 1; i++)
        {
            GameObject newGameObject = Instantiate(enemySpawnDataSo.Enemies[0], transform);
            newGameObject.name = "EnemyCommon" + i;
            enemyCommons[i] = newGameObject;
            enemyCommons[i].SetActive(false);
        }
        for (int i = 0; i <= enemyFliers.Length - 1; i++)
        {
            GameObject newGameObject = Instantiate(enemySpawnDataSo.Enemies[1], transform);
            newGameObject.name = "EnemyFlier" + i;
            enemyFliers[i] = newGameObject;
            enemyFliers[i].SetActive(false);
        }*/
    }

    void Update()
    {
        /*// Spawner
        enemyTimeSpawn += Time.deltaTime;
        if (enemyTimeSpawn > enemyCurrentRandomSpawn && enemiesSpawned <= enemySpawnQuantity)
        {
            enemyCurrentRandomSpawn = UnityEngine.Random.Range(enemySpawnDataSo.EnemyMinRandomSpawn, enemySpawnDataSo.EnemyMaxRandomSpawn);
            enemyTimeSpawn = 0;
            SetEnemyActive();
            enemiesSpawned += 1;
        }

        // Round Over
        if (enemiesStillAlive == 0)
        {
            enemiesSpawned = 0;
            onRoundOver?.Invoke();
            enemySpawnQuantity += UnityEngine.Random.Range(5, 11);
        }*/
    }

    private void FixedUpdate()
    {
    }

    /*private void EnemyController_onEnemyDie()
    {
        enemiesStillAlive -= 1;
    }*/

    /*private void SetEnemyActive()
    {
        float choice = UnityEngine.Random.Range(0f, 2f);
        if (choice >= 0 && choice <= 1)
        {
            CheckForExistingEnemies(enemyCommons, groundSpawns);
        }
        if (choice >= 1.1 && choice <= 2)
        {
            CheckForExistingEnemies(enemyFliers, airSpawns);   
        }
    }*/

    private void CheckForExistingEnemies(GameObject[] gameObjectArr, GameObject[] spawns)
    {
        for (int i = 0; gameObjectArr.Length - 1 >= i; i++)
        {
            if (gameObjectArr[i].activeSelf == false)
            {
                float randomSpawn = UnityEngine.Random.Range(0f, 2f);
                if (randomSpawn >= 0 && randomSpawn <= 1)
                {
                    if (spawns[0].transform.position.x < enemySpawnDataSo.SpawnLimitLeft || spawns[0].transform.position.x > enemySpawnDataSo.SpawnLimitRight)
                    {
                        CheckForExistingEnemies(gameObjectArr, spawns);
                    }
                    else
                    {
                        gameObjectArr[i].transform.position = new Vector3(spawns[0].transform.position.x, spawns[0].transform.position.y, 0f);
                    }
                }
                else
                {
                    if (spawns[1].transform.position.x < enemySpawnDataSo.SpawnLimitLeft || spawns[1].transform.position.x > enemySpawnDataSo.SpawnLimitRight)
                    {
                        CheckForExistingEnemies(gameObjectArr, spawns);
                    }
                    else
                    {
                        gameObjectArr[i].transform.position = new Vector3(spawns[1].transform.position.x, spawns[1].transform.position.y, 0f);
                    }
                }
                gameObjectArr[i].GetComponent<HealthSystem>().ResetLife();
                gameObjectArr[i].SetActive(true);
                break;
            }
        }
    }
}
