using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameDataManager : SingletonAutoMonoBase<GameDataManager>
{
    private Dictionary<string, Room> roomInfoDic = new Dictionary<string, Room>();  
    private void Awake()
    {
        LoadRoomDataFromAB();
    }
    public void Init()
    {

    }
    private T LoadData<T>(string fileName) where T : new()
    {
        return JsonManager.Instance.LoadData<T>(fileName);
    }
    private void LoadDataFromAB<T>(string fileName, Action<T> callback) where T : new()
    {
        JsonManager.Instance.LoadDataFromAB(fileName, callback);
    }
    private void SaveData<T>(T obj) where T : new()
    {
        JsonManager.Instance.SaveData(obj, typeof(T).Name);
    }
    #region 房间描述数据
    private void LoadRoomDataFromAB()
    {
        LoadDataFromAB<List<Room>>(nameof(Room), (roomList) =>
        {
            foreach (Room room in roomList)
            {
                if (roomInfoDic.ContainsKey(room.roomName))
                {
                    roomInfoDic[room.roomName] = room;
                }
                else
                {
                    roomInfoDic.Add(room.roomName, room);
                }
            }
        });
    }
    public Room GetRoomInfo(string roomName)
    {
        if(roomInfoDic.TryGetValue(roomName, out Room room))
        {
            return room;
        }
        return null;
    }
    #endregion
}

