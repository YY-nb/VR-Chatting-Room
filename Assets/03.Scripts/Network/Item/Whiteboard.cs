using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Whiteboard : MonoBehaviour,IPunObservable
{
    public Texture2D texture;
    public Vector2 textureSize = new Vector2(2048, 2048);
    private PhotonView view;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    void Start()
    {
        view = GetComponent<PhotonView>();  
        var r = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = texture;
    }
    public void DisplayDrawing(int x, int y, int _penSize, Vector3[] colorVectors, Vector2 lastTouchPos)
    {
        view.RPC(nameof(DrawRPC), RpcTarget.All, x, y, _penSize, colorVectors, lastTouchPos);
    }
    [PunRPC]
    public void DrawRPC(int x, int y, int _penSize, Vector3[] colorVectors, Vector2 lastTouchPos)
    {
        Color[] colors = ColorUtil.Vector3ArrayToColorArray(colorVectors);
        
        texture.SetPixels(x, y, _penSize, _penSize, colors);
        for (float f = 0.01f; f < 0.9f; f += 0.01f)
        {
            var lerpX = (int)Mathf.Lerp(lastTouchPos.x, x, f);
            var lerpY = (int)Mathf.Lerp(lastTouchPos.y, y, f);

            texture.SetPixels(lerpX, lerpY, _penSize, _penSize, colors);
        }
        //transform.rotation = lastTouchRot;

        texture.Apply();
    }

}
