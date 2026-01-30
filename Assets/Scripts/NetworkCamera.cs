using System;
using UnityEngine;

public class NetworkCamera : MonoBehaviour
{
    private Transform player;
    [SerializeField] private Vector3 cameraOffset;

    private void LateUpdate()
    {
        if (player == null) return;
        transform.position = player.position + cameraOffset;
    }

    public void SetPlayer(Transform _player)
    {
        player = _player;
    }
}