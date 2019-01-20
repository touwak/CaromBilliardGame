using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    #region Variables
    [Range(0.0f, 10.0f)]
    public float camRotationSpeed = 5.0f;
    [Range(0.0f, 10.0f)]
    public float smoothFactor = 5.0f;
    public Transform rotateAxis;

    Quaternion camTurnAngle;
    Vector3 cameraOffset;
    #endregion

    void Start() {
       
        cameraOffset = transform.position - rotateAxis.position;
    }

    void Update() {
        if (!GameManager.instance.IsOver) {
            RotateCamera(rotateAxis);
        }
    }

    /// <summary>
    /// Rotate the camera around a transform
    /// </summary>
    void RotateCamera(Transform center) {
        camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * camRotationSpeed, Vector3.up);

        cameraOffset = camTurnAngle * cameraOffset;

        Vector3 newPos = center.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
        transform.LookAt(center);
    }
}
