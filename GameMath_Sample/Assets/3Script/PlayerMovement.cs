using UnityEngine;
using System.Collections; 

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("이동 및 시점")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public Transform playerCamera;
    private float cameraPitch = 0f;

    [Header("점프 (단일 점프)")]
    public float jumpForce = 5f;
    private bool isGrounded;                 

    [Header("대쉬 (Dash)")]
    public float dashSpeed = 20f;            
    public float dashDuration = 0.2f;       
    public float dashCooldown = 1f;          
    private bool isDashing = false;         
    private float nextDashTime = 0f;         

    [Header("이동 범위 제한")]
    public float minX = -14f;
    public float maxX = 14f;
    public float minZ = -14f;
    public float maxZ = 14f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;


        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);


        if (Input.GetButtonDown("Jump") && isGrounded && !isDashing)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time >= nextDashTime)
        {
            StartCoroutine(DashRoutine());
        }
    }

    void FixedUpdate()
    {

        if (!isDashing)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            Vector3 moveDir = (transform.right * x + transform.forward * z).normalized;
            rb.linearVelocity = new Vector3(moveDir.x * moveSpeed, rb.linearVelocity.y, moveDir.z * moveSpeed);
        }


        Vector3 clampedPos = rb.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
        clampedPos.z = Mathf.Clamp(clampedPos.z, minZ, maxZ);

        rb.position = clampedPos;
    }


    private IEnumerator DashRoutine()
    {
        isDashing = true; 
        nextDashTime = Time.time + dashCooldown; 

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 dashDir = (transform.right * x + transform.forward * z).normalized;


        if (dashDir == Vector3.zero)
        {
            dashDir = transform.forward;
        }

        float startTime = Time.time;

       
        while (Time.time < startTime + dashDuration)
        {
   
            rb.linearVelocity = new Vector3(dashDir.x * dashSpeed, rb.linearVelocity.y, dashDir.z * dashSpeed);

  
            yield return new WaitForFixedUpdate();
        }

        isDashing = false; 
    }
}