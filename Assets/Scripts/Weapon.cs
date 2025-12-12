using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private int currentBullets;
    [SerializeField] private int maxMagazine;
    [SerializeField] private int totalBullets;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private bool automatic;

    private float timePassedSinceLastShot;
    private bool isTriggerPressed;

    // muzzle flash, damage
    // Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

    public static event Action OnAmmoChanged;

    private void Update()
    {
        if (isTriggerPressed)
        {
            Shoot();
            if (!automatic)
            {
                isTriggerPressed = false;
            }
        }

        timePassedSinceLastShot += Time.deltaTime;
    }

    public void Shoot()
    {
        if (timePassedSinceLastShot < fireRate) return;
        if (currentBullets <= 0) return;

        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)); // 0,0 es la esquina inferior izquierda
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 bulletDirection = (hit.point - bulletSpawnPoint.position).normalized; // bullet spawn point
            Bullet bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bulletClone.GetComponent<Rigidbody>().linearVelocity = bulletDirection * bulletSpeed;
        }
        else
        {
            Bullet bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bulletClone.GetComponent<Rigidbody>().linearVelocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        currentBullets--;
        timePassedSinceLastShot = 0f;
        OnAmmoChanged?.Invoke();
    }

    public void TriggerPressed()
    {
        isTriggerPressed = true;
    }

    public void TriggerReleased()
    {
        isTriggerPressed = false;
    }

    public void Reload()
    {
        int bulletsToReload = maxMagazine - currentBullets;
        if (bulletsToReload < totalBullets)
        {
            currentBullets = maxMagazine;
            totalBullets -= bulletsToReload;
        }
        else
        {
            currentBullets += totalBullets;
            totalBullets = 0;
        }

        OnAmmoChanged?.Invoke();
    }

    public string MagazineBullets => currentBullets + "/" + maxMagazine;

    public string TotalBullets => totalBullets.ToString();
}