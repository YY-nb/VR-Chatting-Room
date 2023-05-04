using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using BNG;
using UnityEngine.Rendering;

public class VirtualWorldManager : MonoBehaviourPunCallbacks
{
    public UnityEvent OnNewPlayerEntered;
    private static VirtualWorldManager instance;
    public static VirtualWorldManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        UI3DManager.Instance.InitCanvas();
    }


    public void LeaveRoomAndLoadHomeScene()
    {
        PhotonNetwork.LeaveRoom();
    }


    #region Photon Callback Methods
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        Debug.Log(newPlayer.NickName + " joined to:" + "Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        OnNewPlayerEntered.Invoke();
    }

    public override void OnLeftRoom()
    {
        //PhotonNetwork.Disconnect();
        NetworkSceneLoader.Instance.LoadScene("HomeScene", false);
    }

    public override void OnJoinedRoom()
    {
        

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //NetworkSceneLoader.Instance.LoadScene("HomeScene", false);
    }
    #endregion
}
