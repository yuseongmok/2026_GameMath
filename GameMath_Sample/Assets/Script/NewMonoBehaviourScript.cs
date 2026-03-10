using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using static UnityEngine.Rendering.DebugUI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float moveSpeed = 3f; //변하는 값 
    public float walkSpeed = 3f; //평소
    public float sprintSpeed = 5f; //가속 
    private Vector2 mouseScreenPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isSprinting;
    
            

    // Update is called once per frame
    public void OnPoint(InputValue value)
    {
        mouseScreenPosition = value.Get<Vector2>();
    }
    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject != gameObject)
                {

                    targetPosition = hit.point;
                    targetPosition.y = transform.position.y;
                    isMoving = true;


                    break;
                }
            }

        }
    }
    public void OnSprint(InputValue value)
    {
        Debug.Log("실행됨");
        isSprinting = value.isPressed;
    }
    void Update()
        {
            if (isMoving)
            {
            Vector3 A = targetPosition - transform.position; // 방향= 타겟포지션-나의위치

            transform.position += A * moveSpeed * Time.deltaTime;

         
            if (isSprinting)
            {
                moveSpeed = sprintSpeed;
                Debug.Log(sprintSpeed);
            }
            else
            {
                moveSpeed = walkSpeed;
                Debug.Log(walkSpeed);
            }


            if (A.magnitude <= 0.1)
            {
                isMoving = false;
            }
            }
        }
 }