using UnityEngine;

public class NetworkBullet : MonoBehaviour
{
    private Rigidbody rb;
    public float damage;
    [SerializeField] private float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out BaseEnemy enemy))
        {
            Debug.Log("[Bullet] bullet hit enemy");
        }

        Destroy(gameObject);
    }
}