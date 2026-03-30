using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // 어디서든 접근 가능하게 싱글톤 설정
    public static TimeManager Instance;

    private void Awake() { Instance = this; }

    public void DoSlowMotion(float scale, float duration)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        StartCoroutine(ResetTime(duration));
    }

    System.Collections.IEnumerator ResetTime(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
    }
}