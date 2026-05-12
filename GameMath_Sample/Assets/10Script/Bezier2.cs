using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bezier2 : MonoBehaviour
{
    public Transform p0;          //시작점 (고정) 
    public Transform p3;          //도착점 (고정)

    [Header("Random Ranges")]
    public float p1Radius = 2f;  //p0 근처에서 뽑는 반경
    public float p2Radius = 2f;  //p3 근처에서 뽑는 반경
    public float p1Height = 3f;  //p1 y축 추가 높이 (선택)
    public float p2Height = 3f;  //p2 y축 추가 높이 (선택)

    [HideInInspector] public Vector3 p1;
    [HideInInspector] public Vector3 p2;

    List<Vector3> points;

    float time = 0f;

    void Awake()
    {
        GenerateRandomControlPoints();
        points = new List<Vector3> { p0.position, p1, p2, p3.position };
    }

    void GenerateRandomControlPoints()
    {
        Vector2 rand1 = Random.insideUnitCircle * p1Radius;
        p1 = p0.position + new Vector3(rand1.x, 0f, rand1.y);
        p1.y += p1Height;                //살짝 위로 띄워 궤적 상승

        Vector2 rand2 = Random.insideUnitCircle * p2Radius;
        p2 = p3.position + new Vector3(rand2.x, 0f, rand2.y);
        p2.y += p2Height;               // 도착 직전 살짝 꺾이도록
    }

    Vector3 DeCasteljau(List<Vector3> p, float t)
    {
        while (p.Count > 1)
        {
            int last = p.Count - 1;

            var next = new List<Vector3>(last);
            for (int i = 0;  i < last; i++)
                next.Add(Vector3.Lerp(p[i], p[i + 1], t));
            p= next;
        }
        
        return p[0];
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime / 1f;
        transform.position = DeCasteljau(points, time);
    }
}
