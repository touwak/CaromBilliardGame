using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    #region Variables
    public static GameManager instance = null;

    float forceApplied;
    int numShoots;
    int pointsGained;

    // UI
    public Text shootsText;
    public Text pointsText;
    public Text timeText;
    public Slider forceSlider;

    // Time counter
    int seconds;
    int minutes;
    int hours;

    // Replay
    public bool isReplaying;
    public GameObject whiteBall;
    public GameObject redBall;
    public GameObject  yellowBall;
    Vector3 whiteBallPosition;
    Vector3 whiteBallDirection;
    Vector3 redBallPosition;
    Vector3 yellowBallPosition;
    #endregion

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else if(instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        forceApplied = 0.0f;
        numShoots = 0;
        pointsGained = 0;

        isReplaying = false;
    }

    private void Update() {
        // Time counter
        TimeCounter();
    }

    public float ForceApplied {
        get { return forceApplied; }
        set { forceApplied = value;

            // Force UI
            forceSlider.value = forceApplied;
        }
    }

    public int NumShoots {
        get { return numShoots; }
        set { numShoots = value;

            // SHoots UI
            shootsText.text = string.Concat("Shoots: " + numShoots);
        }
    }

    public int PointsGained{
        get { return pointsGained; }
        set { pointsGained = value;

            // Points UI
            pointsText.text = string.Concat("Points: " + pointsGained);
        }
    }

    /// <summary>
    /// Display the duration of the game
    /// </summary>
    void TimeCounter() {
        seconds = (int)(Time.time % 60f);
        minutes = (int)(Time.time / 60f);
        hours = (int)(Time.time / 3600f);

        // Time counter UI
        timeText.text = string.Concat(
            hours.ToString("00") + ":" + 
            minutes.ToString("00") + ":" + 
            seconds.ToString("00")
            );
    }

    #region Replay
    /// <summary>
    /// The necessary data for the replay 
    /// </summary>
    /// <param name="direction"> The direction and force of the white ball</param>
    public void ReplayData(Vector3 direction) {
        whiteBallPosition = whiteBall.transform.position;
        redBallPosition = redBall.transform.position;
        yellowBallPosition = yellowBall.transform.position;
        whiteBallDirection = direction;
    }

    /// <summary>
    /// Replay the last movement
    /// </summary>
    public void Replay() {
        if (!isReplaying) {
            isReplaying = true;

            whiteBall.transform.position = whiteBallPosition;
            redBall.transform.position = redBallPosition;
            yellowBall.transform.position = yellowBallPosition;

            whiteBall.GetComponent<Rigidbody>().AddForce(whiteBallDirection, ForceMode.Impulse);
        }
    }

    #endregion
}
