using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBNGHandController : HandController
{
    [Header("Pose Setting")]
    public HandPose Pose1;
    public HandPose Pose2;

    public HandSide handSide;
    public string characterName;
    

    private void InitPose()
    {
        if(Pose1 == null)
        {
            string pose1 = $"{characterName} Open";
            if (handSide == HandSide.Left)
            {
                Pose1 = Resources.Load<HandPose>(pose1 + "(L)");
            }
            else
            {
                Pose1 = Resources.Load<HandPose>(pose1 + "(R)");
            }
        }
        if(Pose2 == null)
        {
            string pose2 = $"{characterName} Closed";
            if (handSide == HandSide.Left)
            {
                Pose2 = Resources.Load<HandPose>(pose2 + "(L)");
            }
            else
            {
                Pose2 = Resources.Load<HandPose>(pose2 + "(R)");
            }
        }
       
       
    }
    public override bool SetupPoseBlender()
    {
        // Make sure we have a valid handPoser to work with
        if (handPoser == null || !handPoser.isActiveAndEnabled)
        {
            handPoser = GetComponentInChildren<HandPoser>(false);
        }

        // No HandPoser is found, we should just exit
        if (handPoser == null)
        {
            return false;
            // Debug.Log("Adding Hand Poser to " + transform.name);
            // handPoser = this.gameObject.AddComponent<HandPoser>();
        }

        // If no pose blender is found, add it and set it up so we can use it in Update()
        if (poseBlender == null || !poseBlender.isActiveAndEnabled)
        {
            poseBlender = GetComponentInChildren<HandPoseBlender>();
        }

        // If no pose blender is found, add it and set it up so we can use it in Update()
        if (poseBlender == null)
        {
            // Set up the blend to use some default poses
            InitPose();
            if (handPoser != null)
            {
                poseBlender = handPoser.gameObject.AddComponent<HandPoseBlender>();
                poseBlender.Pose1 = Pose1;
                poseBlender.Pose2 = Pose2; 
            }
            else
            {
                poseBlender = this.gameObject.AddComponent<HandPoseBlender>();
                poseBlender.Pose1 = Pose1;
                poseBlender.Pose2 = Pose2; 
            }

            // Don't update pose in Update since we will be controlling this ourselves
            poseBlender.UpdatePose = false;

            
           // poseBlender.Pose1 = GetDefaultOpenPose();
           // poseBlender.Pose2 = GetDefaultClosedPose();
        }

        return true;
    }
    private void UpdateHandPoseID()
    {
        var grabbable = (grabber.HeldGrabbable) as CustomNetworkedGrabbable;
        if(grabbable is CustomNetworkedGrabbable networkedGrabbable)
        {
            PoseId = (int)networkedGrabbable.handPoseId; 
        }
       
    }
    
    public override void UpdateHeldObjectState()
    {
        // Holding Animator Grabbable
        if (IsAnimatorGrabbable())
        {
            UpdateAnimimationStates();
        }
        // Holding Hand Poser Grabbable
        else if (IsHandPoserGrabbable())
        {
            UpdateHandPoseID();
            UpdateHandPoser();
        }
        // Holding Auto Poser Grabbable
        else if (IsAutoPoserGrabbable())
        {
            //EnableAutoPoser();
        }
    }
    public override void UpdateIdleState()
    {
        // Not holding something - update the idle state
        if (IdlePoseType == HandPoserType.Animator)
        {
            UpdateAnimimationStates();
        }
        else if (IdlePoseType == HandPoserType.HandPoser)
        {
            //UpdateHandPoser();
            UpdateHandPoserIdleState();
            PoseId = 0;

        }
        else if (IdlePoseType == HandPoserType.AutoPoser)
        {
            EnableAutoPoser(true);
        }
    }
}
