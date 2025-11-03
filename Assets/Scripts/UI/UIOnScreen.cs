using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOnScreen : MonoBehaviour
{
    [SerializeField] private Image[] fullHearts;
    [SerializeField] private Image[] empyHearts;
    [SerializeField] private Image[] keys;
    [SerializeField] private TextMeshProUGUI coinCounterText;

    private int coinCounter;

    private void Awake()
    {
        Hill.onKeyBought += Hill_onKeyBought;
        KeyYellow.onKeyYellowPickedUp += KeyYellow_onKeyYellowPickedUp;
        KeyBlue.onKeyBluePickedUp += KeyBlue_onKeyBluePickedUp;
        KeyGreen.onKeyGreenPickedUp += KeyGreen_onKeyGreenPickedUp;
        KeyRed.onKeyRedPickedUp += KeyRed_onKeyRedPickedUp;
        Coin.onCoinPickedUp += Coin_onCoinPickedUp;
        EnemyController.onPlayerHit += EnemyController_onPlayerHit;
    }

    void Start()
    {
        coinCounter = 0;
        for (int i = 0; i <= fullHearts.Length - 1; i++)
        {
            fullHearts[i].gameObject.SetActive(true);
            empyHearts[i].gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        Hill.onKeyBought -= Hill_onKeyBought;
        KeyYellow.onKeyYellowPickedUp -= KeyYellow_onKeyYellowPickedUp;
        KeyBlue.onKeyBluePickedUp -= KeyBlue_onKeyBluePickedUp;
        KeyGreen.onKeyGreenPickedUp -= KeyGreen_onKeyGreenPickedUp;
        KeyRed.onKeyRedPickedUp -= KeyRed_onKeyRedPickedUp;
        Coin.onCoinPickedUp -= Coin_onCoinPickedUp;
        EnemyController.onPlayerHit -= EnemyController_onPlayerHit;
    }

    private void Hill_onKeyBought()
    {
        coinCounter -= 30;
        coinCounterText.text = coinCounter.ToString();
    }

    private void KeyYellow_onKeyYellowPickedUp()
    {
        keys[0].gameObject.SetActive(true);
    }
    private void KeyBlue_onKeyBluePickedUp()
    {
        keys[1].gameObject.SetActive(true);
    }

    private void KeyGreen_onKeyGreenPickedUp()
    {
        keys[2].gameObject.SetActive(true);
    }

    private void KeyRed_onKeyRedPickedUp()
    {
        keys[3].gameObject.SetActive(true);
    }

    private void Coin_onCoinPickedUp()
    {
        coinCounter++;
        coinCounterText.text = coinCounter.ToString();
    }

    private void EnemyController_onPlayerHit()
    {
        for (int i = 0; i <= fullHearts.Length - 1; i++)
        {
            if (fullHearts[i].gameObject.activeSelf)
            {
                fullHearts[i].gameObject.SetActive(false);
                empyHearts[i].gameObject.SetActive(true);
                break;
            }
        }
    }
}
