using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTest : MonoBehaviour
{
    public Transform LeftTarget;
    public Transform RightTarget;
    public Vector3 LeftPos;
    public Vector3 RightPos;
    public Vector3 LeftAngle;
    public Vector3 RightAngle;
    // Start is called before the first frame update
    void Start()
    {
        LeftPos = LeftTarget.position;
        RightPos = RightTarget.position;
        LeftAngle = LeftTarget.eulerAngles;
        RightAngle = RightTarget.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
