using UnityEngine;

public class RemoteMine : MonoBehaviour
{
    public float radius = 6f;          // 폭발 반경
    public float force = 400f;         // 밀쳐내는 힘
    public float upwardsModifier = 1.5f; // 위로 띄워주는 보정치
    public GameObject explosionEffectPrefab; // 폭발 이펙트

    public void Explode()
    {
        Vector3 explosionPos = transform.position;

        //이펙트 생성
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, explosionPos, Quaternion.identity);
        }

        //주변 오브젝트 감지 및 폭발력 적용
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (var col in colliders)
        {
            Rigidbody rb = col.attachedRigidbody;
            if (rb == null) continue;

            //정크랫 지뢰 점프의 핵심 피드백 계산
            Vector3 toTarget = rb.position - explosionPos;
            float distance = toTarget.magnitude;
            Vector3 dir = toTarget.normalized;

            //거리에 따른 힘의 감쇠
            float attenuation = 1f - Mathf.Clamp01(distance / radius);

            // 위로 뜨는 느낌을 주기 위해 Y축 방향 힘 추가
            dir += Vector3.up * upwardsModifier;
            dir = dir.normalized;

            // 플레이어 캐릭터 구별 로직
            if (col.CompareTag("Player"))
            {
                Vector3 impulse = dir * force * attenuation;
                rb.AddForce(impulse, ForceMode.Impulse);

                // 만약 낙하 대미지 시스템이 있다면, 여기서 플레이어에게 지뢰 점프 면제 플래그를 줄 수도 있습니다.
            }
            else
            {
                // 적이나 다른 물체 처리
                Vector3 impulse = dir * force * attenuation;
                rb.AddForce(impulse, ForceMode.Impulse);

                // 필요하다면 여기에 적에게 대미지를 주는 코드를 추가하세요.
            }
        }

        // 3. 지뢰 오브젝트 제거
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}