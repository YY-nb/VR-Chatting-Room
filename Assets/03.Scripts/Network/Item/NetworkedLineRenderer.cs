// Import the necessary Photon PUN2 libraries
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

// Define a class for the networked line renderer
public class NetworkedLineRenderer : MonoBehaviourPunCallbacks, IPunObservable
{
    // Define variables for the line renderer and its positions
    private LineRenderer lineRenderer;
    //private Vector3[] positions;
    private List<Vector3> positions;
    // Define a method to initialize the line renderer
    void Start()
    {
        // Get the line renderer component
        lineRenderer = GetComponent<LineRenderer>();

        // Set the initial positions of the line renderer
        positions = new List<Vector3>();
        //lineRenderer.GetPositions(positions);
    }

    // Define a method to update the line renderer positions
    void Update()
    {
        // Only update the line renderer if it belongs to the local player
        if (photonView.IsMine)
        {
            // Get the updated positions of the line renderer
            //lineRenderer.GetPositions(positions);
            lineRenderer.SetPositions(positions.ToArray());
            // Update the positions of the line renderer
            //photonView.RPC(nameof(UpdateLineRenderer), RpcTarget.Others, positions);
        }
    }

    // Define a method to update the line renderer positions for all players
    [PunRPC]
    void UpdateLineRenderer(Vector3[] newPositions)
    {
        // Update the positions of the line renderer
        lineRenderer.SetPositions(newPositions);
    }
    public void AddPosition(Vector3 newPosition)
    {
        positions.Add(newPosition);
    }
    // Implement the IPunObservable interface
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // If writing to the stream, send the positions of the line renderer
        if (stream.IsWriting)
        {
            // Send the list of positions over the network
            stream.SendNext(positions.Count);
            foreach (Vector3 position in positions)
            {
                stream.SendNext(position);
            }
        }

        // If reading from the stream, receive the positions of the line renderer
        else if (stream.IsReading)
        {
            // Receive the list of positions over the network
            int count = (int)stream.ReceiveNext();
            positions.Clear();
            for (int i = 0; i < count; i++)
            {
                positions.Add((Vector3)stream.ReceiveNext());
            }
        }
    }
}