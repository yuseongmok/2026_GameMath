using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float speed = 2.0f;

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1.0f);
        transform.position = Vector3.Lerp(startPos, endPos, t);
    }
}