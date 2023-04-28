using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkSetup : MonoBehaviour
{
    public GameObject playerAvatar;

    private PhotonView photonView;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            InitLocal();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void InitLocal()
    {
        //Adding Audio Listener for voice chat setup
        playerAvatar?.AddComponent<AudioListener>();
    }
}
