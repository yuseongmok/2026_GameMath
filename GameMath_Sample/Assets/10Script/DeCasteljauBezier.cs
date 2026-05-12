using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeCasteljauBezier : MonoBehaviour
{
    public List<Transform> points = new List<Transform> ();
    List<Vector3> pointPositions = new List<Vector3> ();

    float timeValue = 0f;

    void Awake()
    {
        foreach (var pt in points)
        {
            if (pt != null)
                pointPositions.Add(pt.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeValue += Time.deltaTime / 2f;   //2초 동안 애니메이션
        transform.position = DeCasteljau(pointPositions, timeValue);
    }

    Vector3 DeCasteljau(List<Vector3> p, float t)
    {
        while (p.Count > 1)
        {
            int last = p.Count - 1;  //마지막 점의 인덱스

            var next = new List<Vector3>(last);
            for (int i = 0; i < last; i++)
                next.Add(Vector3.Lerp(p[i], p[i + 1], t));
            p = next;                     //한 단계 줄이기
        }

        // count가 1이 되면, p[0]에 남은 점이 곡선 위치

        return p[0];                          //남은 한 점이 곡선 위치
    }
}
