using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    const string PLAYER_TAG = "Player";
    private bool playerDetected;
    private Animator animator;
    private Transform player;
    private NavMeshAgent agent;

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
            agent.SetDestination(player.position);
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