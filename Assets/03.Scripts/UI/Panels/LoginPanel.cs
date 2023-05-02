using Microsoft.MixedReality.Toolkit.Experimental.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class LoginPanel : BaseAnimatedPanel
{
    private TMP_InputField nameInputField;
    private void Start()
    {
        nameInputField = GetUIComponentByName<TMP_InputField>("Name_InputField");
        nameInputField.onSelect.AddListener(x => OpenKeyboaed());
    }

    protected override void OnClick(UnityEngine.UI.Button button)
    {
        base.OnClick(button);
        switch (button.name)
        {
            case "Start Button":
                OnLogin();
                break;
        }

    }
    private void OnLogin()
    {
        //将输入的名字传给LoginManager
        LoginManager.Instance.PlayerNickName = nameInputField.text;
        LoginManager.Instance.ConnectToPhotonServer();
        DestroySelf(() =>
        {
            ShowConnectingInfo();
        });
    }
    private void ShowConnectingInfo()
    {
        UI3DManager.Instance.ShowPanel<HintPanel>(nameof(HintPanel), CanvasName.HintCanvas, (panel) =>
        {
            panel.ShowHint("Connecting to the server...");
        });
    }
    public void OpenKeyboaed()
    {
        NonNativeKeyboard.Instance.InputField = nameInputField;
        NonNativeKeyboard.Instance.PresentKeyboard(nameInputField.text);
    }
}
