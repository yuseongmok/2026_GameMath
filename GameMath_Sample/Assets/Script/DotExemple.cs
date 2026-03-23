using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class DotExemple : MonoBehaviour
{
    public Transform player;
    public float viewAngle = 60f; // 시야각
    public float maxDistance = 10f; // 최대 시야 거리 

    void Update()
    {
        if (player == null) return;

        Vector3 toPlayer = player.position - transform.position; 
        float distance = toPlayer.magnitude; // 거리 계산
        Vector3 forward = transform.forward;

        // 내적 계산 시 방향만 쓰도록
        float dot = Vector3.Dot(forward, toPlayer.normalized);
        float angle = Mathf.Acos(Mathf.Clamp(dot, -1f, 1f)) * Mathf.Rad2Deg;

        if (angle < viewAngle / 2 && distance < maxDistance)
        {
            transform.localScale = Vector3.one * 10;
            Debug.Log("플레이어가 시야 안에 있음");
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }
}
