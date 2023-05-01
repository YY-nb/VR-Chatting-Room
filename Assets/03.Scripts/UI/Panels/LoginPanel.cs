using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BaseAnimatedPanel
{
    protected override void OnClick(Button button)
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
}
