using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour {

    #region Variables
    Camera camera;
    Rigidbody rigidbody;

    [SerializeField]
    float minStrength = 10.0f;
    [SerializeField]
    float maxStrength = 2000.0f;
    [SerializeField]
    float accelerationTime = 3.0f;
    float acceleration;
    float finalStrength;
    #endregion

	void Start () {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rigidbody = GetComponent<Rigidbody>();

        acceleration = (maxStrength - minStrength) / accelerationTime;
        finalStrength = minStrength;
	}

    void Update() {

        if (Input.GetButton("HitBall")) {
            IncrementHitForce();
        }
        if (Input.GetButtonUp("HitBall")) {
            HitBall();
        }
    }

    /// <summary>
    /// Hit the ball in the forward direction of the camera
    /// </summary>
    void HitBall() {
        Vector3 direction = camera.transform.forward;
        direction.y = 0.0f;
        rigidbody.AddForce(direction * finalStrength);

        // reset strength
        finalStrength = minStrength;
    }

    /// <summary>
    /// Increment strength in time
    /// </summary>
    void IncrementHitForce() {
        if(finalStrength < maxStrength) {
            finalStrength += acceleration * Time.deltaTime;
        }

        Debug.Log("acceleration: " + finalStrength);
    }
}
