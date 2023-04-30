using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelectionPanel : BaseAnimatedPanel
{
    protected override void OnClick(Button button)
    {
        base.OnClick(button);
       
        switch (button.name)
        {
            case "Meeting Room Button":
                DestroySelf(() =>
                {
                    RoomManager.Instance.OnEnterRoomButtonClicked_MeetingRoom();
                    ShowHint();
                });
                
                break;

            case "Jet Room Button":
                DestroySelf(() =>
                {
                    RoomManager.Instance.OnEnterRoomButtonClicked_JetRoom();
                    ShowHint();
                });
                
                break;
        }

    }
    private void ShowHint()
    {
        UI3DManager.Instance.ShowPanel<HintPanel>(nameof(HintPanel), CanvasName.HintCanvas, (panel) =>
        {
            panel.ShowHint("Joining the room...");
        });
    }

}
