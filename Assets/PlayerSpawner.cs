using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private NetworkObject playerPrefab;
    void Start()
    {
        Debug.Log($"Conn Id {InstanceFinder.ClientManager.Connection.ClientId}, Active: {InstanceFinder.ClientManager.Connection.IsActive}");
        Spawn(InstanceFinder.ClientManager.Connection);
        //if (InstanceFinder.IsServerStarted)
        //    StartCoroutine(SpawnHostWhenReady());
    }
    //private void SpawnHost()
    //{
    //    if (InstanceFinder.IsServerStarted)
    //    {
    //        Debug.Log("Spawning player for host.");
    //        NetworkConnection conn = InstanceFinder.ClientManager.Connection;
    //        Debug.Log("Connection: " + conn);
    //        NetworkObject nob = InstanceFinder.NetworkManager.GetPooledInstantiated(playerPrefab, playerPrefab.transform.position, playerPrefab.transform.rotation, true);
    //        InstanceFinder.NetworkManager.ServerManager.Spawn(nob, conn);
    //    }

    //}

    //private IEnumerator SpawnHostWhenReady()
    //{
    //    // Wait until host connection is valid
    //    while (InstanceFinder.ClientManager.Connection == null ||
    //           !InstanceFinder.ClientManager.Connection.IsActive)
    //        yield return null;

    //    Debug.Log("Spawning player for host.");

    //    NetworkConnection conn = InstanceFinder.ClientManager.Connection;
    //    Debug.Log("Connection: " + conn);

    //    NetworkObject nob = InstanceFinder.NetworkManager.GetPooledInstantiated(playerPrefab, playerPrefab.transform.position, playerPrefab.transform.rotation, true);
    //    InstanceFinder.NetworkManager.ServerManager.Spawn(nob, conn);

    //}


    [ServerRpc(RequireOwnership = false)]
    void Spawn(NetworkConnection conn)
    {
        Debug.Log("Spawning player for client " + conn.ClientId);
        InstanceFinder.NetworkManager.ServerManager.Spawn(playerPrefab, conn);
    }

}
