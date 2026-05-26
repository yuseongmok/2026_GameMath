using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    public GameObject grenadePrefab; // 유탄 프리팹
    public Transform firePoint;      // 발사 위치
    public float launchForce = 20f;  // 발사하는 힘

    void Update()
    {
        // 마우스 좌클릭 시 발사
        if (Input.GetButtonDown("Fire1"))
        {
            LaunchGrenade();
        }
    }

    void LaunchGrenade()
    {
        // 유탄 생성
        GameObject grenade = Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        // 정면 방향으로 힘 가하기
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * launchForce, ForceMode.VelocityChange);
        }
    }
}