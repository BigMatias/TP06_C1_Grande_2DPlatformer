using System;
using UnityEngine;

public class Locks : MonoBehaviour
{
    [SerializeField] private GameObject[] locks;
    public static event Action onAllLocksOpened;

    private int openedLocksCount = 0;
    private int totalLocks;

    private void Awake()
    {
        totalLocks = locks.Length;

        PlayerMovement.onLockYellowOpened += PlayerMovement_onLockYellowOpened;
        PlayerMovement.onLockBlueOpened += PlayerMovement_onLockBlueOpened;
        PlayerMovement.onLockGreenOpened += PlayerMovement_onLockGreenOpened;
        PlayerMovement.onLockRedOpened += PlayerMovement_onLockRedOpened;
    }

    private void OnDestroy()
    {
        PlayerMovement.onLockYellowOpened -= PlayerMovement_onLockYellowOpened;
        PlayerMovement.onLockBlueOpened -= PlayerMovement_onLockBlueOpened;
        PlayerMovement.onLockGreenOpened -= PlayerMovement_onLockGreenOpened;
        PlayerMovement.onLockRedOpened -= PlayerMovement_onLockRedOpened;
    }

    private void RegisterLockOpened()
    {
        openedLocksCount++;

        if (openedLocksCount >= totalLocks)
        {
            onAllLocksOpened?.Invoke();
        }
    }

    private void PlayerMovement_onLockYellowOpened()
    {
        RegisterLockOpened();
        locks[0].gameObject.SetActive(false);
    }

    private void PlayerMovement_onLockBlueOpened()
    {
        RegisterLockOpened();
        locks[1].gameObject.SetActive(false);
    }

    private void PlayerMovement_onLockGreenOpened()
    {
        RegisterLockOpened();
        locks[2].gameObject.SetActive(false);
    }

    private void PlayerMovement_onLockRedOpened()
    {
        RegisterLockOpened();
        locks[3].gameObject.SetActive(false);
    }

}
