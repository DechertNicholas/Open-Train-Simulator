using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClientInstance : NetworkBehaviour
{
    [Tooltip("Prefab for the player")]
    [SerializeField]
    private NetworkIdentity _playerPrefab = null;
    [Command]
    private void CmdRequestSpawn()
    {
        NetworkSpawnPlayer();
    }

    [Server]
    private void NetworkSpawnPlayer()
    {
        GameObject gameObject = Instantiate(_playerPrefab.gameObject, transform.position, Quaternion.identity);
        NetworkServer.Spawn(gameObject, base.connectionToClient);
    }
}
