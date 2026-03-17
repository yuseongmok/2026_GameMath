using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;

public class Field : MonoBehaviour
{
    public TMP_InputField angleInputField;
    public GameObject spherePrefab;
    public Transform firePoint;
    public float force = 15f;

    public void Launch()  // Canvas에 버튼을 만들고 Onclick 이벤트에 연결
    {
        Debug.Log("발사");
        
        float angle = float.Parse(angleInputField.text);
        float rad = angle * Mathf.Deg2Rad;

        Vector3 dir = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));

        GameObject sphere = Instantiate(spherePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = sphere.GetComponent<Rigidbody>();

        rb.AddForce((dir + Vector3.up * 0.3f).normalized * force, ForceMode.Impulse);

    }
}
