using UnityEngine;
using System.Collections; 
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("UI 설정")]
    public Image dashCooldownImage;
    public TextMeshProUGUI dashCooldownText;

    [Header("이동 및 시점")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public Transform playerCamera;
    private float cameraPitch = 0f;

    [Header("점프")]
    public float jumpForce = 5f;
    private bool isGrounded;                 

    [Header("대쉬")]
    public float dashSpeed = 20f;            
    public float dashDuration = 0.2f;       
    public float dashCooldown = 1f;          
    private bool isDashing = false;         
    private float nextDashTime = 0f;         

    [Header("범위 제한")]
    public float minX = -14f;
    public float maxX = 14f;
    public float minZ = -14f;
    public float maxZ = 14f;

    private Rigidbody rb;


    // 1. 내적 (Dot Product) 직접 계산: x1*x2 + y1*y2 + z1*z2
    float CustomDot(Vector3 a, Vector3 b)
    {
        return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
    }

    // 2. 외적 (Cross Product) 직접 계산: (y1z2 - z1y2, z1x2 - x1z2, x1y2 - y1x2)
    Vector3 CustomCross(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x
        );
    }

    // 3. 벡터의 크기(Magnitude) 직접 계산 
    float GetMagnitude(Vector3 v)
    {
        return Mathf.Sqrt(CustomDot(v, v));
    }

    // --------------------------------

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 시점 회전 및 대쉬 UI 업데이트
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        UpdateDashUI();

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        // 지면 체크
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        // 점프
        if (Input.GetButtonDown("Jump") && isGrounded && !isDashing)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // 대쉬 입력
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time >= nextDashTime)
        {
            StartCoroutine(DashRoutine());
        }
    }

    void UpdateDashUI()
    {
        if (dashCooldownImage == null || dashCooldownText == null) return;

        if (Time.time < nextDashTime)
        {
            float remaining = nextDashTime - Time.time;
            dashCooldownImage.fillAmount = remaining / dashCooldown;
            dashCooldownText.text = remaining.ToString("F1") + "s";
            dashCooldownText.gameObject.SetActive(true);
        }
        else
        {
            dashCooldownImage.fillAmount = 0f;
            dashCooldownText.text = ""; 
            dashCooldownText.gameObject.SetActive(false);
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

        // 위치 제한
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

        if (dashDir == Vector3.zero) dashDir = transform.forward;

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            rb.linearVelocity = new Vector3(dashDir.x * dashSpeed, rb.linearVelocity.y, dashDir.z * dashSpeed);
            yield return new WaitForFixedUpdate();
        }
        isDashing = false; 
    }
}