using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject doorOpen;
    [SerializeField] private GameObject doorClosed;

    private bool doorOpened = false;
    public static event Action onGameFinished;

    private void Awake()
    {
        Locks.onAllLocksOpened += Locks_onAllLocksOpened;
    }

    private void OnDestroy()
    {
        Locks.onAllLocksOpened -= Locks_onAllLocksOpened;
    }

    private void Locks_onAllLocksOpened()
    {
        doorClosed.gameObject.SetActive(false);
        doorOpen.gameObject.SetActive(true);
        doorOpened = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Layers.Player && doorOpened)
        {
            onGameFinished?.Invoke();
        }
    }

}
