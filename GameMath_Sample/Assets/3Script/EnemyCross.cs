using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody))] 
public class EnemyCross : MonoBehaviour
{
    [Header("패링 설정")]
    public float parryDistance = 2.5f;   
    public GameObject deathEffect;      

    [Header("패링 효과 설정")]
    public float slowMotionScale = 0.2f; 
    public float slowDuration = 0.8f;    

    [Header("패링 물리 연출 설정")]
    public float knockbackForce = 25f;   
    public float physicsDuration = 0.4f; 

    [Header("대상 설정")]
    public Transform target;
    public float moveSpeed = 5f;
    public float rotateSpeed = 150f;
    public float stopDistance = 0.5f;

    [Header("시야 설정")]
    public float viewAngle = 45f;
    public float viewDistance = 15f;
    private bool isPlayerSpotted = false;

    [Header("시야 시각화 설정")]
    public int thetaOfSegements = 30;
    public Color normalColor = new Color(1f, 1f, 0f, 0.2f);
    public Color spottedColor = new Color(1f, 0f, 0f, 0.4f);

    private Rigidbody rb;
    private LineRenderer lineRenderer;
    private bool isDead = false; 


    // 내적: x1*x2 + y1*y2 + z1*z2
    float CustomDot(Vector3 a, Vector3 b)
    {
        return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
    }

    // 외적: (y1z2 - z1y2, z1x2 - x1z2, x1y2 - y1x2)
    Vector3 CustomCross(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x
        );
    }

    // 벡터 사이의 각도 계산 (Vector3.Angle 대체용)
    float CustomAngle(Vector3 from, Vector3 to)
    {
        float dot = CustomDot(from.normalized, to.normalized);
        // 내적값을 -1 ~ 1 사이로 클램핑 후 아크코사인을 구해 각도 도출
        return Mathf.Acos(Mathf.Clamp(dot, -1f, 1f)) * Mathf.Rad2Deg;
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = thetaOfSegements + 3;
        
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }
    }

    private void Update()
    {
        if (target == null || isDead) return;

        CheckVisualField();
        CheckParry();

        if (isPlayerSpotted) TrackPlayer();
        else IdleRotate();

        DrawViewField();
    }

    void CheckParry()
    {
        if (isDead) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= parryDistance)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
            {
                isDead = true; 
                StartCoroutine(ParrySequence());
            }
        }
    }

    IEnumerator ParrySequence()
    {
        // 카메라 쉐이크 호출 (타격 시점)
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(0.2f, 0.5f);
        }

        // 1. 이펙트 생성
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position + Vector3.up * 1.0f, Quaternion.identity);

        // 2. 슬로우 모션 발동
        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // 3. 물리 연출 (튕겨내기)
        if (lineRenderer != null) lineRenderer.enabled = false;
        rb.constraints = RigidbodyConstraints.None; 
        
        Vector3 knockbackDir = (transform.position - target.position).normalized;
        knockbackDir = (knockbackDir + Vector3.up * 0.5f).normalized; 

        rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * knockbackForce, ForceMode.Impulse);

        // 4. 물리 효과 관찰 대기
        yield return new WaitForSecondsRealtime(physicsDuration);

        // 5. 시각적 제거
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers) r.enabled = false;
        
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false; 

        // 6. 남은 슬로우 시간 대기 후 복구
        float remainingSlow = slowDuration - physicsDuration;
        if (remainingSlow > 0) yield return new WaitForSecondsRealtime(remainingSlow);

        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;

        Destroy(gameObject);
    }

    void CheckVisualField()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        
        // Vector3.Angle 대신 직접 만든 CustomAngle 사용
        float angle = CustomAngle(transform.forward, dirToTarget);

        if (distance <= viewDistance && angle <= viewAngle)
        {
            RaycastHit hit;
            Vector3 rayStartPos = transform.position + Vector3.up * 0.2f;
            Vector3 rayDir = (target.position + Vector3.up * 0.2f) - rayStartPos;

            if (Physics.Raycast(rayStartPos, rayDir, out hit, viewDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    isPlayerSpotted = true;
                    return;
                }
            }
        }
        isPlayerSpotted = false;
    }

    void TrackPlayer()
    {
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target.position);

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dirToTarget.x, 0, dirToTarget.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (distance > stopDistance)
        {
            Vector3 moveDir = transform.forward * moveSpeed;
            rb.linearVelocity = new Vector3(moveDir.x, rb.linearVelocity.y, moveDir.z);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    void IdleRotate()
    {
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        transform.Rotate(Vector3.up * (rotateSpeed * 0.5f) * Time.deltaTime);
    }

    void DrawViewField()
    {
        if (lineRenderer == null || !lineRenderer.enabled) return;

        Color currentColor = isPlayerSpotted ? spottedColor : normalColor;
        lineRenderer.startColor = currentColor;
        lineRenderer.endColor = currentColor;

        float angleStep = (viewAngle * 2f) / thetaOfSegements;
        float currentAngle = -viewAngle;
        float yOffset = 0.8f;

        lineRenderer.SetPosition(0, new Vector3(0, yOffset, 0));

        for (int i = 0; i <= thetaOfSegements; i++)
        {
            float rad = currentAngle * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Sin(rad) * viewDistance, yOffset, Mathf.Cos(rad) * viewDistance);
            lineRenderer.SetPosition(i + 1, pos);
            currentAngle += angleStep;
        }

        lineRenderer.SetPosition(thetaOfSegements + 2, new Vector3(0, yOffset, 0));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return; 

        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}