using UnityEngine;

public class WinConditionTrigger : MonoBehaviour
{
    public GameObject winScreenUI; // Reference to a UI element representing the win screen.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) ShowWinScreen();
    }

    private void ShowWinScreen()
    {
        if (winScreenUI != null) winScreenUI.SetActive(true);
    }
}