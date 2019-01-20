using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainMenuBehaviour : MonoBehaviour {

    #region Variables
    public Slider volumeSlider;

    public Text shootsText;
    public Text pointsText;
    public Text timeText;
    #endregion

    private void Start() {
        LoadPlayerData();
    }

    /// <summary>
    /// Load a scene and set the value for the master volume
    /// </summary>
    /// <param name="levelName">The scene name to be loaded</param>
    public void LoadLevel(string levelName) {
        // Set the volume value
        PlayerPrefs.SetFloat("MasterVolume", volumeSlider.value);
        // load the scene
        SceneManager.LoadScene(levelName);
    }

    /// <summary>
    /// Close the program
    /// </summary>
    public void QuitGame() {
        Application.Quit();
    }

    void LoadPlayerData() {

        string path = Path.Combine(Application.persistentDataPath, "PlayerData.txt");
        GameData loadedData;

        if (File.Exists(path)) {
            using (StreamReader streamReader = File.OpenText(path)) {
                string jsonString = streamReader.ReadToEnd();
                loadedData = JsonUtility.FromJson<GameData>(jsonString);
            }

            int seconds = (int)(loadedData.time % 60f);
            int minutes = (int)(loadedData.time / 60f);
            int hours = (int)(loadedData.time / 3600f);

            shootsText.text = string.Concat("Shoots: " + loadedData.shoots);
            pointsText.text = string.Concat("Points: " + loadedData.points);
            timeText.text = string.Concat(
                hours.ToString("00") + ":" +
                minutes.ToString("00") + ":" +
                seconds.ToString("00")
                );
        }
    }



}
