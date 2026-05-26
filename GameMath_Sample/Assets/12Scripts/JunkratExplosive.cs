using UnityEngine;

public class JunkratExplosive : MonoBehaviour
{
    public float delay = 1.5f;       // 수동/시간 제한 폭발용 대기 시간
    public float radius = 5f;
    public float force = 300f;
    public float upwardsModifier = 1f;

    [Header("Effects")]
    public GameObject explosionEffectPrefab; // 폭발 이펙트 프리팹

    private bool hasExploded = false; // 중복 폭발 방지 플래그

    void Start()
    {
        // 지정된 시간이 지나면 자동으로 터지도록 설정
        Invoke("Explode", delay);
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 이미 터지는 중이 아니라면 폭발 실행
            if (!hasExploded)
            {
                CancelInvoke("Explode"); // 예약되어 있던 시간 제한 폭발 취소
                Explode();
            }
        }
    }

    void Explode()
    {
        hasExploded = true;
        Vector3 explosionPos = transform.position;

        // 1. 폭발 이펙트 생성
        if (explosionEffectPrefab != null)
        {
            // 폭탄이 있던 자리에 이펙트 생성
            Instantiate(explosionEffectPrefab, explosionPos, Quaternion.identity);
        }

        // 2. 물리 폭발 물리 효과 적용
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (var col in colliders)
        {
            Rigidbody rb = col.attachedRigidbody;
            if (rb == null) continue;

            Vector3 toTarget = rb.position - explosionPos;
            float distance = toTarget.magnitude;
            Vector3 dir = toTarget.normalized;

            float attenuation = 1f - Mathf.Clamp01(distance / radius);
            dir += Vector3.up * upwardsModifier;
            dir = dir.normalized;

            Vector3 impulse = dir * force * attenuation;
            rb.AddForce(impulse, ForceMode.Impulse);
        }

        // 3. 폭탄 오브젝트 삭제
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}