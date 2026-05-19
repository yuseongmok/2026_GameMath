using UnityEditor.Embree;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsTest : MonoBehaviour
{
    public float forcePower = 10f;
    private Rigidbody rb;
    [SerializeField] private float speed;
    private bool isSprint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnSprint(InputValue value)
    {
        isSprint = value.isPressed;
    }

    void FixedUpdate()
    {
        if (isSprint)
        {
            rb.AddForce(Vector3.forward *forcePower, ForceMode.Force);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        speed = rb.linearVelocity.magnitude;
    }
}
