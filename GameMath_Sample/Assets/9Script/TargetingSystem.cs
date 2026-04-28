using UnityEngine;
using UnityEngine.InputSystem;

public class TargetingSystem : MonoBehaviour
{
    public Transform playerCamera; 
    private Transform currentTarget; 
    public LineRenderer aimLine; 

    public void OnRightClick(InputValue value)
    {
        if (!value.isPressed) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // 타겟팅
                currentTarget = hit.transform;
                aimLine.enabled = true;
                Debug.Log("맞음");
            }
            else
            {
                // 초기화
                ResetTargeting();
                Debug.Log("초기화");
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1.0f);
    }

    void Update()
    {
        // 타겟이 설정되어 있으면 카메라가 적을 바라보게 함
        if (currentTarget != null)
        {
            playerCamera.LookAt(currentTarget);

            // 조준선 업데이트 (카메라 위치 -> 적 위치)
            aimLine.SetPosition(0, playerCamera.position);
            aimLine.SetPosition(1, currentTarget.position);
        }
        if (Input.GetMouseButtonDown(1)) // 전통적인 방식의 우클릭 체크
        {
            Debug.Log("우클릭 감지됨");
        }
    }

    void ResetTargeting()
    {
        currentTarget = null;
        aimLine.enabled = false;
    }
}