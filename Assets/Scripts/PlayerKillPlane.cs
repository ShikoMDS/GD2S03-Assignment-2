using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerKillPlane : MonoBehaviour
    {
        public float killY = -10f; // The Y-coordinate below which the player dies
        public GameObject loseScreenCanvas; // Reference to the Lose Screen canvas

        private void Update()
        {
            CheckKillY();
        }

        private void CheckKillY()
        {
            // Check if the player's Y position is below the kill Y
            if (transform.position.y < killY) ShowLoseScreen();
        }

        private void ShowLoseScreen()
        {
            if (loseScreenCanvas != null) loseScreenCanvas.SetActive(true); // Show the Lose Screen canvas
            //Time.timeScale = 0; // Pause the game
        }
    }
}