using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class EnemyTargetingSystem : MonoBehaviour
{
    public Transform startPos;  // 보통 카메라 위치
    private Transform targetEnemy; // 현재 타겟팅 중인 적
    [Range(1f, 5f)] public float extend = 1.5f;

    private LineRenderer lr;
    private bool isTargeting = false;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.widthMultiplier = 0.05f;
        lr.material = new Material(Shader.Find("Unlit/Color")) { color = Color.red };
        lr.enabled = false; // 기본은 비활성화
    }

    void Update()
    {
        //카메라가 바라보게함 
        if (isTargeting && targetEnemy != null)
        {
            Vector3 direction = targetEnemy.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            // 조준선 업데이트
            lr.SetPosition(0, startPos.position);
            lr.SetPosition(1, Vector3.LerpUnclamped(startPos.position, targetEnemy.position, extend));
        }
    }

    public void OnRightClick()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) return;
        Debug.Log("클릭");

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // 타겟팅 시작
                targetEnemy = hit.collider.transform;
                isTargeting = true;
                lr.enabled = true;
            }
            else
            {
                // 초기화 (빈 곳 클릭)
                ResetTargeting();
            }
        }
    }

    void ResetTargeting()
    {
        isTargeting = false;
        targetEnemy = null;
        lr.enabled = false;
    }
}