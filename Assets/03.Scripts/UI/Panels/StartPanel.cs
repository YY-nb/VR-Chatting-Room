using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BaseAnimatedPanel
{
    protected override void OnClick(Button button)
    {
        base.OnClick(button);
        OnStart();
    }
    private void OnStart()
    {
        //LoginManager.Instance.ConnectToPhotonServer();
        DestroySelf(() =>
        {
            UI3DManager.Instance.ShowPanel<LoginPanel>(nameof(LoginPanel), CanvasName.StartCanvas);
            //ShowConnectingInfo();
        });
    }
    
}
