using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] enemyAudios;
    [SerializeField] private AudioClip keyPickedUp;
    [SerializeField] private AudioClip lockOpened;
    [SerializeField] private AudioClip coinPickedUp;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource audioSourceSfx;

    private void Awake()
    {
        KeyYellow.onKeyYellowPickedUp += KeyYellow_onKeyYellowPickedUp;
        KeyBlue.onKeyBluePickedUp += KeyBlue_onKeyBluePickedUp;
        KeyGreen.onKeyGreenPickedUp += KeyGreen_onKeyGreenPickedUp;
        KeyRed.onKeyRedPickedUp += KeyRed_onKeyRedPickedUp;

        PlayerMovement.onLockYellowOpened += PlayerMovement_onLockYellowOpened; 
        PlayerMovement.onLockBlueOpened += PlayerMovement_onLockBlueOpened; 
        PlayerMovement.onLockGreenOpened += PlayerMovement_onLockGreenOpened;
        PlayerMovement.onLockRedOpened += PlayerMovement_onLockRedOpened;

        Coin.onCoinPickedUp += Coin_onCoinPickedUp;
        PlayerMovement.onPlayerDied += PlayerMovement_playerDied;
        EnemyController.onEnemyDie += EnemyController_onEnemyDie;
    }


    private void OnDestroy()
    {
        KeyYellow.onKeyYellowPickedUp -= KeyYellow_onKeyYellowPickedUp;
        KeyBlue.onKeyBluePickedUp -= KeyBlue_onKeyBluePickedUp;
        KeyGreen.onKeyGreenPickedUp -= KeyGreen_onKeyGreenPickedUp;
        KeyRed.onKeyRedPickedUp -= KeyRed_onKeyRedPickedUp;

        PlayerMovement.onLockYellowOpened -= PlayerMovement_onLockYellowOpened;
        PlayerMovement.onLockBlueOpened -= PlayerMovement_onLockBlueOpened;
        PlayerMovement.onLockGreenOpened -= PlayerMovement_onLockGreenOpened;
        PlayerMovement.onLockRedOpened -= PlayerMovement_onLockRedOpened;

        Coin.onCoinPickedUp -= Coin_onCoinPickedUp;
        PlayerMovement.onPlayerDied -= PlayerMovement_playerDied;
        EnemyController.onEnemyDie -= EnemyController_onEnemyDie;
    }
    private void PlayerMovement_onLockYellowOpened()
    {
        audioSourceSfx.PlayOneShot(lockOpened);
    }

    private void PlayerMovement_onLockBlueOpened()
    {
        audioSourceSfx.PlayOneShot(lockOpened);
    }
    private void PlayerMovement_onLockGreenOpened()
    {
        audioSourceSfx.PlayOneShot(lockOpened);
    }

    private void PlayerMovement_onLockRedOpened()
    {
        audioSourceSfx.PlayOneShot(lockOpened);
    }

    private void Coin_onCoinPickedUp()
    {
        audioSourceSfx.PlayOneShot(coinPickedUp);
    }
    private void KeyRed_onKeyRedPickedUp()
    {
        audioSourceSfx.PlayOneShot(keyPickedUp);
    }

    private void KeyGreen_onKeyGreenPickedUp()
    {
        audioSourceSfx.PlayOneShot(keyPickedUp);
    }

    private void KeyYellow_onKeyYellowPickedUp()
    {
        audioSourceSfx.PlayOneShot(keyPickedUp);
    }

    private void KeyBlue_onKeyBluePickedUp()
    {
        audioSourceSfx.PlayOneShot(keyPickedUp);
    }

    private void PlayerMovement_playerDied()
    {
        audioSourceMusic.Stop();
    }

    private void EnemyController_onEnemyDie()
    {
        audioSourceSfx.PlayOneShot(enemyAudios[0]);
    }
}
