using UnityEngine;
using UnityEngine.InputSystem;

public class TargetingSystem : MonoBehaviour
{
    public Transform playerCamera;
    public float rotationSpeed = 5f;
    private Transform targetEnemy;
    private bool isTargeting = false;

    // 조준선
    public RectTransform reticleUI;
    private float reticleScale = 0f;

    public void OnRightClick(InputValue value)
    {
        if (!value.isPressed) return;

        // 마우스 위치에서 레이 발사
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // 타겟팅 시작
                targetEnemy = hit.transform;
                isTargeting = true;
            }
            else
            {
                // 빈 곳이나 다른 물체 클릭 시 해제
                ResetTargeting();
            }
        }
        else
        {
            ResetTargeting();
        }
    }

    void Update()
    {
        if (isTargeting && targetEnemy != null)
        {
            Vector3 direction = targetEnemy.position - playerCamera.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            //조준선 연출
            reticleScale = Mathf.LerpUnclamped(reticleScale, 1.0f, Time.deltaTime * 10f);
        }
        else
        {
            reticleScale = Mathf.Lerp(reticleScale, 0f, Time.deltaTime * 10f);
        }

        // 조준선 크기 적용
        if(reticleUI != null) reticleUI.localScale = Vector3.one * reticleScale;
    }

    void ResetTargeting()
    {
        isTargeting = false;
        targetEnemy = null;
    }
}