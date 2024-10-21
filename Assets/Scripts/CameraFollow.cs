using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the ball transform.
    public float smoothSpeed = 0.125f; // Smoothness of camera movement.
    public float cameraDistance = 5f; // Desired distance behind the ball.
    public float cameraHeight = 2f; // Height of the camera relative to the ball.

    private Vector3 previousBallPosition;

    private void Start()
    {
        // Initialize the previous ball position to avoid large jumps.
        if (target != null) previousBallPosition = target.position;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Calculate the direction in which the ball is moving.
        var ballMovementDirection = target.position - previousBallPosition;

        // Update the previous position of the ball.
        previousBallPosition = target.position;

        // If the ball is moving, calculate a new position for the camera.
        if (ballMovementDirection.magnitude > 0.01f) // To avoid minor jitters
        {
            ballMovementDirection.y = 0; // Keep movement in the horizontal plane.
            ballMovementDirection.Normalize();

            // Calculate the desired position for the camera behind the ball.
            var desiredPosition = target.position - ballMovementDirection * cameraDistance + Vector3.up * cameraHeight;

            // Smoothly move the camera to the desired position.
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Make the camera look at the target (ball).
            var targetRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed);
        }
    }
}