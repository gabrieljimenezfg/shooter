using System;
using Photon.Pun;
using UnityEngine;

public class NetworkLevelManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        PhotonNetwork.Instantiate(Constants.Prefabs.NetworkPlayer, spawnPoints[0].position, spawnPoints[0].rotation, 0);
    }
}