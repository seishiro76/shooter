using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text interactionText;

    [Header("Message Objects")]
    [SerializeField] private GameObject doorMessage;
    [SerializeField] private GameObject finishMessage;
    [SerializeField] private GameObject winMessage;
    [SerializeField] private GameObject deathMessage;

    [Header("Message Settings")]
    [SerializeField] private float temporaryMessageTime = 2f;

    private Coroutine doorMessageCoroutine;
    private Coroutine finishMessageCoroutine;

    private void Awake()
    {
        HideAllMessages();
        HideInteractionPrompt();
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = "Здоровье: " + currentHealth + " / " + maxHealth;
        }
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        if (ammoText != null)
        {
            ammoText.text = "Патроны: " + currentAmmo + " / " + maxAmmo;
        }
    }

    public void ShowInteractionPrompt()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(true);
        }
    }

    public void HideInteractionPrompt()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    public void ShowDoorOpened()
    {
        ShowTemporaryMessage(doorMessage, ref doorMessageCoroutine);
    }

    public void ShowFinishZoneActivated()
    {
        ShowTemporaryMessage(finishMessage, ref finishMessageCoroutine);
    }

    public void ShowLevelCompleted()
    {
        HideAllMessages();

        if (winMessage != null)
        {
            winMessage.SetActive(true);
        }
    }

    public void ShowPlayerDied()
    {
        HideAllMessages();

        if (deathMessage != null)
        {
            deathMessage.SetActive(true);
        }
    }

    private void ShowTemporaryMessage(GameObject messageObject, ref Coroutine coroutine)
    {
        if (messageObject == null)
        {
            return;
        }

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(TemporaryMessageCoroutine(messageObject));
    }

    private IEnumerator TemporaryMessageCoroutine(GameObject messageObject)
    {
        messageObject.SetActive(true);

        yield return new WaitForSeconds(temporaryMessageTime);

        messageObject.SetActive(false);
    }

    private void HideAllMessages()
    {
        if (doorMessage != null)
        {
            doorMessage.SetActive(false);
        }

        if (finishMessage != null)
        {
            finishMessage.SetActive(false);
        }

        if (winMessage != null)
        {
            winMessage.SetActive(false);
        }

        if (deathMessage != null)
        {
            deathMessage.SetActive(false);
        }
    }
}