using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOnScreen : MonoBehaviour
{
    [SerializeField] private Image[] fullHearts;
    [SerializeField] private Image[] empyHearts;
    [SerializeField] private Image[] keys;
    [SerializeField] private TextMeshProUGUI coinCounterText;
    [SerializeField] private HealthSystem playerHealthSystem;

    private int coinCounter;

    private void Awake()
    {
        Hill.onKeyBought += Hill_onKeyBought;
        KeyYellow.onKeyYellowPickedUp += KeyYellow_onKeyYellowPickedUp;
        KeyBlue.onKeyBluePickedUp += KeyBlue_onKeyBluePickedUp;
        KeyGreen.onKeyGreenPickedUp += KeyGreen_onKeyGreenPickedUp;
        KeyRed.onKeyRedPickedUp += KeyRed_onKeyRedPickedUp;
        Coin.onCoinPickedUp += Coin_onCoinPickedUp;
        playerHealthSystem.onLifeUpdated += PlayerHealthSystem_onLifeUpdated;
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
        playerHealthSystem.onLifeUpdated -= PlayerHealthSystem_onLifeUpdated;
    }

    private void Hill_onKeyBought(int keyPrice)
    {
        coinCounter -= keyPrice;
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

    private void PlayerHealthSystem_onLifeUpdated(int life, int maxLife)
    {
        for (int i = 0; i <= fullHearts.Length - 1; i++)
        {
            if (i + 1 <= life)
            {
                empyHearts[i].gameObject.SetActive(false);
                fullHearts[i].gameObject.SetActive(true);
            }
            else if (i + 1 > life)
            {
                empyHearts[i].gameObject.SetActive(true);
                fullHearts[i].gameObject.SetActive(false);
            }
        }
    }
}
