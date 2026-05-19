using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShooter : MonoBehaviour
{
    [Header("References")]
    public GameObject projectilePrefab; // 발사할 유도탄 프리팹
    public Transform p0;                // 시작점 (예: 캐릭터 위치)
    public Transform p3;                // 도착점 (예: 몬스터 위치)

    [Header("Spawn Settings")]
    public int count = 10;              // 한 번에 발사할 개수
    public float projectileSpeed = 1f;  // 날아가는 속도 배율

    [Header("Random Ranges")]
    public float p1Radius = 2f;
    public float p2Radius = 2f;
    public float p1Height = 3f;
    public float p2Height = 3f;

    void Update()
    {
        // 마우스 왼쪽 클릭 시 발사
        if (Input.GetMouseButtonDown(0))
        {
            FireProjectiles();
        }
    }

    void FireProjectiles()
    {
        if (projectilePrefab == null || p0 == null || p3 == null)
        {
            Debug.LogWarning("Spawner에 필요한 reference가 비어있습니다!");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(projectilePrefab, p0.position, Quaternion.identity);

            Bezier2 projectile = go.GetComponent<Bezier2>();

            if (projectile == null)
            {
                projectile = go.AddComponent<Bezier2>();
            }

            projectile.Initialize(p0.position, p3.position, p1Radius, p2Radius, p1Height, p2Height, projectileSpeed);
        }
    }
}