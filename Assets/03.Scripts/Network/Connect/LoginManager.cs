using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using BNG;
using TMPro;
using UnityEngine.Rendering;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public string PlayerNickName { get; set; }
    private static LoginManager instance;
    public static LoginManager Instance { get { return instance; } }
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    #region UI Callback Methods
    public void ConnectToPhotonServer()
    {
        
        if (PlayerNickName != null)
        {
            PhotonNetwork.NickName = PlayerNickName;
            PhotonNetwork.ConnectUsingSettings();
        }
        
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ConnectAnonymously()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    #endregion

    #region Photon Callback Methods
    public override void OnConnected()
    {
        print("OnConnected is called. The server is available.");

    }

    public override void OnConnectedToMaster()
    {
        print("Connected to the Master Server with player name: " + PhotonNetwork.NickName);
        NetworkSceneLoader.Instance.LoadScene("HomeScene", false);
        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print(cause.ToString());
    }
    #endregion




   
}
