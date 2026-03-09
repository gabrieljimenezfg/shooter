using System;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out NetworkPlayer player))
        {
            Debug.Log("Player Hit");
        }
    }
}