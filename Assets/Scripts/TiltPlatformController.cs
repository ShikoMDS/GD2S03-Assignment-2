using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TiltPlatformController : MonoBehaviour
    {
        private float ClampAngle(float angle)
        {
            if (angle > 180)
                angle -= 360;
            return Mathf.Clamp(angle, -89f, 89f);
        }

        public GameObject levelPlatform;  // Reference to the level platform you want to tilt.
        public Joystick joystick;         // On-screen analog stick.
        public float tiltSpeed = 2f;      // Speed of tilting the object.

        private bool useGyroscope;
        private Gyroscope gyro;

        private Vector3 previousTouchPosition;
        private bool isTouching;

        void Start()
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

        void Update()
        {
            // Gyroscope input (available on mobile devices)
            if (useGyroscope)
            {
                TiltWithGyroscope();
            }

            // On-screen analog stick input
            TiltWithJoystick();

            // Relative touch input
            TiltWithTouch();
        }

        void TiltWithGyroscope()
        {
            Quaternion deviceRotation = gyro.attitude;
            Quaternion rotationFix = new Quaternion(0, 0, 1, 0); // Fix the rotation from the gyro to match the game coordinates
            levelPlatform.transform.rotation = deviceRotation * rotationFix;
        }

        void TiltWithJoystick()
        {
            // Map the joystick input to tilt the platform within a semi-circle dome.
            // If the joystick is not being used, reset the platform to its original tilt.
            if (joystick.Horizontal == 0 && joystick.Vertical == 0)
            {
                levelPlatform.transform.rotation = Quaternion.Slerp(levelPlatform.transform.rotation, Quaternion.identity, tiltSpeed * 0.5f * Time.deltaTime);
                return;
            }
            // Map the joystick's input to create a semi-circle dome effect.
            float horizontalInput = joystick.Horizontal;
            float verticalInput = joystick.Vertical;

            float maxAngle = 89f;
            float targetX = verticalInput * maxAngle;
            float targetZ = -horizontalInput * maxAngle;

            // Smoothly interpolate towards the target angles to create a semi-circle dome effect.
            Vector3 newEulerAngles = new Vector3(
                Mathf.LerpAngle(levelPlatform.transform.eulerAngles.x, targetX, tiltSpeed * Time.deltaTime),
                levelPlatform.transform.eulerAngles.y,
                Mathf.LerpAngle(levelPlatform.transform.eulerAngles.z, targetZ, tiltSpeed * Time.deltaTime)
            );
            newEulerAngles.x = ClampAngle(newEulerAngles.x);
            newEulerAngles.z = ClampAngle(newEulerAngles.z);
            levelPlatform.transform.eulerAngles = newEulerAngles;

            // Ensure the platform does not tilt beyond the bounds of a semi-circle dome.

            // Ensure the platform remains within a semi-circle dome range of tilt.
            Vector3 currentEulerAngles = levelPlatform.transform.eulerAngles;
            currentEulerAngles.x = ClampAngle(currentEulerAngles.x);
            currentEulerAngles.z = ClampAngle(currentEulerAngles.z);
            levelPlatform.transform.eulerAngles = currentEulerAngles;
        }

        void TiltWithTouch()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    isTouching = true;
                    previousTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved && isTouching)
                {
                    Vector3 deltaTouch = new Vector3(touch.position.x - previousTouchPosition.x, touch.position.y - previousTouchPosition.y, 0);
                    Vector3 tiltDirection = new Vector3(deltaTouch.y, 0, -deltaTouch.x);

                    levelPlatform.transform.Rotate(tiltDirection * tiltSpeed * Time.deltaTime, Space.World);
                    previousTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    isTouching = false;
                }
            }
        }
    }
}
