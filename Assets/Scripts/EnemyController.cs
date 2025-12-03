using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float speed;
    private Transform player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        transform.LookAt(player.position);
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }
}