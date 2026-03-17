using UnityEngine;
using UnityEngine.iOS;

public class TestRadian : MonoBehaviour
{




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float degrees = 45f;
        float radians = degrees * Mathf.Deg2Rad;
        Debug.Log("45도 -> 라디안 : " + radians);


        float radianValue = Mathf.PI / 3;
        float degreeValue = radianValue * Mathf.Rad2Deg;
        Debug.Log("파이/3 라디안 -> 도 변환 : " + degreeValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
