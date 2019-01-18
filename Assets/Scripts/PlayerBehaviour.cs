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

    bool redBallTouched;
    bool yellowBallTouched;

    [SerializeField]
    int rayCastingSections = 2;
    List<Vector3> rayCastingPoints;
    LineRenderer lineRenderer;
    #endregion

    void Start () {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rigidbody = GetComponent<Rigidbody>();

        // Calculate the acceleration in time
        acceleration = (maxStrength - minStrength) / accelerationTime;
        finalStrength = minStrength;

        IsMoving = false;
        redBallTouched = false;
        yellowBallTouched = false;

        rayCastingPoints = new List<Vector3>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = rayCastingSections;
	}

    void Update() {

        DetectMovement();

        // Shoot
        if (Input.GetButton("HitBall") && !IsMoving) {
            IncrementHitForce();
        }
        if (Input.GetButtonUp("HitBall") && !IsMoving) {
            HitBall();
        }

        // Ball direction
        if (!IsMoving) {
            BallDirection();
        }
    }

    #region Movement Detection
    public bool IsMoving { get; private set; }

    /// <summary>
    ///  Check if the ball is moving
    /// </summary>
    void DetectMovement() {
        if(rigidbody.velocity != Vector3.zero) {
            IsMoving = true;
        }
        else {
            IsMoving = false;
            redBallTouched = false;
            yellowBallTouched = false;
        }

        //Debug.Log("isMoving: " + isMoving);
    }
    #endregion

    #region Hit Ball
    /// <summary>
    /// Hit the ball in the forward direction of the camera
    /// </summary>
    void HitBall() {
        Vector3 direction = camera.transform.forward;
        direction.y = 0.0f;
        rigidbody.AddForce(direction * finalStrength, ForceMode.Impulse);

        // reset strength
        finalStrength = minStrength;

        IsMoving = true;
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
    #endregion

    private void OnCollisionEnter(Collision collision) {

        // Red ball
        if(collision.collider.CompareTag("RedBall") && IsMoving && !redBallTouched) {
            redBallTouched = true;
        }

        // Yellow ball
        if (collision.collider.CompareTag("YellowBall") && IsMoving && !yellowBallTouched) {
            yellowBallTouched = true;
        }

        if(redBallTouched && yellowBallTouched) {
            redBallTouched = false;
            yellowBallTouched = false;

            Debug.Log("1 point!");
        }
    }

    void BallDirection() {

        Vector3 direction = camera.transform.forward;
        direction.y = 0.0f;

        rayCastingPoints.Add(transform.position);


        RaycastHit hit;
        for (int i = 0; i < rayCastingSections; i++) {
            if (Physics.Raycast(rayCastingPoints[i], direction, out hit, Mathf.Infinity)) {
                //Debug.DrawRay(transform.position, direction * hit.distance, Color.yellow);

                rayCastingPoints.Add(hit.point);
                direction = Vector3.Reflect(direction, hit.normal);

                //Debug.DrawRay(hit.point, Vector3.Reflect(direction, hit.normal) * hit.distance, Color.yellow);
            }
        }       

        lineRenderer.SetPositions(rayCastingPoints.ToArray());

        rayCastingPoints.Clear();
        
    }


}
