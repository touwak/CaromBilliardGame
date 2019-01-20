using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehaviour : MonoBehaviour {

    #region Variables
    public Slider volumeSlider;
    #endregion

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
}
