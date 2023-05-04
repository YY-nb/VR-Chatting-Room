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
        EventCenter.Instance.AddListener<int>(EventName.OnUpdateRoom, OnUpdateRoomPlayerCount);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventCenter.Instance.RemoveListener<int>(EventName.OnUpdateRoom, OnUpdateRoomPlayerCount);
    }
    private void OnUpdateRoomPlayerCount(int playerCount)
    {
        playerCountText.text = $"Current Players:{playerCount}/20";
    }
    protected override void OnClick(Button button)
    {
        base.OnClick(button);
        switch (button.name)
        {
            case "Join Button":
                DestroySelf(() =>
                {
                    ShowHint();
                    RoomManager.Instance.OnEnterRoomButtonClicked(roomName);
                   
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
        UI3DManager.Instance.ShowPanel<HintPanel>(nameof(HintPanel), CanvasName.HintCanvas, null, (panel) =>
        {
            panel.ShowHint("Joining the room...");
        });
    }
}
