using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UI3DManager.Instance.ShowPanel<StartPanel>(nameof(StartPanel),CanvasName.StartCanvas);
    }

    
}
