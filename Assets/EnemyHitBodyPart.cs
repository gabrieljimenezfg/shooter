using System;
using UnityEngine;

public class EnemyHitBodyPart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out NetworkPlayer player))
        {
            Debug.Log(player.name + " has hit " + other.name);
        }
    }
}
