using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PCQuickSceneSwitcher : MonoBehaviour
{
    [SerializeField] private InputActionReference login;
    [SerializeField] private InputActionReference joinMeetingRoom;
    [SerializeField] private InputActionReference joinJetRoom;
    [SerializeField] private InputActionReference leaveRoom;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        EnableAction(login);
        EnableAction(joinMeetingRoom);
        EnableAction(leaveRoom);
        EnableAction(joinJetRoom);
        login.action.performed += Login;
        joinMeetingRoom.action.performed += JoinMeetingRoom;
        joinJetRoom.action.performed += JoinJetRoom;
        leaveRoom.action.performed += LeaveRoom;
    }


    private void Login(InputAction.CallbackContext obj)
    {
        UI3DManager.Instance.DestroyLastPanelInList(() =>
        {
            UI3DManager.Instance.ShowPanel<HintPanel>(nameof(HintPanel), CanvasName.HintCanvas, (panel) =>
            {
                panel.ShowHint("Connecting to the server...");
            });
        });
        string randomName = "Player" + UnityEngine.Random.Range(1, 10000);
        PhotonNetwork.LocalPlayer.NickName = randomName;
        LoginManager.Instance?.ConnectAnonymously();
    }
    private void JoinMeetingRoom(InputAction.CallbackContext obj)
    {
        RoomManager.Instance?.OnEnterRoomButtonClicked_MeetingRoom();
    }
    private void JoinJetRoom(InputAction.CallbackContext obj)
    {
        RoomManager.Instance?.OnEnterRoomButtonClicked_JetRoom();
    }
    private void LeaveRoom(InputAction.CallbackContext obj)
    {
        VirtualWorldManager.Instance?.LeaveRoomAndLoadHomeScene();
    }
    private void EnableAction(InputActionReference actionReference)
    {
        var action = GetInputAction(actionReference);
        if (action != null && !action.enabled)
            action.Enable();
    }

    private void DisableAction(InputActionReference actionReference)
    {
        var action = GetInputAction(actionReference);
        if (action != null && action.enabled)
            action.Disable();
    }

    private InputAction GetInputAction(InputActionReference actionReference)
    {
        return actionReference != null ? actionReference.action : null;
    }

}
