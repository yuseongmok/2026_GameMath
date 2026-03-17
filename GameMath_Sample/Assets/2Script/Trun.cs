using UnityEngine;

public class Trun : MonoBehaviour
{
    [Header("공전 설정")]
    public Transform target;       // 중심점 (태양 또는 지구)
    public float radius = 5f;      // 중심점으로부터의 거리 (반지름)
    public float speed = 50f;      // 공전 속도 (1초에 몇 도씩 돌 것인가)

    private float currentAngle = 0f; // 현재 각도 (Degree)

    void Update()
    {
        // 타겟이 없으면 실행하지 않음 (에러 방지)
        if (target == null) return;

        // 1. Time.deltaTime을 곱해 기기 성능에 상관없이 일정한 속도로 각도 증가 (디그리)
        currentAngle += speed * Time.deltaTime;

        // 각도가 계속 커지는 것을 방지 (0~360도 유지)
        if (currentAngle >= 360f) currentAngle -= 360f;

        // 2. 삼각함수(Mathf.Sin, Mathf.Cos)는 라디안 값을 요구하므로 변환 (Degree -> Radian)
        float rad = currentAngle * Mathf.Deg2Rad;

        // 3. sin, cos를 이용하여 x, z 좌표 계산
        float x = Mathf.Cos(rad) * radius;
        float z = Mathf.Sin(rad) * radius;

        // 4. 타겟(중심)의 현재 위치를 기준으로 오프셋(x, z)을 더해 최종 위치 적용
        transform.position = target.position + new Vector3(x, 0f, z);
    }
}