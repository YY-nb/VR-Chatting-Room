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
        string buttonName = button.name;
        DestroySelf(() =>
        {
            UI3DManager.Instance.ShowPanel<RoomDetailPanel>(nameof(RoomDetailPanel),CanvasName.HomeCanvas, null, (panel) =>
            {
                panel.InitRoomInfo(buttonName);
            });
            

        });
        /*
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
        */

    }
   

}
