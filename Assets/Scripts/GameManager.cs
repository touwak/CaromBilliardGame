using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Variables
    public static GameManager instance = null;

    float forceApplied;
    int numShoots;
    int pointsGained;

    // UI
    [Header("Gameplay UI")]
    public Text shootsText;
    public Text pointsText;
    public Text timeText;
    public Slider forceSlider;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public Text gameOverShootsText;
    public Text gameOverPointsText;
    public Text gameOverTimeText;

    // Time counter
    int seconds;
    int minutes;
    int hours;
    float startTime;
    float actualTime;

    // Replay
    [HideInInspector]
    public bool isReplaying;
    [Header("Replay")]
    public GameObject whiteBall;
    public GameObject redBall;
    public GameObject yellowBall;

    Vector3 whiteBallPosition;
    Vector3 whiteBallDirection;
    Vector3 redBallPosition;
    Vector3 yellowBallPosition;

    // Game Over
    bool isOver;
    [Header("Game Over")]
    public int victoryThreeshold = 3;
    string dataPath;
    #endregion

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        gameOverPanel.SetActive(false);
    }

    private void Start() {
        forceApplied = 0.0f;
        numShoots = 0;
        pointsGained = 0;

        isReplaying = false;
        isOver = false;

        startTime = Time.time;

        dataPath = Path.Combine(Application.persistentDataPath, "PlayerData.txt");
    }

    private void Update() {
        // Time counter
        if (!isOver) {
            TimeCounter();
        }
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

    public int PointsGained {
        get { return pointsGained; }
        set { pointsGained = value;

            // Points UI
            pointsText.text = string.Concat("Points: " + pointsGained);

            // check if the player has reach all the points
            if (pointsGained >= victoryThreeshold) {
                GameOver();
            }
        }
    }

    public bool IsOver {
        get { return isOver; }
    }

    public string DataPath {
        get { return dataPath; }
    }

    /// <summary>
    /// Display the duration of the game
    /// </summary>
    void TimeCounter() {

        actualTime = Time.time - startTime;

        seconds = (int)(actualTime % 60f);
        minutes = (int)(actualTime / 60f);
        hours = (int)(actualTime / 3600f);

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

    public void LoadLevel(string levelName) {
        // load the scene
        SceneManager.LoadScene(levelName);
    }

    #region Game Over
    /// <summary>
    /// Stop the game and show the Game Over menu
    /// </summary>
    void GameOver() {
        isOver = true;
        SavePlayerData();

        gameOverShootsText.text = string.Concat("Shoots: " + numShoots);
        gameOverPointsText.text = string.Concat("Points: " + pointsGained);
        gameOverTimeText.text = string.Concat(
            hours.ToString("00") + ":" +
            minutes.ToString("00") + ":" +
            seconds.ToString("00")
            );

        gameOverPanel.SetActive(true);
    }

    /// <summary>
    /// Save the player data in a JSON file
    /// </summary>
    void SavePlayerData() {
        GameData data = new GameData() ;
        data.shoots = numShoots;
        data.points = pointsGained;
        data.time = actualTime;

        string playerData = JsonUtility.ToJson(data);

        using (StreamWriter streamWriter = File.CreateText(dataPath)) {
            streamWriter.Write(playerData);
        }


    }

    #endregion
}

[SerializeField]
class GameData{
    public int shoots;
    public int points;
    public float time;
}
