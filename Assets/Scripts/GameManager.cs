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

    StringBuilder timeString;

    // Time counter
    float startTime;
    float actualTime;
    int seconds;
    int minutes;
    int hours;
    #endregion

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else if(instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        // Time counter
        startTime = Time.time;
    }

    private void Start() {
        forceApplied = 0.0f;
        numShoots = 0;
        pointsGained = 0;
    }

    private void Update() {
        // Time counter
        TimeCounter();
    }

    public float ForceApplied {
        get { return forceApplied; }
        set { forceApplied = value;

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

    

}
