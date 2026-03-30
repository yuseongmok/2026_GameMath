using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // 어디서든 접근 가능하게 싱글톤 설정
    public static CameraShake Instance;

    private Vector3 originalPos;

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        originalPos = transform.localPosition;
    }

    // 외부에서 호출할 함수 
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // 무작위 위치 계산
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            // 슬로우 모션 중에도 정상적으로 작동하도록 현실 시간 기준 증분 사용
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        // 흔들림이 끝나면 원래 위치로 복구
        transform.localPosition = originalPos;
    }
}