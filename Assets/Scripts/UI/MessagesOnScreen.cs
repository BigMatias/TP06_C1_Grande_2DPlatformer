using TMPro;
using UnityEngine;

public class MessagesOnScreen : MonoBehaviour
{
    [SerializeField] private GameObject keyMessage;
    [SerializeField] private TextMeshProUGUI keyMessageText;

    private BoxCollider2D keyMessageColl;

    private void Awake()
    {
        keyMessageColl = keyMessage.GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.IsTouching(keyMessageColl))
        {
            keyMessageText.gameObject.SetActive(true);
        }
    }
}
