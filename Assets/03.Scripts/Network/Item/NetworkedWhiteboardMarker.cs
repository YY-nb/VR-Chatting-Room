
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkedWhiteboardMarker : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _tip;
    [SerializeField] private int _penSize = 5;
    [SerializeField] private float _tipHeight = 0.01f;
    [SerializeField] private Transform RaycastStart;

    private Renderer _renderer;
    private Color[] _colors;
    public Color DrawColor = Color.black;
   
    private RaycastHit _touch;
    private Whiteboard _whiteboard;
    private Vector2 _touchPos, _lastTouchPos;
    private bool _touchedLastFrame;
    private Quaternion _lastTouchRot;

    void Start()
    {
        _renderer = _tip.GetComponent<Renderer>();
        _colors = Enumerable.Repeat(_renderer.material.color, _penSize * _penSize).ToArray();
       
       // _tipHeight = _tip.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            Draw();
        }
        
    }
    
  
    private void Draw()
    {
        if (Physics.Raycast(RaycastStart.position, RaycastStart.up, out RaycastHit _touch, _tipHeight))
        {
            
            if (_touch.collider.transform.CompareTag("Whiteboard"))
            {
                if (_whiteboard == null)
                {
                    _whiteboard = _touch.collider.transform.GetComponent<Whiteboard>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                var x = (int)(_touchPos.x * _whiteboard.textureSize.x - (_penSize / 2));
                var y = (int)(_touchPos.y * _whiteboard.textureSize.y - (_penSize / 2));

                if (y < 0 || y > _whiteboard.textureSize.y || x < 0 || x > _whiteboard.textureSize.x)
                {
                    return;
                }


                if (_touchedLastFrame)
                {

                    Vector3[] colorVectors = ColorUtil.ColorArrayToVector3Array(_colors);
                    //_whiteboard.view.RPC("DrawRPC", RpcTarget.All, x, y, _penSize, colorVectors, _lastTouchPos, _lastTouchRot);
                    _whiteboard.DisplayDrawing(x, y, _penSize, colorVectors, _lastTouchPos);
                    transform.rotation = _lastTouchRot;
                    //photonView.RPC(nameof(DrawRPC), RpcTarget.All, x, y, colorVectors, _lastTouchPos, _lastTouchRot);
                }
                _lastTouchPos = new Vector2(x, y);
                _lastTouchRot = transform.rotation;
                _touchedLastFrame = true;
                return;

            }
        }
        _whiteboard = null;
        _touchedLastFrame = false;
    }
    /*
    [PunRPC]
    private void DrawRPC(int x, int y, Vector3[] colorVectors, Vector2 lastTouchPos, Quaternion lastTouchRot)
    {
        Color[] colors = ColorUtil.Vector3ArrayToColorArray(colorVectors);
        if (_whiteboard == null) print("°×°åÎ´³õÊ¼»¯");
        _whiteboard.texture.SetPixels(x, y, _penSize, _penSize, colors);
        for (float f = 0.01f; f < 1.00f; f += 0.01f)
        {
            var lerpX = (int)Mathf.Lerp(lastTouchPos.x, x, f);
            var lerpY = (int)Mathf.Lerp(lastTouchPos.y, y, f);

            _whiteboard.texture.SetPixels(lerpX, lerpY, _penSize, _penSize, colors);
        }
        transform.rotation = lastTouchRot;

        _whiteboard.texture.Apply();
    }
    */
}

/*
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkedWhiteboardMarker : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _tip;
    [SerializeField] private int _penSize = 5;
    [SerializeField] private float _tipHeight = 0.01f;
    [SerializeField] private Transform RaycastStart;

    private Renderer _renderer;
    private Color[] _colors;
    public Color DrawColor = Color.black;
   
    private RaycastHit _touch;
    private Whiteboard _whiteboard;
    private Vector2 _touchPos, _lastTouchPos;
    private bool _touchedLastFrame;
    private Quaternion _lastTouchRot;

    void Start()
    {
        _renderer = _tip.GetComponent<Renderer>();
        _colors = Enumerable.Repeat(_renderer.material.color, _penSize * _penSize).ToArray();
       
        _tipHeight = _tip.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        Draw();

    }

    private void Draw()
    {
        if (Physics.Raycast(RaycastStart.position, RaycastStart.up, out RaycastHit _touch, _tipHeight))
        {
            
            if (_touch.collider.transform.CompareTag("Whiteboard"))
            {
                if (_whiteboard == null)
                {
                    _whiteboard = _touch.collider.transform.GetComponent<Whiteboard>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y); 

                var x = (int)(_touchPos.x * _whiteboard.textureSize.x - (_penSize / 2));
                var y = (int)(_touchPos.y * _whiteboard.textureSize.y - (_penSize / 2));

                if (y < 0 || y > _whiteboard.textureSize.y || x < 0 || x > _whiteboard.textureSize.x)
                {
                    return;
                }
                

                if (_touchedLastFrame)
                {
                    _whiteboard.texture.SetPixels(x, y, _penSize, _penSize, _colors);
                    for (float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);

                        _whiteboard.texture.SetPixels(lerpX, lerpY, _penSize, _penSize, _colors);
                    }
                    transform.rotation = _lastTouchRot;

                    _whiteboard.texture.Apply();
                }
                _lastTouchPos = new Vector2(x, y);
                _lastTouchRot = transform.rotation;
                _touchedLastFrame = true;
                return;
            }
        }
        _whiteboard = null;
        _touchedLastFrame = false;
    }
}
*/