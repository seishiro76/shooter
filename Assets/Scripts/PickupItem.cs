using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    private enum PickupType
    {
        Health,
        Ammo
    }

    [Header("Pickup Settings")]
    [SerializeField] private PickupType pickupType;
    [SerializeField] private int amount = 25;

    [Header("Interaction Text")]
    [SerializeField] private string interactionText = "Нажмите E";

    private bool isUsed;

    public string GetInteractionText()
    {
        return interactionText;
    }

    public void Interact(GameObject player)
    {
        if (isUsed)
        {
            return;
        }

        bool wasUsed = false;

        if (pickupType == PickupType.Health)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Heal(amount);
                wasUsed = true;
            }
        }
        else if (pickupType == PickupType.Ammo)
        {
            PlayerShooting playerShooting = player.GetComponentInChildren<PlayerShooting>();

            if (playerShooting != null)
            {
                playerShooting.AddAmmo(amount);
                wasUsed = true;
            }
        }

        if (!wasUsed)
        {
            return;
        }

        isUsed = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SoundType.Pickup);
        }

        Destroy(gameObject);
    }
}