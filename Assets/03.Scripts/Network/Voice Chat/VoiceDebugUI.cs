using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Voice.PUN;
using Photon.Pun;
using Photon.Voice.Unity;
using System;

public class VoiceDebugUI : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI voiceState;

    private PunVoiceClient punVoiceNetwork;

    private void Awake()
    {
        punVoiceNetwork = PunVoiceClient.Instance;
    }

    private void OnEnable()
    {
        punVoiceNetwork.Client.StateChanged += VoiceClientStateChanged;
    }

    private void OnDisable()
    {
        punVoiceNetwork.Client.StateChanged -= VoiceClientStateChanged;
    }

    private void Update()
    {
        if (punVoiceNetwork == null)
        {
            punVoiceNetwork = PunVoiceClient.Instance;
        }
    }


    private void VoiceClientStateChanged(Photon.Realtime.ClientState fromState, Photon.Realtime.ClientState toState)
    {
        UpdateUIBasedOnVoiceState(toState);
    }

    private void UpdateUIBasedOnVoiceState(Photon.Realtime.ClientState voiceClientState)
    {
        voiceState.text = string.Format("PhotonVoice: {0}", voiceClientState);
        if (voiceClientState == Photon.Realtime.ClientState.Joined)
        {
            voiceState.gameObject.SetActive(false);
        }
    }
}


