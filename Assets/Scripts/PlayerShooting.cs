using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private PlayerInputHandler inputHandler;

    [Header("Shooting Settings")]
    [SerializeField] private float shootDistance = 50f;
    [SerializeField] private int damage = 25;

    [Header("Ammo Settings")]
    [SerializeField] private int magazineSize = 10;
    [SerializeField] private int startReserveAmmo = 30;
    [SerializeField] private int maxReserveAmmo = 50;

    [Header("Raycast Settings")]
    [SerializeField] private LayerMask shootMask;

    [Header("Muzzle Flash")]
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private float muzzleFlashTime = 0.05f;

    [Header("Events")]
    [SerializeField] private UnityEvent<int, int> onAmmoChanged;

    private int magazineAmmo;
    private int reserveAmmo;
    private Coroutine muzzleFlashCoroutine;

    private void Awake()
    {
        magazineAmmo = magazineSize;
        reserveAmmo = startReserveAmmo;

        if (playerCamera == null)
        {
            playerCamera = GetComponent<Camera>();
        }

        if (inputHandler == null)
        {
            inputHandler = GetComponentInParent<PlayerInputHandler>();
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(false);
        }
    }

    private void Start()
    {
        UpdateAmmoUI();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (inputHandler != null && inputHandler.ReloadPressed)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        if (magazineAmmo <= 0)
        {
            return;
        }

        magazineAmmo--;
        UpdateAmmoUI();

        ShowMuzzleFlash();

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, shootDistance, shootMask))
        {
            EnemyHealth enemyHealth = hitInfo.collider.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    private void Reload()
    {
        if (magazineAmmo >= magazineSize)
        {
            return;
        }

        if (reserveAmmo <= 0)
        {
            return;
        }

        int neededAmmo = magazineSize - magazineAmmo;
        int ammoToReload = Mathf.Min(neededAmmo, reserveAmmo);

        magazineAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        UpdateAmmoUI();
    }

    private void ShowMuzzleFlash()
    {
        if (muzzleFlash == null)
        {
            return;
        }

        if (muzzleFlashCoroutine != null)
        {
            StopCoroutine(muzzleFlashCoroutine);
        }

        muzzleFlashCoroutine = StartCoroutine(MuzzleFlashCoroutine());
    }

    private IEnumerator MuzzleFlashCoroutine()
    {
        muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(muzzleFlashTime);

        muzzleFlash.SetActive(false);
    }

    public void AddAmmo(int amount)
    {
        reserveAmmo += amount;

        if (reserveAmmo > maxReserveAmmo)
        {
            reserveAmmo = maxReserveAmmo;
        }

        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        onAmmoChanged?.Invoke(magazineAmmo, reserveAmmo);
    }

    public int GetMagazineAmmo()
    {
        return magazineAmmo;
    }

    public int GetReserveAmmo()
    {
        return reserveAmmo;
    }
}