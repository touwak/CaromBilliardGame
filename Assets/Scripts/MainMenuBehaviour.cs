using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using DG.Tweening;

public class MainMenuBehaviour : MonoBehaviour {

    #region Variables
    [SerializeField]
    Slider volumeSlider;
    [SerializeField]
    Text shootsText;
    [SerializeField]
    Text pointsText;
    [SerializeField]
    Text timeText;

    [Header("Tween")]
    [SerializeField]
    GameObject title;
    [SerializeField]
    GameObject menu;
    [SerializeField]
    GameObject lastScore;

    #endregion

    private void Start() {
        LoadPlayerData();

        title.transform.DOMoveY(title.transform.position.y * 1.5f, 0.5f).SetEase(Ease.OutBack).From();
        menu.transform.DOScale(0, 0.5f).From().SetDelay(0.25f);
        lastScore.transform.DOScale(2, 0.5f).From().SetDelay(0.5f);
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

    /// <summary>
    /// Load the player data from a JSON file and show it in the UI
    /// </summary>
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
