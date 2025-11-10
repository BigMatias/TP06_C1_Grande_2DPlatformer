using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public partial class UICooldowns : MonoBehaviour
{
    [SerializeField] private Image[] cooldownImage;

    void Awake()
    {
        PlayerMovement.onPlayerPunch += PlayerMovement_onPlayerPunch;
        PlayerInteractionPowerUps.onPowerUpPickedUp += PlayerInteractionPowerUps_onPowerUpPickedUp;
    }

    private void Start()
    {
        for (int i = 0; i < cooldownImage.Length; i++)
        {
            cooldownImage[i].gameObject.SetActive(false);
            cooldownImage[i].fillAmount = 0;
        }
    }

    private void OnDestroy()
    {
        PlayerMovement.onPlayerPunch -= PlayerMovement_onPlayerPunch;
        PlayerInteractionPowerUps.onPowerUpPickedUp -= PlayerInteractionPowerUps_onPowerUpPickedUp;
    }

    private void PlayerInteractionPowerUps_onPowerUpPickedUp(int id, float cooldownTime)
    {
        StartCooldown(cooldownTime, id);
    }

    private void PlayerMovement_onPlayerPunch(float cooldownTime, int id)
    {
        StartCooldown(cooldownTime, id);
    }

    private void StartCooldown(float cooldownTime, int id)
    {
        switch (id)
        {
            case (int)PlayerActionType.Punch:
                StartCoroutine(DoCooldown(cooldownTime, id));
                cooldownImage[id].gameObject.SetActive(true);
                cooldownImage[(int)PlayerActionType.Punch].fillAmount = 1;
                break;
            case (int)PlayerActionType.TripleJump:
                StartCoroutine(DoCooldown(cooldownTime, id));
                cooldownImage[id].gameObject.SetActive(true);
                cooldownImage[(int)PlayerActionType.TripleJump].fillAmount = 1;
                break;
            case (int)PlayerActionType.Damage:
                StartCoroutine(DoCooldown(cooldownTime, id));
                cooldownImage[id].gameObject.SetActive(true);
                cooldownImage[(int)PlayerActionType.Damage].fillAmount = 1;
                break;
            case (int)PlayerActionType.Invulnerability:
                StartCoroutine(DoCooldown(cooldownTime, id));
                cooldownImage[id].gameObject.SetActive(true);
                cooldownImage[(int)PlayerActionType.Invulnerability].fillAmount = 1;
                break;
        }
    }

    private IEnumerator DoCooldown(float cooldownTime, int id)
    {
        float timer = cooldownTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            cooldownImage[id].fillAmount = timer / cooldownTime;

            yield return null;
        }

        cooldownImage[id].fillAmount = 0;
    }
}
