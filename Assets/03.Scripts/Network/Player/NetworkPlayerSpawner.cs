using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject remotePlayer;
    void Start()
    {
        InitNetworkPlayer();
    }

    private void InitNetworkPlayer()
    {
        // Network Instantiate the object used to represent our player. This will have a View on it and represent the player         
        GameObject player = PhotonNetwork.Instantiate(remotePlayer.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
        NetworkPlayerIK np = player.GetComponent<NetworkPlayerIK>();
        if (np)
        {
            np.transform.name = "MyRemotePlayer";
            np.AssignPlayerObjects();
        }
    }
}
