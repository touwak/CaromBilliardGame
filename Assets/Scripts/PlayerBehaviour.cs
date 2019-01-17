using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour {

    #region Variables
    Camera camera;
    Rigidbody rigidbody;

    [SerializeField]
    [Tooltip("Min hit strength")]
    float minStrength = 10.0f;
    [SerializeField]
    [Tooltip("Max hit strength")]
    float maxStrength = 2000.0f;
    [SerializeField]
    [Tooltip("Time to reach max hit strength")]
    float accelerationTime = 3.0f;
    float acceleration;
    float finalStrength;


    bool isMoving;
    bool redBallTouched;
    bool yellowBallTouched;
    #endregion

    void Start () {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rigidbody = GetComponent<Rigidbody>();

        // Calculate the acceleration in time
        acceleration = (maxStrength - minStrength) / accelerationTime;
        finalStrength = minStrength;

        isMoving = false;
        redBallTouched = false;
        yellowBallTouched = false;
	}

    void Update() {

        IsMoving();

        if (Input.GetButton("HitBall") && !isMoving) {
            IncrementHitForce();
        }
        if (Input.GetButtonUp("HitBall") && !isMoving) {
            HitBall();
        }
    }

    /// <summary>
    ///  Check if the ball is moving
    /// </summary>
    void IsMoving() {
        if(rigidbody.velocity != Vector3.zero) {
            isMoving = true;
        }
        else {
            isMoving = false;
            redBallTouched = false;
            yellowBallTouched = false;
        }

        //Debug.Log("isMoving: " + isMoving);
    }

    /// <summary>
    /// Hit the ball in the forward direction of the camera
    /// </summary>
    void HitBall() {
        Vector3 direction = camera.transform.forward;
        direction.y = 0.0f;
        rigidbody.AddForce(direction * finalStrength, ForceMode.Impulse);

        // reset strength
        finalStrength = minStrength;

        isMoving = true;
    }

    /// <summary>
    /// Increment strength in time
    /// </summary>
    void IncrementHitForce() {
        if(finalStrength < maxStrength) {
            finalStrength += acceleration * Time.deltaTime;
        }

        //Debug.Log("acceleration: " + finalStrength);
    }

    private void OnCollisionEnter(Collision collision) {

        // Red ball
        if(collision.collider.CompareTag("RedBall") && isMoving && !redBallTouched) {
            redBallTouched = true;
        }

        // Yellow ball
        if (collision.collider.CompareTag("YellowBall") && isMoving && !yellowBallTouched) {
            yellowBallTouched = true;
        }

        if(redBallTouched && yellowBallTouched) {
            redBallTouched = false;
            yellowBallTouched = false;

            Debug.Log("1 point!");
        }
    }
}
