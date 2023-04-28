/*
	Copyright (c) CompuGenius Programs. All rights reserved.
	https://www.cgprograms.com
*/

using UnityEngine;

namespace CGPrograms
{
    public class VRIKCharacterYOffset : MonoBehaviour
    {
        // This is used to offset a FinalIK character by it's parent. This fixes some positional issues.
        private void LateUpdate()
        {
            var parent = transform.parent;
            var yOffset = parent.localPosition.y;
            
            var floorElevationOffset = parent.GetComponent<CharacterController>().bounds.min.y;
            
            transform.localPosition = new Vector3(transform.localPosition.x, floorElevationOffset - yOffset, transform.localPosition.z);
        }
    }
}