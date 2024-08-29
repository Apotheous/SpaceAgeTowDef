using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCamFollower : MonoBehaviour
{
    private void OnMouseDown()
    {
        CameraController.instance.followTransform = transform;
    }
}
