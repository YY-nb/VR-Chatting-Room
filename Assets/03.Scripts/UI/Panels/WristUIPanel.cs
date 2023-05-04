using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WristUIPanel : BaseAnimatedPanel
{
    protected override void OnClick(Button button)
    {
        base.OnClick(button);
        switch (button.name)
        {
            case "Leave Button":
                this.TriggerEvent(EventName.OnEnableWristUI, false);
                DestroySelf(() =>
                {
                    VirtualWorldManager.Instance.LeaveRoomAndLoadHomeScene();
                });
                break;
        }
    }
}
