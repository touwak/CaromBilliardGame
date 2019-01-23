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
    float finalStrengthNormalize;

    bool redBallTouched;
    bool yellowBallTouched;

    [SerializeField]
    int rayCastingSections = 2;
    List<Vector3> rayCastingPoints;
    LineRenderer lineRenderer;

    AudioSource hitSound;
    #endregion

    void Start () {
        camera = Camera.main;
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

        hitSound = GetComponent<AudioSource>();
	}

    void Update() {

        if (!GameManager.instance.IsOver) {
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
    }
    #endregion

    #region Hit Ball
    /// <summary>
    /// Hit the ball in the forward direction of the camera
    /// </summary>
    void HitBall() {
        // Set the replaying to false
        GameManager.instance.isReplaying = false;

        Vector3 direction = camera.transform.forward;
        direction.y = 0.0f;
        Vector3 force = direction * finalStrength;
        
        // Save the data of the balls for the replay
        GameManager.instance.ReplayData(force);

        rigidbody.AddForce(force, ForceMode.Impulse);

        // reset strength
        finalStrength = minStrength;

        // Increment num shoots for the UI
        GameManager.instance.NumShoots++;
        // Restart the force slider
        GameManager.instance.ForceApplied = 0f;


    }

    /// <summary>
    /// Increment strength in time
    /// </summary>
    void IncrementHitForce() {
        if(finalStrength < maxStrength) {
            finalStrength += acceleration * Time.deltaTime;

            // For the UI
            finalStrengthNormalize = finalStrength / maxStrength;
            GameManager.instance.ForceApplied = finalStrengthNormalize; 
        }

    }
    #endregion

    private void OnCollisionEnter(Collision collision) {

        // Sound
        if (!collision.collider.CompareTag("Surface")) {
            PlayHitSound();
        }

        // Check that is not replaying the last movement
        if (!GameManager.instance.isReplaying) {
            // Red ball
            if (collision.collider.CompareTag("RedBall") && IsMoving && !redBallTouched) {
                redBallTouched = true;
            }

            // Yellow ball
            if (collision.collider.CompareTag("YellowBall") && IsMoving && !yellowBallTouched) {
                yellowBallTouched = true;
            }

            // Score 1 point
            if (redBallTouched && yellowBallTouched) {
                redBallTouched = false;
                yellowBallTouched = false;

                // Increment the points gained for the UI and Game Over
                GameManager.instance.PointsGained++;
            }
        }
    }

    /// <summary>
    /// Draw the trajectory of the ball
    /// </summary>
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

    /// <summary>
    /// Adjust the volume and play the hit sound
    /// </summary>
    void PlayHitSound() {

        float masterVolume = PlayerPrefs.GetFloat("MasterVolume");
        float volume = Mathf.Clamp((masterVolume + finalStrengthNormalize), 0f, 1f);
        hitSound.volume = volume;
        hitSound.Play();
    }


}
