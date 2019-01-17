using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    #region Variables
    [Header("Camera")]
    [Range(0.0f, 10.0f)]
    public float camRotationSpeed = 5.0f;
    [Range(0.0f, 10.0f)]
    public float smoothFactor = 5.0f;
    public Transform playerTransform;

    Quaternion camTurnAngle;
    Vector3 cameraOffset;
    #endregion

    // Use this for initialization
    void Start() {
       
        cameraOffset = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void Update() {
        RotateCamera();
    }

    void RotateCamera() {
        camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * camRotationSpeed, Vector3.up);

        cameraOffset = camTurnAngle * cameraOffset;

        Vector3 newPos = playerTransform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
        transform.LookAt(playerTransform);
    }
}
