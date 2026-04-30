using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private float shootDistance = 50f;
    [SerializeField] private int damage = 25;

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private int currentAmmo = 30;

    [Header("Visual Effects")]
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private float muzzleFlashTime = 0.05f;

    private Camera playerCamera;

    private void Awake()
    {
        playerCamera = GetComponent<Camera>();

        if (playerCamera == null)
        {
            Debug.LogError("PlayerShooting должен быть на объекте с компонентом Camera");
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("Нет патронов");
            return;
        }

        currentAmmo--;

        if (muzzleFlash != null)
        {
            StartCoroutine(ShowMuzzleFlash());
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, shootDistance))
        {
            Debug.Log("Попадание в объект: " + hitInfo.collider.name);

            EnemyHealth enemyHealth = hitInfo.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log("Выстрел мимо");
        }

        Debug.Log("Патроны: " + currentAmmo);
    }

    private IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(muzzleFlashTime);

        muzzleFlash.SetActive(false);
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;

        if (currentAmmo > maxAmmo)
        {
            currentAmmo = maxAmmo;
        }

        Debug.Log("Патроны пополнены: " + currentAmmo);
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
}