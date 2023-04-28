using BNG;
using Photon.Pun;
using Photon.Realtime;
using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Rendering;
using UnityEngine;

public class NetworkPlayerIK :
#if PUN_2_OR_NEWER
MonoBehaviourPunCallbacks, IPunObservable, IPunOwnershipCallbacks
#else
        MonoBehaviour
#endif
{
    #region 本地角色
    [Header("Local Role Component")]

    [Tooltip("Transform of the local player's head to track. This will be applied to the Remote Player's Head Transform")]
    public Transform PlayerHeadTransform;
    public Transform PlayerLeftHandTransform;
    public Transform PlayerRightHandTransform;

    

    // Store positions to move between updates
    private Vector3 _syncHeadStartPosition = Vector3.zero;
    private Vector3 _syncHeadEndPosition = Vector3.zero;
    private Quaternion _syncHeadStartRotation = Quaternion.identity;
    private Quaternion _syncHeadEndRotation = Quaternion.identity;

   

    // Send Hand Animation info to others
    public HandController LeftHandController;
    public HandController RightHandController;

   

    [Tooltip("Local Player's Left Grabber. Used to determine which objects are nearby")]
    public Grabber LeftGrabber;
    GrabbablesInTrigger gitLeft;

    [Tooltip("Local Player's Right Grabber. Used to determine which objects are nearby")]
    public Grabber RightGrabber;
    GrabbablesInTrigger gitRight;

    #endregion

    #region Remote角色
    [Header("Remote Role Component")]

    public Transform RemoteIKRig;
    [Tooltip("Transform of the remote player's head. This will be updated during Update")]
    public Transform RemoteHeadTransform;

    [Tooltip("Transform of the remote player's left hand / controller. This will be updated during Update")]
    public Transform RemoteLeftHandTransform;

    // Store positions to move between updates
    private Vector3 _syncLHandStartPosition = Vector3.zero;
    private Vector3 _syncLHandEndPosition = Vector3.zero;
    private Quaternion _syncLHandStartRotation = Quaternion.identity;
    private Quaternion _syncLHandEndRotation = Quaternion.identity;

    [Tooltip("Transform of the remote player's right hand / controller. This will be updated during Update")]
    public Transform RemoteRightHandTransform;

    // Store positions to move between updates
    private Vector3 _syncRHandStartPosition = Vector3.zero;
    private Vector3 _syncRHandEndPosition = Vector3.zero;
    private Quaternion _syncRHandStartRotation = Quaternion.identity;
    private Quaternion _syncRHandEndRotation = Quaternion.identity;

    // Receive Animation info
    public Animator RemoteLeftHandAnimator;
    public Animator RemoteRightHandAnimator;
    public Animator RemoteAnimator;

    #endregion

    [Tooltip("How fast to animate the fingers on the remote players hands")]
    public float HandAnimationSpeed = 20f;

    private enum AnimationLayer
    {
        BaseLayer,
        RightHandBaseLayer,
        RightPoint,
        RightThumb,
        LeftHandBaseLayer,
        LeftPoint,
        LeftThumb,
    }

    // Used for Hand Interpolation
    private float _syncLeftGripStart;
    private float _syncRightGripStart;
    private float _syncLeftPointStart;
    private float _syncRightPointStart;
    private float _syncLeftThumbStart;
    private float _syncRightThumbStart;

    private float _syncLeftGripEnd;
    private float _syncRightGripEnd;
    private float _syncLeftPointEnd;
    private float _syncRightPointEnd;
    private float _syncLeftThumbEnd;
    private float _syncRightThumbEnd;

    // Interpolation values
    private float _lastSynchronizationTime = 0f;
    private float _syncDelay = 0f;
    private float _syncTime = 0f;

    // network request grabbable permission
    protected double lastRequestTime = 0;
    protected float requestInterval = 0.1f; // 0.1 = 10 times per second
    Dictionary<int, double> requestedGrabbables;

    bool disabledObjects = false;

    private bool _syncLeftHoldingItem;
    private bool _syncRightHoldingItem;

    private Transform player;

    #region 常用字符串名字
    private string playerTag = "Player";
    private string Role = "Role";
    private string RoleGrabber = "Role Grabber";
    private string LeftControllerName = "LeftController";
    private string RightControllerName = "RightController";
    private string HeadIKTarget = nameof(HeadIKTarget);
    private string LeftHandIKTarget = nameof(LeftHandIKTarget);
    private string RightHandIKTarget = nameof(RightHandIKTarget);
    private string LeftHandName = "ModelLeftHand";
    private string RightHandName = "ModelRightHand";
    private string LeftGrabberPath = "GrabberOffsetLeft/Grabber";
    private string RightGrabberPath = "GrabberOffsetRight/Grabber";
    //动画参数
    private string A_RightGrip = "R_Grip";
    private string A_RightTrigger = "R_Trigger";
    private string A_RightThumb = "R_Thumb";
    private string A_LeftGrip = "L_Grip";
    private string A_LeftTrigger = "L_Trigger";
    private string A_LeftThumb = "L_Thumb";
    #endregion

#if PUN_2_OR_NEWER
    private void Awake()
    {
        player = GameObject.FindWithTag(playerTag).transform;
    }
    void Start()
    {       
        InitGrabber();

    }

    void Update()
    {

        // Check for request to grab object
        checkGrabbablesTransfer();

        // Remote Player
        if (!photonView.IsMine)
        {

            if (disabledObjects)
            {
                ToggleObjects(true);
            }

            // Keeps latency in mind to keep player in sync
            _syncTime += Time.deltaTime;
            float synchValue = _syncTime / _syncDelay;

            // Update Head and Hands Positions
            UpdateRemotePositionRotation(RemoteHeadTransform, _syncHeadStartPosition, _syncHeadEndPosition, _syncHeadStartRotation, _syncHeadEndRotation, synchValue);
            UpdateRemotePositionRotation(RemoteLeftHandTransform, _syncLHandStartPosition, _syncLHandEndPosition, _syncLHandStartRotation, _syncLHandEndRotation, synchValue);
            UpdateRemotePositionRotation(RemoteRightHandTransform, _syncRHandStartPosition, _syncRHandEndPosition, _syncRHandStartRotation, _syncRHandEndRotation, synchValue);

            // Update animation info
            UpdateAnimationInfo();
            
        }
        else
        {
            if (!disabledObjects)
            {
                ToggleObjects(false);
            }
        }
    }
    private void InitGrabber()
    {
        if (LeftGrabber == null)
        {
            var leftHand =  GetChildTransformByTag(player.Find(RoleGrabber), LeftHandName);
            var grabberObj = leftHand.Find(LeftGrabberPath);
            LeftGrabber = grabberObj.GetComponent<Grabber>();
        }
        gitLeft = LeftGrabber.GetComponent<GrabbablesInTrigger>();
        if (RightGrabber == null)
        {
            var rightHand = GetChildTransformByTag(player.Find(RoleGrabber), RightHandName);
            var grabberObj = rightHand.Find(RightGrabberPath);
            RightGrabber = grabberObj.GetComponent<Grabber>();
        }
        gitRight = RightGrabber.GetComponent<GrabbablesInTrigger>();
        requestedGrabbables = new Dictionary<int, double>();
    }
    private void UpdateAnimationInfo()
    {
        /*
        if (RemoteLeftHandAnimator)
        {
            _syncLeftGripStart = Mathf.Lerp(_syncLeftGripStart, _syncLeftGripEnd, Time.deltaTime * HandAnimationSpeed);
            RemoteLeftHandAnimator.SetFloat("Flex", _syncLeftGripStart);
            RemoteLeftHandAnimator.SetLayerWeight(0, 1);

            _syncLeftPointStart = Mathf.Lerp(_syncLeftPointStart, _syncLeftPointEnd, Time.deltaTime * HandAnimationSpeed);
            RemoteLeftHandAnimator.SetLayerWeight(2, _syncLeftPointStart);

            _syncLeftThumbStart = Mathf.Lerp(_syncLeftThumbStart, _syncLeftThumbEnd, Time.deltaTime * HandAnimationSpeed);
            RemoteLeftHandAnimator.SetLayerWeight(1, _syncLeftThumbStart);

            // Default to grip if holding an item
            if (_syncLeftHoldingItem)
            {
                RemoteLeftHandAnimator.SetLayerWeight(0, 0);
                RemoteLeftHandAnimator.SetFloat("Flex", 1);
                RemoteLeftHandAnimator.SetFloat(1, 0);
                RemoteLeftHandAnimator.SetFloat(2, 0);
            }
            else
            {
                RemoteLeftHandAnimator.SetInteger("Pose", 0);
            }
        }
        if (RemoteRightHandAnimator)
        {
            _syncRightGripStart = Mathf.Lerp(_syncRightGripStart, _syncRightGripEnd, Time.deltaTime * HandAnimationSpeed);
            RemoteRightHandAnimator.SetFloat("Flex", _syncRightGripStart);

            _syncRightPointStart = Mathf.Lerp(_syncRightPointStart, _syncRightPointEnd, Time.deltaTime * HandAnimationSpeed);
            RemoteRightHandAnimator.SetLayerWeight(2, _syncRightPointStart);

            _syncRightThumbStart = Mathf.Lerp(_syncRightThumbStart, _syncRightThumbEnd, Time.deltaTime * HandAnimationSpeed);
            RemoteRightHandAnimator.SetLayerWeight(1, _syncRightThumbStart);

            // Default to grip if holding an item
            if (_syncRightHoldingItem)
            {
                RemoteRightHandAnimator.SetLayerWeight(0, 0);
                RemoteRightHandAnimator.SetFloat("Flex", 1);
                RemoteRightHandAnimator.SetLayerWeight(1, 0);
                RemoteRightHandAnimator.SetLayerWeight(2, 0);
            }
            else
            {
                RemoteRightHandAnimator.SetInteger("Pose", 0);
            }
        }*/
        if (RemoteAnimator)
        {
            SetLeftAnimator();
            SetRightAnimator();

        }
    }
    private void SetRightAnimator()
    {
        //设置grip
        _syncRightGripStart = Mathf.Lerp(_syncRightGripStart, _syncRightGripEnd, Time.deltaTime * HandAnimationSpeed);
        RemoteAnimator.SetFloat(A_RightGrip, _syncRightGripStart);
        RemoteAnimator.SetLayerWeight((int)AnimationLayer.RightHandBaseLayer, 1);
        //设置食指
        _syncRightPointStart = Mathf.Lerp(_syncRightPointStart, _syncRightPointEnd, Time.deltaTime * HandAnimationSpeed); 
        RemoteAnimator.SetFloat(A_RightTrigger, _syncRightPointStart);
        RemoteAnimator.SetLayerWeight((int)AnimationLayer.RightPoint, Mathf.Abs(1- _syncRightPointStart));//食指弯曲时值是0，但是要让动画层级权重为1
        //设置大拇指
        _syncRightThumbStart = Mathf.Lerp(_syncRightThumbStart, _syncRightThumbEnd, Time.deltaTime * HandAnimationSpeed);
        RemoteAnimator.SetFloat(A_RightThumb, _syncRightThumbStart);
        RemoteAnimator.SetLayerWeight((int)AnimationLayer.RightThumb, Mathf.Abs(1 - _syncRightThumbStart));

        if (_syncRightHoldingItem)
        {
            RemoteAnimator.SetLayerWeight((int)AnimationLayer.RightHandBaseLayer, 0);
            RemoteAnimator.SetFloat(A_RightGrip, 1);
            RemoteAnimator.SetLayerWeight((int)AnimationLayer.RightPoint, 0);
            RemoteAnimator.SetLayerWeight((int)AnimationLayer.RightThumb, 0);
        }
        else
        {
            RemoteAnimator.SetInteger("RightPose", 0);
        }
    }
    private void SetLeftAnimator()
    {
        _syncLeftGripStart = Mathf.Lerp(_syncLeftGripStart, _syncLeftGripEnd, Time.deltaTime * HandAnimationSpeed);
        RemoteAnimator.SetFloat(A_LeftGrip, _syncLeftGripStart);
        RemoteAnimator.SetLayerWeight((int)AnimationLayer.LeftHandBaseLayer, 1);

        _syncLeftPointStart = Mathf.Lerp(_syncLeftPointStart, _syncLeftPointEnd, Time.deltaTime * HandAnimationSpeed);
        RemoteAnimator.SetFloat(A_LeftTrigger, _syncLeftPointStart);
        RemoteAnimator.SetLayerWeight((int)AnimationLayer.LeftPoint, Mathf.Abs(1 - _syncLeftPointStart));

        _syncLeftThumbStart = Mathf.Lerp(_syncLeftThumbStart, _syncLeftThumbEnd, Time.deltaTime * HandAnimationSpeed);
        RemoteAnimator.SetFloat(A_LeftThumb, _syncLeftThumbStart);
        RemoteAnimator.SetLayerWeight((int)AnimationLayer.LeftThumb, Mathf.Abs(1 - _syncLeftThumbStart));

        // Default to grip if holding an item
        if (_syncLeftHoldingItem)
        {
            RemoteAnimator.SetLayerWeight((int)AnimationLayer.LeftHandBaseLayer, 0);
            RemoteAnimator.SetFloat(A_LeftGrip, 1);
            RemoteAnimator.SetLayerWeight((int)AnimationLayer.LeftPoint, 0);
            RemoteAnimator.SetLayerWeight((int)AnimationLayer.LeftThumb, 0);
        }
        else
        {
            RemoteAnimator.SetInteger("LeftPose", 0);
        }
    }
    public void AssignPlayerObjects()
    {
        
        Transform cameraRig = player.Find("CameraRig");
        Transform role = player.Find(Role);
        VRIK roleVRIK = role.GetComponent<VRIK>();
        if (PlayerHeadTransform == null)
        {
            PlayerHeadTransform = GetChildTransformByName(cameraRig, HeadIKTarget);
           // PlayerHeadTransform = roleVRIK.solver.spine.headTarget;
        }
        
        if(PlayerLeftHandTransform == null)
        {
            // Using an explicit Transform name to make sure we grab the right one in the scene
           PlayerLeftHandTransform = GetChildTransformByName(cameraRig, LeftHandIKTarget);
            //PlayerLeftHandTransform = roleVRIK.solver.leftArm.target;
        }
        if(LeftHandController == null)
        {
            //LeftController/Models/LeftHandIKTarget
            LeftHandController = PlayerLeftHandTransform.parent.parent.GetComponent<HandController>(); 
            //LeftHandController = GetChildTransformByName(cameraRig, LeftControllerName)?.GetComponent<HandController>();
        }
        if(PlayerRightHandTransform == null)
        {
           PlayerRightHandTransform = GetChildTransformByName(cameraRig, RightHandIKTarget);
            //PlayerRightHandTransform = roleVRIK.solver.rightArm.target;
        }
        if(RightHandController == null)
        {
            RightHandController = PlayerRightHandTransform.parent.parent.GetComponent<HandController>();
            //RightHandController = GetChildTransformByName(cameraRig, RightControllerName)?.GetComponent<HandController>();
        }
        
        
    }

    private Transform GetChildTransformByName(Transform search, string name)
    {
        Transform[] children = search.GetComponentsInChildren<Transform>();
        for (int x = 0; x < children.Length; x++)
        {
            Transform child = children[x];
            if (child.name == name)
            {
                return child;
            }
        }

        return null;
    }

    private Transform GetChildTransformByTag(Transform search, string tagName)
    {
        Transform[] children = search.GetComponentsInChildren<Transform>();
        for (int x = 0; x < children.Length; x++)
        {
            Transform child = children[x];
            if (child.CompareTag(tagName))
            {
                return child;
            }
        }

        return null;
    }

    void ToggleObjects(bool enableObjects)
    {
        RemoteIKRig.gameObject.SetActive(enableObjects);
        RemoteHeadTransform.gameObject.SetActive(enableObjects);
        RemoteLeftHandTransform.gameObject.SetActive(enableObjects);
        RemoteRightHandTransform.gameObject.SetActive(enableObjects);
        disabledObjects = !enableObjects;
    }

    void checkGrabbablesTransfer()
    {

        // Cap the request period
        if (PhotonNetwork.Time - lastRequestTime < requestInterval)
        {
            return;
        }

        requestOwnerShipForNearbyGrabbables(gitLeft);
        requestOwnerShipForNearbyGrabbables(gitRight);
    }

    void requestOwnerShipForNearbyGrabbables(GrabbablesInTrigger grabbables)
    {

        if (grabbables == null)
        {
            return;
        }

        // In Hand
        foreach (var grab in grabbables.NearbyGrabbables)
        {
            PhotonView view = grab.Value.GetComponent<PhotonView>();

            if (view != null && RecentlyRequested(view) == false && !view.AmOwner)
            {
                RequestGrabbableOwnership(view);
            }
        }

        // Remote Grabbables
        foreach (var grab in grabbables.ValidRemoteGrabbables)
        {
            PhotonView view = grab.Value.GetComponent<PhotonView>();

            if (view != null && RecentlyRequested(view) == false && !view.AmOwner)
            {
                RequestGrabbableOwnership(view);
            }
        }
    }

    public virtual bool RecentlyRequested(PhotonView view)
    {
        // Previously requested if in list and requested less than 3 seconds ago
        return requestedGrabbables != null && requestedGrabbables.ContainsKey(view.ViewID) && PhotonNetwork.Time - requestedGrabbables[view.ViewID] < 3f;
    }

    public virtual void RequestGrabbableOwnership(PhotonView view)
    {

        lastRequestTime = PhotonNetwork.Time;

        if (requestedGrabbables.ContainsKey(view.ViewID))
        {
            requestedGrabbables[view.ViewID] = lastRequestTime;
        }
        else
        {
            requestedGrabbables.Add(view.ViewID, lastRequestTime);
        }

        view.RequestOwnership();
    }

    void UpdateRemotePositionRotation(Transform moveTransform, Vector3 startPosition, Vector3 endPosition, Quaternion syncStartRotation, Quaternion syncEndRotation, float syncValue)
    {
        float dist = Vector3.Distance(startPosition, endPosition);

        // If far away just teleport there
        if (dist > 0.5f)
        {
            moveTransform.position = endPosition;
            moveTransform.rotation = syncEndRotation;
        }
        else
        {
            moveTransform.position = Vector3.Lerp(startPosition, endPosition, syncValue);
            moveTransform.rotation = Quaternion.Lerp(syncStartRotation, syncEndRotation, syncValue);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // This is our player, send our positions to the other players
        if (stream.IsWriting)
        {

            // Player Head / Hand Information
            stream.SendNext(PlayerHeadTransform.position);
            stream.SendNext(PlayerHeadTransform.rotation);
            stream.SendNext(PlayerLeftHandTransform.position);
            stream.SendNext(PlayerLeftHandTransform.rotation);
            stream.SendNext(PlayerRightHandTransform.position);
            stream.SendNext(PlayerRightHandTransform.rotation);

            // Hand Animator Info
            if (LeftHandController)
            {
                stream.SendNext(LeftHandController.GripAmount);
                stream.SendNext(LeftHandController.PointAmount);
                stream.SendNext(LeftHandController.ThumbAmount);
                stream.SendNext(LeftHandController.PoseId);
                stream.SendNext(LeftHandController.grabber.HoldingItem);
            }
            if (RightHandController)
            {
                stream.SendNext(RightHandController.GripAmount);
                stream.SendNext(RightHandController.PointAmount);
                stream.SendNext(RightHandController.ThumbAmount);
                stream.SendNext(RightHandController.PoseId);  
                stream.SendNext(RightHandController.grabber.HoldingItem);
            }
        }
        else
        {
            // Remote player, receive data
            // Head
            this._syncHeadStartPosition = RemoteHeadTransform.position;
            this._syncHeadEndPosition = (Vector3)stream.ReceiveNext();
            this._syncHeadStartRotation = RemoteHeadTransform.rotation;
            this._syncHeadEndRotation = (Quaternion)stream.ReceiveNext();

            // Left Hand
            this._syncLHandStartPosition = RemoteLeftHandTransform.position;
            this._syncLHandEndPosition = (Vector3)stream.ReceiveNext(); 
            this._syncLHandStartRotation = RemoteLeftHandTransform.rotation;
            this._syncLHandEndRotation = (Quaternion)stream.ReceiveNext();

            // Right Hand
            this._syncRHandStartPosition = RemoteRightHandTransform.position;
            this._syncRHandEndPosition = (Vector3)stream.ReceiveNext();
            this._syncRHandStartRotation = RemoteRightHandTransform.rotation;
            this._syncRHandEndRotation = (Quaternion)stream.ReceiveNext();
            /*
            // Left Hand Animation Updates
            if (RemoteLeftHandAnimator)
            {
                _syncLeftGripEnd = (float)stream.ReceiveNext();
                _syncLeftPointEnd = (float)stream.ReceiveNext();
                _syncLeftThumbEnd = (float)stream.ReceiveNext();

                // Can set hand pose immediately
                RemoteLeftHandAnimator.SetInteger("Pose", (int)stream.ReceiveNext());

                _syncLeftHoldingItem = (bool)stream.ReceiveNext();
            }
            
            if (RemoteRightHandAnimator)
            {
                _syncRightGripEnd = (float)stream.ReceiveNext();
                _syncRightPointEnd = (float)stream.ReceiveNext();
                _syncRightThumbEnd = (float)stream.ReceiveNext();

                // Can set hand pose immediately
                RemoteRightHandAnimator.SetInteger("Pose", (int)stream.ReceiveNext());

                _syncRightHoldingItem = (bool)stream.ReceiveNext();
            }
            */
            if (RemoteAnimator)
            {
                //左手
                _syncLeftGripEnd = (float)stream.ReceiveNext();
                _syncLeftPointEnd = (float)stream.ReceiveNext();
                _syncLeftThumbEnd = (float)stream.ReceiveNext();
               // Can set hand pose immediately
                RemoteAnimator.SetInteger("LeftPose", (int)stream.ReceiveNext());

                _syncLeftHoldingItem = (bool)stream.ReceiveNext();

                //右手
                _syncRightGripEnd = (float)stream.ReceiveNext();
                _syncRightPointEnd = (float)stream.ReceiveNext();
                _syncRightThumbEnd = (float)stream.ReceiveNext();
                // Can set hand pose immediately
                int id = (int)stream.ReceiveNext();
                RemoteAnimator.SetInteger("RightPose", id); 

                _syncRightHoldingItem = (bool)stream.ReceiveNext();
            }

            _syncTime = 0f;
            _syncDelay = Time.time - _lastSynchronizationTime;
            _lastSynchronizationTime = Time.time;
        }
    }

    // Handle Ownership Requests (Ex: Grabbable Ownership)
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {

        bool amOwner = targetView.AmOwner || (targetView.Owner == null && PhotonNetwork.IsMasterClient);

        NetworkedGrabbable netGrabbable = targetView.gameObject.GetComponent<NetworkedGrabbable>();
        if (netGrabbable != null)
        {
            // Authorize transfer of ownership if we're not holding it
            if (amOwner && !netGrabbable.BeingHeld)
            {
                targetView.TransferOwnership(requestingPlayer.ActorNumber);
                return;
            }
        }
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player requestingPlayer)
    {
        // Debug.Log("OnOwnershipTransfered to Player " + requestingPlayer);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player requestingPlayer)
    {
        // Debug.Log("OnOwnershipTransferFailed for Player " + requestingPlayer);
    }

#endif
}