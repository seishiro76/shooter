using UnityEngine;
using UnityEngine.Events;

public class ButtonInteractable : MonoBehaviour, IInteractable
{
    [Header("Interaction Text")]
    [SerializeField] private string interactionText = "Нажмите E, чтобы начать";

    [Header("Settings")]
    [SerializeField] private bool oneShot = true; // сработать только один раз

    [Header("Events")]
    [SerializeField] private UnityEvent onPressed;

    private bool isUsed;

    public string GetInteractionText()
    {
        return interactionText;
    }

    public void Interact(GameObject player)
    {
        if (oneShot && isUsed)
        {
            return;
        }

        isUsed = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SoundType.DoorOpen);
        }

        onPressed?.Invoke();
    }
}