using UnityEngine;

public class Bezier : MonoBehaviour
{
    public Transform point0;
    public Transform point1;
    public Transform point2;
    public Transform point3;

    float timeValue = 0f;

    void Update()
    {
        timeValue += Time.deltaTime / 2f;   //2초 동안 애니메이션
        transform.position = GetPointOnBezierCurve(point0.position, point1.position, point2.position, point3.position, timeValue);
    }

    Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return Mathf.Pow(1 - t, 3) * p0
            + Mathf.Pow(1 - t, 2) * 3 * t * p1
            + Mathf.Pow(t, 2) * 3 * (1 - t) * p2
            + Mathf.Pow(t, 3) * p3;
    }
}
