using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeMain : MonoBehaviour
{
    private void Awake()
    {
        UI3DManager.Instance.InitCanvas();
        UI3DManager.Instance.ShowPanel<RoomSelectionPanel>(nameof(RoomSelectionPanel), CanvasName.HomeCanvas);
        
    }
}
