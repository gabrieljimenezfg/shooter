using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private int currentBullets;
    [SerializeField] private int maxMagazine;
    [SerializeField] private int totalBullets;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private bool automatic;

    // muzzle flash, damage
    // Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

    public void Shoot()
    {
        if (currentBullets <= 0) return;

        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)); // 0,0 es la esquina inferior izquierda
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 bulletDirection = (hit.point - bulletSpawnPoint.position).normalized; // bullet spawn point
            GameObject bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bulletClone.GetComponent<Rigidbody>().linearVelocity = bulletDirection * bulletSpeed;
        }
        else
        {
            GameObject bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bulletClone.GetComponent<Rigidbody>().linearVelocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        currentBullets--;
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
    }
}