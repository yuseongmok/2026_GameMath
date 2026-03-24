using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCross : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;
    public float stopDistance = 2f;

    [Header("시야 설정")]
    public float viewAngle = 45f;     
    public float viewDistance = 15f;  
    private bool isPlayerSpotted = false; 

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
       
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        if (target == null) return;

       
        CheckVisualField();

        if (isPlayerSpotted)
        {
            TrackPlayer();
        }
        else
        {
            IdleRotate();
        }
    }

    void CheckVisualField()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        Vector3 dirToTarget = (target.position - transform.position).normalized;

        float angle = Vector3.Angle(transform.forward, dirToTarget);

        if (distance <= viewDistance && angle <= viewAngle)
        {
            isPlayerSpotted = true;
        }
        else
        {
            isPlayerSpotted = false;
        }
    }

    void TrackPlayer()
    {
        Vector3 forward = transform.forward;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target.position);

        Vector3 crossProduct = Vector3.Cross(forward, dirToTarget);
        if (crossProduct.y > 0.1f) transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        else if (crossProduct.y < -0.1f) transform.Rotate(Vector3.up * -rotateSpeed * Time.deltaTime);

 
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
        // 제자리에서 천천히 회전
        transform.Rotate(Vector3.up * (rotateSpeed * 0.5f) * Time.deltaTime);
    }

    // 플레이어와 충돌했을 때 실행
    private void OnCollisionEnter(Collision collision)
    {
        // 부딪힌 물체의 태그가 "Player"라면 씬 재시작
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어 사망! 씬을 재시작합니다.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}