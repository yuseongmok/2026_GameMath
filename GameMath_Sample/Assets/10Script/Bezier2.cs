using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bezier2 : MonoBehaviour
{
    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;

    private List<Vector3> points;
    private float time = 0f;
    private float speed = 1f; // 발사 속도 (기본 1초 소요)
    private bool isInitialized = false;

    public void Initialize(Vector3 start, Vector3 end, float p1Radius, float p2Radius, float p1Height, float p2Height, float moveSpeed)
    {
        p0 = start;
        p3 = end;
        speed = moveSpeed;

        // 각각의 유도탄마다 고유한 랜덤 제어점(P1, P2) 생성
        Vector2 rand1 = Random.insideUnitCircle * p1Radius;
        p1 = p0 + new Vector3(rand1.x, 0f, rand1.y);
        p1.y += p1Height;

        Vector2 rand2 = Random.insideUnitCircle * p2Radius;
        p2 = p3 + new Vector3(rand2.x, 0f, rand2.y);
        p2.y += p2Height;

        points = new List<Vector3> { p0, p1, p2, p3 };
        transform.position = p0;

        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized) return;

        // 속도(speed)에 따라 time 증가
        time += Time.deltaTime * speed;

        if (time <= 1f)
        {
            transform.position = DeCasteljau(new List<Vector3>(points), time);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    Vector3 DeCasteljau(List<Vector3> p, float t)
    {
        while (p.Count > 1)
        {
            int last = p.Count - 1;
            var next = new List<Vector3>(last);
            for (int i = 0; i < last; i++)
                next.Add(Vector3.Lerp(p[i], p[i + 1], t));
            p = next;
        }
        return p[0];
    }
}
