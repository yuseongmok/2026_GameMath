using UnityEngine;

public class EnemyCross : MonoBehaviour
{
    public Transform target;         
    public float moveSpeed = 5f;     
    public float rotateSpeed = 100f;  
    public float stopDistance = 2f; 

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        if (target == null) return;


        Vector3 forward = transform.forward;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target.position);


        Vector3 crossProduct = Vector3.Cross(forward, dirToTarget);

        if (crossProduct.y > 0.1f)
        {

            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        }
        else if (crossProduct.y < -0.1f)
        {

            transform.Rotate(Vector3.up * -rotateSpeed * Time.deltaTime);
        }


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
}
