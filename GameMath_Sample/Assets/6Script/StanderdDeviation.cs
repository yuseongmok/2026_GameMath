using UnityEngine;

public class StanderdDeviation : MonoBehaviour
{
    [Header("설정값")]
    public float mean = 100f;    // 평균
    public float stdDev = 50f;  // 표준편차

    // 버튼에서 호출할 함수
    public void OnClickGenerate()
    {
        float result = Generate(mean, stdDev);
        Debug.Log($"[정규분포 난수 생성] 결과: {result} (평균: {mean}, 표준편차: {stdDev})");
    }

    float Generate(float mean, float stdDev)
    {
        // 0.0 ~ 1.0 사이의 값을 가져오되, Log(0) 에러를 피하기 위해 1에서 뺍니다.
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;

        // 표준 정규 분포 공식
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

        // 원하는 평균과 표준편차로 변환
        return mean + stdDev * randStdNormal;
    }
}