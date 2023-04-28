using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LoginManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        print("OnConnect");
    }
    public override void OnConnectedToMaster()
    {
        print("connect to master server");
        PhotonNetwork.LoadLevel("HomeScene");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        print(cause.ToString());
    }
}
