using System;
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
    TimeSpan time;
    #endregion

    public float ForceApplied {
        get { return forceApplied; }
        set { forceApplied = value; }
    }

    public int NumShoots {
        get { return numShoots; }
        set { numShoots = value;
            shootsText.text = "Shoots: " + numShoots.ToString();
        }
    }

    public int PointsGained{
        get { return pointsGained; }
        set { pointsGained = value;
            pointsText.text = "Points: " + pointsGained.ToString();
        }
    }

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else if(instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        //time = new TimeSpan()
    }
}
