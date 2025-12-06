using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    const string PLAYER_TAG = "Player";
    private bool playerDetected;
    private Animator animator;
    private Transform player;
    private NavMeshAgent agent;
    [SerializeField] private Transform[] patrolPoints;
    private int patrolIndex;
    [SerializeField] private float speed;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (playerDetected)
        {
            agent.speed = speed;
            agent.stoppingDistance = 5f;
            animator.SetFloat(Vertical, 1f);
            agent.SetDestination(player.position);
        }
        else
        {
            animator.SetFloat(Vertical, 0.4f);
            agent.speed = speed * 0.5f;
            agent.SetDestination(patrolPoints[patrolIndex].position);
            float distance = (patrolPoints[patrolIndex].position - transform.position).magnitude;
            if (distance < 1f)
            {
                patrolIndex += 1;
                if (patrolIndex >= patrolPoints.Length)
                {
                    patrolIndex = 0;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(PLAYER_TAG)) return;

        var playerDirection = (player.position - transform.position).normalized;
        Ray ray = new Ray(transform.position + new Vector3(0, 1.65f, 0), playerDirection);

        if (Physics.Raycast(ray, out var hit))
        {
            Debug.Log(hit.collider.name);
            Debug.DrawRay(transform.position + new Vector3(0, 1.65f, 0), playerDirection * 100f, Color.red, 5f);
            if (hit.transform.CompareTag(PLAYER_TAG))
            {
                playerDetected = true;
            }
        }
    }
}