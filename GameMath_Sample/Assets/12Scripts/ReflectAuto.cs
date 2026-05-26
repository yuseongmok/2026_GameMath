using UnityEngine;

public class ReflectAuto : MonoBehaviour
{
    public Vector3 velocity = new Vector3(2f, -3f, 0f);

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal.normalized; //충돌 지점의 법선 벡터

        Vector3 reflect = Vector3.Reflect(velocity, normal); //반사 벡터 계산

        velocity = reflect;
    }
}
