using System;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float explosionRadius, damage, timeToExplode, explosionTimer, knockBackForce;
    [SerializeField] private GameObject explosionPrefab;
    private bool countdownActive;

    private void Update()
    {
        if (!countdownActive) return;

        explosionTimer += Time.deltaTime;
        if (explosionTimer > timeToExplode)
        {
            Explode();
        }
    }

    private void Explode()
    {
        // TODO: explosion sfx

        var vfx = Instantiate(explosionPrefab, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddExplosionForce(knockBackForce, transform.position, explosionRadius);

                if (collider.TryGetComponent(out EnemyController enemyController))
                {
                    enemyController.TakeDamage(damage);
                }
                else if (collider.TryGetComponent(out PlayerController playerController))
                {
                    playerController.TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }

    public void StartCountdown()
    {
        countdownActive = true;
    }
}