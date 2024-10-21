using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    //public float smallConstantForce = 0.0f; // Small constant force to avoid settling

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        AddInitialForce();
    }

    private void AddInitialForce()
    {
        // Add a small initial force to prevent the ball from sticking
        rb.AddForce(new Vector3(0, 0, 0.1f), ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        // Apply a very small force to keep the ball from getting stuck
        //rb.AddForce(Vector3.forward * smallConstantForce);
    }
}