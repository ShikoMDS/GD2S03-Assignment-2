using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ButtonController : MonoBehaviour
    {
        // UI Buttons for input type selection
        public Button joystickButton;
        public Button touchButton;
        public Button gyroButton;
        public Color selectedColor = Color.green;
        public Color defaultColor = Color.white;

        // Reference to the joystick GameObject
        public static GameObject joystickGameObject;

        private void Start()
        {
            UpdateButtonColors();
        }

        public void PlayGame()
        {
            SceneManager.LoadScene("LevelSelection");
        }

        public void OpenInstructions()
        {
            SceneManager.LoadScene("Instructions");
        }

        public void OpenSettings()
        {
            SceneManager.LoadScene("Settings");
        }

        public void OpenMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false; // Stops play mode in the editor
#else
            Application.Quit();
#endif
        }

        // New methods for LevelSelection buttons
        public void LoadLevel1()
        {
            SceneManager.LoadScene("Level1");
        }

        public void LoadLevel2()
        {
            SceneManager.LoadScene("Level2");
        }

        public void LoadLevel3()
        {
            SceneManager.LoadScene("Level3");
        }

        public void LoadLevel4()
        {
            SceneManager.LoadScene("Level4");
        }

        public void LoadLevel5()
        {
            SceneManager.LoadScene("Level5");
        }

        public void NextLevel()
        {
            var currentSceneName = SceneManager.GetActiveScene().name;

            // Extract the level number from the current scene name
            if (currentSceneName.StartsWith("Level"))
            {
                int currentLevelNumber;
                if (int.TryParse(currentSceneName.Substring(5), out currentLevelNumber))
                {
                    var nextLevelNumber = currentLevelNumber + 1;

                    // Ensure that next level exists in the range 1-5
                    if (nextLevelNumber <= 5)
                    {
                        var nextLevelName = "Level" + nextLevelNumber;
                        SceneManager.LoadScene(nextLevelName);
                    }
                    else
                    {
                        Debug.Log("No more levels available.");
                    }
                }
            }
        }

        public void RestartLevel()
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName); // Reload the current scene
        }

        // Methods for selecting input types
        public void SelectJoystick()
        {
            InputManager.currentInputType = InputManager.InputType.Joystick;
            UpdateButtonColors();
        }

        public void SelectTouch()
        {
            InputManager.currentInputType = InputManager.InputType.Touch;
            UpdateButtonColors();
        }

        public void SelectGyro()
        {
            InputManager.currentInputType = InputManager.InputType.Gyro;
            UpdateButtonColors();
        }

        private void UpdateButtonColors()
        {
            // Reset all buttons to default color
            joystickButton.image.color = defaultColor;
            touchButton.image.color = defaultColor;
            gyroButton.image.color = defaultColor;

            // Set selected button to selected color
            switch (InputManager.currentInputType)
            {
                case InputManager.InputType.Joystick:
                    joystickButton.image.color = selectedColor;
                    break;
                case InputManager.InputType.Touch:
                    touchButton.image.color = selectedColor;
                    break;
                case InputManager.InputType.Gyro:
                    gyroButton.image.color = selectedColor;
                    break;
            }
        }
    }
}