using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameWon : MonoBehaviour
{
    [SerializeField] private GameObject gameWonPanel;
    [SerializeField] private GameManager gameManager;    
    [SerializeField] private Button yesBtn;
    [SerializeField] private Button noBtn;

    private void Awake()
    {
        Door.onGameFinished += Door_onGameFinished;
        yesBtn.onClick.AddListener(YesBtnClicked);
        noBtn.onClick.AddListener(NoBtnClicked);
    }


    private void OnDestroy()
    {
        Door.onGameFinished -= Door_onGameFinished;
        yesBtn.onClick.RemoveListener(YesBtnClicked);
        noBtn.onClick.RemoveListener(NoBtnClicked);
    }

    private void Door_onGameFinished()
    {
        gameManager.PauseGame();
        gameWonPanel.SetActive(true);
    }

    private void NoBtnClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void YesBtnClicked()
    {
        SceneManager.LoadScene("Game");
    }

}
