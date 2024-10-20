using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelSelection"); // Ensure you have a LevelSelection scene
    }

    public void OpenInstructions()
    {
        SceneManager.LoadScene("Instructions"); // Ensure you have an Instructions scene
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings"); // Ensure you have a Settings scene
    }

    public void QuitGame()
    {
        // Only works in a built version, will do nothing in the editor
        Application.Quit();
    }
}
