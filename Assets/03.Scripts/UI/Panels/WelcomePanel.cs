using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WelcomePanel : BaseAnimatedPanel
{
    private TextMeshProUGUI welcomeText;

    private void Start()
    {
        welcomeText= transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        welcomeText.text = $"Welcome,{PhotonNetwork.NickName}!";
    }
}
