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

    public string GetInteractionText()
    {
        return interactionText;
    }

    public void Interact(GameObject player)
    {
        if (pickupType == PickupType.Health)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Heal(amount);
                Destroy(gameObject);
            }
        }

        if (pickupType == PickupType.Ammo)
        {
            PlayerShooting playerShooting = player.GetComponentInChildren<PlayerShooting>();

            if (playerShooting != null)
            {
                playerShooting.AddAmmo(amount);
                Destroy(gameObject);
            }
        }
    }
}