using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(RoomManager),true)]
public class RoomManagerEditorScript : Editor
{
    string roomNameToCreate = "DefaultRoomToCreate";
    string roomNameToJoin = "DefaultRoomToJoin";


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.HelpBox("This script is responsible for creating and joining rooms", MessageType.Info);

        RoomManager roomManager = (RoomManager)target;


        if (GUILayout.Button("Join Meeting Room"))
        {
            roomManager.OnEnterRoomButtonClicked_MeetingRoom();
        }

        if (GUILayout.Button("Join Jet Room"))
        {
            roomManager.OnEnterRoomButtonClicked_JetRoom();
        }
        EditorGUILayout.Space(10);


        EditorGUILayout.LabelField("Private Room Testing Section", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);


        roomNameToCreate = EditorGUILayout.TextField("Room name to join: ", roomNameToCreate);
        if (GUILayout.Button("Create Private Room"))
        {
            roomManager.CreatePrivateRoom(roomNameToCreate, (byte)8, MultiplayerVRConstants.MAP_TYPE_VALUE_MEETING_ROOM);

        }
        EditorGUILayout.Space(30);




        roomNameToJoin = EditorGUILayout.TextField("Room name to join: ", roomNameToJoin);
        if (GUILayout.Button("Join Private Room"))
        {
            roomManager.JoinPrivateRoom(roomNameToJoin);
            Debug.Log(roomNameToJoin);
        }

        EditorGUILayout.EndFoldoutHeaderGroup();


    }


}
