using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomDetailPanel : BaseAnimatedPanel
{
    private TextMeshProUGUI title;
    private TextMeshProUGUI description;
    private TextMeshProUGUI playerCountText;
    private Room roomInfo;

    private string roomName;
    protected override void Awake()
    {
        base.Awake();
        title = GetUIComponentByName<TextMeshProUGUI>("Title");
        description =GetUIComponentByName<TextMeshProUGUI>("Description");
        playerCountText = GetUIComponentByName<TextMeshProUGUI>("CountText");
    }

    protected override void OnClick(Button button)
    {
        base.OnClick(button);
        switch (button.name)
        {
            case "Join Button":
                DestroySelf(() =>
                {
                    RoomManager.Instance.OnEnterRoomButtonClicked(roomName);
                    ShowHint();
                });
               
                break;
            case "Back Button":
                DestroySelf(() =>
                {
                    UI3DManager.Instance.ShowPanel<RoomSelectionPanel>(nameof(RoomSelectionPanel),CanvasName.HomeCanvas);
                });
                break;
        }
    }
    public void InitRoomInfo(string roomName)
    {
        this.roomName = roomName; 
        roomInfo = GameDataManager.Instance.GetRoomInfo(roomName);
        title.text = roomName;
        description.text = roomInfo.roomDescription;
        playerCountText.text = $"Current Players:{RoomManager.Instance.PlayerCount}/20";
    }
    private void ShowHint()
    {
        UI3DManager.Instance.ShowPanel<HintPanel>(nameof(HintPanel), CanvasName.HintCanvas, (panel) =>
        {
            panel.ShowHint("Joining the room...");
        });
    }
}
