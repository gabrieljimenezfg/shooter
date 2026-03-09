using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public static class PhotonUtils
{
    public static void PerformRPC(this MonoBehaviourPun target, string methodName, RpcTarget rpcTarget,
        params object[] parameters)
    {
        if (PhotonNetwork.OfflineMode)
        {
            var method = target.GetType().GetMethod(methodName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(target, parameters.Length > 0 ? parameters : null);
        }
        else
        {
            target.photonView.RPC(methodName, rpcTarget, parameters);
        }
    }
}