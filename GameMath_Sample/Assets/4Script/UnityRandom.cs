using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnityRandom : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Random.InitState(1234);   //Unity 난수 시드 고정
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(Random.Range(1, 7)); //1~6 사이의 난수
        }
    }
}
