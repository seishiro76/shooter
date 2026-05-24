using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler inputHandler;

    [Header("Events")]
    [SerializeField] private UnityEvent onInteractionAvailable;
    [SerializeField] private UnityEvent onInteractionLost;

    private IInteractable currentInteractable;
    private GameObject currentInteractableObject;

    private void Awake()
    {
        if (inputHandler == null)
        {
            inputHandler = GetComponent<PlayerInputHandler>();
        }
    }

    private void Update()
    {
        if (currentInteractable != null && inputHandler.InteractPressed)
        {
            currentInteractable.Interact(gameObject);
            ClearInteraction();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable == null)
        {
            return;
        }

        currentInteractable = interactable;
        currentInteractableObject = other.gameObject;

        onInteractionAvailable?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentInteractableObject)
        {
            ClearInteraction();
        }
    }

    private void ClearInteraction()
    {
        currentInteractable = null;
        currentInteractableObject = null;

        onInteractionLost?.Invoke();
    }
}