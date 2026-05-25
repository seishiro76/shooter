using UnityEngine;

public class DoorController : MonoBehaviour
{
    public void Open()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SoundType.DoorOpen);
        }

        gameObject.SetActive(false);
    }
}