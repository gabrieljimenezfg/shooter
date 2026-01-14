using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public float damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("[Bullet] bullet hit enemy");
            other.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }

        Debug.Log("[Bullet] bullet hit something else");

        Destroy(gameObject);
    }
}