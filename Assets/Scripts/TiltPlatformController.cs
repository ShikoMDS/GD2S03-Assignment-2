using UnityEngine;

namespace Assets.Scripts
{
    public class InputManager : MonoBehaviour
    {
        public enum InputType
        {
            Joystick,
            Touch,
            Gyro
        }

        public static InputType currentInputType;
        public GameObject joystickGameObject;

        public static void CheckInputType()
        {
            if (Input.touchSupported && Input.touchCount > 0)
                currentInputType = InputType.Touch;
            else if (SystemInfo.supportsGyroscope && Input.gyro.enabled)
                currentInputType = InputType.Gyro;
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                currentInputType = InputType.Joystick;
        }

        private void Update()
        {
            CheckInputType();
            UpdateJoystickVisibility();
        }

        private void UpdateJoystickVisibility()
        {
            if (joystickGameObject != null) joystickGameObject.SetActive(currentInputType == InputType.Joystick);
        }
    }

    public class TiltPlatformController : MonoBehaviour
    {
        private float ClampAngle(float angle)
        {
            if (angle > 180)
                angle -= 360;
            return Mathf.Clamp(angle, -89f, 89f);
        }

        public GameObject platformGroup; // Reference to the parent of all platforms.
        public Joystick joystick; // On-screen analog stick.
        public float tiltSpeed = 2f; // Speed of tilting the platform group.
        public Transform followCamera; // Reference to the follow camera transform.
        public float cameraDistance = 5f; // Distance behind the ball.
        public float cameraHeight = 2f; // Height of the camera relative to the ball.

        private bool useGyroscope;
        private Gyroscope gyro;

        private void Start()
        {
            // Check if the device supports the gyroscope.
            if (SystemInfo.supportsGyroscope)
            {
                gyro = Input.gyro;
                gyro.enabled = true;
                useGyroscope = true;
            }
            else
            {
                useGyroscope = false;
            }
        }

        private void Update()
        {
            // Gyroscope input
            if (useGyroscope && InputManager.currentInputType == InputManager.InputType.Gyro) TiltWithGyroscope();

            // On-screen analog stick input
            if (InputManager.currentInputType == InputManager.InputType.Joystick) TiltWithCameraDirection();
        }

        private void TiltWithGyroscope()
        {
            var deviceRotation = gyro.attitude;
            var rotationFix =
                new Quaternion(0, 0, 1, 0); // Fix the rotation from the gyro to match the game coordinates.
            platformGroup.transform.rotation = deviceRotation * rotationFix;
        }

        private void TiltWithCameraDirection()
        {
            // If there is no joystick input, reset the platform group to its original tilt.
            Vector3 newEulerAngles;
            if (joystick.Horizontal == 0 && joystick.Vertical == 0)
            {
                // Smoothly return the platform group back to a flat position.
                var targetEulerAngles = new Vector3(0, platformGroup.transform.eulerAngles.y, 0);
                newEulerAngles = new Vector3(
                    Mathf.LerpAngle(platformGroup.transform.eulerAngles.x, targetEulerAngles.x,
                        tiltSpeed * Time.deltaTime),
                    platformGroup.transform.eulerAngles.y,
                    Mathf.LerpAngle(platformGroup.transform.eulerAngles.z, targetEulerAngles.z,
                        tiltSpeed * Time.deltaTime)
                );
                platformGroup.transform.eulerAngles = newEulerAngles;
                return;
            }

            // Get the joystick input.
            var horizontalInput = joystick.Horizontal;
            var verticalInput = joystick.Vertical;

            // Get the forward and right directions relative to the camera.
            var cameraForward = followCamera.forward;
            var cameraRight = followCamera.right;

            // Ignore vertical direction (y-axis).
            cameraForward.y = 0;
            cameraRight.y = 0;

            // Normalize the direction vectors.
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Calculate the tilt direction relative to the camera.
            var tiltDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

            // Calculate target rotation based on tilt direction.
            var maxAngle = 89f;
            var targetX = tiltDirection.z * maxAngle;
            var targetZ = -tiltDirection.x * maxAngle;

            // Smoothly interpolate towards the target angles
            newEulerAngles = new Vector3(
                Mathf.LerpAngle(platformGroup.transform.eulerAngles.x, targetX, tiltSpeed * Time.deltaTime),
                platformGroup.transform.eulerAngles.y,
                Mathf.LerpAngle(platformGroup.transform.eulerAngles.z, targetZ, tiltSpeed * Time.deltaTime)
            );
            newEulerAngles.x = ClampAngle(newEulerAngles.x);
            newEulerAngles.z = ClampAngle(newEulerAngles.z);
            platformGroup.transform.eulerAngles = newEulerAngles;

            // Ensure the platform remains within a semi-circle dome range of tilt
            var currentEulerAngles = platformGroup.transform.eulerAngles;
            currentEulerAngles.x = ClampAngle(currentEulerAngles.x);
            currentEulerAngles.z = ClampAngle(currentEulerAngles.z);
            platformGroup.transform.eulerAngles = currentEulerAngles;
        }
    }
}