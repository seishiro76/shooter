using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Leaves")]
    [SerializeField] private Transform leftLeaf;
    [SerializeField] private Transform rightLeaf;

    [Header("Slide Settings")]
    [SerializeField] private float slideDistance = 1.5f;   // на сколько уезжает каждая створка
    [SerializeField] private float slideDuration = 1f;     // за сколько секунд открывается
    [SerializeField] private Vector3 slideAxis = Vector3.right; // в какую сторону едут створки (локально)

    private bool isOpen;

    public void Open()
    {
        if (isOpen)
        {
            return;
        }

        isOpen = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SoundType.DoorOpen);
        }

        StartCoroutine(SlideCoroutine());
    }

    private IEnumerator SlideCoroutine()
    {
        // запоминаем стартовые локальные позиции створок
        Vector3 leftStart = leftLeaf != null ? leftLeaf.localPosition : Vector3.zero;
        Vector3 rightStart = rightLeaf != null ? rightLeaf.localPosition : Vector3.zero;

        // целевые позиции: створки разъезжаются в противоположные стороны
        Vector3 leftTarget = leftStart - slideAxis * slideDistance;
        Vector3 rightTarget = rightStart + slideAxis * slideDistance;

        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            // SmoothStep даёт плавный разгон/торможение вместо линейного движения
            float smooth = Mathf.SmoothStep(0f, 1f, t);

            if (leftLeaf != null)
            {
                leftLeaf.localPosition = Vector3.Lerp(leftStart, leftTarget, smooth);
            }

            if (rightLeaf != null)
            {
                rightLeaf.localPosition = Vector3.Lerp(rightStart, rightTarget, smooth);
            }

            yield return null;
        }

        // гарантированно ставим в конечную позицию
        if (leftLeaf != null)
        {
            leftLeaf.localPosition = leftTarget;
        }

        if (rightLeaf != null)
        {
            rightLeaf.localPosition = rightTarget;
        }
    }
}